// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'MONOGAME LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using SimplexLab.Lwfix;

// ReSharper disable once CheckNamespace
namespace SimplexLab.LwfixPhysics.Velcro.Primitives
{
    /// <summary>Describes a 3D-vector.</summary>
    [DataContract]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct Vector3 : IEquatable<Vector3>
    {
        /// <summary>The x coordinate of this <see cref="Vector3" />.</summary>
        [DataMember]
        public Fixed32 X;

        /// <summary>The y coordinate of this <see cref="Vector3" />.</summary>
        [DataMember]
        public Fixed32 Y;

        /// <summary>The z coordinate of this <see cref="Vector3" />.</summary>
        [DataMember]
        public Fixed32 Z;

        /// <summary>Returns a <see cref="Vector3" /> with components 0, 0, 0.</summary>
        public static Vector3 Zero { get; } = new Vector3(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);

        /// <summary>Returns a <see cref="Vector3" /> with components 1, 1, 1.</summary>
        public static Vector3 One { get; } = new Vector3(Fixed32.One, Fixed32.One, Fixed32.One);

        /// <summary>Returns a <see cref="Vector3" /> with components 1, 0, 0.</summary>
        public static Vector3 UnitX { get; } = new Vector3(Fixed32.One, Fixed32.Zero, Fixed32.Zero);

        /// <summary>Returns a <see cref="Vector3" /> with components 0, 1, 0.</summary>
        public static Vector3 UnitY { get; } = new Vector3(Fixed32.Zero, Fixed32.One, Fixed32.Zero);

        /// <summary>Returns a <see cref="Vector3" /> with components 0, 0, 1.</summary>
        public static Vector3 UnitZ { get; } = new Vector3(Fixed32.Zero, Fixed32.Zero, Fixed32.One);

        /// <summary>Returns a <see cref="Vector3" /> with components 0, 1, 0.</summary>
        public static Vector3 Up { get; } = new Vector3(Fixed32.Zero, Fixed32.One, Fixed32.Zero);

        /// <summary>Returns a <see cref="Vector3" /> with components 0, -1, 0.</summary>
        public static Vector3 Down { get; } = new Vector3(Fixed32.Zero, Fixed32.NegativeOne, Fixed32.Zero);

        /// <summary>Returns a <see cref="Vector3" /> with components 1, 0, 0.</summary>
        public static Vector3 Right { get; } = new Vector3(Fixed32.One, Fixed32.Zero, Fixed32.Zero);

        /// <summary>Returns a <see cref="Vector3" /> with components -1, 0, 0.</summary>
        public static Vector3 Left { get; } = new Vector3(Fixed32.NegativeOne, Fixed32.Zero, Fixed32.Zero);

        /// <summary>Returns a <see cref="Vector3" /> with components 0, 0, -1.</summary>
        public static Vector3 Forward { get; } = new Vector3(Fixed32.Zero, Fixed32.Zero, Fixed32.NegativeOne);

        /// <summary>Returns a <see cref="Vector3" /> with components 0, 0, 1.</summary>
        public static Vector3 Backward { get; } = new Vector3(Fixed32.Zero, Fixed32.Zero, Fixed32.One);

        internal string DebugDisplayString =>
            string.Concat(
                X.ToString(), "  ",
                Y.ToString(), "  ",
                Z.ToString()
            );

        /// <summary>Constructs a 3d vector with X, Y and Z from three values.</summary>
        /// <param name="x">The x coordinate in 3d-space.</param>
        /// <param name="y">The y coordinate in 3d-space.</param>
        /// <param name="z">The z coordinate in 3d-space.</param>
        public Vector3(Fixed32 x, Fixed32 y, Fixed32 z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>Constructs a 3d vector with X, Y and Z set to the same value.</summary>
        /// <param name="value">The x, y and z coordinates in 3d-space.</param>
        public Vector3(Fixed32 value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>Constructs a 3d vector with X, Y from <see cref="Vector2" /> and Z from a scalar.</summary>
        /// <param name="value">The x and y coordinates in 3d-space.</param>
        /// <param name="z">The z coordinate in 3d-space.</param>
        public Vector3(Vector2 value, Fixed32 z)
        {
            X = value.X;
            Y = value.Y;
            Z = z;
        }

        /// <summary>Performs vector addition on <paramref name="value1" /> and <paramref name="value2" />.</summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <returns>The result of the vector addition.</returns>
        public static Vector3 Add(Vector3 value1, Vector3 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }

        /// <summary>
        /// Performs vector addition on <paramref name="value1" /> and <paramref name="value2" />, storing the result of
        /// the addition in <paramref name="result" />.
        /// </summary>
        /// <param name="value1">The first vector to add.</param>
        /// <param name="value2">The second vector to add.</param>
        /// <param name="result">The result of the vector addition.</param>
        public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains the cartesian coordinates of a vector specified in
        /// barycentric coordinates and relative to 3d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 3d-triangle.</param>
        /// <param name="value2">The second vector of 3d-triangle.</param>
        /// <param name="value3">The third vector of 3d-triangle.</param>
        /// <param name="amount1">
        /// Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of
        /// 3d-triangle.
        /// </param>
        /// <param name="amount2">
        /// Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of
        /// 3d-triangle.
        /// </param>
        /// <returns>The cartesian translation of barycentric coordinates.</returns>
        public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, Fixed32 amount1, Fixed32 amount2)
        {
            return new Vector3(
                MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
                MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2),
                MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2));
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains the cartesian coordinates of a vector specified in
        /// barycentric coordinates and relative to 3d-triangle.
        /// </summary>
        /// <param name="value1">The first vector of 3d-triangle.</param>
        /// <param name="value2">The second vector of 3d-triangle.</param>
        /// <param name="value3">The third vector of 3d-triangle.</param>
        /// <param name="amount1">
        /// Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of
        /// 3d-triangle.
        /// </param>
        /// <param name="amount2">
        /// Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of
        /// 3d-triangle.
        /// </param>
        /// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
        public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, Fixed32 amount1, Fixed32 amount2, out Vector3 result)
        {
            result.X = MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            result.Y = MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
            result.Z = MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains CatmullRom interpolation of the specified vectors.</summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of CatmullRom interpolation.</returns>
        public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, Fixed32 amount)
        {
            return new Vector3(
                MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
                MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount),
                MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount));
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains CatmullRom interpolation of the specified vectors.</summary>
        /// <param name="value1">The first vector in interpolation.</param>
        /// <param name="value2">The second vector in interpolation.</param>
        /// <param name="value3">The third vector in interpolation.</param>
        /// <param name="value4">The fourth vector in interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
        public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, Fixed32 amount, out Vector3 result)
        {
            result.X = MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            result.Y = MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
            result.Z = MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
        }

        /// <summary>Round the members of this <see cref="Vector3" /> towards positive infinity.</summary>
        public void Ceiling()
        {
            X = FMath.Ceil(X);
            Y = FMath.Ceil(Y);
            Z = FMath.Ceil(Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains members from another vector rounded towards positive
        /// infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <returns>The rounded <see cref="Vector3" />.</returns>
        public static Vector3 Ceiling(Vector3 value)
        {
            value.X = FMath.Ceil(value.X);
            value.Y = FMath.Ceil(value.Y);
            value.Z = FMath.Ceil(value.Z);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains members from another vector rounded towards positive
        /// infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <param name="result">The rounded <see cref="Vector3" />.</param>
        public static void Ceiling(ref Vector3 value, out Vector3 result)
        {
            result.X = FMath.Ceil(value.X);
            result.Y = FMath.Ceil(value.Y);
            result.Z = FMath.Ceil(value.Z);
        }

        /// <summary>Clamps the specified value within a range.</summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
        {
            return new Vector3(
                MathHelper.Clamp(value1.X, min.X, max.X),
                MathHelper.Clamp(value1.Y, min.Y, max.Y),
                MathHelper.Clamp(value1.Z, min.Z, max.Z));
        }

        /// <summary>Clamps the specified value within a range.</summary>
        /// <param name="value1">The value to clamp.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <param name="result">The clamped value as an output parameter.</param>
        public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            result.X = MathHelper.Clamp(value1.X, min.X, max.X);
            result.Y = MathHelper.Clamp(value1.Y, min.Y, max.Y);
            result.Z = MathHelper.Clamp(value1.Z, min.Z, max.Z);
        }

        /// <summary>Computes the cross product of two vectors.</summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product of two vectors.</returns>
        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            Cross(ref vector1, ref vector2, out vector1);
            return vector1;
        }

        /// <summary>Computes the cross product of two vectors.</summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="result">The cross product of two vectors as an output parameter.</param>
        public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            Fixed32 x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
            Fixed32 y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
            Fixed32 z = vector1.X * vector2.Y - vector2.X * vector1.Y;
            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>Returns the distance between two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between two vectors.</returns>
        public static Fixed32 Distance(Vector3 value1, Vector3 value2)
        {
            Fixed32 result;
            DistanceSquared(ref value1, ref value2, out result);
            return Fixed32.Sqrt(result);
        }

        /// <summary>Returns the distance between two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The distance between two vectors as an output parameter.</param>
        public static void Distance(ref Vector3 value1, ref Vector3 value2, out Fixed32 result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = Fixed32.Sqrt(result);
        }

        /// <summary>Returns the squared distance between two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between two vectors.</returns>
        public static Fixed32 DistanceSquared(Vector3 value1, Vector3 value2)
        {
            return (value1.X - value2.X) * (value1.X - value2.X) +
                   (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                   (value1.Z - value2.Z) * (value1.Z - value2.Z);
        }

        /// <summary>Returns the squared distance between two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The squared distance between two vectors as an output parameter.</param>
        public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out Fixed32 result)
        {
            result = (value1.X - value2.X) * (value1.X - value2.X) +
                     (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                     (value1.Z - value2.Z) * (value1.Z - value2.Z);
        }

        /// <summary>Divides the components of a <see cref="Vector3" /> by the components of another <see cref="Vector3" />.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="value2">Divisor <see cref="Vector3" />.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector3 Divide(Vector3 value1, Vector3 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
        }

        /// <summary>Divides the components of a <see cref="Vector3" /> by a scalar.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector3 Divide(Vector3 value1, Fixed32 divider)
        {
            Fixed32 factor = Fixed32.One / divider;
            value1.X *= factor;
            value1.Y *= factor;
            value1.Z *= factor;
            return value1;
        }

        /// <summary>Divides the components of a <see cref="Vector3" /> by a scalar.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
        public static void Divide(ref Vector3 value1, Fixed32 divider, out Vector3 result)
        {
            Fixed32 factor = Fixed32.One / divider;
            result.X = value1.X * factor;
            result.Y = value1.Y * factor;
            result.Z = value1.Z * factor;
        }

        /// <summary>Divides the components of a <see cref="Vector3" /> by the components of another <see cref="Vector3" />.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="value2">Divisor <see cref="Vector3" />.</param>
        /// <param name="result">The result of dividing the vectors as an output parameter.</param>
        public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
        }

        /// <summary>Returns a dot product of two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static Fixed32 Dot(Vector3 value1, Vector3 value2)
        {
            return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
        }

        /// <summary>Returns a dot product of two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The dot product of two vectors as an output parameter.</param>
        public static void Dot(ref Vector3 value1, ref Vector3 value2, out Fixed32 result)
        {
            result = value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
        }

        /// <summary>Compares whether current instance is equal to specified <see cref="Object" />.</summary>
        /// <param name="obj">The <see cref="Object" /> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
                return false;

            Vector3 other = (Vector3)obj;
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        /// <summary>Compares whether current instance is equal to specified <see cref="Vector3" />.</summary>
        /// <param name="other">The <see cref="Vector3" /> to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public bool Equals(Vector3 other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        /// <summary>Round the members of this <see cref="Vector3" /> towards negative infinity.</summary>
        public void Floor()
        {
            X = FMath.Floor(X);
            Y = FMath.Floor(Y);
            Z = FMath.Floor(Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains members from another vector rounded towards negative
        /// infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <returns>The rounded <see cref="Vector3" />.</returns>
        public static Vector3 Floor(Vector3 value)
        {
            value.X = FMath.Floor(value.X);
            value.Y = FMath.Floor(value.Y);
            value.Z = FMath.Floor(value.Z);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains members from another vector rounded towards negative
        /// infinity.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <param name="result">The rounded <see cref="Vector3" />.</param>
        public static void Floor(ref Vector3 value, out Vector3 result)
        {
            result.X = FMath.Floor(value.X);
            result.Y = FMath.Floor(value.Y);
            result.Z = FMath.Floor(value.Z);
        }

        /// <summary>Gets the hash code of this <see cref="Vector3" />.</summary>
        /// <returns>Hash code of this <see cref="Vector3" />.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains hermite spline interpolation.</summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The hermite spline interpolation vector.</returns>
        public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, Fixed32 amount)
        {
            return new Vector3(MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount),
                MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount),
                MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount));
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains hermite spline interpolation.</summary>
        /// <param name="value1">The first position vector.</param>
        /// <param name="tangent1">The first tangent vector.</param>
        /// <param name="value2">The second position vector.</param>
        /// <param name="tangent2">The second tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
        public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, Fixed32 amount, out Vector3 result)
        {
            result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
            result.Z = MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
        }

        /// <summary>Returns the length of this <see cref="Vector3" />.</summary>
        /// <returns>The length of this <see cref="Vector3" />.</returns>
        public Fixed32 Length()
        {
            return Fixed32.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summary>Returns the squared length of this <see cref="Vector3" />.</summary>
        /// <returns>The squared length of this <see cref="Vector3" />.</returns>
        public Fixed32 LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains linear interpolation of the specified vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Vector3 Lerp(Vector3 value1, Vector3 value2, Fixed32 amount)
        {
            return new Vector3(
                MathHelper.Lerp(value1.X, value2.X, amount),
                MathHelper.Lerp(value1.Y, value2.Y, amount),
                MathHelper.Lerp(value1.Z, value2.Z, amount));
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains linear interpolation of the specified vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void Lerp(ref Vector3 value1, ref Vector3 value2, Fixed32 amount, out Vector3 result)
        {
            result.X = MathHelper.Lerp(value1.X, value2.X, amount);
            result.Y = MathHelper.Lerp(value1.Y, value2.Y, amount);
            result.Z = MathHelper.Lerp(value1.Z, value2.Z, amount);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains linear interpolation of the specified vectors. Uses
        /// <see cref="MathHelper.LerpPrecise" /> on MathHelper for the interpolation. Less efficient but more precise compared to
        /// <see cref="Vector3.Lerp(Vector3, Vector3, float)" />. See remarks section of <see cref="MathHelper.LerpPrecise" /> on
        /// MathHelper for more info.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <returns>The result of linear interpolation of the specified vectors.</returns>
        public static Vector3 LerpPrecise(Vector3 value1, Vector3 value2, Fixed32 amount)
        {
            return new Vector3(
                MathHelper.LerpPrecise(value1.X, value2.X, amount),
                MathHelper.LerpPrecise(value1.Y, value2.Y, amount),
                MathHelper.LerpPrecise(value1.Z, value2.Z, amount));
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains linear interpolation of the specified vectors. Uses
        /// <see cref="MathHelper.LerpPrecise" /> on MathHelper for the interpolation. Less efficient but more precise compared to
        /// <see cref="Vector3.Lerp(ref Vector3, ref Vector3, float, out Vector3)" />. See remarks section of
        /// <see cref="MathHelper.LerpPrecise" /> on MathHelper for more info.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
        /// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
        public static void LerpPrecise(ref Vector3 value1, ref Vector3 value2, Fixed32 amount, out Vector3 result)
        {
            result.X = MathHelper.LerpPrecise(value1.X, value2.X, amount);
            result.Y = MathHelper.LerpPrecise(value1.Y, value2.Y, amount);
            result.Z = MathHelper.LerpPrecise(value1.Z, value2.Z, amount);
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a maximal values from the two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vector3" /> with maximal values from the two vectors.</returns>
        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            return new Vector3(
                MathHelper.Max(value1.X, value2.X),
                MathHelper.Max(value1.Y, value2.Y),
                MathHelper.Max(value1.Z, value2.Z));
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a maximal values from the two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vector3" /> with maximal values from the two vectors as an output parameter.</param>
        public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = MathHelper.Max(value1.X, value2.X);
            result.Y = MathHelper.Max(value1.Y, value2.Y);
            result.Z = MathHelper.Max(value1.Z, value2.Z);
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a minimal values from the two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The <see cref="Vector3" /> with minimal values from the two vectors.</returns>
        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            return new Vector3(
                MathHelper.Min(value1.X, value2.X),
                MathHelper.Min(value1.Y, value2.Y),
                MathHelper.Min(value1.Z, value2.Z));
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a minimal values from the two vectors.</summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The <see cref="Vector3" /> with minimal values from the two vectors as an output parameter.</param>
        public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = MathHelper.Min(value1.X, value2.X);
            result.Y = MathHelper.Min(value1.Y, value2.Y);
            result.Z = MathHelper.Min(value1.Z, value2.Z);
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a multiplication of two vectors.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="value2">Source <see cref="Vector3" />.</param>
        /// <returns>The result of the vector multiplication.</returns>
        public static Vector3 Multiply(Vector3 value1, Vector3 value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a multiplication of <see cref="Vector3" /> and a scalar.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <returns>The result of the vector multiplication with a scalar.</returns>
        public static Vector3 Multiply(Vector3 value1, Fixed32 scaleFactor)
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            value1.Z *= scaleFactor;
            return value1;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a multiplication of <see cref="Vector3" /> and a scalar.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="scaleFactor">Scalar value.</param>
        /// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
        public static void Multiply(ref Vector3 value1, Fixed32 scaleFactor, out Vector3 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a multiplication of two vectors.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="value2">Source <see cref="Vector3" />.</param>
        /// <param name="result">The result of the vector multiplication as an output parameter.</param>
        public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains the specified vector inversion.</summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <returns>The result of the vector inversion.</returns>
        public static Vector3 Negate(Vector3 value)
        {
            value = new Vector3(-value.X, -value.Y, -value.Z);
            return value;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains the specified vector inversion.</summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <param name="result">The result of the vector inversion as an output parameter.</param>
        public static void Negate(ref Vector3 value, out Vector3 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
        }

        /// <summary>Turns this <see cref="Vector3" /> to a unit vector with the same direction.</summary>
        public void Normalize()
        {
            Fixed32 factor = Fixed32.Sqrt(X * X + Y * Y + Z * Z);
            factor = Fixed32.One / factor;
            X *= factor;
            Y *= factor;
            Z *= factor;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a normalized values from another vector.</summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <returns>Unit vector.</returns>
        public static Vector3 Normalize(Vector3 value)
        {
            Fixed32 factor = Fixed32.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
            factor = Fixed32.One / factor;
            return new Vector3(value.X * factor, value.Y * factor, value.Z * factor);
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains a normalized values from another vector.</summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <param name="result">Unit vector as an output parameter.</param>
        public static void Normalize(ref Vector3 value, out Vector3 result)
        {
            Fixed32 factor = Fixed32.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
            factor = Fixed32.One / factor;
            result.X = value.X * factor;
            result.Y = value.Y * factor;
            result.Z = value.Z * factor;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains reflect vector of the given vector and normal.</summary>
        /// <param name="vector">Source <see cref="Vector3" />.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <returns>Reflected vector.</returns>
        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            // I is the original array
            // N is the normal of the incident plane
            // R = I - (2 * N * ( DotProduct[ I,N] ))
            Vector3 reflectedVector;

            // inline the dotProduct here instead of calling method
            Fixed32 dotProduct = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
            reflectedVector.X = vector.X - Fixed32.Two * normal.X * dotProduct;
            reflectedVector.Y = vector.Y - Fixed32.Two * normal.Y * dotProduct;
            reflectedVector.Z = vector.Z - Fixed32.Two * normal.Z * dotProduct;

            return reflectedVector;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains reflect vector of the given vector and normal.</summary>
        /// <param name="vector">Source <see cref="Vector3" />.</param>
        /// <param name="normal">Reflection normal.</param>
        /// <param name="result">Reflected vector as an output parameter.</param>
        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
        {
            // I is the original array
            // N is the normal of the incident plane
            // R = I - (2 * N * ( DotProduct[ I,N] ))

            // inline the dotProduct here instead of calling method
            Fixed32 dotProduct = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
            result.X = vector.X - Fixed32.Two * normal.X * dotProduct;
            result.Y = vector.Y - Fixed32.Two * normal.Y * dotProduct;
            result.Z = vector.Z - Fixed32.Two * normal.Z * dotProduct;
        }

        /// <summary>Round the members of this <see cref="Vector3" /> towards the nearest integer value.</summary>
        public void Round()
        {
            X = FMath.Round(X);
            Y = FMath.Round(Y);
            Z = FMath.Round(Z);
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains members from another vector rounded to the nearest integer
        /// value.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <returns>The rounded <see cref="Vector3" />.</returns>
        public static Vector3 Round(Vector3 value)
        {
            value.X = FMath.Round(value.X);
            value.Y = FMath.Round(value.Y);
            value.Z = FMath.Round(value.Z);
            return value;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains members from another vector rounded to the nearest integer
        /// value.
        /// </summary>
        /// <param name="value">Source <see cref="Vector3" />.</param>
        /// <param name="result">The rounded <see cref="Vector3" />.</param>
        public static void Round(ref Vector3 value, out Vector3 result)
        {
            result.X = FMath.Round(value.X);
            result.Y = FMath.Round(value.Y);
            result.Z = FMath.Round(value.Z);
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains cubic interpolation of the specified vectors.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="value2">Source <see cref="Vector3" />.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Cubic interpolation of the specified vectors.</returns>
        public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, Fixed32 amount)
        {
            return new Vector3(
                MathHelper.SmoothStep(value1.X, value2.X, amount),
                MathHelper.SmoothStep(value1.Y, value2.Y, amount),
                MathHelper.SmoothStep(value1.Z, value2.Z, amount));
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains cubic interpolation of the specified vectors.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="value2">Source <see cref="Vector3" />.</param>
        /// <param name="amount">Weighting value.</param>
        /// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
        public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, Fixed32 amount, out Vector3 result)
        {
            result.X = MathHelper.SmoothStep(value1.X, value2.X, amount);
            result.Y = MathHelper.SmoothStep(value1.Y, value2.Y, amount);
            result.Z = MathHelper.SmoothStep(value1.Z, value2.Z, amount);
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains subtraction of on <see cref="Vector3" /> from a another.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="value2">Source <see cref="Vector3" />.</param>
        /// <returns>The result of the vector subtraction.</returns>
        public static Vector3 Subtract(Vector3 value1, Vector3 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }

        /// <summary>Creates a new <see cref="Vector3" /> that contains subtraction of on <see cref="Vector3" /> from a another.</summary>
        /// <param name="value1">Source <see cref="Vector3" />.</param>
        /// <param name="value2">Source <see cref="Vector3" />.</param>
        /// <param name="result">The result of the vector subtraction as an output parameter.</param>
        public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
        }

        /// <summary>
        /// Returns a <see cref="String" /> representation of this <see cref="Vector3" /> in the format: {X:[
        /// <see cref="X" />] Y:[<see cref="Y" />] Z:[<see cref="Z" />]}
        /// </summary>
        /// <returns>A <see cref="String" /> representation of this <see cref="Vector3" />.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(32);
            sb.Append("{X:");
            sb.Append(X);
            sb.Append(" Y:");
            sb.Append(Y);
            sb.Append(" Z:");
            sb.Append(Z);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains a transformation of 3d-vector by the specified
        /// <see cref="Matrix" />.
        /// </summary>
        /// <param name="position">Source <see cref="Vector3" />.</param>
        /// <param name="matrix">The transformation <see cref="Matrix" />.</param>
        /// <returns>Transformed <see cref="Vector3" />.</returns>
        public static Vector3 Transform(Vector3 position, Matrix matrix)
        {
            Transform(ref position, ref matrix, out position);
            return position;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains a transformation of 3d-vector by the specified
        /// <see cref="Matrix" />.
        /// </summary>
        /// <param name="position">Source <see cref="Vector3" />.</param>
        /// <param name="matrix">The transformation <see cref="Matrix" />.</param>
        /// <param name="result">Transformed <see cref="Vector3" /> as an output parameter.</param>
        public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector3 result)
        {
            Fixed32 x = position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41;
            Fixed32 y = position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42;
            Fixed32 z = position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43;
            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Apply transformation on vectors within array of <see cref="Vector3" /> by the specified <see cref="Matrix" />
        /// and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix" />.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">
        /// The starting index in the destination array, where the first <see cref="Vector3" />
        /// should be written.
        /// </param>
        /// <param name="length">The number of vectors to be transformed.</param>
        public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            // TODO: Are there options on some platforms to implement a vectorized version of this?

            for (int i = 0; i < length; i++)
            {
                Vector3 position = sourceArray[sourceIndex + i];
                destinationArray[destinationIndex + i] =
                    new Vector3(
                        position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
                        position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
                        position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43);
            }
        }

        /// <summary>
        /// Apply transformation on all vectors within array of <see cref="Vector3" /> by the specified
        /// <see cref="Matrix" /> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix" />.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void Transform(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination array length is lesser than source array length");

            // TODO: Are there options on some platforms to implement a vectorized version of this?

            for (int i = 0; i < sourceArray.Length; i++)
            {
                Vector3 position = sourceArray[i];
                destinationArray[i] =
                    new Vector3(
                        position.X * matrix.M11 + position.Y * matrix.M21 + position.Z * matrix.M31 + matrix.M41,
                        position.X * matrix.M12 + position.Y * matrix.M22 + position.Z * matrix.M32 + matrix.M42,
                        position.X * matrix.M13 + position.Y * matrix.M23 + position.Z * matrix.M33 + matrix.M43);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains a transformation of the specified normal by the specified
        /// <see cref="Matrix" />.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector3" /> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix" />.</param>
        /// <returns>Transformed normal.</returns>
        public static Vector3 TransformNormal(Vector3 normal, Matrix matrix)
        {
            TransformNormal(ref normal, ref matrix, out normal);
            return normal;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3" /> that contains a transformation of the specified normal by the specified
        /// <see cref="Matrix" />.
        /// </summary>
        /// <param name="normal">Source <see cref="Vector3" /> which represents a normal vector.</param>
        /// <param name="matrix">The transformation <see cref="Matrix" />.</param>
        /// <param name="result">Transformed normal as an output parameter.</param>
        public static void TransformNormal(ref Vector3 normal, ref Matrix matrix, out Vector3 result)
        {
            Fixed32 x = normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31;
            Fixed32 y = normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32;
            Fixed32 z = normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33;
            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Apply transformation on normals within array of <see cref="Vector3" /> by the specified <see cref="Matrix" />
        /// and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="sourceIndex">The starting index of transformation in the source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix" />.</param>
        /// <param name="destinationArray">Destination array.</param>
        /// <param name="destinationIndex">
        /// The starting index in the destination array, where the first <see cref="Vector3" />
        /// should be written.
        /// </param>
        /// <param name="length">The number of normals to be transformed.</param>
        public static void TransformNormal(Vector3[] sourceArray,
                                           int sourceIndex,
                                           ref Matrix matrix,
                                           Vector3[] destinationArray,
                                           int destinationIndex,
                                           int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (sourceArray.Length < sourceIndex + length)
                throw new ArgumentException("Source array length is lesser than sourceIndex + length");
            if (destinationArray.Length < destinationIndex + length)
                throw new ArgumentException("Destination array length is lesser than destinationIndex + length");

            for (int x = 0; x < length; x++)
            {
                Vector3 normal = sourceArray[sourceIndex + x];

                destinationArray[destinationIndex + x] =
                    new Vector3(
                        normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31,
                        normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32,
                        normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33);
            }
        }

        /// <summary>
        /// Apply transformation on all normals within array of <see cref="Vector3" /> by the specified
        /// <see cref="Matrix" /> and places the results in an another array.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <param name="matrix">The transformation <see cref="Matrix" />.</param>
        /// <param name="destinationArray">Destination array.</param>
        public static void TransformNormal(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
        {
            if (sourceArray == null)
                throw new ArgumentNullException(nameof(sourceArray));
            if (destinationArray == null)
                throw new ArgumentNullException(nameof(destinationArray));
            if (destinationArray.Length < sourceArray.Length)
                throw new ArgumentException("Destination array length is lesser than source array length");

            for (int i = 0; i < sourceArray.Length; i++)
            {
                Vector3 normal = sourceArray[i];

                destinationArray[i] =
                    new Vector3(
                        normal.X * matrix.M11 + normal.Y * matrix.M21 + normal.Z * matrix.M31,
                        normal.X * matrix.M12 + normal.Y * matrix.M22 + normal.Z * matrix.M32,
                        normal.X * matrix.M13 + normal.Y * matrix.M23 + normal.Z * matrix.M33);
            }
        }

        /// <summary>Deconstruction method for <see cref="Vector3" />.</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Deconstruct(out Fixed32 x, out Fixed32 y, out Fixed32 z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        /// <summary>Compares whether two <see cref="Vector3" /> instances are equal.</summary>
        /// <param name="value1"><see cref="Vector3" /> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Vector3" /> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Vector3 value1, Vector3 value2)
        {
            return value1.X == value2.X
                   && value1.Y == value2.Y
                   && value1.Z == value2.Z;
        }

        /// <summary>Compares whether two <see cref="Vector3" /> instances are not equal.</summary>
        /// <param name="value1"><see cref="Vector3" /> instance on the left of the not equal sign.</param>
        /// <param name="value2"><see cref="Vector3" /> instance on the right of the not equal sign.</param>
        /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Vector3 value1, Vector3 value2)
        {
            return !(value1 == value2);
        }

        /// <summary>Adds two vectors.</summary>
        /// <param name="value1">Source <see cref="Vector3" /> on the left of the add sign.</param>
        /// <param name="value2">Source <see cref="Vector3" /> on the right of the add sign.</param>
        /// <returns>Sum of the vectors.</returns>
        public static Vector3 operator +(Vector3 value1, Vector3 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }

        /// <summary>Inverts values in the specified <see cref="Vector3" />.</summary>
        /// <param name="value">Source <see cref="Vector3" /> on the right of the sub sign.</param>
        /// <returns>Result of the inversion.</returns>
        public static Vector3 operator -(Vector3 value)
        {
            value = new Vector3(-value.X, -value.Y, -value.Z);
            return value;
        }

        /// <summary>Subtracts a <see cref="Vector3" /> from a <see cref="Vector3" />.</summary>
        /// <param name="value1">Source <see cref="Vector3" /> on the left of the sub sign.</param>
        /// <param name="value2">Source <see cref="Vector3" /> on the right of the sub sign.</param>
        /// <returns>Result of the vector subtraction.</returns>
        public static Vector3 operator -(Vector3 value1, Vector3 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }

        /// <summary>Multiplies the components of two vectors by each other.</summary>
        /// <param name="value1">Source <see cref="Vector3" /> on the left of the mul sign.</param>
        /// <param name="value2">Source <see cref="Vector3" /> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication.</returns>
        public static Vector3 operator *(Vector3 value1, Vector3 value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            value1.Z *= value2.Z;
            return value1;
        }

        /// <summary>Multiplies the components of vector by a scalar.</summary>
        /// <param name="value">Source <see cref="Vector3" /> on the left of the mul sign.</param>
        /// <param name="scaleFactor">Scalar value on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector3 operator *(Vector3 value, Fixed32 scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            value.Z *= scaleFactor;
            return value;
        }

        /// <summary>Multiplies the components of vector by a scalar.</summary>
        /// <param name="scaleFactor">Scalar value on the left of the mul sign.</param>
        /// <param name="value">Source <see cref="Vector3" /> on the right of the mul sign.</param>
        /// <returns>Result of the vector multiplication with a scalar.</returns>
        public static Vector3 operator *(Fixed32 scaleFactor, Vector3 value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            value.Z *= scaleFactor;
            return value;
        }

        /// <summary>Divides the components of a <see cref="Vector3" /> by the components of another <see cref="Vector3" />.</summary>
        /// <param name="value1">Source <see cref="Vector3" /> on the left of the div sign.</param>
        /// <param name="value2">Divisor <see cref="Vector3" /> on the right of the div sign.</param>
        /// <returns>The result of dividing the vectors.</returns>
        public static Vector3 operator /(Vector3 value1, Vector3 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            value1.Z /= value2.Z;
            return value1;
        }

        /// <summary>Divides the components of a <see cref="Vector3" /> by a scalar.</summary>
        /// <param name="value1">Source <see cref="Vector3" /> on the left of the div sign.</param>
        /// <param name="divider">Divisor scalar on the right of the div sign.</param>
        /// <returns>The result of dividing a vector by a scalar.</returns>
        public static Vector3 operator /(Vector3 value1, Fixed32 divider)
        {
            Fixed32 factor = Fixed32.One / divider;
            value1.X *= factor;
            value1.Y *= factor;
            value1.Z *= factor;
            return value1;
        }
    }
}
