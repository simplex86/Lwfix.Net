using System.Numerics;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.VelcroDemo.Renderer;

using PhysicsVector2 = SimplexLab.LwfixPhysics.Velcro.Primitives.Vector2; // engine Fixed32 vector (alias to disambiguate from System.Numerics.Vector2)

/// <summary>
/// 2D camera operating in simulation units. Provides the orthographic projection
/// used by the primitive batch and converts between screen pixels and simulation
/// coordinates (for mouse picking). Mirrors the behaviour of the original
/// MonoGame sample's Camera2D.
/// </summary>
public class Camera2D
{
    private int _screenWidth;
    private int _screenHeight;

    /// <summary>Conversion ratio: how many pixels represent one simulation unit.</summary>
    public const float PixelsPerSimUnit = 40f;

    /// <summary>Simulation-space position the camera is centered on.</summary>
    public PhysicsVector2 Position { get; set; } = PhysicsVector2.Zero;

    /// <summary>Zoom factor (larger = more zoomed in).</summary>
    public Fixed32 Zoom { get; set; } = (Fixed32)1.0;

    public Camera2D(int screenWidth, int screenHeight)
    {
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public int Width => _screenWidth;
    public int Height => _screenHeight;

    /// <summary>Optional body the camera follows each frame (used by the racing demo).</summary>
    public SimplexLab.LwfixPhysics.Velcro.Dynamics.Body? TrackingBody { get; set; }

    /// <summary>Call once per frame to follow <see cref="TrackingBody"/> if set.</summary>
    public void UpdateTracking()
    {
        if (TrackingBody != null)
            Position = TrackingBody.Position;
    }

    public void Resize(int width, int height)
    {
        _screenWidth = width;
        _screenHeight = height;
    }

    public void ResetCamera()
    {
        Position = PhysicsVector2.Zero;
        Zoom = (Fixed32)1.0;
    }

    public void MoveCamera(PhysicsVector2 delta) => Position += delta;

    /// <summary>Combined orthographic projection * view, ready for the primitive batch shader.</summary>
    public Matrix4x4 SimProjectionView
    {
        get
        {
            float z = (float)Zoom;
            float ppu = z * PixelsPerSimUnit;
            float halfW = _screenWidth * 0.5f / ppu;
            float halfH = _screenHeight * 0.5f / ppu;
            // Orthographic with the camera position at the center.
            float left = (float)Position.X - halfW;
            float right = (float)Position.X + halfW;
            float bottom = (float)Position.Y - halfH;
            float top = (float)Position.Y + halfH;
            return Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, -1f, 1f);
        }
    }

    /// <summary>Convert a screen pixel coordinate to simulation units (for mouse picking).</summary>
    public PhysicsVector2 ConvertScreenToWorld(float screenX, float screenY)
    {
        float z = (float)Zoom;
        float ppu = z * PixelsPerSimUnit;
        float simX = (screenX - _screenWidth * 0.5f) / ppu + (float)Position.X;
        float simY = (screenY - _screenHeight * 0.5f) / ppu + (float)Position.Y;
        return new PhysicsVector2((Fixed32)simX, (Fixed32)simY);
    }

    /// <summary>Convert simulation units to screen pixels (for HUD anchoring).</summary>
    public (float x, float y) ConvertWorldToScreen(PhysicsVector2 sim)
    {
        float z = (float)Zoom;
        float ppu = z * PixelsPerSimUnit;
        float x = (float)(sim.X - Position.X) * ppu + _screenWidth * 0.5f;
        float y = (float)(sim.Y - Position.Y) * ppu + _screenHeight * 0.5f;
        return (x, y);
    }
}
