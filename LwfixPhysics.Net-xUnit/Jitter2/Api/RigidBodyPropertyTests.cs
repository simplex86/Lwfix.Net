namespace SimplexLab.LwfixPhysics.Jitter2.Test.Api;

/// <summary>
/// Tests for RigidBody property getters and setters in isolation.
/// </summary>
public class RigidBodyPropertyTests
{
    // -------------------------------------------------------------------------
    // Position
    // -------------------------------------------------------------------------

    [Fact]
    public void Position_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var expected = new JVector(1, 2, 3);
        body.Position = expected;
        Assert.Equal(expected, body.Position);
        world.Dispose();
    }

    [Fact]
    public void Position_SetToZero()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.Position = new JVector(5, 5, 5);
        body.Position = JVector.Zero;
        Assert.Equal(JVector.Zero, body.Position);
        world.Dispose();
    }

    [Fact]
    public void Position_SetNegative()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var expected = new JVector(-10, -20, -30);
        body.Position = expected;
        Assert.Equal(expected, body.Position);
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Orientation
    // -------------------------------------------------------------------------

    [Fact]
    public void Orientation_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var expected = JQuaternion.CreateFromAxisAngle(JVector.UnitY, Real.PI / 4);
        body.Orientation = expected;
        Assert.Equal(expected.X.ToDouble(), body.Orientation.X.ToDouble(), 1e-5);
        Assert.Equal(expected.Y.ToDouble(), body.Orientation.Y.ToDouble(), 1e-5);
        Assert.Equal(expected.Z.ToDouble(), body.Orientation.Z.ToDouble(), 1e-5);
        Assert.Equal(expected.W.ToDouble(), body.Orientation.W.ToDouble(), 1e-5);
        world.Dispose();
    }

    [Fact]
    public void Orientation_SetToIdentity()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.Orientation = JQuaternion.CreateFromAxisAngle(JVector.UnitX, Real.PI / 2);
        body.Orientation = JQuaternion.Identity;
        Assert.Equal(JQuaternion.Identity, body.Orientation);
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Velocity
    // -------------------------------------------------------------------------

    [Fact]
    public void Velocity_RoundTrip_Dynamic()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var expected = new JVector(1, 2, 3);
        body.Velocity = expected;
        Assert.Equal(expected, body.Velocity);
        world.Dispose();
    }

    [Fact]
    public void Velocity_SetToZero_IsZero()
    {
        // Regression: velocity setter must store zero even when value is JVector.Zero.
        var world = new World();
        var body = world.CreateRigidBody();
        body.Velocity = new JVector(5, 0, 0);
        body.Velocity = JVector.Zero;
        Assert.Equal(JVector.Zero, body.Velocity);
        world.Dispose();
    }

    [Fact]
    public void Velocity_SetNegative()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var expected = new JVector(-3, -2, -1);
        body.Velocity = expected;
        Assert.Equal(expected, body.Velocity);
        world.Dispose();
    }

    [Fact]
    public void Velocity_OnKinematic_Allowed()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.MotionType = MotionType.Kinematic;
        var expected = new JVector(1, 0, 0);
        body.Velocity = expected;
        Assert.Equal(expected, body.Velocity);
        world.Dispose();
    }

    [Fact]
    public void Velocity_OnStatic_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.MotionType = MotionType.Static;
        Assert.Throws<InvalidOperationException>(() => body.Velocity = new JVector(1, 0, 0));
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // AngularVelocity
    // -------------------------------------------------------------------------

    [Fact]
    public void AngularVelocity_RoundTrip_Dynamic()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var expected = new JVector(0, Real.PI, 0);
        body.AngularVelocity = expected;
        Assert.Equal(expected, body.AngularVelocity);
        world.Dispose();
    }

    [Fact]
    public void AngularVelocity_SetToZero_IsZero()
    {
        // Mirrors the velocity-zero regression for angular velocity.
        var world = new World();
        var body = world.CreateRigidBody();
        body.AngularVelocity = new JVector(0, 1, 0);
        body.AngularVelocity = JVector.Zero;
        Assert.Equal(JVector.Zero, body.AngularVelocity);
        world.Dispose();
    }

    [Fact]
    public void AngularVelocity_OnStatic_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.MotionType = MotionType.Static;
        Assert.Throws<InvalidOperationException>(() => body.AngularVelocity = new JVector(0, 1, 0));
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Force / Torque
    // -------------------------------------------------------------------------

    [Fact]
    public void Force_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var expected = new JVector(10, 0, 0);
        body.Force = expected;
        Assert.Equal(expected, body.Force);
        world.Dispose();
    }

    [Fact]
    public void Force_SetToZero()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.Force = new JVector(10, 0, 0);
        body.Force = JVector.Zero;
        Assert.Equal(JVector.Zero, body.Force);
        world.Dispose();
    }

    [Fact]
    public void Torque_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var expected = new JVector(0, 5, 0);
        body.Torque = expected;
        Assert.Equal(expected, body.Torque);
        world.Dispose();
    }

    [Fact]
    public void Torque_SetToZero()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.Torque = new JVector(0, 5, 0);
        body.Torque = JVector.Zero;
        Assert.Equal(JVector.Zero, body.Torque);
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Material / misc
    // -------------------------------------------------------------------------

    [Fact]
    public void Friction_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.Friction = (Real)0.7;
        Assert.Equal(0.7, body.Friction.ToDouble(), 1e-6);
        world.Dispose();
    }

    [Fact]
    public void Friction_Negative_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentOutOfRangeException>(() => body.Friction = (Real)(-0.1));
        world.Dispose();
    }

    [Fact]
    public void Restitution_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.Restitution = (Real)0.5;
        Assert.Equal(0.5, body.Restitution.ToDouble(), 1e-6);
        world.Dispose();
    }

    [Fact]
    public void Restitution_Negative_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentOutOfRangeException>(() => body.Restitution = (Real)(-0.1));
        world.Dispose();
    }

    [Fact]
    public void Restitution_GreaterThanOne_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentOutOfRangeException>(() => body.Restitution = (Real)1.1);
        world.Dispose();
    }

    [Fact]
    public void AffectedByGravity_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AffectedByGravity = false;
        Assert.False(body.AffectedByGravity);
        body.AffectedByGravity = true;
        Assert.True(body.AffectedByGravity);
        world.Dispose();
    }

    [Fact]
    public void Tag_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var tag = new object();
        body.Tag = tag;
        Assert.Same(tag, body.Tag);
        world.Dispose();
    }

    [Fact]
    public void Tag_CanBeCleared()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.Tag = new object();
        body.Tag = null;
        Assert.Null(body.Tag);
        world.Dispose();
    }

    [Fact]
    public void Damping_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.Damping = ((Real)0.1, (Real)0.2);
        Assert.Equal(0.1, body.Damping.linear.ToDouble(), 1e-6);
        Assert.Equal(0.2, body.Damping.angular.ToDouble(), 1e-6);
        world.Dispose();
    }

    [Fact]
    public void Damping_LinearNegative_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentOutOfRangeException>(() => body.Damping = ((Real)(-0.1), (Real)0.2));
        world.Dispose();
    }

    [Fact]
    public void Damping_LinearGreaterThanOne_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentOutOfRangeException>(() => body.Damping = ((Real)1.1, (Real)0.2));
        world.Dispose();
    }

    [Fact]
    public void Damping_AngularNegative_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentOutOfRangeException>(() => body.Damping = ((Real)0.1, (Real)(-0.2)));
        world.Dispose();
    }

    [Fact]
    public void Damping_AngularGreaterThanOne_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentOutOfRangeException>(() => body.Damping = ((Real)0.1, (Real)1.2));
        world.Dispose();
    }

    [Fact]
    public void DeactivationThreshold_RoundTrip()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.DeactivationThreshold = ((Real)0.3, (Real)0.4);
        Assert.Equal(0.3, body.DeactivationThreshold.angular.ToDouble(), 1e-6);
        Assert.Equal(0.4, body.DeactivationThreshold.linear.ToDouble(), 1e-6);
        world.Dispose();
    }

    [Fact]
    public void DeactivationThreshold_AngularNegative_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentOutOfRangeException>(() => body.DeactivationThreshold = ((Real)(-0.3), (Real)0.4));
        world.Dispose();
    }

    [Fact]
    public void DeactivationThreshold_LinearNegative_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentOutOfRangeException>(() => body.DeactivationThreshold = ((Real)0.3, (Real)(-0.4)));
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Damping — simulation behavior
    // -------------------------------------------------------------------------

    [Fact]
    public void Damping_Linear_SlowsBodyOverTime()
    {
        var world = new World();
        world.AllowDeactivation = false;
        world.Gravity = JVector.Zero;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.Damping = ((Real)0.1, (Real)0.0);
        body.Velocity = new JVector(10, 0, 0);
        var speedBefore = body.Velocity.X;
        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);
        Assert.True(body.Velocity.X < speedBefore);
        world.Dispose();
    }

    [Fact]
    public void Damping_Angular_SlowsRotationOverTime()
    {
        var world = new World();
        world.AllowDeactivation = false;
        world.Gravity = JVector.Zero;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.Damping = ((Real)0.0, (Real)0.1);
        body.AngularVelocity = new JVector(0, 5, 0);
        var angSpeedBefore = body.AngularVelocity.Y;
        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);
        Assert.True(body.AngularVelocity.Y < angSpeedBefore);
        world.Dispose();
    }

    [Fact]
    public void Damping_Zero_DoesNotSlowBody()
    {
        var world = new World();
        world.AllowDeactivation = false;
        world.Gravity = JVector.Zero;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.Damping = ((Real)0.0, (Real)0.0);
        body.Velocity = new JVector(10, 0, 0);
        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);
        Assert.Equal(10.0, body.Velocity.X.ToDouble(), 1e-3);
        world.Dispose();
    }
}
