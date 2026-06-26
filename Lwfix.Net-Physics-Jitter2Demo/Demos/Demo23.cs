using System;
using SimplexLab.Fixed.Physics.Collision.Shapes;
using SimplexLab.Fixed.Physics.Dynamics;
using SimplexLab.Fixed.Physics.LinearMath;

namespace SimplexLab.Fixed.Physics.JDemo;

public class Demo23 : IDemo, IDrawUpdate
{
    public string Name => "Rotating Cube";
    public string Description => "Bodies inside a large kinematic rotating hollow cube.";

    private RigidBody rotatingBox = null!;

    public void Build(Playground pg, World world)
    {
        rotatingBox = world.CreateRigidBody();

        float size = 50;

        var bs0 = new TransformedShape(new BoxShape((Real)size, (Real)1, (Real)size), new JVector(0, (Real)(+size / 2), 0));
        var bs1 = new TransformedShape(new BoxShape((Real)size, (Real)1, (Real)size), new JVector(0, (Real)(-size / 2), 0));

        var bs2 = new TransformedShape(new BoxShape((Real)1, (Real)size, (Real)size), new JVector((Real)(+size / 2), 0, 0));
        var bs3 = new TransformedShape(new BoxShape((Real)1, (Real)size, (Real)size), new JVector((Real)(-size / 2), 0, 0));

        var bs4 = new TransformedShape(new BoxShape((Real)size, (Real)size, (Real)1), new JVector(0, 0, (Real)(+size / 2)));
        var bs5 = new TransformedShape(new BoxShape((Real)size, (Real)size, (Real)1), new JVector(0, 0, (Real)(-size / 2)));

        rotatingBox.AddShapes([bs0, bs1, bs2, bs3, bs4, bs5]);
        rotatingBox.Tag = new RigidBodyTag(true);

        rotatingBox.MotionType = MotionType.Kinematic;

        rotatingBox.DeactivationTime = TimeSpan.MaxValue;
        rotatingBox.SetActivationState(true);

        for (int i = -10; i < 10; i++)
        {
            for (int e = -10; e < 10; e++)
            {
                for (int j = -10; j < 10; j++)
                {
                    RigidBody rb = world.CreateRigidBody();
                    rb.AddShape(new BoxShape((Real)1.5));
                    rb.Position = new JVector((Real)i, (Real)e, (Real)j) * (Real)2;
                }
            }
        }
    }

    public void DrawUpdate()
    {
        rotatingBox.AngularVelocity = new JVector((Real)0.14, (Real)0.02, (Real)0.03);
    }
}
