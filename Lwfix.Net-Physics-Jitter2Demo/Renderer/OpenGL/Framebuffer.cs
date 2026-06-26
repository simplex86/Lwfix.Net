using Silk.NET.OpenGL;

namespace SimplexLab.Fixed.Physics.JDemo.Renderer;

public sealed class Framebuffer
{
    private static readonly GL _gl = GLContext.Gl;

    public uint Handle { get; }

    public Framebuffer()
    {
        Handle = _gl.GenFramebuffer();
    }

    private Framebuffer(uint handle) { Handle = handle; }

    public static readonly Framebuffer Default = new(0);

    public void Bind() => _gl.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);

    public void AttachDepthTexture(Texture2D texture)
    {
        Bind();
        _gl.FramebufferTexture2D(FramebufferTarget.Framebuffer,
            FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, texture.Handle, 0);
        _gl.DrawBuffer(DrawBufferMode.None);
    }
}
