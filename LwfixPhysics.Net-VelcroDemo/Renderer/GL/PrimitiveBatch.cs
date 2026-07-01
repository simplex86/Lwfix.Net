using System.Numerics;
using Silk.NET.OpenGL;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Renderer.OpenGL;

/// <summary>
/// Immediate-mode 2D primitive batcher: streams lines and filled triangles
/// with per-vertex color. All coordinates are in simulation units; the caller
/// supplies an orthographic projection each frame.
/// </summary>
public sealed class PrimitiveBatch : IDisposable
{
    private const int MaxVertices = 1 << 16;

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    private struct Vertex2D
    {
        public float X, Y;
        public float R, G, B, A;
    }

    private readonly GL _gl = GlContext.Gl;
    private readonly Shader _shader;
    private readonly uint _lineVao, _lineVbo;
    private readonly uint _triVao, _triVbo;
    private readonly Vertex2D[] _lineVerts = new Vertex2D[MaxVertices];
    private readonly Vertex2D[] _triVerts = new Vertex2D[MaxVertices];
    private int _lineCount;
    private int _triCount;

    public PrimitiveBatch()
    {
        _shader = new Shader(VsSrc, FsSrc);

        _lineVao = _gl.CreateVertexArray();
        _lineVbo = _gl.CreateBuffer();
        _gl.BindVertexArray(_lineVao);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _lineVbo);
        SetupAttribs();

        _triVao = _gl.CreateVertexArray();
        _triVbo = _gl.CreateBuffer();
        _gl.BindVertexArray(_triVao);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _triVbo);
        SetupAttribs();

        _gl.BindVertexArray(0);
    }

    private unsafe void SetupAttribs()
    {
        // position (location 0)
        _gl.EnableVertexAttribArray(0);
        _gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 6 * sizeof(float), (void*)0);
        // color (location 1)
        _gl.EnableVertexAttribArray(1);
        _gl.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 6 * sizeof(float), (void*)(2 * sizeof(float)));
    }

    public void Begin(in Matrix4x4 projection)
    {
        _shader.Use();
        _shader.Set("uProjection", in projection);
        _lineCount = 0;
        _triCount = 0;
    }

    public void AddLine(float x1, float y1, float x2, float y2, Vector4 c)
    {
        if (_lineCount + 2 > MaxVertices) FlushLines();
        _lineVerts[_lineCount++] = new Vertex2D { X = x1, Y = y1, R = c.X, G = c.Y, B = c.Z, A = c.W };
        _lineVerts[_lineCount++] = new Vertex2D { X = x2, Y = y2, R = c.X, G = c.Y, B = c.Z, A = c.W };
    }

    public void AddTriangle(float x1, float y1, float x2, float y2, float x3, float y3, Vector4 c)
    {
        if (_triCount + 3 > MaxVertices) FlushTriangles();
        AddTriVertex(x1, y1, c);
        AddTriVertex(x2, y2, c);
        AddTriVertex(x3, y3, c);
    }

    private void AddTriVertex(float x, float y, Vector4 c)
    {
        _triVerts[_triCount++] = new Vertex2D { X = x, Y = y, R = c.X, G = c.Y, B = c.Z, A = c.W };
    }

    public void End()
    {
        FlushLines();
        FlushTriangles();
    }

    private unsafe void FlushLines()
    {
        if (_lineCount == 0) return;
        _gl.BindVertexArray(_lineVao);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _lineVbo);
        fixed (Vertex2D* p = _lineVerts)
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(_lineCount * sizeof(Vertex2D)), p, BufferUsageARB.DynamicDraw);
        _gl.DrawArrays(PrimitiveType.Lines, 0, (uint)_lineCount);
        _lineCount = 0;
    }

    private unsafe void FlushTriangles()
    {
        if (_triCount == 0) return;
        _gl.BindVertexArray(_triVao);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _triVbo);
        fixed (Vertex2D* p = _triVerts)
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(_triCount * sizeof(Vertex2D)), p, BufferUsageARB.DynamicDraw);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)_triCount);
        _triCount = 0;
    }

    private const string VsSrc = @"
#version 330 core
layout(location = 0) in vec2 aPos;
layout(location = 1) in vec4 aColor;
uniform mat4 uProjection;
out vec4 vColor;
void main() { vColor = aColor; gl_Position = uProjection * vec4(aPos, 0.0, 1.0); }
";

    private const string FsSrc = @"
#version 330 core
in vec4 vColor;
out vec4 FragColor;
void main() { FragColor = vColor; }
";

    public void Dispose()
    {
        _gl.DeleteVertexArray(_lineVao);
        _gl.DeleteBuffer(_lineVbo);
        _gl.DeleteVertexArray(_triVao);
        _gl.DeleteBuffer(_triVbo);
        _shader.Dispose();
    }
}
