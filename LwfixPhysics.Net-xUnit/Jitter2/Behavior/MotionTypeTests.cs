namespace SimplexLab.LwfixPhysics.Jitter2.Test.Behavior;

public class MotionTypeTests
{
    [Fact]
    public void CheckInternalMass()
    {
        var world = new World();

        var sphere = world.CreateRigidBody();
        sphere.AddShape(new SphereShape(1));

        var sphereMass = sphere.Mass;

        sphere.MotionType = MotionType.Kinematic;

        Assert.Equal((Real)0, sphere.Data.InverseMass);
        Assert.Equal(sphereMass, sphere.Mass);

        sphere.MotionType = MotionType.Dynamic;

        Assert.True(MathR.Abs(sphere.Data.InverseMass - (Real)1.0 / sphereMass) < (Real)1e-6);
        Assert.Equal(sphereMass, sphere.Mass);

        sphere.MotionType = MotionType.Static;

        Assert.Equal((Real)0, sphere.Data.InverseMass);
        Assert.Equal(sphereMass, sphere.Mass);

        world.Dispose();
    }

    private void PrepareTwoStack(World world, out RigidBody platform, out List<RigidBody> boxes)
    {
        // Create a static body. The platform dimensions are kept modest: a 10x2x10 box
        // would yield a diagonal inertia whose determinant (~1e10) overflows Fixed32's
        // 1/det resolution (~2.3e-10 smallest positive), so SetMassInertia would throw.
        platform = world.CreateRigidBody();
        platform.AddShape(new BoxShape((Real)4, (Real)1, (Real)4));
        platform.Position = new JVector(0, -1, 0);
        platform.MotionType = MotionType.Static;

        boxes = new List<RigidBody>();

        // Create two boxes stacked
        for (int i = 0; i < 2; i++)
        {
            var box = world.CreateRigidBody();
            box.AddShape(new BoxShape(1));
            box.Position = new JVector(0, (Real)0.5 + i, 0);
            boxes.Add(box);
        }

        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);

        // Static bodies actually do NOT build connections. We will have
        // two islands here.
        Assert.Equal(0, platform.Connections.Count);
        Assert.Equal(1, boxes[0].Connections.Count);
        Assert.Equal(1, boxes[1].Connections.Count);
        Assert.NotEqual(boxes[0].Island, platform.Island);
        Assert.Equal(boxes[0].Island, boxes[1].Island);

        // We do store contacts/constraints
        Assert.Equal(1, platform.Contacts.Count);
        Assert.Equal(2, boxes[0].Contacts.Count);
        Assert.Equal(1, boxes[1].Contacts.Count);
    }

    [Fact]
    public void CheckContactGraph()
    {
        var world = new World();

        PrepareTwoStack(world, out var platform, out var boxes);

        // Switch from static to dynamic. The platform should now be part of the island.
        platform.MotionType = MotionType.Dynamic;

        // Same as before
        Assert.Equal(1, platform.Contacts.Count);
        Assert.Equal(2, boxes[0].Contacts.Count);
        Assert.Equal(1, boxes[1].Contacts.Count);

        // Different contact graph
        Assert.Equal(1, platform.Connections.Count);
        Assert.Equal(2, boxes[0].Connections.Count);
        Assert.Equal(1, boxes[1].Connections.Count);
        Assert.Equal(boxes[0].Island, platform.Island);
        Assert.Equal(boxes[0].Island, boxes[1].Island);

        // Switch from dynamic to kinematic. Contact graph should stay the same
        platform.MotionType = MotionType.Kinematic;

        // Same as before
        Assert.Equal(1, platform.Contacts.Count);
        Assert.Equal(2, boxes[0].Contacts.Count);
        Assert.Equal(1, boxes[1].Contacts.Count);

        // Same as before
        Assert.Equal(1, platform.Connections.Count);
        Assert.Equal(2, boxes[0].Connections.Count);
        Assert.Equal(1, boxes[1].Connections.Count);
        Assert.Equal(boxes[0].Island, platform.Island);
        Assert.Equal(boxes[0].Island, boxes[1].Island);

        // Simulate a bit and check that nothing changed
        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);

        // Same as before
        Assert.Equal(1, platform.Contacts.Count);
        Assert.Equal(2, boxes[0].Contacts.Count);
        Assert.Equal(1, boxes[1].Contacts.Count);

        // Same as before
        Assert.Equal(1, platform.Connections.Count);
        Assert.Equal(2, boxes[0].Connections.Count);
        Assert.Equal(1, boxes[1].Connections.Count);
        Assert.Equal(boxes[0].Island, platform.Island);
        Assert.Equal(boxes[0].Island, boxes[1].Island);

        // Switch from kinematic to static.
        platform.MotionType = MotionType.Static;

        // Static bodies actually do NOT build connections. We will have
        // two islands here.
        Assert.Equal(0, platform.Connections.Count);
        Assert.Equal(1, boxes[0].Connections.Count);
        Assert.Equal(1, boxes[1].Connections.Count);
        Assert.NotEqual(boxes[0].Island, platform.Island);
        Assert.Equal(boxes[0].Island, boxes[1].Island);

        // We do store contacts/constraints
        Assert.Equal(1, platform.Contacts.Count);
        Assert.Equal(2, boxes[0].Contacts.Count);
        Assert.Equal(1, boxes[1].Contacts.Count);

        world.Dispose();
    }

    [Fact]
    public void CheckNoStaticKinematicContacts()
    {
        var world = new World();

        PrepareTwoStack(world, out var platform, out var boxes);

        boxes[0].MotionType = MotionType.Kinematic;

        // Jitter should now remove the contacts connecting a static and a kinematic body.
        // Only dynamic <-> kinematic contacts should remain.

        Assert.Equal(0, platform.Contacts.Count);
        Assert.Equal(1, boxes[0].Contacts.Count);
        Assert.Equal(1, boxes[1].Contacts.Count);

        Assert.NotEqual(boxes[0].Island, platform.Island);
        Helper.AdvanceWorld(world, 1, (Real)(1.0 / 100.0), false);
        Assert.Equal(0, platform.Contacts.Count);

        Assert.False(platform.IsActive);
        Assert.True(boxes[0].IsActive);
        Assert.True(boxes[1].IsActive);

        Helper.AdvanceWorld(world, 10, (Real)(1.0 / 100.0), false);

        Assert.False(platform.IsActive);
        Assert.False(boxes[0].IsActive);
        Assert.False(boxes[1].IsActive);

        world.Dispose();
    }
}
