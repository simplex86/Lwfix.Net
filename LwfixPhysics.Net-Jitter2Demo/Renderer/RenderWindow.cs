using System;
using System.Collections.Generic;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace SimplexLab.LwfixPhysics.Jitter2Demo.Renderer;

public struct CreationSettings
{
    public int Width;
    public int Height;
    public string Title;

    public CreationSettings(int width, int height, string title = "Lwfix.Net-Physics Demo")
    {
        Width = width;
        Height = height;
        Title = title;
    }

    public static implicit operator WindowOptions(CreationSettings cs)
    {
        return WindowOptions.Default with
        {
            Size = new Silk.NET.Maths.Vector2D<int>(cs.Width, cs.Height),
            Title = cs.Title,
            API = GraphicsAPI.Default with { API = ContextAPI.OpenGL, Version = new(3, 3) },
            WindowState = WindowState.Normal,
            VSync = false,
            ShouldSwapAutomatically = true
        };
    }
}

/// Scene-level host for the engine. Owns the shadow caster, lit shader, debug
/// renderer, ImGui overlay, skybox, camera, and the registry of instanced drawables.
public class RenderWindow
{
    public static RenderWindow Instance { get; private set; } = null!;

    private readonly IWindow _window;
    private IInputContext _input = null!;
    private ImGuiController _imGui = null!;

    public GL Gl { get; private set; } = null!;
    public Keyboard Keyboard { get; private set; } = null!;
    public Mouse Mouse { get; private set; } = null!;

    public Camera Camera { get; set; }
    public ShadowCaster Shadow { get; private set; } = null!;
    public DebugRenderer DebugRenderer { get; private set; } = null!;

    public bool ShowShadowDebug { get; set; }

    private Skybox _skybox = null!;
    private Shader _litShader = null!;

    private readonly Dictionary<Type, InstancedDrawable> _drawables = new();

    private double _lastTime;
    private double _time;

    public RenderWindow(IWindow window)
    {
        Instance = this;
        _window = window;
        Camera = new FreeCamera();
    }

    public double Time => _time;
    public int Width => _window.Size.X;
    public int Height => _window.Size.Y;

    public (int Width, int Height) FramebufferSize => (_window.FramebufferSize.X, _window.FramebufferSize.Y);

    public bool VerticalSync
    {
        set => _window.VSync = value;
    }

    public bool WantsCaptureMouse { get; protected set; }
    public bool WantsCaptureKeyboard { get; protected set; }

    /// Registers a drawable the first time it is requested, caching it by its runtime type.
    public T GetDrawable<T>() where T : InstancedDrawable, new()
    {
        if (!_drawables.TryGetValue(typeof(T), out var d))
        {
            d = new T();
            _drawables.Add(typeof(T), d);
        }
        return (T)d;
    }

    /// Registers an externally-constructed drawable. Useful when construction needs arguments.
    public T RegisterDrawable<T>(T drawable) where T : InstancedDrawable
    {
        _drawables[typeof(T)] = drawable;
        return drawable;
    }

    public IReadOnlyCollection<InstancedDrawable> Drawables => _drawables.Values;

    /// <summary>Hook for subclasses to build ImGui overlays each frame.</summary>
    protected virtual void DrawCustomOverlay() { }

    public virtual void Load()
    {
        // Initialize GL & input on the IWindow (called once after the window opens).
        Gl = _window.CreateOpenGL();
        GLContext.Initialize(Gl);

        _input = _window.CreateInput();
        if (_input.Keyboards.Count > 0) Keyboard = new Keyboard(_input.Keyboards[0]);
        if (_input.Mice.Count > 0) Mouse = new Mouse(_input.Mice[0]);

        _imGui = new ImGuiController(Gl, _window, _input);

        Shadow = new ShadowCaster();
        DebugRenderer = new DebugRenderer();

        _skybox = new Skybox();
        _skybox.Load();

        _litShader = LitShader.Create();

        DebugRenderer.Load();

        VerticalSync = true;

        Camera.Position = new Vector3(0, 4, 8);
        Camera.Update(Keyboard, Mouse, Width > 0 ? (float)Width / Math.Max(1, Height) : 1f);

        _lastTime = _time;
    }

    public virtual void Draw()
    {
        float dt = (float)(_time - _lastTime);
        _lastTime = _time;

        // --- Render setup -------------------------------------------------
        GLDevice.Enable(Capability.DepthTest);
        GLDevice.Enable(Capability.Blend);
        GLDevice.Blend(BlendFunc.SrcAlpha, BlendFunc.OneMinusSrcAlpha);

        GLDevice.ClearColor(73f / 255f, 76f / 255f, 92f / 255f, 1f);
        GLDevice.Clear(ClearFlags.ColorAndDepth);

        _skybox.Draw(Camera);

        // --- Upload all drawable instance buffers once --------------------
        foreach (var d in _drawables.Values) d.UploadInstances();

        // --- Shadow pass --------------------------------------------------
        Shadow.Render(Camera, Width, Math.Max(1, Height), _ =>
        {
            foreach (var d in _drawables.Values) d.DrawShadow();
        });

        // --- Lit pass -----------------------------------------------------
        (int fbw, int fbh) = FramebufferSize;
        GLDevice.Viewport(0, 0, fbw, fbh);

        _litShader.Use();
        _litShader.Set("uProjection", Camera.ProjectionMatrix);
        _litShader.Set("uView", Camera.ViewMatrix);
        _litShader.Set("uViewPos", Camera.Position);
        Shadow.BindToLitShader(_litShader);

        foreach (var d in _drawables.Values) d.DrawLit(_litShader);

        // --- Debug lines --------------------------------------------------
        DebugRenderer.Draw(Camera);

        if (Keyboard.IsKeyDown(Keyboard.Key.Escape)) _window.Close();

        // --- ImGui overlay ------------------------------------------------
        _imGui.Update(dt);
        DrawCustomOverlay();
        _imGui.Render();

        // ImGuiNET updates the capture flags via the IO struct.
        var io = ImGuiNET.ImGui.GetIO();
        WantsCaptureKeyboard = io.WantCaptureKeyboard;
        WantsCaptureMouse = io.WantCaptureMouse;

        // --- Clear per-frame instance data after we've used it ------------
        foreach (var d in _drawables.Values) d.Clear();

        // --- Camera update ------------------------------------------------
        Camera.IgnoreKeyboardInput = WantsCaptureKeyboard;
        Camera.IgnoreMouseInput = WantsCaptureMouse;
        float aspect = Width > 0 ? (float)Width / Math.Max(1, Height) : 1f;
        Camera.Update(Keyboard, Mouse, aspect);
    }

    public void Close() => _window.Close();

    // ---------------------------------------------------------------------
    // Window lifecycle glue. Called by Program.cs to wire IWindow events.
    // ---------------------------------------------------------------------
    public void Open(CreationSettings settings) => Open((WindowOptions)settings);

    public void Open(WindowOptions options)
    {
        _window.Load += OnWindowLoad;
        _window.Render += OnWindowRender;
        _window.Closing += OnWindowClosing;
        _window.Run();
    }

    private void OnWindowLoad()
    {
        Load();
    }

    private void OnWindowRender(double dt)
    {
        _time += dt;
        Draw();
        Keyboard?.SwapStates();
        Mouse?.SwapStates();
    }

    private void OnWindowClosing()
    {
        _imGui?.Dispose();
        _input?.Dispose();
        Gl?.Dispose();
    }
}
