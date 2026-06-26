namespace SimplexLab.Fixed.Physics.JDemo.Renderer;

/// <summary>
/// Surface description consumed by the lit shader. Controls how vertex color,
/// flat tint, and optional texture mix into the final fragment color.
/// </summary>
public struct Material
{
    public Vector3 Tint;
    public Vector3 Specular;
    public float Shininess;
    public float Alpha;
    public float VertexColorWeight;
    public float TextureWeight;
    public bool FlipNormals;
    public Texture2D? Texture;

    public static Material Default => new()
    {
        Tint = Vector3.Zero,
        Specular = new Vector3(0.1f, 0.1f, 0.1f),
        Shininess = 128f,
        Alpha = 1f,
        VertexColorWeight = 1f,
        TextureWeight = 0f,
        FlipNormals = false,
        Texture = null
    };
}
