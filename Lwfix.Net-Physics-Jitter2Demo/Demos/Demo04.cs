using static SimplexLab.Lwfix.Physics.JDemo.Common;
using SimplexLab.Lwfix.Physics.LinearMath;

namespace SimplexLab.Lwfix.Physics.JDemo;

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
