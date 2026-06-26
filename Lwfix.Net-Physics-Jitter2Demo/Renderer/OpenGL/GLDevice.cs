using Silk.NET.OpenGL;

namespace SimplexLab.Lwfix.Physics.JDemo.Renderer;

public enum DrawMode
{
    Lines = (int)PrimitiveType.Lines,
    Triangles = (int)PrimitiveType.Triangles
}

public enum CullMode
{
    Front = (int)TriangleFace.Front,
    Back = (int)TriangleFace.Back,
    FrontAndBack = (int)TriangleFace.FrontAndBack
}

public enum IndexType
{
    UnsignedInt = (int)DrawElementsType.UnsignedInt,
    UnsignedShort = (int)DrawElementsType.UnsignedShort
}

public enum ClearFlags
{
    Color = (int)ClearBufferMask.ColorBufferBit,
    Depth = (int)ClearBufferMask.DepthBufferBit,
    Stencil = (int)ClearBufferMask.StencilBufferBit,
    ColorAndDepth = Color | Depth
}

public enum BlendFunc
{
    Zero = (int)BlendingFactor.Zero,
    One = (int)BlendingFactor.One,
    SrcAlpha = (int)BlendingFactor.SrcAlpha,
    OneMinusSrcAlpha = (int)BlendingFactor.OneMinusSrcAlpha,
    SrcColor = (int)BlendingFactor.SrcColor,
    DstAlpha = (int)BlendingFactor.DstAlpha,
    DstColor = (int)BlendingFactor.DstColor,
    OneMinusSrcColor = (int)BlendingFactor.OneMinusSrcColor
}

public enum Capability
{
    Blend = (int)EnableCap.Blend,
    DepthTest = (int)EnableCap.DepthTest,
    CullFace = (int)EnableCap.CullFace,
    ScissorTest = (int)EnableCap.ScissorTest
}

public static class GLDevice
{
    private static readonly GL _gl = GLContext.Gl;

    public static void Clear(ClearFlags flags) => _gl.Clear((uint)flags);
    public static void ClearColor(float r, float g, float b, float a) => _gl.ClearColor(r, g, b, a);

    public static void Enable(Capability cap) => _gl.Enable((EnableCap)cap);
    public static void Disable(Capability cap) => _gl.Disable((EnableCap)cap);

    public static void CullFace(CullMode mode) => _gl.CullFace((TriangleFace)mode);
    public static void Viewport(int x, int y, int w, int h) => _gl.Viewport(x, y, (uint)w, (uint)h);
    public static void Blend(BlendFunc src, BlendFunc dst) => _gl.BlendFunc((BlendingFactor)src, (BlendingFactor)dst);

    public static void DepthMask(bool write) => _gl.DepthMask(write);

    public static void DrawArrays(DrawMode mode, int first, int count)
        => _gl.DrawArrays((PrimitiveType)mode, first, (uint)count);

    public static unsafe void DrawElements(DrawMode mode, int count, IndexType type, int offsetBytes)
        => _gl.DrawElements((PrimitiveType)mode, (uint)count, (DrawElementsType)type, (void*)offsetBytes);

    public static unsafe void DrawElementsInstanced(DrawMode mode, int count, IndexType type, int offsetBytes, int instances)
        => _gl.DrawElementsInstanced((PrimitiveType)mode, (uint)count, (DrawElementsType)type, (void*)offsetBytes, (uint)instances);

    public static unsafe void DrawElementsBaseVertex(DrawMode mode, int count, IndexType type, int offsetBytes, int baseVertex)
        => _gl.DrawElementsBaseVertex((PrimitiveType)mode, (uint)count, (DrawElementsType)type, (void*)offsetBytes, baseVertex);

    public static void Scissor(int x, int y, int w, int h) => _gl.Scissor(x, y, (uint)w, (uint)h);
}
