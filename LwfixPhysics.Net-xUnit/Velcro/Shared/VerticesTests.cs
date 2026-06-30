using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Test;
using Xunit;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Shared;

/// <summary>
/// Additional coverage for the <see cref="Vertices"/> helper class — area, centroid,
/// convexity, winding, point-in-polygon, AABB and vertex iteration helpers.
/// </summary>
public class VerticesTests
{
    // ---------------------------------------------------------------------
    // Construction & basic helpers
    // ---------------------------------------------------------------------

    [Fact]
    public void Constructor_Capacity_PreservesCapacity()
    {
        Vertices v = new Vertices(4);
        Assert.Empty(v);
        v.Add(Vector2.Zero);
        Assert.Single(v);
    }

    [Fact]
    public void Constructor_FromEnumerable_CopiesAll()
    {
        var src = new[] { new Vector2(1, 2), new Vector2(3, 4) };
        Vertices v = new Vertices(src);
        Assert.Equal(2, v.Count);
        Assert.Equal(new Vector2(1, 2), v[0]);
        Assert.Equal(new Vector2(3, 4), v[1]);
    }

    [Fact]
    public void NextIndex_WrapsAround()
    {
        Vertices v = new Vertices(new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1) });

        Assert.Equal(1, v.NextIndex(0));
        Assert.Equal(2, v.NextIndex(1));
        Assert.Equal(0, v.NextIndex(2));
    }

    [Fact]
    public void PreviousIndex_WrapsAround()
    {
        Vertices v = new Vertices(new[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1) });

        Assert.Equal(2, v.PreviousIndex(0));
        Assert.Equal(0, v.PreviousIndex(1));
        Assert.Equal(1, v.PreviousIndex(2));
    }

    [Fact]
    public void NextVertex_AndPreviousVertex_ReturnWrappedVertex()
    {
        Vertices v = new Vertices(new[] { new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0) });
        Assert.Equal(new Vector2(1, 0), v.NextVertex(2));
        Assert.Equal(new Vector2(3, 0), v.PreviousVertex(0));
    }

    // ---------------------------------------------------------------------
    // Geometry: area, centroid, winding
    // ---------------------------------------------------------------------

    [Fact]
    public void GetSignedArea_CounterClockwise_IsPositive()
    {
        // Unit square in CCW order.
        Vertices square = new Vertices(new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
        });

        Fixed32 area = square.GetSignedArea();
        Assert.True(area > Fixed32.Zero);
        Assert.Equal(4, area.ToDouble(), 5);
    }

    [Fact]
    public void GetSignedArea_Clockwise_IsNegative()
    {
        Vertices square = new Vertices(new[]
        {
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(1, -1),
        });

        Fixed32 area = square.GetSignedArea();
        Assert.True(area < Fixed32.Zero);
        Assert.Equal(-4, area.ToDouble(), 5);
    }

    [Fact]
    public void GetArea_AlwaysPositive()
    {
        Vertices cw = new Vertices(new[]
        {
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(1, -1),
        });
        Vertices ccw = new Vertices(new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
        });

        Assert.Equal(4, cw.GetArea().ToDouble(), 5);
        Assert.Equal(4, ccw.GetArea().ToDouble(), 5);
    }

    [Fact]
    public void GetSignedArea_LessThanThreeVertices_ReturnsZero()
    {
        Vertices v = new Vertices { new Vector2(0, 0), new Vector2(1, 1) };
        Assert.Equal(Fixed32.Zero, v.GetSignedArea());
    }

    [Fact]
    public void GetCentroid_OfUnitSquareAtOrigin_IsOrigin()
    {
        Vertices square = new Vertices(new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
        });

        Vector2 centroid = square.GetCentroid();
        TestHelper.AssertApprox(Vector2.Zero, centroid, (Fixed32)1e-3);
    }

    [Fact]
    public void GetCentroid_OfTranslatedSquare_IsTranslatedCentroid()
    {
        Vertices square = new Vertices(new[]
        {
            new Vector2(9, 9),
            new Vector2(11, 9),
            new Vector2(11, 11),
            new Vector2(9, 11),
        });

        Vector2 centroid = square.GetCentroid();
        TestHelper.AssertApprox(new Vector2(10, 10), centroid, (Fixed32)1e-3);
    }

    [Fact]
    public void IsCounterClockWise_TrueForCCW()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
        });
        Assert.True(v.IsCounterClockWise());
    }

    [Fact]
    public void IsCounterClockWise_FalseForCW()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0),
        });
        Assert.False(v.IsCounterClockWise());
    }

    [Fact]
    public void ForceCounterClockWise_ReversesIfCW()
    {
        Vertices cw = new Vertices(new[]
        {
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0),
        });
        Assert.False(cw.IsCounterClockWise());
        cw.ForceCounterClockWise();
        Assert.True(cw.IsCounterClockWise());
    }

    [Fact]
    public void IsConvex_Triangle_True()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
        });
        Assert.True(v.IsConvex());
    }

    [Fact]
    public void IsConvex_ConvexQuadrilateral_True()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
        });
        Assert.True(v.IsConvex());
    }

    [Fact]
    public void IsConvex_ConcaveLShape_False()
    {
        // L-shaped polygon — concave.
        Vertices v = new Vertices(new[]
        {
            new Vector2(0, 0),
            new Vector2(2, 0),
            new Vector2(2, 1),
            new Vector2(1, 1),
            new Vector2(1, 2),
            new Vector2(0, 2),
        });
        Assert.False(v.IsConvex());
    }

    [Fact]
    public void IsConvex_TooFewVertices_False()
    {
        Vertices v = new Vertices { new Vector2(0, 0), new Vector2(1, 1) };
        Assert.False(v.IsConvex());
    }

    // ---------------------------------------------------------------------
    // AABB / point-in-polygon
    // ---------------------------------------------------------------------

    [Fact]
    public void GetAABB_ReturnsBoundingExtents()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(3, 4),
            new Vector2(-1, 7),
            new Vector2(2, -2),
        });
        AABB aabb = v.GetAABB();
        Assert.Equal(new Vector2(-1, -2), aabb.LowerBound);
        Assert.Equal(new Vector2(3, 7), aabb.UpperBound);
    }

    [Fact]
    public void PointInPolygon_PointInside_ReturnsOne()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
        });
        Vector2 p = new Vector2((Fixed32)0.5, (Fixed32)0.5);
        Assert.Equal(1, v.PointInPolygon(ref p));
    }

    [Fact]
    public void PointInPolygon_PointOutside_ReturnsMinusOne()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(-1, -1),
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-1, 1),
        });
        Vector2 p = new Vector2((Fixed32)5.0, (Fixed32)5.0);
        Assert.Equal(-1, v.PointInPolygon(ref p));
    }

    // ---------------------------------------------------------------------
    // Mutations
    // ---------------------------------------------------------------------

    [Fact]
    public void Translate_ShiftsAllVertices()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
        });
        Vector2 offset = new Vector2(10, -5);
        v.Translate(ref offset);

        Assert.Equal(new Vector2(10, -5), v[0]);
        Assert.Equal(new Vector2(11, -5), v[1]);
        Assert.Equal(new Vector2(11, -4), v[2]);
    }

    [Fact]
    public void Scale_MultipliesAllVertices()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(1, 2),
            new Vector2(-3, 4),
        });
        Vector2 scale = new Vector2(2, 3);
        v.Scale(ref scale);

        Assert.Equal(new Vector2(2, 6), v[0]);
        Assert.Equal(new Vector2(-6, 12), v[1]);
    }

    [Fact]
    public void Rotate_RotatesAboutOrigin()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(1, 0),
            new Vector2(0, 1),
        });

        v.Rotate(Fixed32.Half_PI); // 90°

        // (1, 0) -> (0, 1); (0, 1) -> (-1, 0)
        TestHelper.AssertApprox(new Vector2(0, 1), v[0], (Fixed32)1e-3);
        TestHelper.AssertApprox(new Vector2(-1, 0), v[1], (Fixed32)1e-3);
    }

    [Fact]
    public void FlipHorizontally_NegatesX()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(1, 2),
            new Vector2(-3, 4),
        });
        v.FlipHorizontally();

        Assert.Equal(new Vector2(-1, 2), v[0]);
        Assert.Equal(new Vector2(3, 4), v[1]);
    }

    [Fact]
    public void FlipVertically_NegatesY()
    {
        Vertices v = new Vertices(new[]
        {
            new Vector2(1, 2),
            new Vector2(-3, 4),
        });
        v.FlipVertically();

        Assert.Equal(new Vector2(1, -2), v[0]);
        Assert.Equal(new Vector2(-3, -4), v[1]);
    }
}
