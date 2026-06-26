using Silk.NET.OpenGL;

namespace SimplexLab.LwfixPhysics.Jitter2Demo.Renderer;

public enum TextureWrap { Repeat = (int)TextureWrapMode.Repeat, ClampToBorder = (int)TextureWrapMode.ClampToBorder, ClampToEdge = (int)TextureWrapMode.ClampToEdge }
public enum TextureFilter { Nearest = (int)TextureMinFilter.Nearest, Linear = (int)TextureMinFilter.Linear }
public enum Anisotropy { None = 1, X2 = 2, X4 = 4, X8 = 8, X16 = 16 }
public enum TextureFormat { Depth = (int)InternalFormat.DepthComponent32f, Rgba8 = (int)InternalFormat.Rgba8, Red8 = (int)InternalFormat.R8 }
public enum TexelType { Float = (int)PixelType.Float, UnsignedByte = (int)PixelType.UnsignedByte }

public sealed class Texture2D : IDisposable
{
    private static readonly GL _gl = GLContext.Gl;
    public uint Handle { get; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Texture2D()
    {
        Handle = _gl.GenTexture();
    }

    public void Bind(uint unit = 0)
    {
        _gl.ActiveTexture((TextureUnit)((uint)TextureUnit.Texture0 + unit));
        _gl.BindTexture(TextureTarget.Texture2D, Handle);
    }

    public unsafe void Allocate(TextureFormat format, int width, int height, TexelType texelType)
    {
        Width = width;
        Height = height;
        Bind(0);
        _gl.TexImage2D(TextureTarget.Texture2D, 0, (InternalFormat)format, (uint)width, (uint)height, 0,
            format == TextureFormat.Depth ? PixelFormat.DepthComponent : PixelFormat.Red,
            (PixelType)texelType, (void*)0);
    }

    public unsafe void LoadImage(void* data, int width, int height)
    {
        Width = width;
        Height = height;
        Bind(0);
        _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)width, (uint)height, 0,
            PixelFormat.Rgba, PixelType.UnsignedByte, data);
    }

    public void SetWrap(TextureWrap wrap)
    {
        Bind(0);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrap);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrap);
    }

    public void SetMinMagFilter(TextureFilter min, TextureFilter mag)
    {
        Bind(0);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)min);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)mag);
    }

    public void SetBorderColor(Vector4 color)
    {
        Bind(0);
        unsafe
        {
            float[] c = { color.X, color.Y, color.Z, color.W };
            fixed (float* p = c)
                _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, p);
        }
    }

    public void SetAnisotropy(Anisotropy anisotropy)
    {
        if (anisotropy == Anisotropy.None) return;
        Bind(0);
        float max;
        _gl.GetFloat((GetPName)0x84FF, out max); // GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT
        float v = MathF.Min((float)anisotropy, max);
        unsafe
        {
            _gl.TexParameter(TextureTarget.Texture2D, (TextureParameterName)0x84FE, ref v); // GL_TEXTURE_MAX_ANISOTROPY_EXT
        }
    }

    private static Texture2D? _empty;
    public static unsafe Texture2D Empty()
    {
        if (_empty != null) return _empty;
        _empty = new Texture2D();
        _empty.LoadImage(null, 1, 1);
        return _empty;
    }

    public void Dispose() => _gl.DeleteTexture(Handle);
}
