using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.TOI;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Test;
using Xunit;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Tests;

/// <summary>
/// Port of <c>VelcroPhysics.Tests/Tests/MathTest.cs</c> adapted to the Fixed32 type,
/// plus additional coverage for <see cref="Sweep"/> (Advance, Normalize),
/// <see cref="Transform"/> (SetIdentity, Set) and <see cref="Rot"/> (Set, GetAngle, axes).
/// </summary>
public class MathTest
{
    // ---------------------------------------------------------------------
    // Original VelcroPhysics test (ported)
    // ---------------------------------------------------------------------

    [Fact]
    public void SweepGetTransform()
    {
        // From issue https://github.com/erincatto/box2d/issues/447
        Sweep sweep = new Sweep();
        sweep.LocalCenter = Vector2.Zero;
        sweep.C0 = new Vector2(-2, 4);
        sweep.C = new Vector2(3, 8);
        sweep.A0 = (Fixed32)0.5;
        sweep.A = 5;
        sweep.Alpha0 = 0;

        sweep.GetTransform(out Transform transform, 0);
        Assert.Equal(transform.p.X, sweep.C0.X);
        Assert.Equal(transform.p.Y, sweep.C0.Y);
        TestHelper.AssertApprox(Fixed32.Cos(sweep.A0), transform.q.c, (Fixed32)1e-3);
        TestHelper.AssertApprox(Fixed32.Sin(sweep.A0), transform.q.s, (Fixed32)1e-3);

        sweep.GetTransform(out transform, 1);
        Assert.Equal(transform.p.X, sweep.C.X);
        Assert.Equal(transform.p.Y, sweep.C.Y);
        TestHelper.AssertApprox(Fixed32.Cos(sweep.A), transform.q.c, (Fixed32)1e-3);
        TestHelper.AssertApprox(Fixed32.Sin(sweep.A), transform.q.s, (Fixed32)1e-3);
    }

    // ---------------------------------------------------------------------
    // Sweep additional coverage
    // ---------------------------------------------------------------------

    [Fact]
    public void Sweep_GetTransform_AtMidpoint_IsInterpolated()
    {
        Sweep sweep = new Sweep
        {
            LocalCenter = Vector2.Zero,
            C0 = new Vector2(0, 0),
            C = new Vector2(10, 20),
            A0 = 0,
            A = 0,
            Alpha0 = 0
        };

        sweep.GetTransform(out Transform xf, (Fixed32)0.5);
        TestHelper.AssertApprox(new Vector2(5, 10), xf.p, (Fixed32)1e-3);
        TestHelper.AssertApprox(Fixed32.One, xf.q.c, (Fixed32)1e-3, "cos(0)");
        TestHelper.AssertApprox(Fixed32.Zero, xf.q.s, (Fixed32)1e-3, "sin(0)");
    }

    [Fact]
    public void Sweep_GetTransform_WithLocalCenter_ShiftsPosition()
    {
        Sweep sweep = new Sweep
        {
            LocalCenter = new Vector2(1, 1),
            C0 = new Vector2(2, 2),
            C = new Vector2(2, 2),
            A0 = 0,
            A = 0,
            Alpha0 = 0
        };

        sweep.GetTransform(out Transform xf, 0);

        // xf.p = C0 - R(q0) * LocalCenter, where R(q0) is identity for angle 0.
        // So xf.p = (2,2) - (1,1) = (1,1)
        TestHelper.AssertApprox(new Vector2(1, 1), xf.p, (Fixed32)1e-3);
    }

    [Fact]
    public void Sweep_Advance_MovesC0A0TowardsC()
    {
        Sweep sweep = new Sweep
        {
            LocalCenter = Vector2.Zero,
            C0 = new Vector2(0, 0),
            C = new Vector2(10, 0),
            A0 = 0,
            A = Fixed32.PI,
            Alpha0 = 0
        };

        sweep.Advance((Fixed32)0.5);
        Assert.Equal((Fixed32)0.5, sweep.Alpha0);
        TestHelper.AssertApprox(new Vector2(5, 0), sweep.C0, (Fixed32)1e-3);
        TestHelper.AssertApprox(Fixed32.Half_PI, sweep.A0, (Fixed32)1e-3);
    }

    [Fact]
    public void Sweep_Normalize_BringsAnglesIntoCanonicalRange()
    {
        Sweep sweep = new Sweep
        {
            LocalCenter = Vector2.Zero,
            C0 = Vector2.Zero,
            C = Vector2.Zero,
            A0 = (Fixed32)5.0 * Fixed32.PI,
            A = (Fixed32)7.0 * Fixed32.PI,
            Alpha0 = 0
        };

        sweep.Normalize();

        // Normalize subtracts 2π * floor(A0/2π) from both A0 and A. With A0 = 5π,
        // floor(2.5) = 2, so subtract 4π: A0 -> π, A -> 3π.
        TestHelper.AssertApprox(Fixed32.PI, sweep.A0, (Fixed32)1e-2);
        TestHelper.AssertApprox((Fixed32)3.0 * Fixed32.PI, sweep.A, (Fixed32)1e-2);
    }

    [Fact]
    public void Sweep_DefaultConstruct_HasZeroedFields()
    {
        Sweep sweep = default;
        Assert.Equal(Vector2.Zero, sweep.C0);
        Assert.Equal(Vector2.Zero, sweep.C);
        Assert.Equal(Vector2.Zero, sweep.LocalCenter);
        Assert.Equal(Fixed32.Zero, sweep.A);
        Assert.Equal(Fixed32.Zero, sweep.A0);
        Assert.Equal(Fixed32.Zero, sweep.Alpha0);
    }

    // ---------------------------------------------------------------------
    // Transform additional coverage
    // ---------------------------------------------------------------------

    [Fact]
    public void Transform_SetIdentity_ZeroesPositionAndRotation()
    {
        Transform t = new Transform
        {
            p = new Vector2(10, -20),
            q = new Rot(Fixed32.Half_PI)
        };
        t.SetIdentity();
        Assert.Equal(Vector2.Zero, t.p);
        Assert.Equal(Fixed32.Zero, t.q.s);
        Assert.Equal(Fixed32.One, t.q.c);
    }

    [Fact]
    public void Transform_Set_AssignsPositionAndAngle()
    {
        Transform t = new Transform();
        t.Set(new Vector2(3, 4), Fixed32.Half_PI);

        Assert.Equal(new Vector2(3, 4), t.p);
        // For angle = π/2: sin = 1, cos = 0.
        TestHelper.AssertApprox(Fixed32.One, t.q.s, (Fixed32)1e-3, "sin(π/2)");
        TestHelper.AssertApprox(Fixed32.Zero, t.q.c, (Fixed32)1e-3, "cos(π/2)");
    }

    // ---------------------------------------------------------------------
    // Rot additional coverage
    // ---------------------------------------------------------------------

    [Fact]
    public void Rot_FromAngle_HasMatchingSinCos()
    {
        Fixed32 angle = (Fixed32)0.7;
        Rot r = new Rot(angle);

        TestHelper.AssertApprox(Fixed32.Sin(angle), r.s, (Fixed32)1e-3);
        TestHelper.AssertApprox(Fixed32.Cos(angle), r.c, (Fixed32)1e-3);
    }

    [Fact]
    public void Rot_SetIdentity_GivesZeroRotation()
    {
        Rot r = new Rot(Fixed32.Half_PI);
        r.SetIdentity();
        Assert.Equal(Fixed32.Zero, r.s);
        Assert.Equal(Fixed32.One, r.c);
    }

    [Fact]
    public void Rot_Set_WithAngleZero_UsesShortcut()
    {
        Rot r = new Rot();
        r.Set(Fixed32.Zero);
        Assert.Equal(Fixed32.Zero, r.s);
        Assert.Equal(Fixed32.One, r.c);
    }

    [Fact]
    public void Rot_Set_WithAngle_ComputesSinCos()
    {
        Rot r = new Rot();
        Fixed32 angle = (Fixed32)1.2;
        r.Set(angle);

        TestHelper.AssertApprox(Fixed32.Sin(angle), r.s, (Fixed32)1e-3);
        TestHelper.AssertApprox(Fixed32.Cos(angle), r.c, (Fixed32)1e-3);
    }

    [Fact]
    public void Rot_GetAngle_ReturnsOriginalAngle()
    {
        Fixed32 angle = (Fixed32)0.75;
        Rot r = new Rot(angle);
        TestHelper.AssertApprox(angle, r.GetAngle(), (Fixed32)1e-2);
    }

    [Fact]
    public void Rot_GetXAxis_ReturnsUnitVectorAlignedWithAngle()
    {
        Fixed32 angle = Fixed32.Zero;
        Rot r = new Rot(angle);
        Vector2 xAxis = r.GetXAxis();
        TestHelper.AssertApprox(new Vector2(1, 0), xAxis, (Fixed32)1e-3);
    }

    [Fact]
    public void Rot_GetYAxis_ReturnsUnitVectorPerpendicular()
    {
        Fixed32 angle = Fixed32.Zero;
        Rot r = new Rot(angle);
        Vector2 yAxis = r.GetYAxis();
        TestHelper.AssertApprox(new Vector2(0, 1), yAxis, (Fixed32)1e-3);
    }
}
