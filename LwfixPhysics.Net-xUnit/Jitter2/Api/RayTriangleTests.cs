namespace SimplexLab.LwfixPhysics.Jitter2.Test.Api;

public class RayIntersectTests
{
    private JTriangle tri;

    public RayIntersectTests()
    {
        // Triangle in XY plane with counter-clockwise winding
        tri = new JTriangle
        {
            V0 = new JVector(0, 0, 0),
            V1 = new JVector(1, 0, 0),
            V2 = new JVector(0, 1, 0)
        };
    }

    [Fact]
    public void RayHitsFrontFace_CullNone_ReturnsTrue()
    {
        var origin = new JVector((Real)0.25, (Real)0.25, 1);
        var direction = new JVector(0, 0, -1);

        bool hit = tri.RayIntersect(origin, direction, JTriangle.CullMode.None, out var normal, out var lambda);

        Assert.True(hit);
        Assert.True(MathHelper.IsZero(lambda - (Real)1.0));
        Assert.True(MathHelper.IsZero(normal - JVector.UnitZ));
    }

    [Fact]
    public void RayHitsBackFace_CullNone_ReturnsTrue()
    {
        var origin = new JVector((Real)0.25, (Real)0.25, -1);
        var direction = new JVector(0, 0, 1);

        bool hit = tri.RayIntersect(origin, direction, JTriangle.CullMode.None, out var normal, out var lambda);

        Assert.True(hit);
        Assert.True(MathHelper.IsZero(lambda - (Real)1.0));
        Assert.True(MathHelper.IsZero(normal + JVector.UnitZ));
    }

    [Fact]
    public void RayHitsFrontFace_CullFront_ReturnsFalse()
    {
        var origin = new JVector((Real)0.25, (Real)0.25, 1);
        var direction = new JVector(0, 0, -1);

        bool hit = tri.RayIntersect(origin, direction, JTriangle.CullMode.FrontFacing, out var normal, out var lambda);

        Assert.False(hit);
    }

    [Fact]
    public void RayHitsBackFace_CullFront_ReturnsTrue()
    {
        var origin = new JVector((Real)0.25, (Real)0.25, -1);
        var direction = new JVector(0, 0, 1);

        bool hit = tri.RayIntersect(origin, direction, JTriangle.CullMode.FrontFacing, out var normal, out var lambda);

        Assert.True(hit);
        Assert.True(MathHelper.IsZero(lambda - (Real)1.0));
        Assert.True(MathHelper.IsZero(normal + JVector.UnitZ));
    }

    [Fact]
    public void RayHitsFrontFace_CullBack_ReturnsTrue()
    {
        var origin = new JVector((Real)0.25, (Real)0.25, 1);
        var direction = new JVector(0, 0, -1);

        bool hit = tri.RayIntersect(origin, direction, JTriangle.CullMode.BackFacing, out var normal, out var lambda);

        Assert.True(hit);
        Assert.True(MathHelper.IsZero(lambda - (Real)1.0));
        Assert.True(MathHelper.IsZero(normal - JVector.UnitZ));
    }

    [Fact]
    public void RayHitsBackFace_CullBack_ReturnsFalse()
    {
        var origin = new JVector((Real)0.25, (Real)0.25, -1);
        var direction = new JVector(0, 0, 1);

        bool hit = tri.RayIntersect(origin, direction, JTriangle.CullMode.BackFacing, out var normal, out var lambda);

        Assert.False(hit);
    }
}
