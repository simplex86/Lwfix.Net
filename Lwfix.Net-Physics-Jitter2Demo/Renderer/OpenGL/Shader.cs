using System.Collections.Generic;
using System.Diagnostics;
using Silk.NET.OpenGL;

namespace SimplexLab.Fixed.Physics.JDemo.Renderer;

public class ShaderException : Exception
{
    public ShaderException(string msg) : base(msg) { }
}

public sealed class Shader : IDisposable
{
    public uint Handle { get; }
    private readonly Dictionary<string, int> _locations = new();
    private static readonly GL _gl = GLContext.Gl;

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
        {
            string log = _gl.GetProgramInfoLog(Handle);
            throw new ShaderException($"Shader link failed:\n{log}");
        }

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
        {
            string log = _gl.GetShaderInfoLog(id);
            throw new ShaderException($"{stage} shader compile failed:\n{log}");
        }
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

    public bool Has(string name) => Loc(name) >= 0;

    public void Set(string name, int value) => _gl.Uniform1(Loc(name), value);
    public void Set(string name, uint value) => _gl.Uniform1(Loc(name), value);
    public void Set(string name, bool value) => _gl.Uniform1(Loc(name), value ? 1 : 0);
    public void Set(string name, float value) => _gl.Uniform1(Loc(name), value);

    public void Set(string name, float x, float y) => _gl.Uniform2(Loc(name), x, y);
    public void Set(string name, float x, float y, float z) => _gl.Uniform3(Loc(name), x, y, z);
    public void Set(string name, float x, float y, float z, float w) => _gl.Uniform4(Loc(name), x, y, z, w);

    public void Set(string name, in Vector2 v) => _gl.Uniform2(Loc(name), v.X, v.Y);
    public void Set(string name, in Vector3 v) => _gl.Uniform3(Loc(name), v.X, v.Y, v.Z);
    public void Set(string name, in Vector4 v) => _gl.Uniform4(Loc(name), v.X, v.Y, v.Z, v.W);

    public unsafe void Set(string name, in Matrix4 m)
    {
        fixed (float* p = &m.M11)
            _gl.UniformMatrix4(Loc(name), 1, false, p);
    }

    public unsafe void Set(string name, float[] values)
    {
        fixed (float* p = values)
            _gl.Uniform1(Loc(name), (uint)values.Length, p);
    }

    public void Dispose()
    {
        _gl.DeleteProgram(Handle);
    }
}
