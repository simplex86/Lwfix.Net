namespace LwfixTest.Physics.Jitter2.Constraints;

public class ConstraintBehaviorTests
{
    [Fact]
    public void BallSocket_Constraint_CreatesAndInitializes()
    {
        using World world = new World();

        RigidBody body1 = world.CreateRigidBody();
        RigidBody body2 = world.CreateRigidBody();

        BallSocket constraint = world.CreateConstraint<BallSocket>(body1, body2);
        constraint.Initialize(JVector.Zero);

        Assert.Equal(body1, constraint.Body1);
        Assert.Equal(body2, constraint.Body2);
        Assert.Equal(JVector.Zero, constraint.Impulse);
    }

    [Fact]
    public void DistanceLimit_Constraint_CreatesAndInitializes()
    {
        using World world = new World();

        RigidBody body1 = world.CreateRigidBody();
        RigidBody body2 = world.CreateRigidBody();

        DistanceLimit constraint = world.CreateConstraint<DistanceLimit>(body1, body2);
        constraint.Initialize(new JVector(0, 0, 0), new JVector(1, 0, 0));

        Assert.True(constraint.TargetDistance > (Real)0);
    }

    [Fact]
    public void FixedAngle_Constraint_CreatesAndInitializes()
    {
        using World world = new World();

        RigidBody body1 = world.CreateRigidBody();
        RigidBody body2 = world.CreateRigidBody();

        FixedAngle constraint = world.CreateConstraint<FixedAngle>(body1, body2);
        constraint.Initialize();

        Assert.Equal(body1, constraint.Body1);
    }

    [Fact]
    public void CreateConstraint_ForeignBody_Throws()
    {
        using World world1 = new World();
        using World world2 = new World();

        RigidBody body1 = world1.CreateRigidBody();
        RigidBody body2 = world2.CreateRigidBody();

        Assert.Throws<ArgumentException>(() =>
            world1.CreateConstraint<BallSocket>(body2, body1));
    }

    [Fact]
    public void RemoveConstraint_DecreasesConstraintCount()
    {
        using World world = new World();

        RigidBody body1 = world.CreateRigidBody();
        RigidBody body2 = world.CreateRigidBody();

        BallSocket constraint = world.CreateConstraint<BallSocket>(body1, body2);

        Assert.Equal(1, body1.Constraints.Count);

        world.Remove(constraint);

        Assert.Equal(0, body1.Constraints.Count);
    }
}
