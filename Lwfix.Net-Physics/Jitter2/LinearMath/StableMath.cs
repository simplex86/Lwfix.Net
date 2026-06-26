/*
 * Jitter2 Physics Library
 * (c) Thorben Linneweber and contributors
 * SPDX-License-Identifier: MIT
 *
 * Fixed-point build: the original engine shipped its own Maclaurin/Taylor
 * polynomial approximations of sin/cos/atan/asin/acos to guarantee bit-identical
 * results across platforms. Those polynomials carry double-precision coefficients
 * (e.g. 1/6227020800 ~= 1.6e-10) that underflow to zero in Fixed32 (epsilon ~= 2.3e-10),
 * so the approximations would silently collapse. We instead delegate to the
 * built-in SimplexLab.Fixed.Fixed32 trig, which is already a deterministic,
 * platform-independent fixed-point implementation.
 */

using System.Runtime.CompilerServices;
using SimplexLab.Fixed;

namespace SimplexLab.Fixed.Physics.LinearMath;

/// <summary>
/// Internal trigonometric helpers. In the fixed-point build every entry point
/// delegates to <see cref="Fixed32"/>, which provides deterministic results
/// across platforms without depending on the BCL's floating-point libm.
/// </summary>
internal static class StableMath
{
    // Fixed32 is a struct, so `const Real` is illegal (CS0283). Use static readonly instead.
    internal static readonly Real Pi = Fixed32.PI;
    internal static readonly Real HalfPi = Fixed32.Half_PI;
    internal static readonly Real QuarterPi = Fixed32.Quarter_PI;
    internal static readonly Real TwoPi = Fixed32.Two_PI;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static (Real sin, Real cos) SinCos(Real angle)
    {
        // Fixed32 does not expose a fused SinCos; compute both directly. The
        // internal fixed-point implementation is cheap enough that this is fine.
        return (Fixed32.Sin(angle), Fixed32.Cos(angle));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Real Sin(Real angle)
    {
        return Fixed32.Sin(angle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Real Cos(Real angle)
    {
        return Fixed32.Cos(angle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Real Tan(Real angle)
    {
        return Fixed32.Tan(angle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Real Atan(Real value)
    {
        return Fixed32.Atan(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Real Atan2(Real y, Real x)
    {
        // NOTE: Fixed32.Atan2(y, x) follows the standard (y, x) convention.
        // The generic FMath.Atan2<T> has an arg-swap bug, so we call Fixed32 directly.
        return Fixed32.Atan2(y, x);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Real Asin(Real value)
    {
        return Fixed32.Asin(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Real Acos(Real value)
    {
        return Fixed32.Acos(value);
    }
}
