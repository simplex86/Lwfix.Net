namespace SimplexLab.LwfixPhysics.Jitter2.Test.Api;

public class SupportMapTests
{
    [Fact]
    public void SupportSphere_ReturnsPointAlongNormalizedDirection()
    {
        var sphere = SupportPrimitives.CreateSphere((Real)2.0);

        sphere.SupportMap(new JVector((Real)3.0, (Real)0.0, (Real)0.0), out JVector result);

        Assert.True(MathHelper.CloseToZero(result - new JVector((Real)2.0, (Real)0.0, (Real)0.0), (Real)1e-6));
    }

    [Fact]
    public void SupportBox_ReturnsCornerMatchingDirectionSigns()
    {
        var box = SupportPrimitives.CreateBox(new JVector((Real)1.0, (Real)2.0, (Real)3.0));

        box.SupportMap(new JVector(-(Real)4.0, (Real)5.0, -(Real)6.0), out JVector result);

        Assert.Equal(new JVector(-(Real)1.0, (Real)2.0, -(Real)3.0), result);
    }

    [Fact]
    public void SupportCapsule_ReturnsSegmentEndpointPlusSphereOffset()
    {
        var capsule = SupportPrimitives.CreateCapsule((Real)0.5, (Real)2.0);

        capsule.SupportMap(new JVector((Real)0.0, (Real)3.0, (Real)0.0), out JVector result);

        Assert.True(MathHelper.CloseToZero(result - new JVector((Real)0.0, (Real)2.5, (Real)0.0), (Real)1e-6));
    }

    [Fact]
    public void SupportCylinder_ReturnsCapPointOnRim()
    {
        var cylinder = SupportPrimitives.CreateCylinder((Real)2.0, (Real)3.0);

        cylinder.SupportMap(new JVector((Real)4.0, (Real)1.0, (Real)0.0), out JVector result);

        Assert.Equal(new JVector((Real)2.0, (Real)3.0, (Real)0.0), result);
    }

    [Fact]
    public void SupportCone_ReturnsTipWhenLookingUp()
    {
        var cone = SupportPrimitives.CreateCone((Real)2.0, (Real)4.0);

        cone.SupportMap(new JVector((Real)0.0, (Real)3.0, (Real)0.0), out JVector result);

        Assert.Equal(new JVector((Real)0.0, (Real)3.0, (Real)0.0), result);
    }

    [Fact]
    public void SupportPrimitives_HaveCenterAtOrigin()
    {
        ISupportMappable[] supports =
        [
            SupportPrimitives.CreatePoint(),
            SupportPrimitives.CreateSphere((Real)1.0),
            SupportPrimitives.CreateBox(new JVector((Real)1.0, (Real)1.0, (Real)1.0)),
            SupportPrimitives.CreateCapsule((Real)0.5, (Real)1.0),
            SupportPrimitives.CreateCylinder((Real)1.0, (Real)1.0),
            SupportPrimitives.CreateCone((Real)1.0, (Real)2.0)
        ];

        foreach (var support in supports)
        {
            support.GetCenter(out JVector center);
            Assert.Equal(JVector.Zero, center);
        }
    }

    [Fact]
    public void VertexSupportMap_ScalarAndAcceleratedAgreeOnTiedMaximum()
    {
        VertexSupportMap map =
            new([
                new JVector((Real)1.0, (Real)0.0, (Real)0.0),
                new JVector((Real)1.0, (Real)1.0, (Real)0.0),
                new JVector((Real)2.0, (Real)0.0, (Real)0.0),
                new JVector((Real)2.0, (Real)1.0, (Real)0.0)
            ]);

        JVector direction = JVector.UnitX;

        map.SupportMapScalarForTests(direction, out JVector scalar);
        map.SupportMapAcceleratedForTests(direction, out JVector accelerated);

        Assert.Equal(scalar, accelerated);
        Assert.Equal(new JVector((Real)2.0, (Real)1.0, (Real)0.0), accelerated);
    }
}
