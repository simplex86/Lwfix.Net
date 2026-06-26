namespace SimplexLab.LwfixPhysics.Jitter2.Test.Behavior;

/// <summary>
/// Tests for AddForce and ApplyImpulse behavior.
/// </summary>
public class ForceImpulseTests
{
    // -------------------------------------------------------------------------
    // ApplyImpulse — immediate velocity change, no step required
    // -------------------------------------------------------------------------

    [Fact]
    public void ApplyImpulse_ChangesVelocityImmediately()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.ApplyImpulse(new JVector(1, 0, 0));
        Assert.True(body.Velocity.X > 0);
        world.Dispose();
    }

    [Fact]
    public void ApplyImpulse_ScaledByInverseMass()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        var impulse = new JVector(10, 0, 0);
        body.ApplyImpulse(impulse);
        var expectedVelocityX = impulse.X / body.Mass;
        Assert.Equal(expectedVelocityX.ToDouble(), body.Velocity.X.ToDouble(), 1e-4);
        world.Dispose();
    }

    [Fact]
    public void ApplyImpulse_WithPosition_AlsoChangesAngularVelocity()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        // Apply impulse off-center to produce angular velocity
        body.ApplyImpulse(new JVector(0, 1, 0), new JVector(1, 0, 0));
        Assert.True(body.AngularVelocity.LengthSquared() > 0);
        world.Dispose();
    }

    [Fact]
    public void ApplyImpulse_OnKinematic_HasNoEffect()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.MotionType = MotionType.Kinematic;
        body.ApplyImpulse(new JVector(1, 0, 0));
        Assert.Equal(JVector.Zero, body.Velocity);
        world.Dispose();
    }

    [Fact]
    public void ApplyImpulse_OnStatic_HasNoEffect()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.MotionType = MotionType.Static;
        body.ApplyImpulse(new JVector(1, 0, 0));
        // Static bodies have InverseMass == 0; velocity stays zero
        Assert.Equal(JVector.Zero, body.Velocity);
        world.Dispose();
    }

    [Fact]
    public void ApplyImpulse_ZeroImpulse_HasNoEffect()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.ApplyImpulse(JVector.Zero);
        Assert.Equal(JVector.Zero, body.Velocity);
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // AddForce — accumulated and applied next step
    // -------------------------------------------------------------------------

    [Fact]
    public void AddForce_AccumulatesInForceProperty()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        var force = new JVector(10, 0, 0);
        body.AddForce(force);
        Assert.Equal(force.X.ToDouble(), body.Force.X.ToDouble(), 1e-5);
        world.Dispose();
    }

    [Fact]
    public void AddForce_Accumulates_MultipleCalls()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.AddForce(new JVector(1, 0, 0));
        body.AddForce(new JVector(2, 0, 0));
        Assert.Equal(3.0, body.Force.X.ToDouble(), 1e-5);
        world.Dispose();
    }

    [Fact]
    public void AddForce_IsResetAfterStep()
    {
        var world = new World();
        world.AllowDeactivation = false;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.AddForce(new JVector(100, 0, 0));
        world.Step((Real)(1.0 / 60.0), false);
        Assert.Equal(JVector.Zero, body.Force);
        world.Dispose();
    }

    [Fact]
    public void AddForce_ChangesVelocityAfterStep()
    {
        // Force applied before step N is converted to DeltaVelocity in UpdateBodies at
        // the end of step N, and applied to Velocity at the start of step N+1.
        // Two steps are therefore needed to observe the velocity change.
        var world = new World();
        world.AllowDeactivation = false;
        world.Gravity = JVector.Zero;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        world.Step((Real)(1.0 / 60.0), false);   // warmup: body active, no forces yet
        body.AddForce(new JVector(100, 0, 0));
        world.Step((Real)(1.0 / 60.0), false);   // DeltaVelocity computed from Force
        world.Step((Real)(1.0 / 60.0), false);   // DeltaVelocity applied to Velocity
        Assert.True(body.Velocity.X > 0);
        world.Dispose();
    }

    [Fact]
    public void MotionTypeChange_ClearsQueuedForceAndTorque()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));

        body.AddForce(new JVector(0, 10, 0), new JVector(1, 0, 0));

        Assert.True(body.Force.LengthSquared() > (Real)0.0);
        Assert.True(body.Torque.LengthSquared() > (Real)0.0);

        body.MotionType = MotionType.Kinematic;

        Assert.Equal(JVector.Zero, body.Force);
        Assert.Equal(JVector.Zero, body.Torque);
        Assert.Equal(JVector.Zero, body.Data.DeltaVelocity);
        Assert.Equal(JVector.Zero, body.Data.DeltaAngularVelocity);

        world.Dispose();
    }

    [Fact]
    public void MotionTypeChange_ClearsForceDerivedVelocityDelta()
    {
        var world = new World
        {
            AllowDeactivation = false,
            Gravity = JVector.Zero
        };

        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.AddForce(new JVector(0, 100, 0), new JVector(1, 0, 0));

        world.Step((Real)(1.0 / 60.0), false);

        Assert.Equal(JVector.Zero, body.Velocity);
        Assert.Equal(JVector.Zero, body.AngularVelocity);
        Assert.True(body.Data.DeltaVelocity.LengthSquared() > (Real)0.0);
        Assert.True(body.Data.DeltaAngularVelocity.LengthSquared() > (Real)0.0);

        body.MotionType = MotionType.Kinematic;

        Assert.Equal(JVector.Zero, body.Data.DeltaVelocity);
        Assert.Equal(JVector.Zero, body.Data.DeltaAngularVelocity);

        body.MotionType = MotionType.Dynamic;
        world.Step((Real)(1.0 / 60.0), false);

        Assert.Equal(JVector.Zero, body.Velocity);
        Assert.Equal(JVector.Zero, body.AngularVelocity);

        world.Dispose();
    }

    [Fact]
    public void AddForce_OnKinematic_HasNoEffect()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.MotionType = MotionType.Kinematic;
        body.AddForce(new JVector(100, 0, 0));
        Assert.Equal(JVector.Zero, body.Force);
        world.Dispose();
    }

    [Fact]
    public void AddForce_OnStatic_HasNoEffect()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.MotionType = MotionType.Static;
        body.AddForce(new JVector(100, 0, 0));
        Assert.Equal(JVector.Zero, body.Force);
        world.Dispose();
    }

    [Fact]
    public void AddForce_WithPosition_AccumulatesTorque()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        // Force applied off-center should produce torque
        body.AddForce(new JVector(0, 10, 0), new JVector(1, 0, 0));
        Assert.True(body.Torque.LengthSquared() > 0);
        world.Dispose();
    }

    [Fact]
    public void AddForce_SleepingBody_WakeupFalse_HasNoEffect()
    {
        var world = new World();
        world.Gravity = JVector.Zero;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.DeactivationTime = TimeSpan.FromSeconds(1);

        Helper.AdvanceWorld(world, 2, (Real)(1.0 / 100.0), false);
        Assert.False(body.IsActive);

        body.AddForce(new JVector(10, 0, 0), wakeup: false);

        Assert.Equal(JVector.Zero, body.Force);
        Assert.False(body.IsActive);
        world.Dispose();
    }

    [Fact]
    public void AddForce_SleepingBody_WakeupTrue_QueuesForceAndReactivatesNextStep()
    {
        var world = new World();
        world.Gravity = JVector.Zero;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.DeactivationTime = TimeSpan.FromSeconds(1);

        Helper.AdvanceWorld(world, 2, (Real)(1.0 / 100.0), false);
        Assert.False(body.IsActive);

        body.AddForce(new JVector(10, 0, 0), wakeup: true);

        Assert.Equal(10.0, body.Force.X.ToDouble(), 1e-6);
        Assert.False(body.IsActive);

        world.Step((Real)(1.0 / 100.0), false);
        Assert.True(body.IsActive);
        world.Dispose();
    }

    [Fact]
    public void ApplyImpulse_SleepingBody_WakeupFalse_HasNoEffect()
    {
        var world = new World();
        world.Gravity = JVector.Zero;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.DeactivationTime = TimeSpan.FromSeconds(1);

        Helper.AdvanceWorld(world, 2, (Real)(1.0 / 100.0), false);
        Assert.False(body.IsActive);

        body.ApplyImpulse(new JVector(10, 0, 0), wakeup: false);

        Assert.Equal(JVector.Zero, body.Velocity);
        Assert.False(body.IsActive);
        world.Dispose();
    }

    [Fact]
    public void ApplyImpulse_SleepingBody_WakeupTrue_ChangesVelocityAndReactivatesNextStep()
    {
        var world = new World();
        world.Gravity = JVector.Zero;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.DeactivationTime = TimeSpan.FromSeconds(1);

        Helper.AdvanceWorld(world, 2, (Real)(1.0 / 100.0), false);
        Assert.False(body.IsActive);

        body.ApplyImpulse(new JVector(10, 0, 0), wakeup: true);

        Assert.True(body.Velocity.X > 0);
        Assert.False(body.IsActive);

        world.Step((Real)(1.0 / 100.0), false);
        Assert.True(body.IsActive);
        world.Dispose();
    }
}
