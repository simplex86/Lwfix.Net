using Silk.NET.OpenGL;

namespace SimplexLab.Lwfix.Physics.JDemo.Renderer;

public enum BufferUsage
{
    StaticDraw = (int)BufferUsageARB.StaticDraw,
    DynamicDraw = (int)BufferUsageARB.DynamicDraw,
    StreamDraw = (int)BufferUsageARB.StreamDraw
}

public sealed class GpuBuffer : IDisposable
{
    private static readonly GL _gl = GLContext.Gl;

    public uint Handle { get; }
    public BufferTargetARB Target { get; }
    public int Capacity { get; private set; }

    private GpuBuffer(BufferTargetARB target)
    {
        Target = target;
        Handle = _gl.GenBuffer();
    }

    public static GpuBuffer Vertex() => new(BufferTargetARB.ArrayBuffer);
    public static GpuBuffer Index() => new(BufferTargetARB.ElementArrayBuffer);

    public void Bind() => _gl.BindBuffer(Target, Handle);

    public unsafe void Upload<T>(ReadOnlySpan<T> data, BufferUsage usage = BufferUsage.StaticDraw) where T : unmanaged
    {
        Bind();
        int bytes = data.Length * sizeof(T);
        fixed (T* p = data)
        {
            _gl.BufferData(Target, (nuint)bytes, p, (BufferUsageARB)usage);
            Capacity = bytes;
        }
    }

    public void Upload<T>(T[] data, BufferUsage usage = BufferUsage.StaticDraw) where T : unmanaged
        => Upload<T>(data.AsSpan(), usage);

    public void Upload<T>(T[] data, int count, BufferUsage usage = BufferUsage.StaticDraw) where T : unmanaged
        => Upload<T>(data.AsSpan(0, count), usage);

    /// <summary>Orphan + subdata: release the old backing store to the driver, write into fresh memory.</summary>
    public unsafe void Stream<T>(ReadOnlySpan<T> data) where T : unmanaged
    {
        Bind();
        int bytes = data.Length * sizeof(T);
        _gl.BufferData(Target, (nuint)bytes, (void*)0, BufferUsageARB.DynamicDraw);
        if (bytes > 0)
        {
            fixed (T* p = data)
                _gl.BufferSubData(Target, 0, (nuint)bytes, p);
        }
        Capacity = bytes;
    }

    public void Stream<T>(T[] data, int count) where T : unmanaged
        => Stream<T>(data.AsSpan(0, count));

    public void Dispose() => _gl.DeleteBuffer(Handle);
}
