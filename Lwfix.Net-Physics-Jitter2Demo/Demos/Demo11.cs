using System;
using SimplexLab.Fixed.Physics.Collision.Shapes;
using SimplexLab.Fixed.Physics.Dynamics.Constraints;
using SimplexLab.Fixed.Physics.JDemo.Renderer;
using SimplexLab.Fixed.Physics.LinearMath;

namespace SimplexLab.Fixed.Physics.JDemo;

public class Demo11 : IDemo, IDrawUpdate
{
    public string Name => "Double Pendulum";

    private RigidBody b0 = null!, b1 = null!;

    private World world = null!;

    public void Build(Playground pg, World world)
    {
        this.world = world;

        pg.AddFloor();

        b0 = world.CreateRigidBody();
        b0.AddShape(new SphereShape((Real)0.2));
        b0.Position = new JVector(0, (Real)12, 0);
        b0.Velocity = new JVector((Real)0.01, 0, 0);
        b0.DeactivationTime = TimeSpan.MaxValue;

        b1 = world.CreateRigidBody();
        b1.AddShape(new SphereShape((Real)0.2));
        b1.Velocity = new JVector(0, 0, (Real)0.01);
        b1.Position = new JVector(0, (Real)13, 0);

        var c0 = world.CreateConstraint<DistanceLimit>(world.NullBody, b0);
        c0.Initialize(new JVector(0, (Real)8, 0), b0.Position);

        var c1 = world.CreateConstraint<DistanceLimit>(b0, b1);
        c1.Initialize(b0.Position, b1.Position);

        world.SubstepCount = 10;
        world.SolverIterations = (2, 2);

        b0.Damping = (Real.Zero, Real.Zero);
        b1.Damping = (Real.Zero, Real.Zero);
    }

    public void DrawUpdate()
    {
        float ekin = 0.5f * ((float)b0.Velocity.LengthSquared() + (float)b1.Velocity.LengthSquared());
        float epot = -(float)world.Gravity.Y * ((float)b0.Position.Y + (float)b1.Position.Y);

        Console.WriteLine($"Energy: {ekin + epot} Kinetic {ekin}; Potential {epot}");

        var dr = RenderWindow.Instance.DebugRenderer;
        dr.PushLine(DebugRenderer.Color.Green, Conversion.FromJitter(new JVector(0, (Real)8, 0)), Conversion.FromJitter(b0.Position));
        dr.PushLine(DebugRenderer.Color.White, Conversion.FromJitter(b0.Position), Conversion.FromJitter(b1.Position));
    }
}
