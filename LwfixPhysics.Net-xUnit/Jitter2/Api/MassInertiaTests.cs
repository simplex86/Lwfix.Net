namespace SimplexLab.LwfixPhysics.Jitter2.Test.Api;

public class MassInertiaTests
{
    // -------------------------------------------------------------------------
    // SetMassInertia() — auto-compute from shapes
    // -------------------------------------------------------------------------

    [Fact]
    public void SetMassInertia_NoShapes_SetsUnitMassAndIdentityInertia()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.SetMassInertia();
        Assert.Equal(1.0, body.Mass.ToDouble(), (double)(Real)1e-6);
        Assert.Equal(1.0, body.InverseInertia.M11.ToDouble(), (double)(Real)1e-6);
        Assert.Equal(1.0, body.InverseInertia.M22.ToDouble(), (double)(Real)1e-6);
        Assert.Equal(1.0, body.InverseInertia.M33.ToDouble(), (double)(Real)1e-6);
        world.Dispose();
    }

    [Fact]
    public void SetMassInertia_WithShape_MatchesMassFromAddShape()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        var massFromAddShape = body.Mass;
        body.SetMassInertia();
        Assert.Equal(massFromAddShape.ToDouble(), body.Mass.ToDouble(), (double)(Real)1e-5);
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // SetMassInertia(Real mass) — scale inertia to a specific mass
    // -------------------------------------------------------------------------

    [Fact]
    public void SetMassInertia_SpecificMass_SetsMassCorrectly()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.SetMassInertia((Real)5.0);
        Assert.Equal(5.0, body.Mass.ToDouble(), (double)(Real)1e-5);
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // SetMassInertia(JMatrix, Real, bool) — fully manual
    // -------------------------------------------------------------------------

    [Fact]
    public void SetMassInertia_Manual_SetsMassAndInertia()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var inertia = JMatrix.Identity * (Real)2.0;
        body.SetMassInertia(inertia, (Real)10.0);
        Assert.Equal(10.0, body.Mass.ToDouble(), (double)(Real)1e-5);
        // InverseInertia should be inverse of 2*I = 0.5*I
        Assert.Equal(0.5, body.InverseInertia.M11.ToDouble(), (double)(Real)1e-5);
        Assert.Equal(0.5, body.InverseInertia.M22.ToDouble(), (double)(Real)1e-5);
        Assert.Equal(0.5, body.InverseInertia.M33.ToDouble(), (double)(Real)1e-5);
        world.Dispose();
    }

    [Fact]
    public void SetMassInertia_ManualInverse_SetsMassAndInertiaDirectly()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        var inverseInertia = JMatrix.Identity * (Real)4.0;
        // inverseMass = 0.1 → mass = 10
        body.SetMassInertia(inverseInertia, (Real)0.1, setAsInverse: true);
        Assert.Equal(10.0, body.Mass.ToDouble(), (double)(Real)1e-4);
        Assert.Equal(4.0, body.InverseInertia.M11.ToDouble(), (double)(Real)1e-5);
        world.Dispose();
    }

    [Fact]
    public void SetMassInertia_PreservedAcrossMotionTypeChanges()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        body.AddShape(new SphereShape(1));
        body.SetMassInertia((Real)7.0);
        var mass = body.Mass;

        body.MotionType = MotionType.Static;
        Assert.Equal(mass.ToDouble(), body.Mass.ToDouble(), (double)(Real)1e-5);

        body.MotionType = MotionType.Kinematic;
        Assert.Equal(mass.ToDouble(), body.Mass.ToDouble(), (double)(Real)1e-5);

        body.MotionType = MotionType.Dynamic;
        Assert.Equal(mass.ToDouble(), body.Mass.ToDouble(), (double)(Real)1e-5);
        world.Dispose();
    }

    // -------------------------------------------------------------------------
    // Throws
    // -------------------------------------------------------------------------

    [Fact]
    public void SetMassInertia_WithMass_Zero_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentException>(() => body.SetMassInertia((Real)0.0));
        world.Dispose();
    }

    [Fact]
    public void SetMassInertia_WithMass_Negative_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentException>(() => body.SetMassInertia((Real)(-1.0)));
        world.Dispose();
    }

    [Fact]
    public void SetMassInertia_WithSingularMatrix_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentException>(() => body.SetMassInertia(JMatrix.Zero, (Real)1.0));
        world.Dispose();
    }

    [Fact]
    public void SetMassInertia_AsInverse_WithNegativeInverseMass_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentException>(() => body.SetMassInertia(JMatrix.Identity, (Real)(-1.0), true));
        world.Dispose();
    }

    [Fact]
    public void SetMassInertia_AsInverse_WithInfiniteInverseMass_Throws()
    {
        var world = new World();
        var body = world.CreateRigidBody();
        Assert.Throws<ArgumentException>(() => body.SetMassInertia(JMatrix.Identity, Real.PositiveInfinity, true));
        world.Dispose();
    }
}
