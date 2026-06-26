using SimplexLab.LwfixPhysics.Jitter2.LinearMath;

namespace SimplexLab.LwfixPhysics.Jitter2Demo;

public class Demo02 : IDemo
{
    public string Name => "Tower of Jitter";
    public string Description => "A single tower of stacked bodies to test solver stability.";

    public void Build(Playground pg, World world)
    {
        pg.AddFloor();

        Common.BuildTower(JVector.Zero);

        world.SolverIterations = (12, 4);
    }
}
