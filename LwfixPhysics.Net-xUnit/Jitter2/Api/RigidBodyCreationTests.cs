namespace SimplexLab.LwfixPhysics.Jitter2.Test.Api;

/// <summary>
/// Tests for the initial state of a RigidBody after creation.
/// </summary>
public class RigidBodyCreationTests
{
    [Fact]
    public void DefaultPosition_IsZero()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(JVector.Zero, body.Position);
        world.Dispose();
    }

    [Fact]
    public void DefaultOrientation_IsIdentity()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(JQuaternion.Identity, body.Orientation);
        world.Dispose();
    }

    [Fact]
    public void DefaultVelocity_IsZero()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(JVector.Zero, body.Velocity);
        world.Dispose();
    }

    [Fact]
    public void DefaultAngularVelocity_IsZero()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(JVector.Zero, body.AngularVelocity);
        world.Dispose();
    }

    [Fact]
    public void DefaultMotionType_IsDynamic()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(MotionType.Dynamic, body.MotionType);
        world.Dispose();
    }

    [Fact]
    public void DefaultForce_IsZero()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(JVector.Zero, body.Force);
        world.Dispose();
    }

    [Fact]
    public void DefaultTorque_IsZero()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(JVector.Zero, body.Torque);
        world.Dispose();
    }

    [Fact]
    public void DefaultAffectedByGravity_IsTrue()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.True(body.AffectedByGravity);
        world.Dispose();
    }

    [Fact]
    public void DefaultEnableSpeculativeContacts_IsFalse()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.False(body.EnableSpeculativeContacts);
        world.Dispose();
    }

    [Fact]
    public void DefaultTag_IsNull()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Null(body.Tag);
        world.Dispose();
    }

    [Fact]
    public void DefaultShapes_IsEmpty()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(0, body.Shapes.Count);
        world.Dispose();
    }

    [Fact]
    public void DefaultContacts_IsEmpty()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(0, body.Contacts.Count);
        world.Dispose();
    }

    [Fact]
    public void DefaultConnections_IsEmpty()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(0, body.Connections.Count);
        world.Dispose();
    }

    [Fact]
    public void DefaultConstraints_IsEmpty()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(0, body.Constraints.Count);
        world.Dispose();
    }

    [Fact]
    public void CreatedBody_BelongsToWorld()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Equal(world, body.World);
        world.Dispose();
    }

    [Fact]
    public void CreatedBodies_HaveUniqueIds()
    {
        var world = new World();
        var ids = new HashSet<ulong>();
        for (int i = 0; i < 100; i++)
        {
            ids.Add(world.CreateRigidBody().RigidBodyId);
        }
        Assert.Equal(100, ids.Count);
        world.Dispose();
    }

    [Fact]
    public void CreatedBody_AppearsInWorldRigidBodies()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.True(world.RigidBodies.Contains(body));
        world.Dispose();
    }

    [Fact]
    public void RemovedBody_DisappearsFromWorldRigidBodies()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        world.Remove(body);
        Assert.False(world.RigidBodies.Contains(body));
        world.Dispose();
    }
}
