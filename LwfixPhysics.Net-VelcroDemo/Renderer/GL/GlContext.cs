using Silk.NET.OpenGL;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Renderer.OpenGL;

/// <summary>Holds the single GL instance for the demo (set by Playground on load).</summary>
public static class GlContext
{
    public static GL Gl { get; private set; } = null!;

    public static void Initialize(GL gl) => Gl = gl;
}
