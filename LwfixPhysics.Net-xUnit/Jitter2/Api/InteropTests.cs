using System.Numerics;

namespace SimplexLab.LwfixPhysics.Jitter2.Test.Api;

public sealed class InteropTests
{
    // --- Tuple conversions ----------------------------------------------------

    [Fact]
    public void JVector_TupleConversion_AssignsComponents()
    {
        JVector v = ((Real)1, (Real)(-2), (Real)3.5);

        Assert.Equal(1f, (float)v.X);
        Assert.Equal(-2f, (float)v.Y);
        Assert.Equal(3.5f, (float)v.Z);
    }

    [Fact]
    public void JQuaternion_TupleConversion_AssignsComponents()
    {
        JQuaternion q = ((Real)1, (Real)(-2), (Real)3.5, (Real)(-4));

        Assert.Equal(1f, (float)q.X);
        Assert.Equal(-2f, (float)q.Y);
        Assert.Equal(3.5f, (float)q.Z);
        Assert.Equal(-4f, (float)q.W);
    }

    // --- System.Numerics conversions -----------------------------------------

    [Fact]
    public void JVector_SystemNumerics_Vector3_Conversion_Roundtrip()
    {
        var v = new JVector((Real)1.25, (Real)(-2.5), (Real)3.75);

        Vector3 n = v;       // JVector -> Vector3
        JVector back = n;    // Vector3 -> JVector

        Assert.Equal(1.25f, n.X);
        Assert.Equal(-2.5f, n.Y);
        Assert.Equal(3.75f, n.Z);

        Assert.Equal(1.25f, (float)back.X);
        Assert.Equal(-2.5f, (float)back.Y);
        Assert.Equal(3.75f, (float)back.Z);
    }

    [Fact]
    public void JQuaternion_SystemNumerics_Quaternion_Conversion_Roundtrip()
    {
        var q = new JQuaternion((Real)0.1, (Real)(-0.2), (Real)0.3, (Real)0.4);

        Quaternion n = q;       // JQuaternion -> Quaternion
        JQuaternion back = n;   // Quaternion -> JQuaternion

        Assert.Equal(0.1f, n.X);
        Assert.Equal(-0.2f, n.Y);
        Assert.Equal(0.3f, n.Z);
        Assert.Equal(0.4f, n.W);

        Assert.Equal(0.1f, (float)back.X);
        Assert.Equal(-0.2f, (float)back.Y);
        Assert.Equal(0.3f, (float)back.Z);
        Assert.Equal(0.4f, (float)back.W);
    }
}
