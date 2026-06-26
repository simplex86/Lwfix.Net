namespace SimplexLab.LwfixPhysics.Jitter2.Test.Api;

/// <summary>
/// Tests for World properties and lifecycle management.
/// </summary>
public class WorldTests
{
    // -------------------------------------------------------------------------
    // Default properties
    // -------------------------------------------------------------------------

    [Fact]
    public void DefaultGravity_IsNegativeY()
    {
        var world = new World();
        Assert.True(world.Gravity.Y < 0);
        Assert.Equal((Real)0, world.Gravity.X);
        Assert.Equal((Real)0, world.Gravity.Z);
        world.Dispose();
    }

    [Fact]
    public void DefaultAllowDeactivation_IsTrue()
    {
        var world = new World();
        Assert.True(world.AllowDeactivation);
        world.Dispose();
    }

    [Fact]
    public void DefaultRigidBodies_ContainsOnlyNullBody()
    {
        var world = new World();
        // NullBody is always present
        Assert.Equal(1, world.RigidBodies.Count);
        Assert.True(world.RigidBodies.Contains(world.NullBody));
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Property setters
    // -------------------------------------------------------------------------

    [Fact]
    public void Gravity_RoundTrip()
    {
        var world = new World();
        var expected = new JVector(0, -20, 0);
        world.Gravity = expected;
        Assert.Equal(expected, world.Gravity);
        world.Dispose();
    }

    [Fact]
    public void Gravity_SetToZero()
    {
        var world = new World();
        world.Gravity = JVector.Zero;
        Assert.Equal(JVector.Zero, world.Gravity);
        world.Dispose();
    }

    [Fact]
    public void AllowDeactivation_RoundTrip()
    {
        var world = new World();
        world.AllowDeactivation = false;
        Assert.False(world.AllowDeactivation);
        world.AllowDeactivation = true;
        Assert.True(world.AllowDeactivation);
        world.Dispose();
    }

    [Fact]
    public void SubstepCount_RoundTrip()
    {
        var world = new World();
        world.SubstepCount = 4;
        Assert.Equal(4, world.SubstepCount);
        world.Dispose();
    }

    [Fact]
    public void SolverIterations_RoundTrip()
    {
        var world = new World();
        world.SolverIterations = (10, 4);
        Assert.Equal(10, world.SolverIterations.solver);
        Assert.Equal(4, world.SolverIterations.relaxation);
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Body management
    // -------------------------------------------------------------------------

    [Fact]
    public void CreateRigidBody_IncrementsBodyCount()
    {
        var world = new World();
        var countBefore = world.RigidBodies.Count;
        world.CreateRigidBody();
        Assert.Equal(countBefore + 1, world.RigidBodies.Count);
        world.Dispose();
    }

    [Fact]
    public void Remove_DecrementsBodyCount()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var countAfterCreate = world.RigidBodies.Count;
        world.Remove(body);
        Assert.Equal(countAfterCreate - 1, world.RigidBodies.Count);
        world.Dispose();
    }

    [Fact]
    public void Clear_RemovesAllBodiesExceptNullBody()
    {
        var world = new World();
        for (int i = 0; i < 10; i++)
            world.CreateRigidBody();
        world.Clear();
        Assert.Equal(1, world.RigidBodies.Count);
        Assert.True(world.RigidBodies.Contains(world.NullBody));
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Step
    // -------------------------------------------------------------------------

    [Fact]
    public void Step_NegativeDt_Throws()
    {
        var world = new World();
        Assert.Throws<ArgumentException>(() => world.Step((Real)(-1.0 / 60.0), false));
        world.Dispose();
    }

    [Fact]
    public void Step_ZeroDt_DoesNotThrow()
    {
        var world = new World();
        Assert.Null(Record.Exception(() => world.Step((Real)0.0, false)));
        world.Dispose();
    }

    [Fact]
    public void Step_WithNoBody_DoesNotThrow()
    {
        var world = new World();
        Assert.Null(Record.Exception(() => world.Step((Real)(1.0 / 60.0), false)));
        world.Dispose();
    }

    [Fact]
    public void Step_WhenInterrupted_PausesThreadPool()
    {
        var world = new World();
        world.PreStep += _ =>
        {
            Assert.False(SimplexLab.LwfixPhysics.Jitter2.Parallelization.ThreadPool.Instance.IsPaused);
            throw new InvalidOperationException("Interrupted step.");
        };

        Assert.Throws<InvalidOperationException>(() => world.Step((Real)(1.0 / 60.0), true));

        Assert.True(SimplexLab.LwfixPhysics.Jitter2.Parallelization.ThreadPool.Instance.IsPaused);
        world.Dispose();
    }

    [Fact]
    public void Stabilize_NegativeDt_Throws()
    {
        var world = new World();
        Assert.Throws<ArgumentException>(() => world.Stabilize((Real)(-1.0 / 60.0), 1, 0, false));
        world.Dispose();
    }

    [Fact]
    public void Stabilize_ZeroDt_DoesNotThrow()
    {
        var world = new World();
        Assert.Null(Record.Exception(() => world.Stabilize((Real)0.0, 1, 0, false)));
        world.Dispose();
    }

    [Fact]
    public void Stabilize_SolverIterationsBelowOne_Throws()
    {
        var world = new World();
        Assert.Throws<ArgumentException>(() => world.Stabilize((Real)(1.0 / 60.0), 0, 0, false));
        world.Dispose();
    }

    [Fact]
    public void Stabilize_RelaxationIterationsBelowZero_Throws()
    {
        var world = new World();
        Assert.Throws<ArgumentException>(() => world.Stabilize((Real)(1.0 / 60.0), 1, -1, false));
        world.Dispose();
    }

    [Fact]
    public void Stabilize_WhenInterrupted_PausesThreadPool()
    {
        var world = new World
        {
            Gravity = JVector.Zero
        };

        var body = world.CreateRigidBody();
        body.AddShape(new BoxShape(1));
        body.DeactivationTime = TimeSpan.Zero;

        world.Step((Real)(1.0 / 60.0), false);

        world.IslandDeactivated += _ =>
        {
            Assert.False(SimplexLab.LwfixPhysics.Jitter2.Parallelization.ThreadPool.Instance.IsPaused);
            throw new InvalidOperationException("Interrupted stabilize.");
        };

        Assert.Throws<InvalidOperationException>(() => world.Stabilize((Real)(1.0 / 60.0), 1, 0, true));

        Assert.True(SimplexLab.LwfixPhysics.Jitter2.Parallelization.ThreadPool.Instance.IsPaused);
        world.Dispose();
    }

    [Fact]
    public void Stabilize_WithConstraintError_SolvesWithoutChangingPositions()
    {
        var world = new World();
        world.AllowDeactivation = false;
        world.Gravity = JVector.Zero;

        var bodyA = world.CreateRigidBody();
        bodyA.AddShape(new SphereShape(1));

        var bodyB = world.CreateRigidBody();
        bodyB.AddShape(new SphereShape(1));

        var socket = world.CreateConstraint<BallSocket>(bodyA, bodyB);
        socket.Initialize(JVector.Zero);

        bodyB.Position = new JVector(1, 0, 0);
        bodyA.Velocity = JVector.Zero;
        bodyB.Velocity = JVector.Zero;
        bodyA.AngularVelocity = JVector.Zero;
        bodyB.AngularVelocity = JVector.Zero;

        JVector positionA = bodyA.Position;
        JVector positionB = bodyB.Position;

        world.Stabilize((Real)(1.0 / 60.0), 4, 2, false);

        Assert.Equal(positionA.X.ToDouble(), bodyA.Position.X.ToDouble(), 1e-6);
        Assert.Equal(positionA.Y.ToDouble(), bodyA.Position.Y.ToDouble(), 1e-6);
        Assert.Equal(positionA.Z.ToDouble(), bodyA.Position.Z.ToDouble(), 1e-6);

        Assert.Equal(positionB.X.ToDouble(), bodyB.Position.X.ToDouble(), 1e-6);
        Assert.Equal(positionB.Y.ToDouble(), bodyB.Position.Y.ToDouble(), 1e-6);
        Assert.Equal(positionB.Z.ToDouble(), bodyB.Position.Z.ToDouble(), 1e-6);
        Assert.True(bodyA.Velocity.LengthSquared() + bodyB.Velocity.LengthSquared() > (Real)0.0);
        Assert.True(socket.Impulse.LengthSquared() > (Real)0.0);

        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Events
    // -------------------------------------------------------------------------

    [Fact]
    public void PreStep_IsFiredOnStep()
    {
        var world = new World();
        bool fired = false;
        world.PreStep += _ => fired = true;
        world.Step((Real)(1.0 / 60.0), false);
        Assert.True(fired);
        world.Dispose();
    }

    [Fact]
    public void PostStep_IsFiredOnStep()
    {
        var world = new World();
        bool fired = false;
        world.PostStep += _ => fired = true;
        world.Step((Real)(1.0 / 60.0), false);
        Assert.True(fired);
        world.Dispose();
    }

    [Fact]
    public void PreStep_ReceivesCorrectDt()
    {
        var world = new World();
        Real receivedDt = (Real)0.0;
        world.PreStep += dt => receivedDt = dt;
        Real expectedDt = (Real)(1.0 / 60.0);
        world.Step(expectedDt, false);
        Assert.Equal(expectedDt.ToDouble(), receivedDt.ToDouble(), 1e-6);
        world.Dispose();
    }

    [Fact]
    public void Stabilize_DoesNotFireStepEvents()
    {
        var world = new World();
        bool preFired = false;
        bool postFired = false;

        world.PreStep += _ => preFired = true;
        world.PostStep += _ => postFired = true;

        world.Stabilize((Real)(1.0 / 60.0), 1, 0, false);

        Assert.False(preFired);
        Assert.False(postFired);
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Gravity effect on bodies
    // -------------------------------------------------------------------------

    [Fact]
    public void Gravity_AffectsFallingBody()
    {
        var world = new World();
        world.AllowDeactivation = false;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        var startY = body.Position.Y;
        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);
        Assert.True(body.Position.Y < startY);
        world.Dispose();
    }

    [Fact]
    public void Gravity_Zero_BodyDoesNotFall()
    {
        var world = new World();
        world.AllowDeactivation = false;
        world.Gravity = JVector.Zero;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        var startY = body.Position.Y;
        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);
        Assert.Equal(startY.ToDouble(), body.Position.Y.ToDouble(), 1e-4);
        world.Dispose();
    }

    [Fact]
    public void AffectedByGravity_False_BodyDoesNotFall()
    {
        // Gravity is on, but the specific body opts out.
        var world = new World();
        world.AllowDeactivation = false;
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.AffectedByGravity = false;
        var startY = body.Position.Y;
        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);
        Assert.Equal(startY.ToDouble(), body.Position.Y.ToDouble(), 1e-4);
        world.Dispose();
    }

    [Fact]
    public void AffectedByGravity_OnlyAffectsOptedOutBody()
    {
        // One body opts out; a second body far away still falls.
        var world = new World();
        world.AllowDeactivation = false;
        var floating = world.CreateRigidBody();
        floating.AddShape(new SphereShape(1));
        floating.AffectedByGravity = false;
        floating.Position = new JVector(0, 0, 0);
        var falling = world.CreateRigidBody();
        falling.AddShape(new SphereShape(1));
        falling.Position = new JVector(100, 0, 0);  // far away, no interaction
        var startY = floating.Position.Y;
        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);
        Assert.Equal(startY.ToDouble(), floating.Position.Y.ToDouble(), 1e-4);
        Assert.True(falling.Position.Y < startY);
        world.Dispose();
    }
}
