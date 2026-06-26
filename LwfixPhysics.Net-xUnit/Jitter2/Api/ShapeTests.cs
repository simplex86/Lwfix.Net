namespace SimplexLab.LwfixPhysics.Jitter2.Test.Api;

/// <summary>
/// Tests for shape management on a RigidBody (add, remove, clear, mass updates).
/// </summary>
public class ShapeTests
{
    // -------------------------------------------------------------------------
    // Adding shapes
    // -------------------------------------------------------------------------

    [Fact]
    public void AddShape_AppearsInShapes()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var shape = new SphereShape(1);
        body.AddShape(shape);
        Assert.True(body.Shapes.Contains(shape));
        world.Dispose();
    }

    [Fact]
    public void AddShape_UpdatesMass()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        Assert.True(body.Mass > 0);
        world.Dispose();
    }

    [Fact]
    public void AddShape_Preserve_DoesNotChangeMass()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        var massBeforeAddition = body.Mass;
        body.AddShape(new SphereShape(1), MassInertiaUpdateMode.Preserve);
        Assert.Equal(massBeforeAddition, body.Mass);
        world.Dispose();
    }

    [Fact]
    public void AddShape_SetsMassAfterTwoShapes()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        var massOneShape = body.Mass;
        body.AddShape(new SphereShape(1));
        Assert.True(body.Mass > massOneShape);
        world.Dispose();
    }

    [Fact]
    public void AddShape_SetsRigidBodyReference()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var shape = new SphereShape(1);
        body.AddShape(shape);
        Assert.Equal(body, shape.RigidBody);
        world.Dispose();
    }

    [Fact]
    public void AddShapes_AllAppearInShapes()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var shapes = new RigidBodyShape[] { new SphereShape(1), new BoxShape(1), new CapsuleShape((Real)0.5, (Real)1.0) };
        body.AddShapes(shapes);
        foreach (var shape in shapes)
            Assert.True(body.Shapes.Contains(shape));
        world.Dispose();
    }

    [Fact]
    public void AddShape_Null_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentNullException>(() => body.AddShape(null!));
        world.Dispose();
    }

    [Fact]
    public void AddShapes_NullEnumerable_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentNullException>(() => body.AddShapes(null!));
        world.Dispose();
    }

    [Fact]
    public void AddShapes_NullEntry_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentNullException>(() => body.AddShapes(new RigidBodyShape[] { new SphereShape(1), null! }));
        world.Dispose();
    }

    [Fact]
    public void AddShape_SameShapeTwice_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var shape = new SphereShape(1);
        body.AddShape(shape);
        Assert.Throws<ArgumentException>(() => body.AddShape(shape));
        world.Dispose();
    }

    [Fact]
    public void AddShape_FromAnotherBody_Throws()
    {
        var world = new World();
        var first = world.CreateRigidBody();
        var second = world.CreateRigidBody();
        var shape = new SphereShape(1);
        first.AddShape(shape);
        Assert.Throws<ArgumentException>(() => second.AddShape(shape));
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Removing shapes
    // -------------------------------------------------------------------------

    [Fact]
    public void RemoveShape_DisappearsFromShapes()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var shape = new SphereShape(1);
        body.AddShape(shape);
        body.RemoveShape(shape);
        Assert.False(body.Shapes.Contains(shape));
        world.Dispose();
    }

    [Fact]
    public void RemoveShape_UpdatesMass()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.AddShape(new SphereShape(1));
        var massTwoShapes = body.Mass;
        var shape = body.Shapes[0];
        body.RemoveShape(shape);
        Assert.True(body.Mass < massTwoShapes);
        world.Dispose();
    }

    [Fact]
    public void RemoveShape_Preserve_DoesNotChangeMass()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.AddShape(new SphereShape(1));
        var massBeforeRemoval = body.Mass;
        var shape = body.Shapes[0];
        body.RemoveShape(shape, MassInertiaUpdateMode.Preserve);
        Assert.Equal(massBeforeRemoval, body.Mass);
        world.Dispose();
    }

    [Fact]
    public void RemoveShapes_AllDisappearFromShapes()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var s1 = new SphereShape(1);
        var s2 = new BoxShape(1);
        body.AddShapes(new RigidBodyShape[] { s1, s2 });
        body.RemoveShapes(new RigidBodyShape[] { s1, s2 });
        Assert.Equal(0, body.Shapes.Count);
        world.Dispose();
    }

    [Fact]
    public void RemoveShape_Null_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentNullException>(() => body.RemoveShape(null!));
        world.Dispose();
    }

    [Fact]
    public void RemoveShape_ForeignShape_Throws()
    {
        var world = new World();
        var first = world.CreateRigidBody();
        var second = world.CreateRigidBody();
        var shape = new SphereShape(1);
        first.AddShape(shape);
        Assert.Throws<ArgumentException>(() => second.RemoveShape(shape));
        world.Dispose();
    }

    [Fact]
    public void RemoveShapes_WithForeignShape_Throws()
    {
        var world = new World();
        var first = world.CreateRigidBody();
        var second = world.CreateRigidBody();
        var s1 = new SphereShape(1);
        var s2 = new SphereShape(1);
        first.AddShape(s1);
        second.AddShape(s2);
        Assert.Throws<ArgumentException>(() => first.RemoveShapes(new[] { s1, s2 }));
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // ClearShapes
    // -------------------------------------------------------------------------

    [Fact]
    public void ClearShapes_RemovesAll()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.AddShape(new BoxShape(1));
        body.AddShape(new CapsuleShape((Real)0.5, (Real)1.0));
        body.ClearShapes();
        Assert.Equal(0, body.Shapes.Count);
        world.Dispose();
    }

    [Fact]
    public void ClearShapes_LastShape_ResetsMassToDefault()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.ClearShapes();
        Assert.Equal(1.0, body.Mass.ToDouble(), 1e-6);
        Assert.Equal(1.0, body.InverseInertia.M11.ToDouble(), 1e-6);
        Assert.Equal(1.0, body.InverseInertia.M22.ToDouble(), 1e-6);
        Assert.Equal(1.0, body.InverseInertia.M33.ToDouble(), 1e-6);
        world.Dispose();
    }

    [Fact]
    public void ClearShapes_OnEmptyBody_IsNoop()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Null(Record.Exception(() => body.ClearShapes()));
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Shape count
    // -------------------------------------------------------------------------

    [Fact]
    public void Shapes_CountMatchesAdded()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        for (int i = 0; i < 5; i++)
            body.AddShape(new SphereShape(1));
        Assert.Equal(5, body.Shapes.Count);
        world.Dispose();
    }
}
