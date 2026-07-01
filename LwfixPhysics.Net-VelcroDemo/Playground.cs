using System.Numerics;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Extensions.DebugView;
using SimplexLab.LwfixPhysics.VelcroDemo.Renderer;
using SimplexLab.LwfixPhysics.VelcroDemo.Renderer.OpenGL;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.OpenGL.Extensions.ImGui;
using ImGuiNET;
using ImGui = ImGuiNET.ImGui;

namespace SimplexLab.LwfixPhysics.VelcroDemo;

/// <summary>
/// Host window for the Velcro demo suite. Owns the Silk.NET window, GL context,
/// ImGui overlay, 2D camera, input, primitive batch, and the active demo screen.
/// Mirrors the role of the original MonoGame sample's Game1 + ScreenManager.
/// </summary>
public sealed class Playground : IDisposable
{
    private readonly IWindow _window;
    private GL _gl = null!;
    private IInputContext _input = null!;
    private ImGuiController _imGui = null!;

    private PrimitiveBatch _batch = null!;
    private DebugView2D? _debugView;

    public Camera2D Camera { get; private set; }
    public InputHelper Input { get; private set; } = null!;

    private readonly List<DemoScreen> _demos = new();
    private int _currentIndex = -1;
    private DemoScreen? _current;

    // Fixed-timestep physics (1/60 s) with accumulator for stable fixed-point sim.
    private const float FixedDt = 1f / 60f;
    private double _accumulator;
    private float _fps;

    // UI state
    private bool _showDetails = true;
    private bool _showMenu = true;
    private int _selectedDebugFlags;

    public IReadOnlyList<DemoScreen> Demos => _demos;
    public int SelectedIndex => _currentIndex;
    public DemoScreen? Current => _current;

    public Playground(IWindow window)
    {
        _window = window;
        Camera = new Camera2D(window.Size.X, window.Size.Y);
    }

    public void RegisterDemo(DemoScreen demo) => _demos.Add(demo);

    public void SelectDemo(int index)
    {
        if (index < 0 || index >= _demos.Count) return;
        _currentIndex = index;
        _current = _demos[index];
        _current.Initialize(this);
        _current.Load();
        // Recreate the debug view bound to the new world.
        _debugView = new DebugView2D(_current.World, _batch);
        _selectedDebugFlags = (int)_debugView.Flags;
    }

    public void Open()
    {
        _window.Load += OnLoad;
        _window.Render += OnRender;
        _window.Closing += OnClosing;
        _window.Resize += OnResize;
        _window.Run();
    }

    private void OnResize(Silk.NET.Maths.Vector2D<int> size) => Camera.Resize(size.X, size.Y);

    private void OnLoad()
    {
        _gl = _window.CreateOpenGL();
        GlContext.Initialize(_gl);

        _input = _window.CreateInput();
        var kb = _input.Keyboards.Count > 0 ? _input.Keyboards[0] : null;
        var mouse = _input.Mice.Count > 0 ? _input.Mice[0] : null;
        Input = new InputHelper(kb!, mouse!);

        _imGui = new ImGuiController(_gl, _window, _input);
        _batch = new PrimitiveBatch();

        Camera = new Camera2D(_window.Size.X, _window.Size.Y);

        if (_demos.Count > 0) SelectDemo(0);
    }

    private void OnRender(double dt)
    {
        _fps = (float)(_fps * 0.9 + (1.0 / Math.Max(dt, 1e-6)) * 0.1);

        Input.Update();

        if (Input.IsNewKeyPress(Key.F1)) _showDetails = !_showDetails;
        if (Input.IsNewKeyPress(Key.F2) || Input.IsNewKeyPress(Key.Tab)) _showMenu = !_showMenu;

        if (_current != null)
        {
            _current.HandleInput((float)dt);
            _accumulator += dt;
            int steps = 0;
            while (_accumulator >= FixedDt && steps < 5)
            {
                _current.World.Step((Fixed32)FixedDt);
                _current.Update(FixedDt);
                _accumulator -= FixedDt;
                steps++;
            }
        }

        // --- Render ---
        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        _gl.Disable(EnableCap.DepthTest);
        _gl.ClearColor(0.16f, 0.17f, 0.21f, 1f);
        _gl.Clear((uint)ClearBufferMask.ColorBufferBit);
        _gl.Viewport(0, 0, (uint)_window.FramebufferSize.X, (uint)_window.FramebufferSize.Y);

        Camera.UpdateTracking();

        if (_debugView != null)
        {
            _debugView.Flags = (DebugViewFlags)_selectedDebugFlags;
            _debugView.RenderDebugData(Camera.SimProjectionView);
        }

        _current?.Draw();

        _imGui.Update((float)dt);
        DrawImGui();
        _imGui.Render();

        Input.EndFrame();
    }

    private void DrawImGui()
    {
        if (_showMenu)
        {
            ImGui.SetNextWindowPos(new Vector2(10, 10), ImGuiCond.Once);
            ImGui.SetNextWindowSize(new Vector2(310, 0), ImGuiCond.Once);
            ImGui.Begin("Velcro Demos", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse);

            for (int i = 0; i < _demos.Count; i++)
            {
                bool selected = i == _currentIndex;
                if (ImGui.Selectable($"{i + 1:D2}. {_demos[i].Name}", selected))
                    SelectDemo(i);
            }

            ImGui.Separator();
            ImGui.TextDisabled("Arrow keys: pan | PgUp/Dn: zoom | Home: reset");
            ImGui.TextDisabled("Left-drag: grab body | WASD/QE: move agent");
            ImGui.TextDisabled("F1: details  F2/Tab: toggle menu");
            ImGui.Separator();
            ImGui.Text("Debug flags");

            foreach (DebugViewFlags f in Enum.GetValues(typeof(DebugViewFlags)))
            {
                if (f == DebugViewFlags.None) continue;
                bool on = (_selectedDebugFlags & (int)f) != 0;
                if (ImGui.Checkbox(f.ToString(), ref on))
                    _selectedDebugFlags = on
                        ? _selectedDebugFlags | (int)f
                        : _selectedDebugFlags & ~(int)f;
            }

            ImGui.End();
        }

        if (_showDetails && _current != null)
        {
            float w = 400;
            ImGui.SetNextWindowPos(new Vector2(Math.Max(10, _window.Size.X - w - 10), 10), ImGuiCond.Once);
            ImGui.SetNextWindowSize(new Vector2(w, 0), ImGuiCond.Once);
            ImGui.Begin("Details", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse);

            ImGui.TextUnformatted(_current.Title);
            ImGui.Separator();
            ImGui.PushTextWrapPos();
            ImGui.TextUnformatted(_current.Details);
            if (!string.IsNullOrEmpty(_current.Controls))
            {
                ImGui.Separator();
                ImGui.TextUnformatted(_current.Controls);
            }
            ImGui.PopTextWrapPos();

            ImGui.Separator();
            ImGui.Text($"Bodies: {_current.World.BodyList.Count}   Joints: {_current.World.JointList.Count}");
            ImGui.Text($"Contacts: {_current.World.ContactManager.ContactCount}   FPS: {_fps:F1}");

            ImGui.End();
        }
    }

    private void OnClosing()
    {
        _imGui?.Dispose();
        _input?.Dispose();
        _batch?.Dispose();
        _gl?.Dispose();
    }

    public void Dispose() => OnClosing();
}
