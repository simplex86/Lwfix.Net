using System;
using System.Runtime.InteropServices;

namespace SimplexLab.LwfixPhysics.Jitter2Demo.Renderer;

[StructLayout(LayoutKind.Sequential)]
internal struct Instance
{
    public Matrix4 Transform;
    public Vector3 Color;
}

/// Per-group material override (used by multi-material meshes such as the demo car).
public struct MaterialSlot
{
    public int GroupIndex;
    public Material Material;

    public MaterialSlot(int groupIndex, in Material material)
    {
        GroupIndex = groupIndex;
        Material = material;
    }
}

/// <summary>
/// Owns the GPU resources for a single mesh and a growable buffer of per-instance
/// transforms + colors. Callers use <see cref="Push"/> each frame; the engine uploads
/// and draws all instances once, both for the shadow pass and the lit pass.
/// </summary>
public class InstancedDrawable
{
    public Mesh Mesh { get; protected set; }

    public Material Material = Material.Default;

    /// Optional per-group material overrides. When null, <see cref="Material"/>
    /// applies to every triangle. When set, groups not listed fall back to
    /// <see cref="Material"/>.
    public MaterialSlot[]? Groups;

    /// Draw both sides of the mesh (two draw calls, normals flipped on the second).
    public bool TwoSided;

    /// Disable back-face culling during the shadow pass (useful for thin geometry).
    public bool ShadowsDoubleSided;

    private Instance[] instances = new Instance[64];
    private int count;
    public int InstanceCount => count;

    protected Vao Vao = null!;
    protected GpuBuffer Vbo = null!;
    protected GpuBuffer Ibo = null!;
    protected GpuBuffer InstanceVbo = null!;

    public InstancedDrawable(Mesh mesh)
    {
        Mesh = mesh;
        CreateBuffers();
    }

    protected InstancedDrawable()
    {
        Mesh = null!;
    }

    protected void CreateBuffers()
    {
        Vao = new Vao();
        // GL_ELEMENT_ARRAY_BUFFER binding is VAO state: bind our VAO first so the
        // index upload below associates the EBO with this VAO rather than
        // silently corrupting whatever VAO happened to be bound before.
        Vao.Bind();

        Vbo = GpuBuffer.Vertex();
        Vbo.Upload<Vertex>(Mesh.Vertices);

        Ibo = GpuBuffer.Index();
        Ibo.Upload<TriangleVertexIndex>(Mesh.Indices);

        InstanceVbo = GpuBuffer.Vertex();

        int sof = sizeof(float);
        // Vertex stream: position(3) + normal(3) + uv(2) = 8 floats
        Vao.Attrib(0, Vbo, 3, AttribType.Float, 8 * sof, 0 * sof);
        Vao.Attrib(1, Vbo, 3, AttribType.Float, 8 * sof, 3 * sof);
        Vao.Attrib(2, Vbo, 2, AttribType.Float, 8 * sof, 6 * sof);

        // Instance stream: mat4(4 attribs, locations 3..6) + color(1 attrib, location 7)
        int instStride = 19 * sof;
        Vao.Attrib(3, InstanceVbo, 4, AttribType.Float, instStride, 0 * sof, divisor: 1);
        Vao.Attrib(4, InstanceVbo, 4, AttribType.Float, instStride, 4 * sof, divisor: 1);
        Vao.Attrib(5, InstanceVbo, 4, AttribType.Float, instStride, 8 * sof, divisor: 1);
        Vao.Attrib(6, InstanceVbo, 4, AttribType.Float, instStride, 12 * sof, divisor: 1);
        Vao.Attrib(7, InstanceVbo, 3, AttribType.Float, instStride, 16 * sof, divisor: 1);

        Vao.AttachIndexBuffer(Ibo);
    }

    public void Push(in Matrix4 transform)
    {
        Push(transform, new Vector3(1, 1, 1));
    }

    public void Push(in Matrix4 transform, in Vector3 color)
    {
        if (count == instances.Length)
        {
            Array.Resize(ref instances, instances.Length * 2);
        }

        ref Instance slot = ref instances[count++];
        slot.Transform = transform;
        slot.Color = color;
    }

    public void Clear() => count = 0;

    /// Called by the engine once per frame before any draw pass.
    public void UploadInstances()
    {
        if (count == 0) return;
        InstanceVbo.Stream<Instance>(instances, count);
    }

    /// Invoked by the shadow pass. Default implementation draws the entire mesh.
    public virtual void DrawShadow()
    {
        if (count == 0) return;
        Vao.Bind();

        if (ShadowsDoubleSided) GLDevice.Disable(Capability.CullFace);
        else GLDevice.CullFace(CullMode.Front);

        GLDevice.DrawElementsInstanced(DrawMode.Triangles, Mesh.Indices.Length * 3,
            IndexType.UnsignedInt, 0, count);

        if (ShadowsDoubleSided) GLDevice.Enable(Capability.CullFace);
        GLDevice.CullFace(CullMode.Back);
    }

    /// Invoked by the lit pass. Default implementation binds the material and
    /// draws the whole mesh. Override to vary material across the mesh.
    public virtual void DrawLit(Shader shader)
    {
        if (count == 0) return;
        Vao.Bind();

        if (Groups is { Length: > 0 })
        {
            DrawLitGrouped(shader);
        }
        else
        {
            DrawAll(shader, Material);
        }

        if (TwoSided)
        {
            Material flipped = Material;
            flipped.FlipNormals = !flipped.FlipNormals;
            GLDevice.CullFace(CullMode.Front);
            DrawAll(shader, flipped);
            GLDevice.CullFace(CullMode.Back);
        }
    }

    private void DrawAll(Shader shader, in Material material)
    {
        ApplyMaterial(shader, material);
        GLDevice.DrawElementsInstanced(DrawMode.Triangles, Mesh.Indices.Length * 3,
            IndexType.UnsignedInt, 0, count);
    }

    private void DrawLitGrouped(Shader shader)
    {
        int sof = sizeof(float);
        var groups = Groups!;

        // Pass 1: draw every index range NOT covered by a group override, using
        // the default material. This assumes the override slots are sorted by
        // GroupIndex (which, given contiguous groups from the OBJ loader, just
        // means "in declaration order").
        ApplyMaterial(shader, Material);
        int cursor = 0;
        for (int i = 0; i < groups.Length; i++)
        {
            var g = Mesh.Groups[groups[i].GroupIndex];
            if (g.FromInclusive > cursor) DrawRange(cursor, g.FromInclusive);
            cursor = g.ToExclusive;
        }
        if (cursor < Mesh.Indices.Length) DrawRange(cursor, Mesh.Indices.Length);

        // Pass 2: draw each override group. Deferring these to the end ensures
        // translucent slots (e.g. the car's glass canopy) blend over the opaque
        // geometry rather than depth-culling it.
        for (int i = 0; i < groups.Length; i++)
        {
            ApplyMaterial(shader, groups[i].Material);
            var g = Mesh.Groups[groups[i].GroupIndex];
            DrawRange(g.FromInclusive, g.ToExclusive);
        }

        void DrawRange(int from, int to)
        {
            if (to <= from) return;
            GLDevice.DrawElementsInstanced(DrawMode.Triangles,
                (to - from) * 3, IndexType.UnsignedInt, from * sof * 3, count);
        }
    }

    protected void ApplyMaterial(Shader shader, in Material material)
    {
        shader.Set("uMat.tint", material.Tint);
        shader.Set("uMat.specular", material.Specular);
        shader.Set("uMat.shininess", material.Shininess);
        shader.Set("uMat.alpha", material.Alpha);
        shader.Set("uMat.vertexColorWeight", material.VertexColorWeight);
        shader.Set("uMat.textureWeight", material.TextureWeight);
        shader.Set("uMat.normalFlip", material.FlipNormals ? -1f : 1f);

        // Always bind SOMETHING to texture unit 3 so the fragment shader's
        // diffuse sample is deterministic regardless of what was bound last.
        (material.Texture ?? Texture2D.Empty()).Bind(3);
    }
}
