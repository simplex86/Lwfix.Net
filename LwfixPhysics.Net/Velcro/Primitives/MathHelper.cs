// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'MONOGAME LICENSE.txt', which is part of this source code package.

using System;
using SimplexLab.Lwfix;

// ReSharper disable once CheckNamespace
namespace SimplexLab.LwfixPhysics.Velcro.Primitives
{
    /// <summary>Contains commonly used precalculated values and mathematical operations.</summary>
    internal static class MathHelper
    {
        /// <summary>Represents the mathematical constant e(2.71828175).</summary>
        public static readonly Fixed32 E = Fixed32.E;

        /// <summary>Represents the log base ten of e(0.4342945).</summary>
        public static readonly Fixed32 Log10E = (Fixed32)0.4342945;

        /// <summary>Represents the log base two of e(1.442695).</summary>
        public static readonly Fixed32 Log2E = (Fixed32)1.442695;

        /// <summary>Represents the value of pi(3.14159274).</summary>
        public static readonly Fixed32 Pi = Fixed32.PI;

        /// <summary>Represents the value of pi divided by two(1.57079637).</summary>
        public static readonly Fixed32 PiOver2 = Fixed32.PI / (Fixed32)2.0;

        /// <summary>Represents the value of pi divided by four(0.7853982).</summary>
        public static readonly Fixed32 PiOver4 = Fixed32.PI / (Fixed32)4.0;

        /// <summary>Represents the value of pi times two(6.28318548).</summary>
        public static readonly Fixed32 TwoPi = Fixed32.PI * (Fixed32)2.0;

        /// <summary>Represents the value of pi times two(6.28318548). This is an alias of TwoPi.</summary>
        public static readonly Fixed32 Tau = TwoPi;

        /// <summary>
        /// Returns the Cartesian coordinate for one axis of a point that is defined by a given triangle and two
        /// normalized barycentric (areal) coordinates.
        /// </summary>
        /// <param name="value1">The coordinate on one axis of vertex 1 of the defining triangle.</param>
        /// <param name="value2">The coordinate on the same axis of vertex 2 of the defining triangle.</param>
        /// <param name="value3">The coordinate on the same axis of vertex 3 of the defining triangle.</param>
        /// <param name="amount1">
        /// The normalized barycentric (areal) coordinate b2, equal to the weighting factor for vertex 2, the
        /// coordinate of which is specified in value2.
        /// </param>
        /// <param name="amount2">
        /// The normalized barycentric (areal) coordinate b3, equal to the weighting factor for vertex 3, the
        /// coordinate of which is specified in value3.
        /// </param>
        /// <returns>Cartesian coordinate of the specified point with respect to the axis being used.</returns>
        public static Fixed32 Barycentric(Fixed32 value1, Fixed32 value2, Fixed32 value3, Fixed32 amount1, Fixed32 amount2)
        {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        /// <summary>Performs a Catmull-Rom interpolation using the specified positions.</summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>A position that is the result of the Catmull-Rom interpolation.</returns>
        public static Fixed32 CatmullRom(Fixed32 value1, Fixed32 value2, Fixed32 value3, Fixed32 value4, Fixed32 amount)
        {
            // Using formula from http://www.mvps.org/directx/articles/catmull/
            // Internally using doubles not to lose precission
            Fixed32 amountSquared = amount * amount;
            Fixed32 amountCubed = amountSquared * amount;
            return (Fixed32)((Fixed32)0.5 * ((Fixed32)2.0 * value2 +
                                  (value3 - value1) * amount +
                                  ((Fixed32)2.0 * value1 - (Fixed32)5.0 * value2 + (Fixed32)4.0 * value3 - value4) * amountSquared +
                                  ((Fixed32)3.0 * value2 - value1 - (Fixed32)3.0 * value3 + value4) * amountCubed));
        }

        /// <summary>Restricts a value to be within a specified range.</summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value. If <c>value</c> is less than <c>min</c>, <c>min</c> will be returned.</param>
        /// <param name="max">The maximum value. If <c>value</c> is greater than <c>max</c>, <c>max</c> will be returned.</param>
        /// <returns>The clamped value.</returns>
        public static Fixed32 Clamp(Fixed32 value, Fixed32 min, Fixed32 max)
        {
            return FMath.Clamp(value, min, max);
        }

        /// <summary>Restricts a value to be within a specified range.</summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value. If <c>value</c> is less than <c>min</c>, <c>min</c> will be returned.</param>
        /// <param name="max">The maximum value. If <c>value</c> is greater than <c>max</c>, <c>max</c> will be returned.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(int value, int min, int max)
        {
            value = value > max ? max : value;
            value = value < min ? min : value;
            return value;
        }

        /// <summary>Calculates the absolute value of the difference of two values.</summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>Distance between the two values.</returns>
        public static Fixed32 Distance(Fixed32 value1, Fixed32 value2)
        {
            return FMath.Abs(value1 - value2);
        }

        /// <summary>Performs a Hermite spline interpolation.</summary>
        /// <param name="value1">Source position.</param>
        /// <param name="tangent1">Source tangent.</param>
        /// <param name="value2">Source position.</param>
        /// <param name="tangent2">Source tangent.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of the Hermite spline interpolation.</returns>
        public static Fixed32 Hermite(Fixed32 value1, Fixed32 tangent1, Fixed32 value2, Fixed32 tangent2, Fixed32 amount)
        {
            // All transformed to double not to lose precission
            // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
            Fixed32 v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
            Fixed32 sCubed = s * s * s;
            Fixed32 sSquared = s * s;

            if (amount == Fixed32.Zero)
                result = value1;
            else if (amount == Fixed32.One)
                result = value2;
            else
            {
                result = ((Fixed32)2 * v1 - (Fixed32)2 * v2 + t2 + t1) * sCubed +
                         ((Fixed32)3 * v2 - (Fixed32)3 * v1 - (Fixed32)2 * t1 - t2) * sSquared +
                         t1 * s +
                         v1;
            }
            return result;
        }

        /// <summary>Linearly interpolates between two values.</summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Destination value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula:
        /// <code>value1 + (value2 - value1) * amount</code>. Passing amount a value of 0 will cause value1 to be returned, a value
        /// of 1 will cause value2 to be returned. See <see cref="MathHelper.LerpPrecise" /> for a less efficient version with more
        /// precision around edge cases.
        /// </remarks>
        public static Fixed32 Lerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)
        {
            return FMath.Lerp(value1, value2, amount);
        }

        /// <summary>
        /// Linearly interpolates between two values. This method is a less efficient, more precise version of
        /// <see cref="MathHelper.Lerp" />. See remarks for more info.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Destination value.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
        /// <returns>Interpolated value.</returns>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula:
        /// <code>((1 - amount) * value1) + (value2 * amount)</code>. Passing amount a value of 0 will cause value1 to be returned,
        /// a value of 1 will cause value2 to be returned. This method does not have the floating point precision issue that
        /// <see cref="MathHelper.Lerp" /> has. i.e. If there is a big gap between value1 and value2 in magnitude (e.g.
        /// value1=10000000000000000, value2=1), right at the edge of the interpolation range (amount=1),
        /// <see cref="MathHelper.Lerp" /> will return 0 (whereas it should return 1). This also holds for value1=10^17, value2=10;
        /// value1=10^18,value2=10^2... so on. For an in depth explanation of the issue, see below references: Relevant Wikipedia
        /// Article: https://en.wikipedia.org/wiki/Linear_interpolation#Programming_language_support Relevant StackOverflow Answer:
        /// http://stackoverflow.com/questions/4353525/floating-point-linear-interpolation#answer-23716956
        /// </remarks>
        public static Fixed32 LerpPrecise(Fixed32 value1, Fixed32 value2, Fixed32 amount)
        {
            return (Fixed32.One - amount) * value1 + value2 * amount;
        }

        /// <summary>Returns the greater of two values.</summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The greater value.</returns>
        public static Fixed32 Max(Fixed32 value1, Fixed32 value2)
        {
            return FMath.Max(value1, value2);
        }

        /// <summary>Returns the greater of two values.</summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The greater value.</returns>
        public static int Max(int value1, int value2)
        {
            return value1 > value2 ? value1 : value2;
        }

        /// <summary>Returns the lesser of two values.</summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The lesser value.</returns>
        public static Fixed32 Min(Fixed32 value1, Fixed32 value2)
        {
            return FMath.Min(value1, value2);
        }

        /// <summary>Returns the lesser of two values.</summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The lesser value.</returns>
        public static int Min(int value1, int value2)
        {
            return value1 < value2 ? value1 : value2;
        }

        /// <summary>Interpolates between two values using a cubic equation.</summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">Weighting value.</param>
        /// <returns>Interpolated value.</returns>
        public static Fixed32 SmoothStep(Fixed32 value1, Fixed32 value2, Fixed32 amount)
        {
            return FMath.SmoothStep(value1, value2, amount);
        }

        /// <summary>Converts radians to degrees.</summary>
        /// <param name="radians">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        /// <remarks>This method uses double precission internally, though it returns single float Factor = 180 / pi</remarks>
        public static Fixed32 ToDegrees(Fixed32 radians)
        {
            return FMath.RadianToDegree(radians);
        }

        /// <summary>Converts degrees to radians.</summary>
        /// <param name="degrees">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        /// <remarks>This method uses double precission internally, though it returns single float Factor = pi / 180</remarks>
        public static Fixed32 ToRadians(Fixed32 degrees)
        {
            return FMath.DegreeToRadian(degrees);
        }

        /// <summary>Reduces a given angle to a value between π and -π.</summary>
        /// <param name="angle">The angle to reduce, in radians.</param>
        /// <returns>The new angle, in radians.</returns>
        public static Fixed32 WrapAngle(Fixed32 angle)
        {
            if (angle > -Pi && angle <= Pi)
                return angle;
            angle %= TwoPi;
            if (angle <= -Pi)
                return angle + TwoPi;
            if (angle > Pi)
                return angle - TwoPi;
            return angle;
        }

        /// <summary>Determines if value is powered by two.</summary>
        /// <param name="value">A value.</param>
        /// <returns><c>true</c> if <c>value</c> is powered by two; otherwise <c>false</c>.</returns>
        public static bool IsPowerOfTwo(int value)
        {
            return value > 0 && (value & (value - 1)) == 0;
        }
    }
}
