using SimplexLab.Lwfix.Physics.Collision.Shapes;
using SimplexLab.Lwfix.Physics.LinearMath;

namespace SimplexLab.Lwfix.Physics.JDemo;

public class Demo10 : IDemo
{
    public string Name => "Stacked Cubes";
    public string Description => "Tall stacks of cubes and cones using sub-stepping for stability.";

    public void Build(Playground pg, World world)
    {
        pg.AddFloor();

        for (int i = 0; i < 32; i++)
        {
            var body = world.CreateRigidBody();

            body.Position = new JVector(0, (Real)(0.5 + i * 0.999), 0);
            body.AddShape(new BoxShape((Real)1));
            body.Damping = ((Real)0.002, (Real)0.002);
        }

        for (int i = 0; i < 32; i++)
        {
            var body = world.CreateRigidBody();

            body.Position = new JVector((Real)10, (Real)(0.5 + i * 0.999), 0);
            body.AddShape(new TransformedShape(new ConeShape(), JVector.Zero, JMatrix.CreateScale((Real)0.4, (Real)1, (Real)1)));
            body.Damping = ((Real)0.002, (Real)0.002);
        }

        world.SolverIterations = (4, 2);
        world.SubstepCount = 3;
    }
}
