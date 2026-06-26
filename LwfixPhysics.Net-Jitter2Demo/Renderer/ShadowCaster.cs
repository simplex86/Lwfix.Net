using System;

namespace SimplexLab.LwfixPhysics.Jitter2Demo.Renderer;

/// <summary>
/// Cascaded shadow maps with a configurable number of cascades and shadow-map resolution.
/// Computes per-cascade light matrices by fitting an orthographic projection to the camera
/// frustum slice, renders each cascade into a depth texture, and binds everything ready
/// for the lit pass.
/// </summary>
public sealed class ShadowCaster
{
    public const int CascadeCount = 3;
    public const int ShadowMapSize = 4096;

    public Texture2D[] DepthMaps { get; } = new Texture2D[CascadeCount];

    private readonly Framebuffer[] depthFramebuffers = new Framebuffer[CascadeCount];
    private readonly Matrix4[] lightMatrices = new Matrix4[CascadeCount];

    /// Linear view-space ranges [near, far] for each cascade in cascade-order.
    public float[] CascadeSplits { get; set; } = { 20f, 60f };

    public Vector3 LightDirection { get; set; } = Vector3.Normalize(new Vector3(1, 2, 1));

    private readonly Shader shadowShader;

    public ShadowCaster()
    {
        for (int i = 0; i < CascadeCount; i++)
        {
            var tex = new Texture2D();
            tex.Allocate(TextureFormat.Depth, ShadowMapSize, ShadowMapSize, TexelType.Float);
            tex.SetMinMagFilter(TextureFilter.Nearest, TextureFilter.Nearest);
            tex.SetWrap(TextureWrap.ClampToBorder);
            tex.SetBorderColor(new Vector4(1, 1, 1, 1));

            var fb = new Framebuffer();
            fb.AttachDepthTexture(tex);

            DepthMaps[i] = tex;
            depthFramebuffers[i] = fb;
        }

        Framebuffer.Default.Bind();

        shadowShader = new Shader(ShadowVs, ShadowFs);
    }

    public ReadOnlySpan<Matrix4> LightMatrices => lightMatrices;

    /// <summary>
    /// Renders the shadow cascades. Calls the supplied <paramref name="drawCallback"/>
    /// once per cascade after binding the framebuffer and setting the uniform light
    /// matrix.
    /// </summary>
    public void Render(Camera camera, int viewportW, int viewportH, Action<Shader> drawCallback)
    {
        GLDevice.Viewport(0, 0, ShadowMapSize, ShadowMapSize);
        shadowShader.Use();

        ComputeLightMatrices(camera, viewportW, viewportH);

        for (int i = 0; i < CascadeCount; i++)
        {
            depthFramebuffers[i].Bind();
            GLDevice.Clear(ClearFlags.Depth);

            shadowShader.Set("uLightMatrix", lightMatrices[i]);
            drawCallback(shadowShader);
        }

        Framebuffer.Default.Bind();

        // Leave the depth textures bound in units [0..CascadeCount-1] so the lit pass
        // can just sample from them.
        for (uint i = 0; i < CascadeCount; i++) DepthMaps[i].Bind(i);
    }

    /// Uploads the cascade matrices, splits, and sun direction to the supplied lit shader.
    public void BindToLitShader(Shader lit)
    {
        for (int i = 0; i < CascadeCount; i++)
        {
            lit.Set($"uLightMatrices[{i}]", lightMatrices[i]);
        }

        float[] splits = new float[CascadeCount];
        for (int i = 0; i < CascadeCount; i++)
        {
            splits[i] = i < CascadeSplits.Length ? CascadeSplits[i] : float.MaxValue;
        }
        lit.Set("uCascadeSplits", splits);
        lit.Set("uSunDir", LightDirection);
    }

    private void ComputeLightMatrices(Camera camera, int viewportW, int viewportH)
    {
        float aspect = (float)viewportW / viewportH;

        for (int i = 0; i < CascadeCount; i++)
        {
            float near = i == 0 ? camera.NearPlane : CascadeSplits[i - 1];
            float far = i < CascadeSplits.Length ? CascadeSplits[i] : camera.FarPlane;
            lightMatrices[i] = FitLightMatrixToFrustum(camera, aspect, near, far);
        }
    }

    private Matrix4 FitLightMatrixToFrustum(Camera camera, float aspect, float near, float far)
    {
        Matrix4 proj = MatrixHelper.CreatePerspectiveFieldOfView(camera.FieldOfView, aspect, near, far);
        Span<Vector4> corners = stackalloc Vector4[8];
        FrustumCorners(corners, proj, camera.ViewMatrix);

        Vector3 center = Vector3.Zero;
        for (int i = 0; i < 8; i++) center += corners[i].XYZ();
        center *= 1f / 8f;

        Matrix4 lightView = MatrixHelper.CreateLookAt(center + LightDirection, center, Vector3.UnitY);

        Vector3 min = new(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new(float.MinValue, float.MinValue, float.MinValue);
        for (int i = 0; i < 8; i++)
        {
            Vector4 t = lightView * corners[i];
            min.X = Math.Min(min.X, t.X); max.X = Math.Max(max.X, t.X);
            min.Y = Math.Min(min.Y, t.Y); max.Y = Math.Max(max.Y, t.Y);
            min.Z = Math.Min(min.Z, t.Z); max.Z = Math.Max(max.Z, t.Z);
        }

        // Pull the near/far planes back so casters outside the frustum still appear in shadow.
        const float zMult = 3f;
        min.Z = min.Z < 0 ? min.Z * zMult : min.Z / zMult;
        max.Z = max.Z < 0 ? max.Z / zMult : max.Z * zMult;

        Matrix4 lightProj = MatrixHelper.CreateOrthographicOffCenter(min.X, max.X, min.Y, max.Y, min.Z, max.Z);
        return lightProj * lightView;
    }

    private static void FrustumCorners(Span<Vector4> corners, in Matrix4 proj, in Matrix4 view)
    {
        if (!Matrix4.Invert(proj * view, out Matrix4 inv))
            throw new InvalidOperationException("Could not invert projection * view matrix.");

        for (int x = 0; x < 2; x++)
        for (int y = 0; y < 2; y++)
        for (int z = 0; z < 2; z++)
        {
            Vector4 p = new(2f * x - 1f, 2f * y - 1f, 2f * z - 1f, 1f);
            p = inv * p;
            corners[4 * x + 2 * y + z] = p * (1f / p.W);
        }
    }

    private const string ShadowVs = @"
#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 3) in mat4 aInstanceModel;

uniform mat4 uLightMatrix;

void main()
{
    gl_Position = uLightMatrix * aInstanceModel * vec4(aPos, 1.0);
}
";

    private const string ShadowFs = @"
#version 330 core
void main() { }
";
}
