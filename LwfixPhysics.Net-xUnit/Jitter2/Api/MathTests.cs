namespace SimplexLab.LwfixPhysics.Jitter2.Test.Api;

public class MathTests
{
    /// <summary>
    /// Verifies that FMath.Atan2 matches Fixed32.Atan2 directly.
    /// This guards against the historical arg-swap bug where FMath.Atan2(y, x)
    /// called T.Atan2(x, y) instead of T.Atan2(y, x).
    /// </summary>
    [Fact]
    public static void FMath_Atan2_MatchesFixed32_AfterBugFix()
    {
        (Real y, Real x)[] inputs =
        [
            ((Real)0.0, (Real)1.0),
            ((Real)1.0, (Real)0.0),
            ((Real)1.0, (Real)1.0),
            ((Real)(-1.0), (Real)1.0),
            ((Real)1.0, (Real)(-1.0)),
            ((Real)(-1.0), (Real)(-1.0)),
            ((Real)0.5, (Real)0.866),
            ((Real)(-0.5), (Real)0.866),
            ((Real)0.866, (Real)0.5),
            ((Real)(-0.866), (Real)(-0.5))
        ];

        foreach (var (y, x) in inputs)
        {
            Real expected = Fixed32.Atan2(y, x);
            Real actual = MathR.Atan2(y, x);
            Assert.True(MathR.Abs(actual - expected) < (Real)1e-6,
                $"Atan2({y}, {x}): expected {expected.ToDouble()}, got {actual.ToDouble()}");
        }
    }

    /// <summary>
    /// Verifies that FMath.SinCos returns (sin, cos) consistent with individual
    /// FMath.Sin and FMath.Cos calls.
    /// </summary>
    [Fact]
    public static void FMath_SinCos_ConsistentWithIndividualCalls()
    {
        Real[] angles =
        [
            (Real)(-16.0) * Real.PI,
            (Real)(-3.0) * Real.PI,
            -Real.PI,
            (Real)(-0.75) * Real.PI,
            (Real)(-0.5) * Real.PI,
            (Real)(-0.25) * Real.PI,
            (Real)(-0.1),
            (Real)0.0,
            (Real)0.1,
            (Real)0.25 * Real.PI,
            (Real)0.5 * Real.PI,
            (Real)0.75 * Real.PI,
            Real.PI,
            (Real)3.0 * Real.PI,
            (Real)16.0 * Real.PI
        ];

        foreach (Real angle in angles)
        {
            var (sin, cos) = MathR.SinCos(angle);
            Assert.True(MathR.Abs(sin - MathR.Sin(angle)) < (Real)2e-6, $"sin({angle})");
            Assert.True(MathR.Abs(cos - MathR.Cos(angle)) < (Real)2e-6, $"cos({angle})");
        }
    }

    /// <summary>
    /// Verifies inverse trig functions match the underlying Fixed32 implementation
    /// on the unit interval.
    /// </summary>
    [Fact]
    public static void FMath_InverseTrig_MatchesFixed32_OnUnitInterval()
    {
        for (int i = -2000; i <= 2000; i++)
        {
            Real value = i / (Real)2000.0;

            Assert.True(MathR.Abs(MathR.Asin(value) - Fixed32.Asin(value)) < (Real)5e-6,
                $"asin({value})");
            Assert.True(MathR.Abs(MathR.Acos(value) - Fixed32.Acos(value)) < (Real)5e-6,
                $"acos({value})");
        }
    }

    [Fact]
    public static void QMatrixProjectMultiplyLeftRight()
    {
        JQuaternion jq1 = new((Real)0.2, (Real)0.3, (Real)0.4, (Real)0.5);
        JQuaternion jq2 = new((Real)0.1, (Real)0.7, (Real)0.1, (Real)0.8);

        var qm1 = QMatrix.CreateLeftMatrix(jq1);
        var qm2 = QMatrix.CreateRightMatrix(jq2);

        JMatrix res1 = QMatrix.Multiply(ref qm1, ref qm2).Projection();
        JMatrix res2 = QMatrix.ProjectMultiplyLeftRight(jq1, jq2);

        JMatrix delta = res1 - res2;
        Assert.True(JVector.MaxAbs(delta.GetColumn(0)) < (Real)1e-06);
        Assert.True(JVector.MaxAbs(delta.GetColumn(1)) < (Real)1e-06);
        Assert.True(JVector.MaxAbs(delta.GetColumn(2)) < (Real)1e-06);
    }

    [Fact]
    public static void TransformTests()
    {
        JVector a = JVector.UnitX;
        JVector b = JVector.UnitY;
        JVector c = JVector.UnitZ;

        Assert.True((a - b % c).Length() < (Real)1e-06);
        Assert.True((b - c % a).Length() < (Real)1e-06);
        Assert.True((c - a % b).Length() < (Real)1e-06);

        JMatrix ar = JMatrix.CreateRotationX((Real)0.123) *
                     JMatrix.CreateRotationY((Real)0.321) *
                     JMatrix.CreateRotationZ((Real)0.213);

        JVector.Transform(a, ar, out a);
        JVector.Transform(b, ar, out b);
        JVector.Transform(c, ar, out c);

        Assert.True((a - b % c).Length() < (Real)1e-06);
        Assert.True((b - c % a).Length() < (Real)1e-06);
        Assert.True((c - a % b).Length() < (Real)1e-06);

        JMatrix.Inverse(ar, out ar);

        JVector.Transform(a, ar, out a);
        JVector.Transform(b, ar, out b);
        JVector.Transform(c, ar, out c);

        Assert.True((a - JVector.UnitX).Length() < (Real)1e-06);
        Assert.True((b - JVector.UnitY).Length() < (Real)1e-06);
        Assert.True((c - JVector.UnitZ).Length() < (Real)1e-06);

        // ---
        // https://arxiv.org/abs/1801.07478

        Real cos = MathR.Cos((Real)(0.321 / 2.0));
        Real sin = MathR.Sin((Real)(0.321 / 2.0));
        JQuaternion quat1 = new(sin, 0, 0, cos);
        JQuaternion quat2 = JQuaternion.CreateFromMatrix(JMatrix.CreateRotationZ((Real)0.321));
        JQuaternion quat = JQuaternion.Multiply(quat1, quat2);
        JQuaternion tv = new(1, 2, 3, 0);
        JQuaternion tmp = JQuaternion.Multiply(JQuaternion.Multiply(quat, tv), JQuaternion.Conjugate(quat));
        JVector resQuaternion = new(tmp.X, tmp.Y, tmp.Z);

        JVector.Transform(new JVector(1, 2, 3), JMatrix.CreateFromQuaternion(quat), out JVector resMatrix1);
        Assert.True((resMatrix1 - resQuaternion).Length() < (Real)1e-06);

        JMatrix rot1 = JMatrix.CreateRotationX((Real)0.321);
        JMatrix rot2 = JMatrix.CreateRotationZ((Real)0.321);
        JMatrix rot = JMatrix.Multiply(rot1, rot2);
        JVector.Transform(new JVector(1, 2, 3), rot, out JVector resMatrix2);

        Assert.True((resMatrix2 - resQuaternion).Length() < (Real)1e-06);
    }
}
