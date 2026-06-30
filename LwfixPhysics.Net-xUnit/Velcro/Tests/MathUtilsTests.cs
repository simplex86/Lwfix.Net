using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Test;
using SimplexLab.LwfixPhysics.Velcro.Utilities;
using Xunit;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Tests;

/// <summary>
/// Additional coverage for the static <see cref="MathUtils"/> helper class:
/// cross products, dot products, Mul / MulT overloads, distance, swap, validity,
/// clamp and equals helpers.
/// </summary>
public class MathUtilsTests
{
    // ---------------------------------------------------------------------
    // Cross
    // ---------------------------------------------------------------------

    [Fact]
    public void Cross_TwoVectors_GivesScalarArea()
    {
        Vector2 a = new Vector2(1, 0);
        Vector2 b = new Vector2(0, 1);
        // a × b = 1*1 - 0*0 = 1
        Assert.Equal(Fixed32.One, MathUtils.Cross(a, b));
    }

    [Fact]
    public void Cross_RefOverload_MatchesByValueOverload()
    {
        Vector2 a = new Vector2(2, 3);
        Vector2 b = new Vector2(4, 5);
        Fixed32 expected = MathUtils.Cross(a, b);
        Fixed32 actual = MathUtils.Cross(ref a, ref b);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Cross_ScalarFirst_RotatesVector()
    {
        // Cross(s, a) = (-s * a.Y, s * a.X)
        Fixed32 s = 2;
        Vector2 a = new Vector2(1, 0);
        Vector2 expected = new Vector2(0, 2);
        Assert.Equal(expected, MathUtils.Cross(s, a));
    }

    [Fact]
    public void Cross_VectorFirst_Complementary()
    {
        // Cross(a, s) = (s * a.Y, -s * a.X)
        Fixed32 s = 2;
        Vector2 a = new Vector2(1, 0);
        Vector2 expected = new Vector2(0, -2);
        Assert.Equal(expected, MathUtils.Cross(a, s));
    }

    [Fact]
    public void Cross_Vector3_RightHanded()
    {
        Vector3 i = new Vector3(1, 0, 0);
        Vector3 j = new Vector3(0, 1, 0);
        Vector3 k = new Vector3(0, 0, 1);

        Vector3 result = MathUtils.Cross(i, j);
        TestHelper.AssertApprox(k.X, result.X, (Fixed32)1e-3, "x");
        TestHelper.AssertApprox(k.Y, result.Y, (Fixed32)1e-3, "y");
        TestHelper.AssertApprox(k.Z, result.Z, (Fixed32)1e-3, "z");
    }

    // ---------------------------------------------------------------------
    // Dot
    // ---------------------------------------------------------------------

    [Fact]
    public void Dot_TwoVectors_SumsComponentProduct()
    {
        Vector2 a = new Vector2(1, 2);
        Vector2 b = new Vector2(3, 4);
        // 1*3 + 2*4 = 11
        Assert.Equal(11, MathUtils.Dot(a, b).ToDouble());
    }

    [Fact]
    public void Dot_RefOverload_MatchesByValueOverload()
    {
        Vector2 a = new Vector2(1, 2);
        Vector2 b = new Vector2(3, 4);
        Fixed32 expected = MathUtils.Dot(a, b);
        Fixed32 actual = MathUtils.Dot(ref a, ref b);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Dot_Vector3()
    {
        Vector3 a = new Vector3(1, 2, 3);
        Vector3 b = new Vector3(4, 5, 6);
        // 1*4 + 2*5 + 3*6 = 32
        Assert.Equal(32, MathUtils.Dot(a, b).ToDouble());
    }

    // ---------------------------------------------------------------------
    // Mul (Rot * Vector2)
    // ---------------------------------------------------------------------

    [Fact]
    public void Mul_Rot_Vector2_RotatesVector()
    {
        Rot r = new Rot(Fixed32.Half_PI); // 90°
        Vector2 v = new Vector2(1, 0);

        Vector2 rotated = MathUtils.Mul(r, v);
        // After 90° rotation: (1, 0) -> (0, 1)
        TestHelper.AssertApprox(new Vector2(0, 1), rotated, (Fixed32)1e-3);
    }

    [Fact]
    public void MulT_Rot_Vector2_InverseRotatesVector()
    {
        Rot r = new Rot(Fixed32.Half_PI); // 90°
        Vector2 v = new Vector2(0, 1);    // already rotated

        Vector2 back = MathUtils.MulT(r, v);
        TestHelper.AssertApprox(new Vector2(1, 0), back, (Fixed32)1e-3);
    }

    [Fact]
    public void Mul_Rot_Rot_ComposesRotations()
    {
        Rot a = new Rot(Fixed32.Half_PI); // 90°
        Rot b = new Rot(Fixed32.Half_PI); // 90°
        Rot c = MathUtils.Mul(a, b);     // 180°

        TestHelper.AssertApprox(Fixed32.PI, c.GetAngle(), (Fixed32)1e-2);
    }

    [Fact]
    public void MulT_Rot_Rot_ComposesInverseRotation()
    {
        Rot a = new Rot(Fixed32.PI);     // 180°
        Rot b = new Rot(Fixed32.PI);     // 180°
        Rot c = MathUtils.MulT(a, b);    // 0°

        TestHelper.AssertApprox(Fixed32.Zero, c.GetAngle(), (Fixed32)1e-2);
    }

    // ---------------------------------------------------------------------
    // Mul (Transform * Vector2)
    // ---------------------------------------------------------------------

    [Fact]
    public void Mul_Transform_Vector2_AppliesTranslationAndRotation()
    {
        Transform t = new Transform
        {
            p = new Vector2(10, 20),
            q = new Rot(Fixed32.Half_PI) // 90°
        };
        Vector2 v = new Vector2(1, 0);
        // Rotated (1,0) -> (0,1), then translated by (10,20) -> (10,21)
        Vector2 result = MathUtils.Mul(ref t, ref v);
        TestHelper.AssertApprox(new Vector2(10, 21), result, (Fixed32)1e-3);
    }

    [Fact]
    public void MulT_Transform_Vector2_InverseTransformsPoint()
    {
        Transform t = new Transform
        {
            p = new Vector2(10, 20),
            q = new Rot(Fixed32.Half_PI)
        };
        Vector2 v = new Vector2(10, 21); // = Mul(t, (1, 0))
        Vector2 back = MathUtils.MulT(ref t, ref v);
        TestHelper.AssertApprox(new Vector2(1, 0), back, (Fixed32)1e-3);
    }

    [Fact]
    public void Mul_Transform_Transform_ComposesTransforms()
    {
        Transform a = new Transform
        {
            p = new Vector2(1, 0),
            q = new Rot(Fixed32.Half_PI)
        };
        Transform b = new Transform
        {
            p = new Vector2(0, 1),
            q = new Rot(Fixed32.Half_PI)
        };
        Transform c = MathUtils.Mul(a, b);

        // Apply (a∘b) to (0,0): expect both translations composed with rotation.
        Vector2 zero = Vector2.Zero;
        Vector2 viaAB = MathUtils.Mul(ref c, ref zero);
        Vector2 bApplied = MathUtils.Mul(ref b, ref zero);
        Vector2 viaBFirst = MathUtils.Mul(ref a, ref bApplied);
        TestHelper.AssertApprox(viaBFirst, viaAB, (Fixed32)1e-3);
    }

    [Fact]
    public void MulT_Transform_Transform_TakesBLocalToALocal()
    {
        // MulT(a, b) is the relative transform that takes a point from B's local frame
        // to A's local frame: Mul(c, vB) == MulT(a, Mul(b, vB)).
        Transform a = new Transform
        {
            p = new Vector2(2, 3),
            q = new Rot((Fixed32)0.4)
        };
        Transform b = new Transform
        {
            p = new Vector2(-1, 1),
            q = new Rot((Fixed32)0.2)
        };

        Transform c = MathUtils.MulT(a, b);

        Vector2 vB = new Vector2(5, 7);

        // B-local -> world -> A-local, via two individual transforms.
        Vector2 vWorld = MathUtils.Mul(ref b, ref vB);
        Vector2 vA = MathUtils.MulT(ref a, ref vWorld);

        // B-local -> A-local directly via c.
        Vector2 vA_direct = MathUtils.Mul(ref c, ref vB);

        TestHelper.AssertApprox(vA, vA_direct, (Fixed32)1e-3);
    }

    [Fact]
    public void MulT_Transform_Transform_WithSameArgs_IsIdentity()
    {
        // MulT(a, a) should produce the identity transform (A-local -> A-local).
        Transform a = new Transform
        {
            p = new Vector2(2, 3),
            q = new Rot((Fixed32)0.4)
        };

        Transform c = MathUtils.MulT(a, a);

        // Identity transform: q.angle ≈ 0, p ≈ 0.
        TestHelper.AssertApprox(Fixed32.Zero, c.q.GetAngle(), (Fixed32)1e-3, "angle");
        TestHelper.AssertApprox(Vector2.Zero, c.p, (Fixed32)1e-3, "p");

        // Applying c to any vector should leave it unchanged.
        Vector2 v = new Vector2(5, 7);
        Vector2 result = MathUtils.Mul(ref c, ref v);
        TestHelper.AssertApprox(v, result, (Fixed32)1e-3);
    }

    // ---------------------------------------------------------------------
    // Mul / MulT (Mat22)
    // ---------------------------------------------------------------------

    [Fact]
    public void Mul_Mat22_Vector2_AppliesLinearMap()
    {
        Mat22 m = new Mat22(new Vector2(2, 0), new Vector2(0, 3));
        Vector2 v = new Vector2(1, 1);
        Vector2 r = MathUtils.Mul(ref m, ref v);
        Assert.Equal(new Vector2(2, 3), r);
    }

    [Fact]
    public void MulT_Mat22_Vector2_AppliesTranspose()
    {
        Mat22 m = new Mat22(new Vector2(1, 2), new Vector2(3, 4));
        Vector2 v = new Vector2(5, 6);
        Vector2 r = MathUtils.MulT(ref m, ref v);
        // [1 3; 2 4]^T * [5; 6] = [1*5+2*6; 3*5+4*6] = [17; 39]
        Assert.Equal(new Vector2(17, 39), r);
    }

    // ---------------------------------------------------------------------
    // Distance
    // ---------------------------------------------------------------------

    [Fact]
    public void Distance_BetweenOriginAnd33_IsRoot32()
    {
        Vector2 a = Vector2.Zero;
        Vector2 b = new Vector2(3, 3);
        Fixed32 d = MathUtils.Distance(a, b);
        TestHelper.AssertApprox((Fixed32)System.Math.Sqrt(18), d, (Fixed32)1e-2);
    }

    [Fact]
    public void DistanceSquared_MatchesSquaredLength()
    {
        Vector2 a = new Vector2(1, 2);
        Vector2 b = new Vector2(4, 6);
        Fixed32 ds = MathUtils.DistanceSquared(ref a, ref b);
        Fixed32 dx = 3, dy = 4;
        Assert.Equal(dx * dx + dy * dy, ds);
    }

    [Fact]
    public void Distance_RefOverload_MatchesByValueOverload()
    {
        Vector2 a = new Vector2(1, 2);
        Vector2 b = new Vector2(4, 6);
        Fixed32 expected = MathUtils.Distance(a, b);
        Fixed32 actual = MathUtils.Distance(ref a, ref b);
        Assert.Equal(expected, actual);
    }

    // ---------------------------------------------------------------------
    // Min / Max / Abs / Clamp / Sign
    // ---------------------------------------------------------------------

    [Fact]
    public void Min_Fixed32_ReturnsSmaller()
    {
        Assert.Equal((Fixed32)2, MathUtils.Min((Fixed32)2, (Fixed32)5));
        Assert.Equal((Fixed32)(-3), MathUtils.Min((Fixed32)5, (Fixed32)(-3)));
    }

    [Fact]
    public void Max_Fixed32_ReturnsLarger()
    {
        Assert.Equal((Fixed32)5, MathUtils.Max((Fixed32)2, (Fixed32)5));
        Assert.Equal((Fixed32)5, MathUtils.Max((Fixed32)5, (Fixed32)(-3)));
    }

    [Fact]
    public void Min_Int32_ReturnsSmaller()
    {
        Assert.Equal(2, MathUtils.Min(2, 5));
        Assert.Equal(-3, MathUtils.Min(5, -3));
    }

    [Fact]
    public void Max_Int32_ReturnsLarger()
    {
        Assert.Equal(5, MathUtils.Max(2, 5));
        Assert.Equal(5, MathUtils.Max(5, -3));
    }

    [Fact]
    public void Abs_Fixed32_ReturnsAbsolute()
    {
        Assert.Equal((Fixed32)3, MathUtils.Abs((Fixed32)3));
        Assert.Equal((Fixed32)3, MathUtils.Abs((Fixed32)(-3)));
    }

    [Fact]
    public void Abs_Vector2_TakesComponentWise()
    {
        Vector2 v = new Vector2(-2, 3);
        Assert.Equal(new Vector2(2, 3), MathUtils.Abs(v));
    }

    [Fact]
    public void Clamp_Fixed32_ConstrainsToRange()
    {
        Assert.Equal((Fixed32)5, MathUtils.Clamp((Fixed32)10, (Fixed32)1, (Fixed32)5));
        Assert.Equal((Fixed32)1, MathUtils.Clamp((Fixed32)(-1), (Fixed32)1, (Fixed32)5));
        Assert.Equal((Fixed32)3, MathUtils.Clamp((Fixed32)3, (Fixed32)1, (Fixed32)5));
    }

    [Fact]
    public void Clamp_Int32_ConstrainsToRange()
    {
        Assert.Equal(5, MathUtils.Clamp(10, 1, 5));
        Assert.Equal(1, MathUtils.Clamp(-1, 1, 5));
        Assert.Equal(3, MathUtils.Clamp(3, 1, 5));
    }

    [Fact]
    public void Clamp_Vector2_ConstrainsComponentWise()
    {
        Vector2 v = new Vector2(-5, 20);
        Vector2 low = new Vector2(-1, -1);
        Vector2 high = new Vector2(1, 1);
        Assert.Equal(new Vector2(-1, 1), MathUtils.Clamp(v, low, high));
    }

    [Fact]
    public void Sign_ReturnsMinusOneZeroOrOne()
    {
        Assert.Equal(-1, MathUtils.Sign((Fixed32)(-5)));
        Assert.Equal(0, MathUtils.Sign(Fixed32.Zero));
        Assert.Equal(1, MathUtils.Sign((Fixed32)5));
    }

    // ---------------------------------------------------------------------
    // Swap / Skew
    // ---------------------------------------------------------------------

    [Fact]
    public void Swap_FlipsTwoValues()
    {
        Fixed32 a = 1, b = 2;
        MathUtils.Swap(ref a, ref b);
        Assert.Equal((Fixed32)2, a);
        Assert.Equal((Fixed32)1, b);
    }

    [Fact]
    public void Swap_GenericVersion_FlipsReferences()
    {
        string a = "hello", b = "world";
        MathUtils.Swap(ref a, ref b);
        Assert.Equal("world", a);
        Assert.Equal("hello", b);
    }

    [Fact]
    public void Skew_ReturnsPerpendicular()
    {
        Vector2 v = new Vector2(1, 0);
        Vector2 skew = MathUtils.Skew(v);
        // Skew(v) = (-v.Y, v.X)
        Assert.Equal(new Vector2(0, 1), skew);
    }

    // ---------------------------------------------------------------------
    // IsValid / FloatEquals / FloatInRange
    // ---------------------------------------------------------------------

    [Fact]
    public void IsValid_FiniteFixed32_True()
    {
        Assert.True(MathUtils.IsValid((Fixed32)1.5));
        Assert.True(MathUtils.IsValid((Fixed32)(-1e6)));
    }

    [Fact]
    public void IsValid_NaN_False()
    {
        Assert.False(MathUtils.IsValid(Fixed32.NaN));
    }

    [Fact]
    public void IsValid_PositiveInfinity_False()
    {
        Assert.False(MathUtils.IsValid(Fixed32.PositiveInfinity));
    }

    [Fact]
    public void IsValid_Vector2_TrueWhenBothComponentsValid()
    {
        Assert.True(MathUtils.IsValid(new Vector2(1, 2)));
    }

    [Fact]
    public void IsValid_Vector2_FalseWhenEitherComponentInvalid()
    {
        Assert.False(MathUtils.IsValid(new Vector2(Fixed32.NaN, 0)));
        Assert.False(MathUtils.IsValid(new Vector2(0, Fixed32.NaN)));
    }

    [Fact]
    public void FloatEquals_WithinEpsilon_True()
    {
        Fixed32 a = 1, b = 1 + MathConstants.Epsilon;
        Assert.True(MathUtils.FloatEquals(a, b));
    }

    [Fact]
    public void FloatEquals_BeyondDelta_False()
    {
        Assert.False(MathUtils.FloatEquals(1, 2, (Fixed32)0.1));
        Assert.True(MathUtils.FloatEquals(1, 1 + (Fixed32)0.001, (Fixed32)0.01));
    }

    [Fact]
    public void FloatInRange_InclusiveBoundaries_True()
    {
        Assert.True(MathUtils.FloatInRange(0, 0, 1));
        Assert.True(MathUtils.FloatInRange(1, 0, 1));
        Assert.False(MathUtils.FloatInRange(-1, 0, 1));
        Assert.False(MathUtils.FloatInRange(2, 0, 1));
    }

    // ---------------------------------------------------------------------
    // VectorAngle / Area / IsCollinear
    // ---------------------------------------------------------------------

    [Fact]
    public void VectorAngle_XToY_IsHalfPi()
    {
        Vector2 a = new Vector2(1, 0);
        Vector2 b = new Vector2(0, 1);
        Fixed32 angle = MathUtils.VectorAngle(ref a, ref b);
        TestHelper.AssertApprox(Fixed32.Half_PI, angle, (Fixed32)1e-2);
    }

    [Fact]
    public void Area_CollinearPoints_IsZero()
    {
        Vector2 a = new Vector2(0, 0);
        Vector2 b = new Vector2(1, 1);
        Vector2 c = new Vector2(2, 2);
        Fixed32 area = MathUtils.Area(ref a, ref b, ref c);
        Assert.Equal(Fixed32.Zero, area);
    }

    [Fact]
    public void Area_Triangle_IsTwiceSignedArea()
    {
        // MathUtils.Area uses the shoelace formula and returns 2× the signed triangle area.
        Vector2 a = new Vector2(0, 0);
        Vector2 b = new Vector2(1, 0);
        Vector2 c = new Vector2(0, 1);
        Fixed32 area = MathUtils.Area(ref a, ref b, ref c);
        Assert.Equal(Fixed32.One, area);
    }

    [Fact]
    public void IsCollinear_TrueForCollinearPoints()
    {
        Vector2 a = new Vector2(0, 0);
        Vector2 b = new Vector2(1, 1);
        Vector2 c = new Vector2(2, 2);
        Assert.True(MathUtils.IsCollinear(ref a, ref b, ref c, (Fixed32)1e-6));
    }

    // ---------------------------------------------------------------------
    // Normalize (in place)
    // ---------------------------------------------------------------------

    [Fact]
    public void Normalize_OfUnitVector_ReturnsOne()
    {
        Vector2 v = new Vector2(3, 4);
        Fixed32 length = MathUtils.Normalize(ref v);
        TestHelper.AssertApprox((Fixed32)5, length, (Fixed32)1e-2);
        TestHelper.AssertApprox(new Vector2((Fixed32)0.6, (Fixed32)0.8), v, (Fixed32)1e-2);
    }

    [Fact]
    public void Normalize_OfZeroVector_ReturnsZero()
    {
        Vector2 v = Vector2.Zero;
        Fixed32 length = MathUtils.Normalize(ref v);
        Assert.Equal(Fixed32.Zero, length);
    }

    // ---------------------------------------------------------------------
    // Transcendental wrappers
    // ---------------------------------------------------------------------

    [Fact]
    public void Sqrt_WrapsFixed32()
    {
        Assert.Equal(Fixed32.Sqrt((Fixed32)9), MathUtils.Sqrt((Fixed32)9));
    }

    [Fact]
    public void Sinf_WrapsFixed32()
    {
        Assert.Equal(Fixed32.Sin(Fixed32.Half_PI), MathUtils.Sinf(Fixed32.Half_PI));
    }

    [Fact]
    public void Cosf_WrapsFixed32()
    {
        Assert.Equal(Fixed32.Cos(Fixed32.Half_PI), MathUtils.Cosf(Fixed32.Half_PI));
    }

    [Fact]
    public void Ceil_WrapsFMath()
    {
        Assert.Equal(FMath.Ceil((Fixed32)1.5), MathUtils.Ceil((Fixed32)1.5));
    }

    [Fact]
    public void Log_WrapsFMath()
    {
        Assert.Equal(FMath.Log(Fixed32.E), MathUtils.Log(Fixed32.E));
    }
}
