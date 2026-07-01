using Silk.NET.Input;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Renderer;

/// <summary>
/// Aggregates keyboard + mouse state with previous-frame snapshots so demos can
/// query "newly pressed" events. Mirrors the original MonoGame sample's InputHelper.
/// </summary>
public sealed class InputHelper
{
    private readonly IKeyboard _keyboard;
    private readonly IMouse _mouse;

    private HashSet<Key> _curKeys = new();
    private HashSet<Key> _prevKeys = new();

    private bool _curLeft, _prevLeft;
    private bool _curRight, _prevRight;
    private bool _curMiddle, _prevMiddle;

    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public float MouseDeltaX { get; private set; }
    public float MouseDeltaY { get; private set; }
    public float ScrollWheel { get; private set; }

    public InputHelper(IKeyboard keyboard, IMouse mouse)
    {
        _keyboard = keyboard;
        _mouse = mouse;

        _keyboard.KeyChar += (_, _) => { };
        _mouse.MouseDown += (_, b) => SetButton(b, true);
        _mouse.MouseUp += (_, b) => SetButton(b, false);
        _mouse.MouseMove += (_, p) =>
        {
            MouseDeltaX = p.X - MouseX;
            MouseDeltaY = p.Y - MouseY;
            MouseX = p.X;
            MouseY = p.Y;
        };
        _mouse.Scroll += (_, s) => ScrollWheel += s.Y;
    }

    private void SetButton(MouseButton b, bool down)
    {
        switch (b)
        {
            case MouseButton.Left: _curLeft = down; break;
            case MouseButton.Right: _curRight = down; break;
            case MouseButton.Middle: _curMiddle = down; break;
        }
    }

    /// <summary>Called at the start of each frame to snapshot the live input state.</summary>
    public void Update()
    {
        _curKeys.Clear();
        // Only poll keys the backend actually supports; passing unsupported
        // enum values (e.g. -1) to IsKeyPressed throws GlfwException.
        foreach (Key k in _keyboard.SupportedKeys)
        {
            if (_keyboard.IsKeyPressed(k))
                _curKeys.Add(k);
        }
    }

    /// <summary>Called at the end of each frame to roll the previous-frame snapshot forward.</summary>
    public void EndFrame()
    {
        (_prevKeys, _curKeys) = (_curKeys, _prevKeys);
        _prevLeft = _curLeft;
        _prevRight = _curRight;
        _prevMiddle = _curMiddle;
        MouseDeltaX = 0;
        MouseDeltaY = 0;
    }

    public bool IsKeyDown(Key k) => _curKeys.Contains(k);
    public bool IsKeyUp(Key k) => !_curKeys.Contains(k);

    public bool IsNewKeyPress(Key k) => _curKeys.Contains(k) && !_prevKeys.Contains(k);

    public bool IsLeftButtonDown => _curLeft;
    public bool IsRightButtonDown => _curRight;
    public bool IsMiddleButtonDown => _curMiddle;

    public bool IsNewLeftMousePress => _curLeft && !_prevLeft;
    public bool IsNewLeftMouseRelease => !_curLeft && _prevLeft;
    public bool IsNewRightMousePress => _curRight && !_prevRight;
}
