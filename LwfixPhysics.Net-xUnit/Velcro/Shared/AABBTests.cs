using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.RayCast;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using Xunit;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Shared;

/// <summary>
/// Port of <c>VelcroPhysics.Tests/Tests/Shared/AABBTests.cs</c> adapted to the Fixed32 type,
/// plus additional coverage for the AABB helpers (Combine, Contains, IsValid, RayCast, properties).
/// </summary>
public class AABBTests
{
    // ---------------------------------------------------------------------
    // Original VelcroPhysics test (ported)
    // ---------------------------------------------------------------------

    [Fact]
    public void TestOverlap()
    {
        {
            AABB bb1 = new AABB(new Vector2(-2, -3), new Vector2(-1, 0));
            Assert.True(AABB.TestOverlap(ref bb1, ref bb1));
        }
        {
            Vector2 vec = new Vector2(-2, -3);
            AABB bb1 = new AABB(vec, vec);
            Assert.True(AABB.TestOverlap(ref bb1, ref bb1));
        }
        {
            AABB bb1 = new AABB(new Vector2(-2, -3), new Vector2(-1, 0));
            AABB bb2 = new AABB(new Vector2(-1, -1), new Vector2(1, 2));
            Assert.True(AABB.TestOverlap(ref bb1, ref bb2));
        }
        {
            AABB bb1 = new AABB(new Vector2(-99, -3), new Vector2(-1, 0));
            AABB bb2 = new AABB(new Vector2(76, -1), new Vector2(-2, 2));
            Assert.True(AABB.TestOverlap(ref bb1, ref bb2));
        }
        {
            AABB bb1 = new AABB(new Vector2(-20, -3), new Vector2(-18, 0));
            AABB bb2 = new AABB(new Vector2(-1, -1), new Vector2(1, 2));
            Assert.False(AABB.TestOverlap(ref bb1, ref bb2));
        }
        {
            AABB bb1 = new AABB(new Vector2(-2, -3), new Vector2(-1, 0));
            AABB bb2 = new AABB(new Vector2(-1, 1), new Vector2(1, 2));
            Assert.False(AABB.TestOverlap(ref bb1, ref bb2));
        }
        {
            AABB bb1 = new AABB(new Vector2(-2, 3), new Vector2(-1, 0));
            AABB bb2 = new AABB(new Vector2(-1, -1), new Vector2(0, -2));
            Assert.False(AABB.TestOverlap(ref bb1, ref bb2));
        }
    }

    // ---------------------------------------------------------------------
    // Additional coverage
    // ---------------------------------------------------------------------

    [Fact]
    public void Constructor_SortsBounds()
    {
        // AABB swaps min/max so that LowerBound is always the lower corner.
        AABB aabb = new AABB(new Vector2(5, 7), new Vector2(1, 2));
        Assert.Equal(new Vector2(1, 2), aabb.LowerBound);
        Assert.Equal(new Vector2(5, 7), aabb.UpperBound);
    }

    [Fact]
    public void Constructor_FromCenterSize_CorrectBounds()
    {
        AABB aabb = new AABB(new Vector2(0, 0), 4, 6);
        Assert.Equal(new Vector2(-2, -3), aabb.LowerBound);
        Assert.Equal(new Vector2(2, 3), aabb.UpperBound);
    }

    [Fact]
    public void Properties_WidthHeightCenterExtentsPerimeter()
    {
        AABB aabb = new AABB(new Vector2(-1, -2), new Vector2(3, 4));

        Assert.Equal(4, aabb.Width.ToDouble());
        Assert.Equal(6, aabb.Height.ToDouble());
        Assert.Equal(new Vector2(1, 1), aabb.Center);
        Assert.Equal(new Vector2(2, 3), aabb.Extents);
        // Perimeter = 2 * (w + h) = 2 * (4 + 6) = 20
        Assert.Equal(20, aabb.Perimeter.ToDouble());
    }

    [Fact]
    public void Vertices_ReturnsFourCorners()
    {
        AABB aabb = new AABB(new Vector2(-1, -1), new Vector2(1, 1));
        var verts = aabb.Vertices;
        Assert.Equal(4, verts.Count);
        Assert.Contains(new Vector2(1, 1), verts);
        Assert.Contains(new Vector2(-1, -1), verts);
    }

    [Fact]
    public void Quadrants_PartitionTheBox()
    {
        AABB aabb = new AABB(new Vector2(-2, -2), new Vector2(2, 2));
        AABB q1 = aabb.Q1;
        AABB q2 = aabb.Q2;
        AABB q3 = aabb.Q3;
        AABB q4 = aabb.Q4;

        // Each quadrant must fit inside the original.
        Assert.True(aabb.Contains(ref q1));
        Assert.True(aabb.Contains(ref q2));
        Assert.True(aabb.Contains(ref q3));
        Assert.True(aabb.Contains(ref q4));

        // Centers of the quadrants should be the four half-quadrants.
        Assert.Equal(new Vector2(1, 1), q1.Center);
        Assert.Equal(new Vector2(-1, 1), q2.Center);
        Assert.Equal(new Vector2(-1, -1), q3.Center);
        Assert.Equal(new Vector2(1, -1), q4.Center);
    }

    [Fact]
    public void IsValid_TrueForWellFormedBounds()
    {
        AABB aabb = new AABB(new Vector2(-1, -1), new Vector2(1, 1));
        Assert.True(aabb.IsValid());
    }

    [Fact]
    public void IsValid_FalseForInvertedBounds()
    {
        // Construct via fields to bypass the auto-sorting constructor.
        AABB aabb = new AABB();
        aabb.LowerBound = new Vector2(1, 1);
        aabb.UpperBound = new Vector2(-1, -1);
        Assert.False(aabb.IsValid());
    }

    [Fact]
    public void Combine_IncludesOther()
    {
        AABB a = new AABB(new Vector2(-1, -1), new Vector2(1, 1));
        AABB b = new AABB(new Vector2(2, 2), new Vector2(3, 3));
        a.Combine(ref b);
        Assert.Equal(new Vector2(-1, -1), a.LowerBound);
        Assert.Equal(new Vector2(3, 3), a.UpperBound);
    }

    [Fact]
    public void Combine_TwoBoxes_CoversBoth()
    {
        AABB target = new AABB(new Vector2(0, 0), new Vector2(1, 1));
        AABB a = new AABB(new Vector2(-2, -2), new Vector2(-1, -1));
        AABB b = new AABB(new Vector2(2, 2), new Vector2(3, 3));
        target.Combine(ref a, ref b);
        Assert.Equal(new Vector2(-2, -2), target.LowerBound);
        Assert.Equal(new Vector2(3, 3), target.UpperBound);
    }

    [Fact]
    public void Contains_Box_TrueWhenInside()
    {
        AABB outer = new AABB(new Vector2(-10, -10), new Vector2(10, 10));
        AABB inner = new AABB(new Vector2(-1, -1), new Vector2(1, 1));
        Assert.True(outer.Contains(ref inner));
    }

    [Fact]
    public void Contains_Box_FalseWhenPartiallyOutside()
    {
        AABB outer = new AABB(new Vector2(-1, -1), new Vector2(1, 1));
        AABB inner = new AABB(new Vector2(-2, -1), new Vector2(1, 1));
        Assert.False(outer.Contains(ref inner));
    }

    [Fact]
    public void Contains_Point_TrueWhenInside()
    {
        AABB aabb = new AABB(new Vector2(-1, -1), new Vector2(1, 1));
        Vector2 p = new Vector2((Fixed32)0.5, (Fixed32)0.5);
        Assert.True(aabb.Contains(ref p));
    }

    [Fact]
    public void Contains_Point_FalseWhenOutside()
    {
        AABB aabb = new AABB(new Vector2(-1, -1), new Vector2(1, 1));
        Vector2 p = new Vector2((Fixed32)2.0, (Fixed32)2.0);
        Assert.False(aabb.Contains(ref p));
    }

    [Fact]
    public void RayCast_HitsAxisAlignedBox()
    {
        AABB aabb = new AABB(new Vector2(-1, -1), new Vector2(1, 1));

        RayCastInput input = new RayCastInput
        {
            Point1 = new Vector2(-5, 0),
            Point2 = new Vector2(5, 0),
            MaxFraction = 1
        };

        bool hit = aabb.RayCast(ref input, out RayCastOutput output);
        Assert.True(hit);
        Assert.True(output.Fraction > Fixed32.Zero && output.Fraction <= Fixed32.One);
    }

    [Fact]
    public void RayCast_MissesBoxWhenRayParallelAndOutside()
    {
        AABB aabb = new AABB(new Vector2(-1, -1), new Vector2(1, 1));

        RayCastInput input = new RayCastInput
        {
            Point1 = new Vector2(-5, 5),
            Point2 = new Vector2(5, 5),
            MaxFraction = 1
        };

        bool hit = aabb.RayCast(ref input, out _);
        Assert.False(hit);
    }

    [Fact]
    public void RayCast_MaxFractionTruncatesRay()
    {
        AABB aabb = new AABB(new Vector2(2, -1), new Vector2(4, 1));

        // Ray would hit the box if extended, but max fraction 0.5 keeps it from getting there.
        RayCastInput input = new RayCastInput
        {
            Point1 = new Vector2(0, 0),
            Point2 = new Vector2(10, 0),
            MaxFraction = (Fixed32)0.1 // ray ends at x = 1, box starts at x = 2
        };

        bool hit = aabb.RayCast(ref input, out _);
        Assert.False(hit);
    }
}
