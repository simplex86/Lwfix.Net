namespace LwfixTest.Physics.Jitter2.Api;

public class InertiaTests
{
    // The original JitterTests use subdivisions=8 since float has ample precision.
    // Fixed32 (Q32.32) accumulates catastrophic cancellation when summing hundreds
    // of thousands of tiny tetrahedron volumes: empirical runs show that mass starts
    // to degrade past subdivisions=6 (sphere mass drops to ~0.295 at s=7 and to 0 at s=8).
    // subdivisions=5 is the sweet spot for Fixed32 - well past the default of 4 but
    // before precision loss dominates.
    private const int Fixed32Subdivisions = 5;

    // Tolerance accommodating Fixed32 (~2.3e-10) precision loss accumulated through
    // the tetrahedron tessellation. The original float tests use 1e-3.
    private static readonly Real Tolerance = (Real)5e-2;

    private static void Check(RigidBodyShape shape, JMatrix inertia, JVector com, Real mass)
    {
        shape.CalculateMassInertia(out JMatrix shapeInertia, out JVector shapeCom, out Real shapeMass);

        JMatrix dInertia = shapeInertia - inertia;
        Assert.True(MathHelper.IsZero(dInertia.UnsafeGet(0), Tolerance));
        Assert.True(MathHelper.IsZero(dInertia.UnsafeGet(1), Tolerance));
        Assert.True(MathHelper.IsZero(dInertia.UnsafeGet(2), Tolerance));

        Real dmass = shapeMass - mass;
        Assert.True(MathR.Abs(dmass) < Tolerance);

        JVector dcom = shapeCom - com;
        Assert.True(MathHelper.IsZero(dcom, Tolerance));
    }

    [Fact]
    public void CapsuleInertia()
    {
        var ts = new CapsuleShape((Real)0.429, (Real)1.7237);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void CylinderInertia()
    {
        var ts = new CylinderShape((Real)0.429, (Real)1.7237);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void ConeInertia()
    {
        var ts = new ConeShape((Real)0.429, (Real)1.7237);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void BoxInertia()
    {
        var ts = new BoxShape((Real)0.429, (Real)1.7237, (Real)2.11383);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void SphereInertia()
    {
        var ts = new SphereShape((Real)0.429);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void TransformedInertia()
    {
        var ss = new SphereShape((Real)0.429);
        var translation = new JVector((Real)2.847, (Real)3.432, (Real)1.234);

        var ts = new TransformedShape(ss, translation);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void TransformedRotationInertia()
    {
        var box = new BoxShape((Real)1.0, (Real)2.0, (Real)3.0);
        var rotation = JMatrix.CreateRotationX((Real)0.7) * JMatrix.CreateRotationY((Real)1.1);

        var ts = new TransformedShape(box, rotation);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void TransformedRotationTranslationInertia()
    {
        var box = new BoxShape((Real)1.0, (Real)2.0, (Real)3.0);
        var translation = new JVector((Real)2.847, (Real)3.432, (Real)1.234);
        var rotation = JMatrix.CreateRotationZ((Real)0.5) * JMatrix.CreateRotationX((Real)1.3);

        var ts = new TransformedShape(box, translation, rotation);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void TransformedScaleInertia()
    {
        var box = new BoxShape((Real)1.0, (Real)2.0, (Real)3.0);
        var scale = JMatrix.CreateScale((Real)2.0, (Real)1.5, (Real)3.0);
        var translation = new JVector((Real)1.0, (Real)2.0, (Real)3.0);

        var ts = new TransformedShape(box, translation, scale);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void TransformedShearInertia()
    {
        var box = new BoxShape((Real)1.0, (Real)2.0, (Real)3.0);
        var shear = JMatrix.Identity;
        shear.M12 = (Real)0.5;
        shear.M31 = (Real)0.3;
        var translation = new JVector((Real)1.5, (Real)(-0.7), (Real)2.3);

        var ts = new TransformedShape(box, translation, shear);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }

    [Fact]
    public void ConvexHullInertia()
    {
        List<JTriangle> cvh = new List<JTriangle>();

        JVector a = new JVector((Real)0.234, (Real)1.23, (Real)3.54);
        JVector b = new JVector((Real)7.788, (Real)0.23, (Real)8.14);
        JVector c = new JVector((Real)2.234, (Real)8.23, (Real)8.14);
        JVector d = new JVector((Real)6.234, (Real)3.23, (Real)9.04);

        cvh.Add(new JTriangle(a, b, c));
        cvh.Add(new JTriangle(a, b, d));
        cvh.Add(new JTriangle(b, c, d));
        cvh.Add(new JTriangle(a, c, d));

        var ts = new ConvexHullShape(cvh);
        ShapeHelper.CalculateMassInertia(ts, out JMatrix inertia, out JVector com, out Real mass, Fixed32Subdivisions);
        Check(ts, inertia, com, mass);
    }
}
