using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 正切
    /// <para>包含定点数的正切函数实现，包括标准实现和快速实现</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>1/3 常量</summary>
        private static readonly Fixed64 T3  = FromRaw(FRAC_SCALE / 3);
        /// <summary>2/15 常量</summary>
        private static readonly Fixed64 T5  = FromRaw(FRAC_SCALE * 2 / 15);
        /// <summary>17/315 常量</summary>
        private static readonly Fixed64 T7  = FromRaw(FRAC_SCALE * 17 / 315);
        /// <summary>62/2835 常量</summary>
        private static readonly Fixed64 T9  = FromRaw(FRAC_SCALE * 62 / 2835);
        /// <summary>1382/155925 常量</summary>
        private static readonly Fixed64 T11 = FromRaw(FRAC_SCALE * 1382 / 155925);
        /// <summary>21844/6081075 常量</summary>
        private static readonly Fixed64 T13 = FromRaw(FRAC_SCALE * 21844 / 6081075);
        /// <summary>929569/638512875 常量</summary>
        private static readonly Fixed64 T15 = FromRaw(FRAC_SCALE * 929569 / 638512875);
        /// <summary>6404582/10854718875 常量</summary>
        private static readonly Fixed64 T17 = FromRaw(FRAC_SCALE * 6404582 / 10854718875);
        /// <summary>44361662/1338557227875 常量</summary>
        private static readonly Fixed64 T19 = FromRaw(FRAC_SCALE * 44361662 / 1338557227875);
        /// <summary>188684306/11097486871875 常量</summary>
        private static readonly Fixed64 T21 = FromRaw(FRAC_SCALE * 188684306 / 11097486871875);
        /// <summary>113042894/61925872973875 常量</summary>
        private static readonly Fixed64 T23 = FromRaw(FRAC_SCALE * 113042894 / 61925872973875L);
        /// <summary>2363308933/6792940263506825 常量</summary>
        private static readonly Fixed64 T25 = FromRaw(FRAC_SCALE * 2363308933 / 6792940263506825L);
        /// <summary>1489809743563/2633793354354620625 常量</summary>
        private static readonly Fixed64 T27 = FromRaw(FRAC_SCALE * 1489809743563 / 2633793354354620625L);

        /// <summary>
        /// 计算正切值
        /// <para>使用泰勒展开计算定点数的正切值</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正切值</returns>
        public static Fixed64 Tan(Fixed64 radian)
        {
            if (PreprocessTan(radian, out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian, PI);
            var referenced = ReduceRadian4Tan(normalized, out var sign);

            if (referenced == Zero)       return Zero;
            if (referenced == Half_PI)    return sign ? MinValue : MaxValue;
            if (referenced == Quarter_PI) return sign ? NegativeOne : One;

            var result = Zero;
            if (referenced < Quarter_PI)
            {
                result = TaylorEvaluate4Tan(referenced);
            }
            else
            {
                // 使用cotangent来计算接近π/2的角度，提高精度
                var temp = Half_PI - referenced;
                var cot = TaylorEvaluate4Tan(temp);
                result = cot.Reciprocal();
            }

            return sign ? -result : result;
        }

        /// <summary>
        /// 将角度缩减到[0, π/2]区间
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="sign">正切值的符号</param>
        /// <returns>缩减到[0, π/2]区间的角度</returns>
        internal static Fixed64 ReduceRadian4Tan(Fixed64 radian, out bool sign)
        {
            sign = false;

            var referenced = radian;
            if (referenced > Half_PI)
            {
                sign = true;
                referenced = PI - referenced;
            }

            return referenced;
        }

        /// <summary>
        /// 使用泰勒展开计算正切值
        /// <para>在[0, π/4]区间内使用泰勒展开计算正切值</para>
        /// </summary>
        /// <param name="x">角度（弧度），范围在[0, π/4]之间</param>
        /// <returns>正切值</returns>
        private static Fixed64 TaylorEvaluate4Tan(Fixed64 x)
        {
            var x1  = x;
            var x2  = x1 * x1;
            var x3  = x1 * x2;
            var x5  = x3 * x2;
            var x7  = x5 * x2;
            var x9  = x7 * x2;
            var x11 = x9 * x2;
            var x13 = x11 * x2;
            var x15 = x13 * x2;
            var x17 = x15 * x2;
            var x19 = x17 * x2;
            var x21 = x19 * x2;
            var x23 = x21 * x2;
            var x25 = x23 * x2;
            var x27 = x25 * x2;

            return x1        +
                   x3  * T3  +
                   x5  * T5  +
                   x7  * T7  +
                   x9  * T9  +
                   x11 * T11 +
                   x13 * T13 +
                   x15 * T15 +
                   x17 * T17 +
                   x19 * T19 +
                   x21 * T21 +
                   x23 * T23 +
                   x25 * T25 +
                   x27 * T27;
        }

        /// <summary>
        /// 快速计算正切值
        /// <para>使用查表法快速计算正切值，速度比Tan函数快，但精度较低</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正切值</returns>
        public static Fixed64 FastTan(Fixed64 radian)
        {
            if (PreprocessTan(radian, out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian, PI);
            var referenced = ReduceRadian4Tan(normalized, out var sign);

            if (referenced == Zero)       return Zero;
            if (referenced == Half_PI)    return sign ? MinValue : MaxValue;
            if (referenced == Quarter_PI) return sign ? NegativeOne : One;

            var index = referenced * (TanLut.Length - 1) / Half_PI;
            var round = index.Round();
            var error = index - round;

            int roundInt = (int)round;
            int nextIndex = roundInt + error.Sign();

            // 确保索引在有效范围内
            if (nextIndex < 0) nextIndex = 0;
            if (nextIndex >= TanLut.Length) nextIndex = TanLut.Length - 1;

            var nearest1 = FromRaw(TanLut[roundInt]);
            var nearest2 = FromRaw(TanLut[nextIndex]);

            var delta = error * (nearest2 - nearest1);
            var interpolated = nearest1.rawvalue + delta.rawvalue;
            if (sign) interpolated = -interpolated;

            return FromRaw(interpolated);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessTan(Fixed64 radian, out Fixed64 r)
        {
            if (radian.IsNaN() ||
                radian.IsPositiveInfinity() ||
                radian.IsNegativeInfinity())
            {
                r = NaN;
                return true;
            }

            r = Zero;
            return false;
        }

        /// <summary>
        /// 计算反正切值
        /// </summary>
        /// <param name="z">正切值</param>
        /// <returns>反正切值，范围在[-π/2, π/2]之间</returns>
        public static Fixed64 Atan(Fixed64 z)
        {
            if (z.IsZero()) return Zero;

            // Force positive values for argument: Atan(-z) = -Atan(z).
            var neg = z.IsNegative();
            if (neg) z = -z;

            var invert = z > One;
            if (invert) z = z.Reciprocal();

            var result = One;
            var term = One;
            var two = Two;
            var three = new Fixed64(3);

            var sq1 = z * z;
            var sq2 = sq1 * two;
            var sqp1 = sq1 + One;
            var sqp2 = sqp1 * two;
            var dividend = sq2;
            var divisor = sqp1 * three;

            for (var i = 2; i < 60; ++i)
            {
                term *= dividend / divisor;
                result += term;

                dividend += sq2;
                divisor += sqp2;

                if (term.IsZero()) break;
            }

            result = result * z / sqp1;
            if (invert) result = Half_PI - result;

            return neg ? -result : result;
        }

        /// <summary>
        /// 计算 y/x 的反正切值
        /// </summary>
        /// <param name="y">分子</param>
        /// <param name="x">分母</param>
        /// <returns>反正切值，范围在 [-π, π]</returns>
        public static Fixed64 Atan2(Fixed64 y, Fixed64 x)
        {
            var yl = y.rawvalue;
            var xl = x.rawvalue;

            if (xl == 0)
            {
                if (yl > 0)  return Half_PI;
                if (yl == 0) return Zero;
                return -Half_PI;
            }

            var z = y / x;
            var sm = new Fixed64(0.28125); // 7/24，用于提高精度的常数

            // 处理溢出情况
            var temp = sm * z * z;
            if (One + temp == MaxValue)
            {
                return y < Zero ? -Half_PI : Half_PI;
            }

            var atan = Zero;
            if (Abs(z) < One)
            {
                atan = z / (One + sm * z * z);
                if (xl < 0) return (yl < 0) ? atan - PI : atan + PI;
            }
            else
            {
                atan = Half_PI - z / (z * z + sm);
                if (yl < 0) return atan - PI;
            }

            return atan;
        }
    }
}
