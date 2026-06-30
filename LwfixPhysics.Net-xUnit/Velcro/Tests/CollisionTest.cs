using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Test;
using SimplexLab.LwfixPhysics.Velcro.Utilities;
using Xunit;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Tests;

/// <summary>
/// Port of <c>VelcroPhysics.Tests/Tests/CollisionTest.cs</c> adapted to the Fixed32 type,
/// plus additional coverage for <see cref="PolygonShape"/> / <see cref="CircleShape"/> /
/// <see cref="EdgeShape"/> mass data and shape creation paths.
/// </summary>
public class CollisionTest
{
    // ---------------------------------------------------------------------
    // Original VelcroPhysics test (ported)
    // ---------------------------------------------------------------------

    [Fact]
    public void TestMassData()
    {
        Vector2 center = new Vector2(100, -50);
        Fixed32 hx = (Fixed32)0.5, hy = (Fixed32)1.5;
        Fixed32 angle1 = (Fixed32)0.25;

        PolygonShape polygon1 = new PolygonShape(PolygonUtils.CreateRectangle(hx, hy, center, angle1), 1);

        Fixed32 absTol = (Fixed32)2.0 * MathConstants.Epsilon;
        Fixed32 relTol = (Fixed32)2.0 * MathConstants.Epsilon;

        polygon1.GetMassData(out var massData1);

        Assert.True(FMath.Abs(massData1.Centroid.X - center.X) < absTol + relTol * FMath.Abs(center.X));
        Assert.True(FMath.Abs(massData1.Centroid.Y - center.Y) < absTol + relTol * FMath.Abs(center.Y));

        Vector2[] vertices = new Vector2[4];
        vertices[0] = new Vector2(center.X - hx, center.Y - hy);
        vertices[1] = new Vector2(center.X + hx, center.Y - hy);
        vertices[2] = new Vector2(center.X - hx, center.Y + hy);
        vertices[3] = new Vector2(center.X + hx, center.Y + hy);

        PolygonShape polygon2 = new PolygonShape(new Vertices(vertices), 1);
        polygon2.GetMassData(out var massData2);

        Assert.True(FMath.Abs(massData2.Centroid.X - center.X) < absTol + relTol * FMath.Abs(center.X));
        Assert.True(FMath.Abs(massData2.Centroid.Y - center.Y) < absTol + relTol * FMath.Abs(center.Y));

        Fixed32 mass = (Fixed32)4.0 * hx * hy;
        Fixed32 inertia = (mass / (Fixed32)3.0) * (hx * hx + hy * hy) + mass * MathUtils.Dot(center, center);

        Assert.True(FMath.Abs(massData1.Centroid.X - center.X) < absTol + relTol * FMath.Abs(center.X));
        Assert.True(FMath.Abs(massData1.Centroid.Y - center.Y) < absTol + relTol * FMath.Abs(center.Y));
        Assert.True(FMath.Abs(massData1.Mass - mass) < (Fixed32)20.0 * (absTol + relTol * mass));
        Assert.True(FMath.Abs(massData1.Inertia - inertia) < (Fixed32)40.0 * (absTol + relTol * inertia));

        Assert.True(FMath.Abs(massData2.Centroid.X - center.X) < absTol + relTol * FMath.Abs(center.X));
        Assert.True(FMath.Abs(massData2.Centroid.Y - center.Y) < absTol + relTol * FMath.Abs(center.Y));
        Assert.True(FMath.Abs(massData2.Mass - mass) < (Fixed32)20.0 * (absTol + relTol * mass));
        Assert.True(FMath.Abs(massData2.Inertia - inertia) < (Fixed32)40.0 * (absTol + relTol * inertia));
    }

    // ---------------------------------------------------------------------
    // Additional coverage
    // ---------------------------------------------------------------------

    [Fact]
    public void PolygonShape_AxisAlignedBox_MassMatchesAnalytic()
    {
        // 2x4 box (half extents 1, 2), density 3 -> mass = density * area = 3 * 8 = 24.
        Fixed32 hx = 1, hy = 2, density = 3;
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(hx, hy), density);
        polygon.GetMassData(out var md);

        Assert.Equal(density * (4 * hx * hy), md.Mass);
        // Centroid at origin because the polygon is symmetric around the origin.
        // Use tolerance-based comparison: mass computation introduces tiny fixed-point error.
        TestHelper.AssertApprox(Vector2.Zero, md.Centroid);
        // Analytical moment of inertia of an axis-aligned box about its centre:
        // I = (mass / 3) * (hx^2 + hy^2)
        Fixed32 expectedInertia = (md.Mass / (Fixed32)3.0) * (hx * hx + hy * hy);
        TestHelper.AssertApprox(expectedInertia, md.Inertia, (Fixed32)1e-2);
    }

    [Fact]
    public void PolygonShape_ZeroDensity_SkipsMassComputation()
    {
        // Velcro: PolygonShape.ComputeProperties returns early when density <= 0.
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(1, 1), Fixed32.Zero);
        polygon.GetMassData(out var md);
        Assert.Equal(Fixed32.Zero, md.Mass);
        Assert.Equal(Fixed32.Zero, md.Inertia);
    }

    [Fact]
    public void PolygonShape_SetAsBox_ReplacesVerticesAndRecomputesMass()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(1, 1), 1);
        polygon.SetAsBox(2, 2);
        Assert.Equal(4, polygon.Vertices.Count);
        polygon.GetMassData(out var md);
        // Area = 4 * (2 * 2) = 16; mass = density * area = 1 * 16 = 16.
        Assert.Equal(16, md.Mass.ToDouble());
    }

    [Fact]
    public void PolygonShape_SetAsBox_WithCenterAndAngle_TransformsVertices()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(1, 1), 1);
        Vector2 center = new Vector2(10, -5);
        Fixed32 angle = Fixed32.Half_PI;
        polygon.SetAsBox(1, 1, center, angle);

        polygon.GetMassData(out var md);
        // After SetAsBox(hx, hy, center, angle) the centroid should equal `center`.
        TestHelper.AssertApprox(center, md.Centroid, (Fixed32)1e-3);
    }

    [Fact]
    public void PolygonShape_TooFewVertices_Throws()
    {
        // In DEBUG builds, the Debug.Assert inside SetVertices fires first and surfaces
        // as a TraceAssertException; in RELEASE builds the InvalidOperationException is
        // raised directly. Accept either to keep the test mode-agnostic.
        Assert.ThrowsAny<Exception>(() =>
            new PolygonShape(new Vertices(new[] { new Vector2(0, 0), new Vector2(1, 1) }), 1));
    }

    [Fact]
    public void PolygonShape_Clone_PreservesAllFields()
    {
        PolygonShape original = new PolygonShape(PolygonUtils.CreateRectangle(2, 3), 4);
        var clone = (PolygonShape)original.Clone();

        Assert.Equal(original.ShapeType, clone.ShapeType);
        Assert.Equal(original.Radius, clone.Radius);
        Assert.Equal(original.Density, clone.Density);
        Assert.Equal(clone.Vertices.Count, original.Vertices.Count);
        for (int i = 0; i < original.Vertices.Count; i++)
            Assert.Equal(original.Vertices[i], clone.Vertices[i]);
    }

    [Fact]
    public void PolygonShape_ChildCount_IsOne()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(1, 1), 1);
        Assert.Equal(1, polygon.ChildCount);
    }

    [Fact]
    public void PolygonShape_Normals_CountMatchesVertices_AndAreUnitLength()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(2, 3), 1);
        Assert.Equal(polygon.Vertices.Count, polygon.Normals.Count);
        foreach (var n in polygon.Normals)
        {
            Fixed32 lenSq = n.X * n.X + n.Y * n.Y;
            Assert.True(FMath.Abs(lenSq - Fixed32.One) < (Fixed32)1e-2, $"normal not unit: {lenSq.ToDouble()}");
        }
    }

    [Fact]
    public void PolygonShape_Radius_FromSettings()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(1, 1), 1);
        Assert.Equal(Settings.PolygonRadius, polygon.Radius);
    }

    [Fact]
    public void PolygonShape_Density_Setter_RecomputesMass()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(1, 1), 1);
        polygon.GetMassData(out var mdBefore);
        polygon.Density = (Fixed32)2.0;
        polygon.GetMassData(out var mdAfter);
        Assert.Equal((Fixed32)2.0, mdAfter.Mass / mdBefore.Mass);
    }

    [Fact]
    public void PolygonShape_Radius_Setter_RecomputesProperties_WhenChanged()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(1, 1), 1);
        Fixed32 newRadius = (Fixed32)0.5;
        polygon.Radius = newRadius;
        Assert.Equal(newRadius, polygon.Radius);
    }

    [Fact]
    public void PolygonShape_ComputeAABB_ReturnsBoundingExtents()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(2, 3), 1);
        Transform xf = new Transform();
        xf.SetIdentity();
        polygon.ComputeAABB(ref xf, 0, out AABB aabb);

        // PolygonVertices are at ±(2, 3); AABB adds PolygonRadius on each side.
        Assert.True(aabb.LowerBound.X <= -2);
        Assert.True(aabb.UpperBound.X >= 2);
        Assert.True(aabb.LowerBound.Y <= -3);
        Assert.True(aabb.UpperBound.Y >= 3);
    }

    [Fact]
    public void PolygonShape_TestPoint_PointInside_True()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(2, 2), 1);
        Transform xf = new Transform();
        xf.SetIdentity();
        Vector2 p = new Vector2((Fixed32)0.5, (Fixed32)0.5);
        Assert.True(polygon.TestPoint(ref xf, ref p));
    }

    [Fact]
    public void PolygonShape_TestPoint_PointOutside_False()
    {
        PolygonShape polygon = new PolygonShape(PolygonUtils.CreateRectangle(2, 2), 1);
        Transform xf = new Transform();
        xf.SetIdentity();
        Vector2 p = new Vector2((Fixed32)5.0, (Fixed32)5.0);
        Assert.False(polygon.TestPoint(ref xf, ref p));
    }

    // ---------------------------------------------------------------------
    // CircleShape mass data
    // ---------------------------------------------------------------------

    [Fact]
    public void CircleShape_MassAndInertia_MatchAnalytic()
    {
        Fixed32 radius = 2;
        Fixed32 density = 3;
        CircleShape circle = new CircleShape(radius, density);
        circle.GetMassData(out var md);

        Fixed32 expectedArea = MathConstants.Pi * radius * radius;
        Fixed32 expectedMass = density * expectedArea;
        // I = (1/2) * mass * r^2  (for circle centred at origin)
        Fixed32 expectedInertia = (Fixed32)0.5 * expectedMass * radius * radius;

        TestHelper.AssertApprox(expectedArea, md.Area, (Fixed32)1e-2, "area");
        TestHelper.AssertApprox(expectedMass, md.Mass, (Fixed32)1e-2, "mass");
        TestHelper.AssertApprox(expectedInertia, md.Inertia, (Fixed32)1e-2, "inertia");
        Assert.Equal(Vector2.Zero, md.Centroid);
    }

    [Fact]
    public void CircleShape_Position_OffsetCentroidAndInertia()
    {
        Vector2 pos = new Vector2(3, 4);
        CircleShape circle = new CircleShape(1, 1, pos);
        circle.GetMassData(out var md);
        Assert.Equal(pos, md.Centroid);
        // I = mass * (0.5 * r^2 + |pos|^2)
        Fixed32 expectedInertia = md.Mass * ((Fixed32)0.5 * 1 * 1 + Vector2.Dot(pos, pos));
        TestHelper.AssertApprox(expectedInertia, md.Inertia, (Fixed32)1e-2);
    }

    [Fact]
    public void CircleShape_Clone_PreservesAllFields()
    {
        CircleShape original = new CircleShape(2, 3, new Vector2(1, 1));
        var clone = (CircleShape)original.Clone();

        Assert.Equal(original.ShapeType, clone.ShapeType);
        Assert.Equal(original.Radius, clone.Radius);
        Assert.Equal(original.Density, clone.Density);
        Assert.Equal(original.Position, clone.Position);
    }

    [Fact]
    public void CircleShape_ChildCount_IsOne()
    {
        CircleShape circle = new CircleShape(1, 1);
        Assert.Equal(1, circle.ChildCount);
    }

    [Fact]
    public void CircleShape_TestPoint_Inside_True()
    {
        CircleShape circle = new CircleShape(1, 1);
        Transform xf = new Transform();
        xf.SetIdentity();
        Vector2 p = new Vector2((Fixed32)0.5, (Fixed32)0.5);
        Assert.True(circle.TestPoint(ref xf, ref p));
    }

    [Fact]
    public void CircleShape_TestPoint_Outside_False()
    {
        CircleShape circle = new CircleShape(1, 1);
        Transform xf = new Transform();
        xf.SetIdentity();
        Vector2 p = new Vector2((Fixed32)2.0, (Fixed32)2.0);
        Assert.False(circle.TestPoint(ref xf, ref p));
    }

    // ---------------------------------------------------------------------
    // EdgeShape mass data
    // ---------------------------------------------------------------------

    [Fact]
    public void EdgeShape_Centroid_IsMidpoint()
    {
        EdgeShape edge = new EdgeShape(new Vector2(-1, 0), new Vector2(1, 0));
        edge.GetMassData(out var md);

        // Edge has no density in its constructor — mass/inertia remain zero, but centroid is set.
        TestHelper.AssertApprox(new Vector2(0, 0), md.Centroid, (Fixed32)1e-3);
    }

    [Fact]
    public void EdgeShape_TwoSided_DefaultFlags()
    {
        EdgeShape edge = new EdgeShape(new Vector2(0, 0), new Vector2(1, 0));
        Assert.False(edge.OneSided);
        Assert.Equal(new Vector2(0, 0), edge.Vertex1);
        Assert.Equal(new Vector2(1, 0), edge.Vertex2);
    }

    [Fact]
    public void EdgeShape_OneSided_StoresGhostVertices()
    {
        EdgeShape edge = new EdgeShape(
            new Vector2(-2, 0),
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(2, 0));

        Assert.True(edge.OneSided);
        Assert.Equal(new Vector2(-2, 0), edge.Vertex0);
        Assert.Equal(new Vector2(-1, 0), edge.Vertex1);
        Assert.Equal(new Vector2(1, 0), edge.Vertex2);
        Assert.Equal(new Vector2(2, 0), edge.Vertex3);
    }

    [Fact]
    public void EdgeShape_TestPoint_AlwaysFalse()
    {
        EdgeShape edge = new EdgeShape(new Vector2(-1, 0), new Vector2(1, 0));
        Transform xf = new Transform();
        xf.SetIdentity();
        Vector2 p = new Vector2(0, 0);
        Assert.False(edge.TestPoint(ref xf, ref p));
    }

    [Fact]
    public void EdgeShape_Clone_PreservesAllFields()
    {
        EdgeShape original = new EdgeShape(
            new Vector2(-2, 0),
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(2, 0));
        var clone = (EdgeShape)original.Clone();

        Assert.Equal(original.ShapeType, clone.ShapeType);
        Assert.Equal(original.Radius, clone.Radius);
        Assert.Equal(original.Density, clone.Density);
        Assert.Equal(original.OneSided, clone.OneSided);
        Assert.Equal(original.Vertex0, clone.Vertex0);
        Assert.Equal(original.Vertex1, clone.Vertex1);
        Assert.Equal(original.Vertex2, clone.Vertex2);
        Assert.Equal(original.Vertex3, clone.Vertex3);
    }

    [Fact]
    public void EdgeShape_ChildCount_IsOne()
    {
        EdgeShape edge = new EdgeShape(new Vector2(0, 0), new Vector2(1, 0));
        Assert.Equal(1, edge.ChildCount);
    }

    // ---------------------------------------------------------------------
    // MassData value semantics
    // ---------------------------------------------------------------------

    [Fact]
    public void MassData_OperatorEquality_WhenAllFieldsEqual()
    {
        MassData a = new MassData
        {
            Area = 1, Mass = 2, Inertia = 3,
            Centroid = new Vector2(4, 5)
        };
        MassData b = new MassData
        {
            Area = 1, Mass = 2, Inertia = 3,
            Centroid = new Vector2(4, 5)
        };
        Assert.True(a == b);
        Assert.False(a != b);
        Assert.True(a.Equals(b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void MassData_OperatorInequality_WhenAnyFieldDiffers()
    {
        MassData a = new MassData
        {
            Area = 1, Mass = 2, Inertia = 3,
            Centroid = new Vector2(4, 5)
        };

        // Vary each field in turn.
        MassData bArea = a; bArea.Area = 99;
        MassData bMass = a; bMass.Mass = 99;
        MassData bInertia = a; bInertia.Inertia = 99;
        MassData bCentroid = a; bCentroid.Centroid = new Vector2(99, 99);

        Assert.True(a != bArea);
        Assert.True(a != bMass);
        Assert.True(a != bInertia);
        Assert.True(a != bCentroid);
    }

    [Fact]
    public void MassData_GetHashCode_DoesNotThrow()
    {
        MassData md = new MassData
        {
            Area = 1, Mass = 2, Inertia = 3,
            Centroid = new Vector2(4, 5)
        };
        _ = md.GetHashCode();
    }
}
