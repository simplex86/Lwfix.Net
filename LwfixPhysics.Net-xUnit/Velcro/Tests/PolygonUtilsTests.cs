using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Test;
using SimplexLab.LwfixPhysics.Velcro.Utilities;
using Xunit;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Tests;

/// <summary>
/// Additional coverage for the static <see cref="PolygonUtils"/> helper class:
/// rectangle, line, ellipse/circle, arc, capsule, gear primitives.
/// </summary>
public class PolygonUtilsTests
{
    // ---------------------------------------------------------------------
    // CreateRectangle
    // ---------------------------------------------------------------------

    [Fact]
    public void CreateRectangle_DefaultCenter_ProducesFourCorners()
    {
        Vertices v = PolygonUtils.CreateRectangle(2, 3);
        Assert.Equal(4, v.Count);
        Assert.Contains(new Vector2(-2, -3), v);
        Assert.Contains(new Vector2(2, -3), v);
        Assert.Contains(new Vector2(2, 3), v);
        Assert.Contains(new Vector2(-2, 3), v);
    }

    [Fact]
    public void CreateRectangle_WithCenterAndAngle_TranslatesAndRotates()
    {
        Vector2 center = new Vector2(10, -5);
        Fixed32 angle = Fixed32.Zero;
        Vertices v = PolygonUtils.CreateRectangle(1, 1, center, angle);
        Assert.Equal(4, v.Count);
        // For angle 0, vertices are simply (center ± (1,1)).
        Assert.Contains(new Vector2(9, -6), v);
        Assert.Contains(new Vector2(11, -6), v);
        Assert.Contains(new Vector2(11, -4), v);
        Assert.Contains(new Vector2(9, -4), v);
    }

    [Fact]
    public void CreateRectangle_WithRotation_RotatesAroundCenter()
    {
        Vector2 center = Vector2.Zero;
        Fixed32 angle = Fixed32.PI; // 180°
        Vertices v = PolygonUtils.CreateRectangle(1, 1, center, angle);
        // After 180° rotation, the (-1, -1) corner maps to (1, 1).
        // Use loose tolerance because Fixed32 trig is approximate.
        bool foundFlipped = false;
        foreach (var p in v)
        {
            if (FMath.Abs(p.X - 1) < (Fixed32)1e-3 && FMath.Abs(p.Y - 1) < (Fixed32)1e-3)
            {
                foundFlipped = true;
                break;
            }
        }
        Assert.True(foundFlipped, "expected a vertex near (1,1) after 180° rotation");
    }

    // ---------------------------------------------------------------------
    // CreateLine / CreateCircle / CreateEllipse / CreateArc
    // ---------------------------------------------------------------------

    [Fact]
    public void CreateLine_ReturnsTwoEndpoints()
    {
        Vector2 a = new Vector2(1, 2);
        Vector2 b = new Vector2(3, 4);
        Vertices v = PolygonUtils.CreateLine(a, b);
        Assert.Equal(2, v.Count);
        Assert.Equal(a, v[0]);
        Assert.Equal(b, v[1]);
    }

    [Fact]
    public void CreateCircle_HasCorrectVertexCount()
    {
        int segments = 16;
        Vertices v = PolygonUtils.CreateCircle(1, segments);
        // CreateEllipse adds 1 vertex on the positive X-axis and segments-1 around.
        Assert.Equal(segments, v.Count);
    }

    [Fact]
    public void CreateCircle_AllVerticesAtGivenRadius()
    {
        int segments = 8;
        Fixed32 radius = 5;
        Vertices v = PolygonUtils.CreateCircle(radius, segments);
        foreach (var p in v)
        {
            Fixed32 r = Fixed32.Sqrt(p.X * p.X + p.Y * p.Y);
            TestHelper.AssertApprox(radius, r, (Fixed32)1e-2);
        }
    }

    [Fact]
    public void CreateEllipse_HasCorrectVertexCount()
    {
        Vertices v = PolygonUtils.CreateEllipse(2, 1, 12);
        Assert.Equal(12, v.Count);
    }

    [Fact]
    public void CreateArc_ReturnsOneVertexPerStepMinusOne()
    {
        int sides = 8;
        Vertices v = PolygonUtils.CreateArc(Fixed32.Half_PI, sides, 1);
        // The loop runs from sides-1 down to 1, so we expect sides-1 vertices.
        Assert.Equal(sides - 1, v.Count);
    }

    // ---------------------------------------------------------------------
    // CreateCapsule
    // ---------------------------------------------------------------------

    [Fact]
    public void CreateCapsule_Symmetric_HasExpectedVertexCount()
    {
        // height = 4, endRadius = 1, edges = 4
        // For each end: 1 vertex at the start, (edges-1) intermediate, 1 at the end -> edges + 1 vertices
        // Total = 2 * (edges + 1) = 10
        Vertices v = PolygonUtils.CreateCapsule(4, 1, 4);
        Assert.Equal(10, v.Count);
    }

    [Fact]
    public void CreateCapsule_BadRadius_Throws()
    {
        Assert.Throws<ArgumentException>(() => PolygonUtils.CreateCapsule(2, 1, 4));
    }

    [Fact]
    public void CreateCapsule_NegativeHeight_Throws()
    {
        Assert.Throws<ArgumentException>(() => PolygonUtils.CreateCapsule(-1, 1, 4));
    }

    [Fact]
    public void CreateCapsule_ZeroTopRadius_Throws()
    {
        Assert.Throws<ArgumentException>(() => PolygonUtils.CreateCapsule(4, 0, 4));
    }

    [Fact]
    public void CreateCapsule_ZeroTopEdges_Throws()
    {
        Assert.Throws<ArgumentException>(() => PolygonUtils.CreateCapsule(4, 1, 0));
    }

    // ---------------------------------------------------------------------
    // CreateGear
    // ---------------------------------------------------------------------

    [Fact]
    public void CreateGear_ReturnsNonEmptyVertices()
    {
        Vertices v = PolygonUtils.CreateGear(2, 8, 50, (Fixed32)0.5);
        Assert.NotEmpty(v);
        // Each tooth contributes 3 or 4 vertices depending on tip percentage;
        // for tipPercentage != 0 we expect 4 per tooth = 32 vertices total.
        Assert.Equal(32, v.Count);
    }

    [Fact]
    public void CreateGear_TipPercentageZero_ReturnsTwoVerticesPerTooth()
    {
        Vertices v = PolygonUtils.CreateGear(2, 8, 0, (Fixed32)0.5);
        Assert.Equal(16, v.Count);
    }

    // ---------------------------------------------------------------------
    // CreateRoundedRectangle (basic shape)
    // ---------------------------------------------------------------------

    [Fact]
    public void CreateRoundedRectangle_SegmentsZero_ReturnsEightVertices()
    {
        Vertices v = PolygonUtils.CreateRoundedRectangle(4, 4, 1, 1, 0);
        Assert.Equal(8, v.Count);
    }

    [Fact]
    public void CreateRoundedRectangle_TooLargeRadius_Throws()
    {
        Assert.Throws<Exception>(() =>
            PolygonUtils.CreateRoundedRectangle(2, 2, 2, 1, 0));
    }

    [Fact]
    public void CreateRoundedRectangle_NegativeSegments_Throws()
    {
        Assert.Throws<Exception>(() =>
            PolygonUtils.CreateRoundedRectangle(4, 4, 1, 1, -1));
    }
}
