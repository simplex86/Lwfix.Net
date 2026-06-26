/*
 * Jitter2 Physics Library
 * (c) Thorben Linneweber and contributors
 * SPDX-License-Identifier: MIT
 *
 * Fixed-point build: Real is SimplexLab.Lwfix.Fixed32 (Q32.32, 8 bytes).
 * MathR aliases the generic FMath helper. Vector/VectorReal alias the
 * custom 4-wide Fixed32 SIMD shim (see LinearMath/FixedVector.cs) since
 * System.Runtime.Intrinsics vectors cannot be formed over Fixed32.
 */

global using Real = SimplexLab.Lwfix.Fixed32;
global using MathR = SimplexLab.Lwfix.FMath;
global using Vector = SimplexLab.Lwfix.Physics.LinearMath.FixedVector;
global using VectorReal = SimplexLab.Lwfix.Physics.LinearMath.FixedVector4;

namespace SimplexLab.Lwfix.Physics;

/// <summary>
/// Provides constants and utilities related to the numeric precision configuration.
/// The library is built on the <see cref="SimplexLab.Lwfix.Fixed32"/> fixed-point type.
/// </summary>
internal static class Precision
{
    /// <summary>
    /// The size in bytes of a single <see cref="Real"/> (Fixed32) value.
    /// Fixed32 stores a Q32.32 fixed-point number in a single <c>long</c>, so it
    /// occupies exactly 8 bytes — the same as <c>double</c>. This constant is used
    /// in <c>[StructLayout]</c> / <c>[FieldOffset]</c> attributes where
    /// <c>sizeof(Real)</c> cannot appear (Fixed32 is a struct, not a primitive).
    /// </summary>
    public const int RealSize = 8;

    /// <summary>
    /// The size in bytes of a full constraint data structure.
    /// Fixed32 is 8 bytes (identical to double), so the layout matches the double-precision build.
    /// </summary>
    public const int ConstraintSizeFull = 512;

    /// <summary>
    /// The size in bytes of a small constraint data structure.
    /// </summary>
    public const int ConstraintSizeSmall = 256;

    /// <summary>
    /// The size in bytes of the <see cref="Dynamics.RigidBodyData"/> structure.
    /// </summary>
    public const int RigidBodyDataSize = 256;

    /// <summary>
    /// Gets a value indicating whether the engine is configured to use double-precision floating-point numbers.
    /// Always false in this fixed-point build.
    /// </summary>
    public static bool IsDoublePrecision => false;
}
