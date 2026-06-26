using static SimplexLab.LwfixPhysics.Jitter2Demo.Common;
using SimplexLab.LwfixPhysics.Jitter2.LinearMath;

namespace SimplexLab.LwfixPhysics.Jitter2Demo;

public class Demo04 : IDemo
{
    public string Name => "Many Ragdolls";
    public string Description => "100 ragdolls dropping from increasing heights with collision filtering between limbs.";

    public void Build(Playground pg, World world)
    {
        pg.AddFloor();

        for (int i = 0; i < 100; i++)
        {
            BuildRagdoll(new JVector(0, (Real)(3 + 2 * i), 0));
        }

        world.SolverIterations = (8, 4);
    }
}
