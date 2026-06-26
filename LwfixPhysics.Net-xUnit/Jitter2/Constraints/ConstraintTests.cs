using SimplexLab.LwfixPhysics.Jitter2.SoftBodies;

namespace SimplexLab.LwfixPhysics.Jitter2.Test.Constraints;

public class ConstraintTests
{
    [Fact]
    public void TestSizes()
    {
        using World world = new World();

        RigidBody b0 = world.CreateRigidBody();
        RigidBody b1 = world.CreateRigidBody();

        world.Remove(world.CreateConstraint<AngularMotor>(b0, b1));
        world.Remove(world.CreateConstraint<BallSocket>(b0, b1));
        world.Remove(world.CreateConstraint<ConeLimit>(b0, b1));
        world.Remove(world.CreateConstraint<DistanceLimit>(b0, b1));
        world.Remove(world.CreateConstraint<FixedAngle>(b0, b1));
        world.Remove(world.CreateConstraint<HingeAngle>(b0, b1));
        world.Remove(world.CreateConstraint<LinearMotor>(b0, b1));
        world.Remove(world.CreateConstraint<PointOnLine>(b0, b1));
        world.Remove(world.CreateConstraint<PointOnPlane>(b0, b1));
        world.Remove(world.CreateConstraint<TwistAngle>(b0, b1));
        world.Remove(world.CreateConstraint<SpringConstraint>(b0, b1));
    }
}
