using SimplexLab.Fixed.Physics.Collision.Shapes;
using SimplexLab.Fixed.Physics.Dynamics.Constraints;
using SimplexLab.Fixed.Physics.LinearMath;

namespace SimplexLab.Fixed.Physics.JDemo;

public class Demo13 : IDemo
{
    public string Name => "Motor and Limit";
    public string Description => "Hinge joints with angular motors, angular limits, and coupled rotating wheels.";

    public void Build(Playground pg, World world)
    {
        pg.AddFloor();

        {
            // Motor
            var b0 = world.CreateRigidBody();
            b0.AddShape(new BoxShape((Real)2, (Real)0.1, (Real)0.1));
            b0.Position = new JVector((Real)(-1.1), (Real)4, 0);

            var b1 = world.CreateRigidBody();
            b1.AddShape(new BoxShape((Real)2, (Real)0.1, (Real)0.1));
            b1.Position = new JVector((Real)1.1, (Real)4, 0);

            HingeJoint hj = new HingeJoint(world, world.NullBody, b0, b0.Position, JVector.UnitX, AngularLimit.Full, true);
            UniversalJoint uj = new UniversalJoint(world, b0, b1, new JVector(0, (Real)4, 0), JVector.UnitX, JVector.UnitX);

            if (hj.Motor != null)
            {
                hj.Motor.TargetVelocity = (Real)4;
                hj.Motor.MaximumForce = (Real)1;
            }

            if (world.BroadPhaseFilter is not Common.IgnoreCollisionBetweenFilter filter)
            {
                filter = new Common.IgnoreCollisionBetweenFilter();
                world.BroadPhaseFilter = filter;
            }

            filter.IgnoreCollisionBetween(b0.Shapes[0], b1.Shapes[0]);
        }

        {
            // Hinge Joint with -120 <-> + 120 Limit
            var b0 = world.CreateRigidBody();
            b0.AddShape(new BoxShape((Real)2, (Real)0.1, (Real)3));
            b0.AddShape(new BoxShape((Real)0.1, (Real)2, (Real)2.9));
            b0.Position = new JVector((Real)(-5), (Real)3, 0);

            HingeJoint hj = new HingeJoint(world, world.NullBody, b0, b0.Position, JVector.UnitZ, AngularLimit.FromDegree(-120, 120));
            hj.HingeAngle.Bias = (Real)0.3;
            hj.HingeAngle.Softness = Real.Zero;
            hj.HingeAngle.LimitBias = (Real)0.3;
            hj.HingeAngle.LimitSoftness = Real.Zero;

            for (int i = 0; i < 4; i++)
                Common.BuildRagdoll(new JVector((Real)(-4), (Real)(5 + i * 3), 0));
        }

        {
            Real angle = (Real)JAngle.FromDegree(90);
            JVector rot1Axis = JVector.Transform(JVector.UnitZ, JQuaternion.CreateRotationY(angle));

            // two free rotating wheels
            var b0 = world.CreateRigidBody();
            b0.Position = new JVector((Real)5, (Real)4, 0);
            b0.Orientation = JQuaternion.CreateRotationX(Real.Half_PI);
            b0.AddShape(new CylinderShape((Real)0.4, (Real)2.0));

            var b1 = world.CreateRigidBody();
            b1.AddShape(new CylinderShape((Real)0.4, (Real)2.0));
            b1.Position = new JVector((Real)9.2, (Real)4, 0);
            b1.Orientation = JQuaternion.CreateRotationY(angle) * JQuaternion.CreateRotationX(Real.Half_PI);

            HingeJoint hj1 = new HingeJoint(world, world.NullBody, b0, b0.Position, JVector.UnitZ, AngularLimit.Full);
            HingeJoint hj2 = new HingeJoint(world, world.NullBody, b1, b1.Position, rot1Axis, AngularLimit.Full);
            hj1.HingeAngle.Softness = Real.Zero;
            hj2.HingeAngle.Softness = Real.Zero;

            // constraint them to have the same rotation
            var relative = world.CreateConstraint<TwistAngle>(b0, b1);
            relative.Initialize(JVector.UnitZ, rot1Axis);
        }

        world.SolverIterations = (4, 2);
        world.SubstepCount = 3;
    }
}
