using SimplexLab.Fixed.Physics.Collision.Shapes;
using SimplexLab.Fixed.Physics.LinearMath;

namespace SimplexLab.Fixed.Physics.JDemo;

public class Demo09 : IDemo
{
    public string Name => "Restitution and Friction";
    public string Description => "Varying restitution and friction values across rows of bodies.";

    public void Build(Playground pg, World world)
    {
        pg.AddFloor();

        world.SolverIterations = (20, 4);

        if (pg.FloorShape != null)
        {
            pg.FloorShape.RigidBody.Friction = Real.Zero;
            pg.FloorShape.RigidBody.Restitution = Real.Zero;
        }

        for (int i = 0; i < 11; i++)
        {
            var body = world.CreateRigidBody();
            body.AddShape(new BoxShape((Real)0.5));
            body.Position = new JVector((Real)(-10 + i * 1), (Real)4, (Real)(-10));
            body.Restitution = (Real)(i * 0.1);
            body.Damping = ((Real)0.001, (Real)0.001);
        }

        for (int i = 0; i < 11; i++)
        {
            var body = world.CreateRigidBody();
            body.AddShape(new BoxShape((Real)0.5));
            body.Position = new JVector((Real)(2 + i), (Real)0.25, 0);
            body.Friction = (Real)(1.0 - i * 0.1);
            body.Velocity = new JVector(0, 0, (Real)(-10));
            body.Damping = ((Real)0.001, (Real)0.001);
        }
    }
}
