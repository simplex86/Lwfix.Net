using System.Collections.Generic;
using Silk.NET.Input;

namespace SimplexLab.Lwfix.Physics.JDemo.Renderer;

/// <summary>
/// Keyboard wrapper around Silk.NET.Input.IKeyboard with edge detection
/// (KeyPressBegin/KeyPressEnded) and a per-frame char buffer.
/// </summary>
public sealed class Keyboard
{
    public enum Key
    {
        Unknown, Space, Apostrophe, Comma, Minus, Period, Slash,
        D0, D1, D2, D3, D4, D5, D6, D7, D8, D9,
        Semicolon, Equal,
        A, B, C, D, E, F, G, H, I, J, K, L, M,
        N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        LeftBracket, BackSlash, RightBracket, GraveAccent,
        World1, World2,
        Escape, Enter, Tab, Backspace, Insert, Delete,
        Right, Left, Down, Up,
        PageUp, PageDown, Home, End,
        CapsLock, ScrollLock, NumLock, PrintScreen, Pause,
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
        F13, F14, F15, F16, F17, F18, F19, F20, F21, F22, F23, F24, F25,
        Kp0, Kp1, Kp2, Kp3, Kp4, Kp5, Kp6, Kp7, Kp8, Kp9,
        KpDecimal, KpDivide, KpMultiply, KpSubtract, KpAdd, KpEnter, KpEqual,
        LeftShift, LeftControl, LeftAlt, LeftSuper,
        RightShift, RightControl, RightAlt, RightSuper, Menu
    }

    private readonly HashSet<Key> _current = new();
    private readonly HashSet<Key> _last = new();
    private readonly List<uint> _chars = new();

    public static Keyboard Instance { get; private set; } = null!;

    private readonly IKeyboard _kb;

    public Keyboard(IKeyboard keyboard)
    {
        _kb = keyboard;
        _kb.KeyChar += OnChar;
        _kb.KeyDown += OnKeyDown;
        _kb.KeyUp += OnKeyUp;
        Instance = this;
    }

    private void OnChar(IKeyboard kb, char c)
    {
        _chars.Add((uint)c);
    }

    private void OnKeyDown(IKeyboard kb, Silk.NET.Input.Key key, int arg)
    {
        Key? mapped = MapKey(key);
        if (mapped.HasValue) _current.Add(mapped.Value);
    }

    private void OnKeyUp(IKeyboard kb, Silk.NET.Input.Key key, int arg)
    {
        Key? mapped = MapKey(key);
        if (mapped.HasValue) _current.Remove(mapped.Value);
    }

    public IEnumerable<uint> CharInput => _chars;

    public void SwapStates()
    {
        _last.Clear();
        foreach (var k in _current) _last.Add(k);
        _chars.Clear();
    }

    public bool KeyPressBegin(Key k) => _current.Contains(k) && !_last.Contains(k);
    public bool KeyPressEnded(Key k) => !_current.Contains(k) && _last.Contains(k);
    public bool IsKeyDown(Key k) => _current.Contains(k);

    private static Key? MapKey(Silk.NET.Input.Key key)
    {
        return key switch
        {
            Silk.NET.Input.Key.Space => Key.Space,
            Silk.NET.Input.Key.Apostrophe => Key.Apostrophe,
            Silk.NET.Input.Key.Comma => Key.Comma,
            Silk.NET.Input.Key.Minus => Key.Minus,
            Silk.NET.Input.Key.Period => Key.Period,
            Silk.NET.Input.Key.Slash => Key.Slash,
            Silk.NET.Input.Key.Number0 => Key.D0,
            Silk.NET.Input.Key.Number1 => Key.D1,
            Silk.NET.Input.Key.Number2 => Key.D2,
            Silk.NET.Input.Key.Number3 => Key.D3,
            Silk.NET.Input.Key.Number4 => Key.D4,
            Silk.NET.Input.Key.Number5 => Key.D5,
            Silk.NET.Input.Key.Number6 => Key.D6,
            Silk.NET.Input.Key.Number7 => Key.D7,
            Silk.NET.Input.Key.Number8 => Key.D8,
            Silk.NET.Input.Key.Number9 => Key.D9,
            Silk.NET.Input.Key.Semicolon => Key.Semicolon,
            Silk.NET.Input.Key.Equal => Key.Equal,
            Silk.NET.Input.Key.A => Key.A,
            Silk.NET.Input.Key.B => Key.B,
            Silk.NET.Input.Key.C => Key.C,
            Silk.NET.Input.Key.D => Key.D,
            Silk.NET.Input.Key.E => Key.E,
            Silk.NET.Input.Key.F => Key.F,
            Silk.NET.Input.Key.G => Key.G,
            Silk.NET.Input.Key.H => Key.H,
            Silk.NET.Input.Key.I => Key.I,
            Silk.NET.Input.Key.J => Key.J,
            Silk.NET.Input.Key.K => Key.K,
            Silk.NET.Input.Key.L => Key.L,
            Silk.NET.Input.Key.M => Key.M,
            Silk.NET.Input.Key.N => Key.N,
            Silk.NET.Input.Key.O => Key.O,
            Silk.NET.Input.Key.P => Key.P,
            Silk.NET.Input.Key.Q => Key.Q,
            Silk.NET.Input.Key.R => Key.R,
            Silk.NET.Input.Key.S => Key.S,
            Silk.NET.Input.Key.T => Key.T,
            Silk.NET.Input.Key.U => Key.U,
            Silk.NET.Input.Key.V => Key.V,
            Silk.NET.Input.Key.W => Key.W,
            Silk.NET.Input.Key.X => Key.X,
            Silk.NET.Input.Key.Y => Key.Y,
            Silk.NET.Input.Key.Z => Key.Z,
            Silk.NET.Input.Key.LeftBracket => Key.LeftBracket,
            Silk.NET.Input.Key.BackSlash => Key.BackSlash,
            Silk.NET.Input.Key.RightBracket => Key.RightBracket,
            Silk.NET.Input.Key.GraveAccent => Key.GraveAccent,
            Silk.NET.Input.Key.World1 => Key.World1,
            Silk.NET.Input.Key.World2 => Key.World2,
            Silk.NET.Input.Key.Escape => Key.Escape,
            Silk.NET.Input.Key.Enter => Key.Enter,
            Silk.NET.Input.Key.Tab => Key.Tab,
            Silk.NET.Input.Key.Backspace => Key.Backspace,
            Silk.NET.Input.Key.Insert => Key.Insert,
            Silk.NET.Input.Key.Delete => Key.Delete,
            Silk.NET.Input.Key.Right => Key.Right,
            Silk.NET.Input.Key.Left => Key.Left,
            Silk.NET.Input.Key.Down => Key.Down,
            Silk.NET.Input.Key.Up => Key.Up,
            Silk.NET.Input.Key.PageUp => Key.PageUp,
            Silk.NET.Input.Key.PageDown => Key.PageDown,
            Silk.NET.Input.Key.Home => Key.Home,
            Silk.NET.Input.Key.End => Key.End,
            Silk.NET.Input.Key.CapsLock => Key.CapsLock,
            Silk.NET.Input.Key.ScrollLock => Key.ScrollLock,
            Silk.NET.Input.Key.NumLock => Key.NumLock,
            Silk.NET.Input.Key.PrintScreen => Key.PrintScreen,
            Silk.NET.Input.Key.Pause => Key.Pause,
            Silk.NET.Input.Key.F1 => Key.F1,
            Silk.NET.Input.Key.F2 => Key.F2,
            Silk.NET.Input.Key.F3 => Key.F3,
            Silk.NET.Input.Key.F4 => Key.F4,
            Silk.NET.Input.Key.F5 => Key.F5,
            Silk.NET.Input.Key.F6 => Key.F6,
            Silk.NET.Input.Key.F7 => Key.F7,
            Silk.NET.Input.Key.F8 => Key.F8,
            Silk.NET.Input.Key.F9 => Key.F9,
            Silk.NET.Input.Key.F10 => Key.F10,
            Silk.NET.Input.Key.F11 => Key.F11,
            Silk.NET.Input.Key.F12 => Key.F12,
            Silk.NET.Input.Key.F13 => Key.F13,
            Silk.NET.Input.Key.F14 => Key.F14,
            Silk.NET.Input.Key.F15 => Key.F15,
            Silk.NET.Input.Key.F16 => Key.F16,
            Silk.NET.Input.Key.F17 => Key.F17,
            Silk.NET.Input.Key.F18 => Key.F18,
            Silk.NET.Input.Key.F19 => Key.F19,
            Silk.NET.Input.Key.F20 => Key.F20,
            Silk.NET.Input.Key.F21 => Key.F21,
            Silk.NET.Input.Key.F22 => Key.F22,
            Silk.NET.Input.Key.F23 => Key.F23,
            Silk.NET.Input.Key.F24 => Key.F24,
            Silk.NET.Input.Key.F25 => Key.F25,
            Silk.NET.Input.Key.Keypad0 => Key.Kp0,
            Silk.NET.Input.Key.Keypad1 => Key.Kp1,
            Silk.NET.Input.Key.Keypad2 => Key.Kp2,
            Silk.NET.Input.Key.Keypad3 => Key.Kp3,
            Silk.NET.Input.Key.Keypad4 => Key.Kp4,
            Silk.NET.Input.Key.Keypad5 => Key.Kp5,
            Silk.NET.Input.Key.Keypad6 => Key.Kp6,
            Silk.NET.Input.Key.Keypad7 => Key.Kp7,
            Silk.NET.Input.Key.Keypad8 => Key.Kp8,
            Silk.NET.Input.Key.Keypad9 => Key.Kp9,
            Silk.NET.Input.Key.KeypadDecimal => Key.KpDecimal,
            Silk.NET.Input.Key.KeypadDivide => Key.KpDivide,
            Silk.NET.Input.Key.KeypadMultiply => Key.KpMultiply,
            Silk.NET.Input.Key.KeypadSubtract => Key.KpSubtract,
            Silk.NET.Input.Key.KeypadAdd => Key.KpAdd,
            Silk.NET.Input.Key.KeypadEnter => Key.KpEnter,
            Silk.NET.Input.Key.KeypadEqual => Key.KpEqual,
            Silk.NET.Input.Key.ShiftLeft => Key.LeftShift,
            Silk.NET.Input.Key.ControlLeft => Key.LeftControl,
            Silk.NET.Input.Key.AltLeft => Key.LeftAlt,
            Silk.NET.Input.Key.SuperLeft => Key.LeftSuper,
            Silk.NET.Input.Key.ShiftRight => Key.RightShift,
            Silk.NET.Input.Key.ControlRight => Key.RightControl,
            Silk.NET.Input.Key.AltRight => Key.RightAlt,
            Silk.NET.Input.Key.SuperRight => Key.RightSuper,
            Silk.NET.Input.Key.Menu => Key.Menu,
            _ => null
        };
    }
}

/// <summary>
/// Mouse wrapper around Silk.NET.Input.IMouse with edge detection
/// (ButtonPressBegin/ButtonPressEnd), position delta and scroll wheel delta.
/// </summary>
public sealed class Mouse
{
    public enum Button { Left, Right, Middle, B4, B5, B6, B7, B8 }

    public struct Coordinate
    {
        public double X;
        public double Y;

        public Coordinate(double x, double y) { X = x; Y = y; }
        public void SetZero() { X = Y = 0; }
    }

    private readonly HashSet<Button> _current = new();
    private readonly HashSet<Button> _last = new();

    private Coordinate _currentPos;
    private Coordinate _lastPos;
    private Coordinate _scrollWheel;

    public Coordinate Position => _currentPos;
    public Coordinate ScrollWheel => _scrollWheel;
    public Coordinate DeltaPosition => new(_currentPos.X - _lastPos.X, _currentPos.Y - _lastPos.Y);

    public static Mouse Instance { get; private set; } = null!;

    private readonly IMouse _mouse;

    public Mouse(IMouse mouse)
    {
        _mouse = mouse;
        _mouse.MouseDown += OnMouseDown;
        _mouse.MouseUp += OnMouseUp;
        _mouse.MouseMove += OnMouseMove;
        _mouse.Scroll += OnMouseScroll;
        Instance = this;
    }

    private void OnMouseDown(IMouse m, Silk.NET.Input.MouseButton button)
    {
        if (MapButton(button) is { } b) _current.Add(b);
    }

    private void OnMouseUp(IMouse m, Silk.NET.Input.MouseButton button)
    {
        if (MapButton(button) is { } b) _current.Remove(b);
    }

    private void OnMouseMove(IMouse m, System.Numerics.Vector2 pos)
    {
        _currentPos = new Coordinate(pos.X, pos.Y);
    }

    private void OnMouseScroll(IMouse m, ScrollWheel sw)
    {
        _scrollWheel.X = sw.X;
        _scrollWheel.Y = sw.Y;
    }

    public bool ButtonPressBegin(Button b) => _current.Contains(b) && !_last.Contains(b);
    public bool ButtonPressEnd(Button b) => !_current.Contains(b) && _last.Contains(b);
    public bool IsButtonDown(Button b) => _current.Contains(b);

    public void SwapStates()
    {
        _last.Clear();
        foreach (var b in _current) _last.Add(b);
        _lastPos = _currentPos;
        _scrollWheel.SetZero();
    }

    private static Button? MapButton(Silk.NET.Input.MouseButton button)
    {
        return button switch
        {
            Silk.NET.Input.MouseButton.Left => Button.Left,
            Silk.NET.Input.MouseButton.Right => Button.Right,
            Silk.NET.Input.MouseButton.Middle => Button.Middle,
            Silk.NET.Input.MouseButton.Button4 => Button.B4,
            Silk.NET.Input.MouseButton.Button5 => Button.B5,
            Silk.NET.Input.MouseButton.Button6 => Button.B6,
            Silk.NET.Input.MouseButton.Button7 => Button.B7,
            Silk.NET.Input.MouseButton.Button8 => Button.B8,
            _ => null
        };
    }
}
