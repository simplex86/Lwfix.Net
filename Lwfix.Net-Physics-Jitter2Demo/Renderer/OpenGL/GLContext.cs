using Silk.NET.OpenGL;

namespace SimplexLab.Fixed.Physics.JDemo.Renderer;

/// <summary>
/// Holds the global Silk.NET GL instance. Set once during window initialization;
/// all GL wrapper classes access it through this static property.
/// </summary>
public static class GLContext
{
    public static GL Gl { get; private set; } = null!;
    public static void Initialize(GL gl) => Gl = gl;
}
