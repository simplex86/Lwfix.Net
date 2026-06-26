using System;

namespace SimplexLab.Fixed.Physics.JDemo.Renderer;

public class Camera
{
    public Matrix4 ViewMatrix { get; protected set; } = Matrix4.Identity;
    public Matrix4 ProjectionMatrix { get; protected set; } = Matrix4.Identity;

    public Vector3 Position { get; set; }
    public Vector3 Direction { get; protected set; }

    public float FieldOfView { get; set; } = MathF.PI / 4f;

    public double Theta { get; set; } = Math.PI / 2.0;
    public double Phi { get; set; }

    public float NearPlane { get; set; } = 0.1f;
    public float FarPlane { get; set; } = 400f;

    public bool IgnoreMouseInput { get; set; }
    public bool IgnoreKeyboardInput { get; set; }

    public virtual void Update(Keyboard keyboard, Mouse mouse, float aspect) { }
}

public sealed class FreeCamera : Camera
{
    public float MoveSpeed { get; set; } = 0.4f;
    public float MouseSensitivity { get; set; } = 0.006f;

    public override void Update(Keyboard keyboard, Mouse mouse, float aspect)
    {
        if (!IgnoreMouseInput && mouse.IsButtonDown(Mouse.Button.Right))
        {
            Phi -= mouse.DeltaPosition.X * MouseSensitivity;
            Theta += mouse.DeltaPosition.Y * MouseSensitivity;
        }

        // Keep the camera from flipping at the poles.
        if (Theta > Math.PI - 0.1) Theta = Math.PI - 0.1;
        if (Theta < 0.1) Theta = 0.1;

        Direction = new Vector3
        {
            Z = -(float)(Math.Sin(Theta) * Math.Cos(Phi)),
            X = -(float)(Math.Sin(Theta) * Math.Sin(Phi)),
            Y = (float)Math.Cos(Theta)
        };

        Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, Direction));
        Vector3 movement = Vector3.Zero;

        if (!IgnoreKeyboardInput && !keyboard.IsKeyDown(Keyboard.Key.LeftControl))
        {
            if (keyboard.IsKeyDown(Keyboard.Key.W)) movement += Direction;
            if (keyboard.IsKeyDown(Keyboard.Key.S)) movement -= Direction;
            if (keyboard.IsKeyDown(Keyboard.Key.A)) movement += right;
            if (keyboard.IsKeyDown(Keyboard.Key.D)) movement -= right;
        }

        if (movement.LengthSquared() > 0.1f) movement = Vector3.Normalize(movement);
        Position += MoveSpeed * movement;

        ViewMatrix = MatrixHelper.CreateLookAt(Position, Position + Direction, Vector3.UnitY);
        ProjectionMatrix = MatrixHelper.CreatePerspectiveFieldOfView(FieldOfView, aspect, NearPlane, FarPlane);
    }
}
