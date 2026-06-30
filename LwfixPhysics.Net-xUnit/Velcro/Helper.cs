using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.Velcro.Test;

/// <summary>
/// Shared helpers for the Velcro test suite.
/// </summary>
internal static class TestHelper
{
    /// <summary>
    /// Tolerance used for Fixed32 comparisons that tolerate small rounding errors
    /// introduced by the fixed-point representation (1e-4 is conservative for Fixed32).
    /// </summary>
    public static readonly Fixed32 Tolerance = (Fixed32)1e-4;

    /// <summary>
    /// Loose tolerance used when the expected value comes from a double computation
    /// that may accumulate larger error before being quantised to Fixed32.
    /// </summary>
    public static readonly Fixed32 LooseTolerance = (Fixed32)1e-3;

    /// <summary>
    /// Asserts that two Fixed32 values are approximately equal within the given tolerance.
    /// </summary>
    public static void AssertApprox(Fixed32 expected, Fixed32 actual, Fixed32? tolerance = null, string? message = null)
    {
        Fixed32 tol = tolerance ?? Tolerance;
        Fixed32 diff = FMath.Abs(expected - actual);
        Assert.True(diff <= tol,
            $"{message ?? "values differ"}: expected={expected.ToDouble()}, actual={actual.ToDouble()}, diff={diff.ToDouble()}, tol={tol.ToDouble()}");
    }

    /// <summary>
    /// Asserts that two Vector2 values are approximately equal within the given tolerance.
    /// </summary>
    public static void AssertApprox(Vector2 expected, Vector2 actual, Fixed32? tolerance = null, string? message = null)
    {
        AssertApprox(expected.X, actual.X, tolerance, (message ?? "vector.x") + " (X)");
        AssertApprox(expected.Y, actual.Y, tolerance, (message ?? "vector.y") + " (Y)");
    }

    /// <summary>
    /// Builds a <see cref="Transform"/> from a position and angle in radians.
    /// </summary>
    public static Transform MakeTransform(Fixed32 x, Fixed32 y, Fixed32 angle)
    {
        Transform t = new Transform();
        t.p = new Vector2(x, y);
        t.q.Set(angle);
        return t;
    }
}
