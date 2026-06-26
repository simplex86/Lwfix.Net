using System;

namespace SimplexLab.Lwfix.Physics.JDemo.Renderer;

/// Immediate-mode line renderer for debug overlays (contact points, AABBs, rays, ...).
/// One array per color; callers push primitives each frame, the renderer streams them
/// to the GPU and flushes before drawing the scene.
public sealed class DebugRenderer
{
    public enum Color
    {
        White = 0,
        Red = 1,
        Green = 2,
        Count = 3
    }

    private static readonly Vector4[] ColorValues =
    {
        new(1, 1, 1, 1),
        new(1, 0, 0, 1),
        new(0, 1, 0, 1)
    };

    private sealed class Bucket
    {
        public Vector3[] Vertices = new Vector3[128];
        public LineVertexIndex[] Indices = new LineVertexIndex[128];
        public int VertexCount;
        public int IndexCount;

        public uint PushVertex(float x, float y, float z)
        {
            if (VertexCount == Vertices.Length) Array.Resize(ref Vertices, Vertices.Length * 2);
            Vertices[VertexCount] = new Vector3(x, y, z);
            return (uint)VertexCount++;
        }

        public void PushIndex(uint i1, uint i2)
        {
            if (IndexCount == Indices.Length) Array.Resize(ref Indices, Indices.Length * 2);
            Indices[IndexCount++] = new LineVertexIndex(i1, i2);
        }

        public void Clear() { VertexCount = 0; IndexCount = 0; }
    }

    private readonly Bucket[] buckets = CreateBuckets();

    private static Bucket[] CreateBuckets()
    {
        var result = new Bucket[(int)Color.Count];
        for (int i = 0; i < result.Length; i++) result[i] = new Bucket();
        return result;
    }

    private Vao vao = null!;
    private GpuBuffer vbo = null!;
    private GpuBuffer ebo = null!;
    private Shader shader = null!;

    public void Load()
    {
        shader = new Shader(Vs, Fs);

        vao = new Vao();
        vbo = GpuBuffer.Vertex();
        ebo = GpuBuffer.Index();

        vao.Attrib(0, vbo, 3, AttribType.Float, 3 * sizeof(float), 0);
        vao.AttachIndexBuffer(ebo);
    }

    public void Draw(Camera camera)
    {
        shader.Use();
        shader.Set("uProjection", camera.ProjectionMatrix);
        shader.Set("uView", camera.ViewMatrix);

        vao.Bind();

        for (int c = 0; c < (int)Color.Count; c++)
        {
            var bucket = buckets[c];
            if (bucket.IndexCount == 0) continue;

            vbo.Stream<Vector3>(bucket.Vertices, bucket.VertexCount);
            ebo.Stream<LineVertexIndex>(bucket.Indices, bucket.IndexCount);

            shader.Set("uColor", ColorValues[c]);
            GLDevice.DrawElements(DrawMode.Lines, bucket.IndexCount * 2, IndexType.UnsignedInt, 0);

            bucket.Clear();
        }
    }

    public void PushLine(Color color, in Vector3 a, in Vector3 b)
    {
        var bucket = buckets[(int)color];
        uint i0 = bucket.PushVertex(a.X, a.Y, a.Z);
        uint i1 = bucket.PushVertex(b.X, b.Y, b.Z);
        bucket.PushIndex(i0, i1);
    }

    public void PushBox(Color color, in Vector3 min, in Vector3 max)
    {
        var bucket = buckets[(int)color];

        uint o0 = bucket.PushVertex(min.X, min.Y, min.Z);
        uint o1 = bucket.PushVertex(max.X, min.Y, min.Z);
        uint o2 = bucket.PushVertex(min.X, max.Y, min.Z);
        uint o3 = bucket.PushVertex(min.X, min.Y, max.Z);
        uint o4 = bucket.PushVertex(max.X, max.Y, min.Z);
        uint o5 = bucket.PushVertex(min.X, max.Y, max.Z);
        uint o6 = bucket.PushVertex(max.X, min.Y, max.Z);
        uint o7 = bucket.PushVertex(max.X, max.Y, max.Z);

        bucket.PushIndex(o0, o1); bucket.PushIndex(o0, o2); bucket.PushIndex(o0, o3);
        bucket.PushIndex(o1, o4); bucket.PushIndex(o1, o6);
        bucket.PushIndex(o2, o4); bucket.PushIndex(o2, o5);
        bucket.PushIndex(o3, o5); bucket.PushIndex(o3, o6);
        bucket.PushIndex(o4, o7); bucket.PushIndex(o5, o7); bucket.PushIndex(o6, o7);
    }

    public void PushPoint(Color color, in Vector3 pos, float halfSize = 1.0f)
    {
        var bucket = buckets[(int)color];
        uint i0 = bucket.PushVertex(pos.X - halfSize, pos.Y, pos.Z);
        uint i1 = bucket.PushVertex(pos.X + halfSize, pos.Y, pos.Z);
        uint i2 = bucket.PushVertex(pos.X, pos.Y - halfSize, pos.Z);
        uint i3 = bucket.PushVertex(pos.X, pos.Y + halfSize, pos.Z);
        uint i4 = bucket.PushVertex(pos.X, pos.Y, pos.Z - halfSize);
        uint i5 = bucket.PushVertex(pos.X, pos.Y, pos.Z + halfSize);

        bucket.PushIndex(i0, i1);
        bucket.PushIndex(i2, i3);
        bucket.PushIndex(i4, i5);
    }

    private const string Vs = @"
#version 330 core
layout(location = 0) in vec3 aPos;

uniform mat4 uView;
uniform mat4 uProjection;

void main() { gl_Position = uProjection * uView * vec4(aPos, 1.0); }
";

    private const string Fs = @"
#version 330 core
uniform vec4 uColor;
out vec4 FragColor;
void main() { FragColor = uColor; }
";
}
