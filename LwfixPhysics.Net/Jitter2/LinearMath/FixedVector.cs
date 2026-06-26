/*
 * Jitter2 Physics Library
 * (c) Thorben Linneweber and contributors
 * SPDX-License-Identifier: MIT
 *
 * Fixed-point replacement for the System.Runtime.Intrinsics SIMD types used by
 * the engine. Fixed32 is a struct and cannot be used as the generic argument of
 * Vector128/Vector256, so a small 4-wide scalar shim is provided instead. It
 * exposes the subset of the Vector / VectorReal API actually consumed by
 * Dynamics/Contact.cs and Collision/DynamicTree/TreeBox.cs, keeping the call
 * sites 1:1 with the original floating-point source.
 */

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Jitter2.LinearMath;

/// <summary>
/// A 4-wide vector of <see cref="Real"/> (Fixed32) values, occupying exactly
/// 4 * <see cref="Precision.RealSize"/> bytes. Used wherever the original engine
/// stored a <c>Vector128&lt;Real&gt;</c> / <c>Vector256&lt;Real&gt;</c> as an
/// embedded field. Named <c>FixedVector4</c> to mirror the <c>FixedVector</c>
/// static helper; the global using <c>VectorReal</c> aliases this type.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 4 * Precision.RealSize)]
public struct FixedVector4
{
    [FieldOffset(0 * Precision.RealSize)] public Real E0;
    [FieldOffset(1 * Precision.RealSize)] public Real E1;
    [FieldOffset(2 * Precision.RealSize)] public Real E2;
    [FieldOffset(3 * Precision.RealSize)] public Real E3;

    /// <summary>
    /// Returns the lane at the given index (0..3).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Real GetElement(int index)
    {
        unsafe
        {
            fixed (Real* p = &E0)
            {
                return p[index];
            }
        }
    }

    /// <summary>
    /// Reinterprets the lanes as a 4-wide integer mask. Since Fixed32 already
    /// stores its bits in a single long, this is an identity operation retained
    /// for source compatibility with the <c>mask.AsInt32()</c> pattern in TreeBox.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly VectorReal AsInt32() => this;

    private static VectorReal FromRaw(long raw)
    {
        VectorReal v = default;
        v.E0 = Fixed32.FromRaw(raw);
        v.E1 = Fixed32.FromRaw(raw);
        v.E2 = Fixed32.FromRaw(raw);
        v.E3 = Fixed32.FromRaw(raw);
        return v;
    }

    internal static long Raw(ref VectorReal v, int index)
    {
        unsafe
        {
            fixed (Real* p = &v.E0)
            {
                return Fixed32.ToRaw(p[index]);
            }
        }
    }
}

/// <summary>
/// Static helper exposing the subset of <c>System.Runtime.Intrinsics.Vector</c>
/// operations used by the engine, operating on <see cref="VectorReal"/>.
/// </summary>
internal static class FixedVector
{
    // All-ones lane used to represent a true SIMD mask lane.
    private static readonly Real MaskTrue = Fixed32.FromRaw(-1);
    private static readonly Real MaskFalse = Fixed32.Zero;

    /// <summary>
    /// Gets a value indicating whether the SIMD code path should be used.
    /// Always true in the fixed-point build: the scalar shim implements the
    /// same API as the original hardware-accelerated path, so the "accelerated"
    /// branch in Contact.cs / TreeBox.cs is taken and produces correct results.
    /// </summary>
    public static bool IsHardwareAccelerated => true;

    /// <summary>Broadcasts a scalar to all four lanes.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal Create(Real value)
    {
        VectorReal v = default;
        v.E0 = value;
        v.E1 = value;
        v.E2 = value;
        v.E3 = value;
        return v;
    }

    /// <summary>Creates a vector from four scalar lanes.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal Create(Real e0, Real e1, Real e2, Real e3)
    {
        VectorReal v = default;
        v.E0 = e0;
        v.E1 = e1;
        v.E2 = e2;
        v.E3 = e3;
        return v;
    }

    /// <summary>
    /// Creates a vector whose lanes carry the given raw bit pattern. Used to form
    /// the all-ones (<c>-1</c>) and all-zero (<c>0</c>) mask constants.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal Create(int value)
    {
        return VectorRealFromRaw(value);
    }

    private static VectorReal VectorRealFromRaw(long raw)
    {
        VectorReal v = default;
        v.E0 = Fixed32.FromRaw(raw);
        v.E1 = Fixed32.FromRaw(raw);
        v.E2 = Fixed32.FromRaw(raw);
        v.E3 = Fixed32.FromRaw(raw);
        return v;
    }

    /// <summary>
    /// Loads four consecutive <see cref="Real"/> values starting at the given reference
    /// into a <see cref="VectorReal"/>. Mirrors <c>Vector256&lt;T&gt;.LoadUnsafe</c> /
    /// <c>Vector128&lt;T&gt;.LoadUnsafe</c> from <c>System.Runtime.Intrinsics</c>,
    /// which cannot be formed over <see cref="Real"/> (a struct).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe VectorReal LoadUnsafe(ref Real start)
    {
        VectorReal v = default;
        fixed (Real* p = &start)
        {
            v.E0 = p[0];
            v.E1 = p[1];
            v.E2 = p[2];
            v.E3 = p[3];
        }
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal Add(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = left.E0 + right.E0;
        v.E1 = left.E1 + right.E1;
        v.E2 = left.E2 + right.E2;
        v.E3 = left.E3 + right.E3;
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal Subtract(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = left.E0 - right.E0;
        v.E1 = left.E1 - right.E1;
        v.E2 = left.E2 - right.E2;
        v.E3 = left.E3 - right.E3;
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal Multiply(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = left.E0 * right.E0;
        v.E1 = left.E1 * right.E1;
        v.E2 = left.E2 * right.E2;
        v.E3 = left.E3 * right.E3;
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal Divide(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = left.E0 / right.E0;
        v.E1 = left.E1 / right.E1;
        v.E2 = left.E2 / right.E2;
        v.E3 = left.E3 / right.E3;
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal Max(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = Fixed32.Max(left.E0, right.E0);
        v.E1 = Fixed32.Max(left.E1, right.E1);
        v.E2 = Fixed32.Max(left.E2, right.E2);
        v.E3 = Fixed32.Max(left.E3, right.E3);
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal Min(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = Fixed32.Min(left.E0, right.E0);
        v.E1 = Fixed32.Min(left.E1, right.E1);
        v.E2 = Fixed32.Min(left.E2, right.E2);
        v.E3 = Fixed32.Min(left.E3, right.E3);
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal LessThan(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = left.E0 < right.E0 ? MaskTrue : MaskFalse;
        v.E1 = left.E1 < right.E1 ? MaskTrue : MaskFalse;
        v.E2 = left.E2 < right.E2 ? MaskTrue : MaskFalse;
        v.E3 = left.E3 < right.E3 ? MaskTrue : MaskFalse;
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal GreaterThan(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = left.E0 > right.E0 ? MaskTrue : MaskFalse;
        v.E1 = left.E1 > right.E1 ? MaskTrue : MaskFalse;
        v.E2 = left.E2 > right.E2 ? MaskTrue : MaskFalse;
        v.E3 = left.E3 > right.E3 ? MaskTrue : MaskFalse;
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal LessThanOrEqual(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = left.E0 <= right.E0 ? MaskTrue : MaskFalse;
        v.E1 = left.E1 <= right.E1 ? MaskTrue : MaskFalse;
        v.E2 = left.E2 <= right.E2 ? MaskTrue : MaskFalse;
        v.E3 = left.E3 <= right.E3 ? MaskTrue : MaskFalse;
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal GreaterThanOrEqual(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = left.E0 >= right.E0 ? MaskTrue : MaskFalse;
        v.E1 = left.E1 >= right.E1 ? MaskTrue : MaskFalse;
        v.E2 = left.E2 >= right.E2 ? MaskTrue : MaskFalse;
        v.E3 = left.E3 >= right.E3 ? MaskTrue : MaskFalse;
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal BitwiseAnd(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = Fixed32.FromRaw(Fixed32.ToRaw(left.E0) & Fixed32.ToRaw(right.E0));
        v.E1 = Fixed32.FromRaw(Fixed32.ToRaw(left.E1) & Fixed32.ToRaw(right.E1));
        v.E2 = Fixed32.FromRaw(Fixed32.ToRaw(left.E2) & Fixed32.ToRaw(right.E2));
        v.E3 = Fixed32.FromRaw(Fixed32.ToRaw(left.E3) & Fixed32.ToRaw(right.E3));
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VectorReal BitwiseOr(VectorReal left, VectorReal right)
    {
        VectorReal v = default;
        v.E0 = Fixed32.FromRaw(Fixed32.ToRaw(left.E0) | Fixed32.ToRaw(right.E0));
        v.E1 = Fixed32.FromRaw(Fixed32.ToRaw(left.E1) | Fixed32.ToRaw(right.E1));
        v.E2 = Fixed32.FromRaw(Fixed32.ToRaw(left.E2) | Fixed32.ToRaw(right.E2));
        v.E3 = Fixed32.FromRaw(Fixed32.ToRaw(left.E3) | Fixed32.ToRaw(right.E3));
        return v;
    }

    /// <summary>
    /// Returns true when every lane of <paramref name="value"/> equals the
    /// corresponding lane of <paramref name="other"/> (compared by raw bits).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(VectorReal value, VectorReal other)
    {
        return Fixed32.ToRaw(value.E0) == Fixed32.ToRaw(other.E0) &&
               Fixed32.ToRaw(value.E1) == Fixed32.ToRaw(other.E1) &&
               Fixed32.ToRaw(value.E2) == Fixed32.ToRaw(other.E2) &&
               Fixed32.ToRaw(value.E3) == Fixed32.ToRaw(other.E3);
    }
}
