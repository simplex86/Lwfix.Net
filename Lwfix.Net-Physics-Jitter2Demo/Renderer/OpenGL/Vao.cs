using Silk.NET.OpenGL;

namespace SimplexLab.Lwfix.Physics.JDemo.Renderer;

public enum AttribType : uint
{
    Byte = (int)VertexAttribPointerType.Byte,
    UnsignedByte = (int)VertexAttribPointerType.UnsignedByte,
    Short = (int)VertexAttribPointerType.Short,
    Int = (int)VertexAttribPointerType.Int,
    UnsignedInt = (int)VertexAttribPointerType.UnsignedInt,
    HalfFloat = (int)VertexAttribPointerType.HalfFloat,
    Float = (int)VertexAttribPointerType.Float,
    Double = (int)VertexAttribPointerType.Double,
}

public sealed class Vao : IDisposable
{
    private static readonly GL _gl = GLContext.Gl;
    public uint Handle { get; }

    public Vao()
    {
        Handle = _gl.GenVertexArray();
    }

    public void Bind() => _gl.BindVertexArray(Handle);

    public unsafe void Attrib(uint index, GpuBuffer buffer, int components, AttribType type,
        int stride, int offset, bool normalized = false, uint divisor = 0)
    {
        Bind();
        buffer.Bind();
        _gl.EnableVertexAttribArray(index);
        _gl.VertexAttribPointer(index, components, (VertexAttribPointerType)type, normalized, (uint)stride, (void*)offset);
        if (divisor != 0) _gl.VertexAttribDivisor(index, divisor);
    }

    public void AttachIndexBuffer(GpuBuffer buffer)
    {
        Bind();
        buffer.Bind();
    }

    public void Dispose() => _gl.DeleteVertexArray(Handle);
}
