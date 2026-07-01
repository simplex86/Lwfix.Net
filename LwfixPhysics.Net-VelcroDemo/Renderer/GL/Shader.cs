using System.Collections.Generic;
using System.Numerics;
using Silk.NET.OpenGL;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Renderer.OpenGL;

public sealed class Shader : IDisposable
{
    public uint Handle { get; }
    private readonly Dictionary<string, int> _locations = new();
    private static readonly GL _gl = GlContext.Gl;

    public Shader(string vertex, string fragment)
    {
        uint vs = CompileStage(ShaderType.VertexShader, vertex);
        uint fs = CompileStage(ShaderType.FragmentShader, fragment);

        Handle = _gl.CreateProgram();
        _gl.AttachShader(Handle, vs);
        _gl.AttachShader(Handle, fs);
        _gl.LinkProgram(Handle);
        _gl.GetProgram(Handle, ProgramPropertyARB.LinkStatus, out int success);
        if (success == 0)
            throw new Exception($"Shader link failed:\n{_gl.GetProgramInfoLog(Handle)}");

        _gl.DeleteShader(vs);
        _gl.DeleteShader(fs);
    }

    private static uint CompileStage(ShaderType stage, string source)
    {
        uint id = _gl.CreateShader(stage);
        _gl.ShaderSource(id, source);
        _gl.CompileShader(id);
        _gl.GetShader(id, ShaderParameterName.CompileStatus, out int success);
        if (success == 0)
            throw new Exception($"{stage} shader compile failed:\n{_gl.GetShaderInfoLog(id)}");
        return id;
    }

    public void Use() => _gl.UseProgram(Handle);

    private int Loc(string name)
    {
        if (_locations.TryGetValue(name, out int loc)) return loc;
        loc = _gl.GetUniformLocation(Handle, name);
        _locations[name] = loc;
        return loc;
    }

    public void Set(string name, float value) => _gl.Uniform1(Loc(name), value);
    public void Set(string name, float x, float y) => _gl.Uniform2(Loc(name), x, y);
    public void Set(string name, float x, float y, float z, float w) => _gl.Uniform4(Loc(name), x, y, z, w);

    public unsafe void Set(string name, in Matrix4x4 m)
    {
        // Copy the by-ref parameter to a local so we can take its address
        // (Matrix4x4 is unmanaged/blittable: 16 floats, no references to pin).
        Matrix4x4 local = m;
        _gl.UniformMatrix4(Loc(name), 1, false, (float*)&local);
    }

    public void Dispose() => _gl.DeleteProgram(Handle);
}
