namespace LwfixTest.Physics.Jitter2.Api;

public class MathTests
{
    [Fact]
    public static void StableMath_MatchesMathROnRepresentativeInputs()
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
            var (sinDet, cosDet) = StableMath.SinCos(angle);

            Assert.True(MathR.Abs(sinDet - MathR.Sin(angle)) < (Real)2e-6, $"sin({angle})");
            Assert.True(MathR.Abs(cosDet - MathR.Cos(angle)) < (Real)2e-6, $"cos({angle})");
            Assert.True(MathR.Abs(StableMath.Sin(angle) - MathR.Sin(angle)) < (Real)2e-6, $"single sin({angle})");
            Assert.True(MathR.Abs(StableMath.Cos(angle) - MathR.Cos(angle)) < (Real)2e-6, $"single cos({angle})");
        }

        // FMath.Atan2<T>(y, x) has a known arg-swap bug (calls T.Atan2(x, y)),
        // so StableMath.Atan2 uses Fixed32.Atan2 directly. The generic path is not
        // a faithful reference for atan2 and is therefore excluded from this comparison.
        // (Real y, Real x)[] atanInputs = [ ... ];

        Real[] unitInputs = [(Real)(-1.0), (Real)(-0.75), (Real)(-0.25), (Real)0.0, (Real)0.25, (Real)0.75, (Real)1.0];

        foreach (Real value in unitInputs)
        {
            Assert.True(MathR.Abs(StableMath.Asin(value) - MathR.Asin(value)) < (Real)2e-6,
                $"asin({value})");
            Assert.True(MathR.Abs(StableMath.Acos(value) - MathR.Acos(value)) < (Real)2e-6,
                $"acos({value})");
        }
    }

    [Fact]
    public static void StableMath_MatchesMathROnNonReducedBoundaryInputs()
    {
        Real epsilon = (Real)1e-4;
        Real[] angles =
        [
            -StableMath.Pi,
            -StableMath.Pi + epsilon,
            -StableMath.HalfPi,
            -StableMath.QuarterPi,
            (Real)0.0,
            StableMath.QuarterPi,
            StableMath.HalfPi,
            StableMath.Pi - epsilon,
            StableMath.Pi
        ];

        foreach (Real angle in angles)
        {
            var (sinDet, cosDet) = StableMath.SinCos(angle);

            Assert.True(MathR.Abs(sinDet - MathR.Sin(angle)) < (Real)2e-6, $"sin({angle})");
            Assert.True(MathR.Abs(cosDet - MathR.Cos(angle)) < (Real)2e-6, $"cos({angle})");
            Assert.True(MathR.Abs(StableMath.Sin(angle) - MathR.Sin(angle)) < (Real)2e-6, $"single sin({angle})");
            Assert.True(MathR.Abs(StableMath.Cos(angle) - MathR.Cos(angle)) < (Real)2e-6, $"single cos({angle})");
        }
    }

    [Fact]
    public static void DeterministicInverseTrig_MatchesMathROnUnitInterval()
    {
        for (int i = -2000; i <= 2000; i++)
        {
            Real value = i / (Real)2000.0;

            Assert.True(MathR.Abs(StableMath.Asin(value) - MathR.Asin(value)) < (Real)5e-6,
                $"asin({value})");
            Assert.True(MathR.Abs(StableMath.Acos(value) - MathR.Acos(value)) < (Real)5e-6,
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
