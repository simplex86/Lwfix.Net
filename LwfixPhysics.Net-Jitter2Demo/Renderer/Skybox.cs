namespace SimplexLab.LwfixPhysics.Jitter2Demo.Renderer;

/// <summary>
/// Procedural gradient skybox. The original JitterDemo drew a cube into a
/// (unused) cubemap texture; here we keep just the geometry + shader since
/// the procedural fragment shader is the only thing that contributes color.
/// </summary>
public sealed class Skybox
{
    private Vao vao = null!;
    private Shader shader = null!;

    public void Load()
    {
        shader = new Shader(Vs, Fs);
        vao = new Vao();

        var vertexBuffer = GpuBuffer.Vertex();
        vertexBuffer.Upload<float>(CubeVertices);
        vao.Attrib(0, vertexBuffer, 3, AttribType.Float, 3 * sizeof(float), 0);
    }

    public void Draw(Camera camera)
    {
        GLDevice.DepthMask(false);
        GLDevice.CullFace(CullMode.Back);

        shader.Use();
        shader.Set("uView", camera.ViewMatrix);
        shader.Set("uProjection", camera.ProjectionMatrix);

        vao.Bind();
        GLDevice.DrawArrays(DrawMode.Triangles, 0, 36);

        GLDevice.DepthMask(true);
    }

    private static readonly float[] CubeVertices =
    {
        -1.0f,  1.0f, -1.0f, -1.0f, -1.0f, -1.0f,  1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,  1.0f,  1.0f, -1.0f, -1.0f,  1.0f, -1.0f,
        -1.0f, -1.0f,  1.0f, -1.0f, -1.0f, -1.0f, -1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f, -1.0f,  1.0f,  1.0f, -1.0f, -1.0f,  1.0f,
         1.0f, -1.0f, -1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f, -1.0f, -1.0f,
        -1.0f, -1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f, -1.0f, -1.0f,  1.0f,
        -1.0f,  1.0f, -1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  1.0f, -1.0f,  1.0f, -1.0f,
        -1.0f, -1.0f, -1.0f, -1.0f, -1.0f,  1.0f,  1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f, -1.0f, -1.0f,  1.0f,  1.0f, -1.0f,  1.0f
    };

    private const string Vs = @"
#version 330 core
layout(location = 0) in vec3 aPos;

out vec3 vDir;

uniform mat4 uProjection;
uniform mat4 uView;

void main()
{
    vDir = aPos;
    gl_Position = uProjection * mat4(mat3(uView)) * vec4(aPos, 1.0);
}
";

    private const string Fs = @"
#version 330 core
in vec3 vDir;
out vec4 FragColor;

void main()
{
    vec3 blue = vec3(66.0/255.0, 135.0/255.0, 245.0/255.0);
    float d = max(dot(vDir / length(vDir), vec3(0, 1, 1)) + 0.4, 0.0);
    FragColor = vec4(blue * 0.9 + vec3(1) * d * 0.1, 1.0);
}
";
}
