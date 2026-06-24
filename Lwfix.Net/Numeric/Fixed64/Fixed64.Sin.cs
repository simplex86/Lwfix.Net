using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 正弦
    /// <para>包含定点数的正弦函数实现，包括标准实现和快速实现</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>1/(3!) 常量</summary>
        private static readonly Fixed64 C3  = FromRaw(FRAC_SCALE / 6);
        /// <summary>1/(5!) 常量</summary>
        private static readonly Fixed64 C5  = FromRaw(FRAC_SCALE / 120);
        /// <summary>1/(7!) 常量</summary>
        private static readonly Fixed64 C7  = FromRaw(FRAC_SCALE / 5040);
        /// <summary>1/(9!) 常量</summary>
        private static readonly Fixed64 C9  = FromRaw(FRAC_SCALE / 362880);
        /// <summary>1/(11!) 常量</summary>
        private static readonly Fixed64 C11 = FromRaw(FRAC_SCALE / 39916800);
        /// <summary>1/(13!) 常量</summary>
        private static readonly Fixed64 C13 = FromRaw(FRAC_SCALE / 6227020800);
        /// <summary>1/(15!) 常量</summary>
        private static readonly Fixed64 C15 = FromRaw(FRAC_SCALE / 1307674368000);
        /// <summary>1/(17!) 常量</summary>
        private static readonly Fixed64 C17 = FromRaw(FRAC_SCALE / 355687428096000);
        /// <summary>1/(19!) 常量</summary>
        private static readonly Fixed64 C19 = FromRaw(FRAC_SCALE / 121645100408832000);
        /// <summary>1/(21!) 常量</summary>
        private static readonly Fixed64 C21 = FromRaw(FRAC_SCALE / Int128.Parse("51090942171709440000"));
        /// <summary>1/(23!) 常量</summary>
        private static readonly Fixed64 C23 = FromRaw(FRAC_SCALE / Int128.Parse("25852016738884976640000"));
        /// <summary>1/(25!) 常量</summary>
        private static readonly Fixed64 C25 = FromRaw(FRAC_SCALE / Int128.Parse("15511210043330985984000000"));
        /// <summary>1/(27!) 常量</summary>
        private static readonly Fixed64 C27 = FromRaw(FRAC_SCALE / Int128.Parse("10888869450418352160768000000"));

        /// <summary>
        /// 计算正弦值
        /// <para>使用泰勒展开计算定点数的正弦值</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正弦值，范围在[-1, 1]之间</returns>
        public static Fixed64 Sin(Fixed64 radian)
        {
            if (PreprocessSin(radian, out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian);
            var referenced = ReduceRadian4Sin(normalized, out var sign);
            var result = TaylorEvaluate4Sin(referenced);

            return sign ? -result : result;
        }

        /// <summary>
        /// 将角度缩减到[0, π/2]区间
        /// <para>利用正弦函数的对称性，将角度缩减到[0, π/2]区间以减少计算误差</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="sign">正弦值的符号</param>
        /// <returns>缩减到[0, π/2]区间的角度</returns>
        internal static Fixed64 ReduceRadian4Sin(Fixed64 radian, out bool sign)
        {
            sign = false;

            var reference = radian;
            var quadrant = (int)((radian.rawvalue << 1) / PI.rawvalue);

            switch (quadrant)
            {
                case 0: // 第一象限 [0, π/2)
                    break;
                case 1: // 第二象限 [π/2, π)
                    reference = PI - radian;
                    break;
                case 2: // 第三象限 [π, 3π/2)
                    reference = radian - PI;
                    sign = true;
                    break;
                case 3: // 第四象限 [3π/2, 2π)
                    reference = Two_PI - radian;
                    sign = true;
                    break;
                default:
                    reference = Zero;
                    break;
            }

            return reference;
        }

        /// <summary>
        /// 使用泰勒展开计算正弦值
        /// <para>在[0, π/2]区间内使用泰勒展开计算正弦值</para>
        /// </summary>
        /// <param name="x">角度（弧度），范围在[0, π/2]之间</param>
        /// <returns>正弦值</returns>
        private static Fixed64 TaylorEvaluate4Sin(Fixed64 x)
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

            return x1 - x3 * C3 + x5 * C5 - x7 * C7 + x9 * C9 - x11 * C11
                 + x13 * C13 - x15 * C15 + x17 * C17 - x19 * C19
                 + x21 * C21 - x23 * C23 + x25 * C25 - x27 * C27;
        }

        /// <summary>
        /// 快速计算正弦值
        /// <para>使用查表法快速计算正弦值，速度比Sin函数快，但精度较低</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正弦值，范围在[-1, 1]之间</returns>
        public static Fixed64 FastSin(Fixed64 radian)
        {
            if (PreprocessSin(radian, out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian);
            var referenced = ReduceRadian4Sin(normalized, out var sign);

            var index = (int)(referenced.rawvalue >> 47);
            if (index >= SinLut.Length) index = SinLut.Length - 1;

            var nearest = SinLut[index];
            if (sign) nearest = -nearest;

            return FromRaw(nearest);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessSin(Fixed64 radian, out Fixed64 r)
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
        /// 计算反正弦值
        /// </summary>
        /// <param name="value">正弦值，范围在[-1, 1]之间</param>
        /// <returns>反正弦值，范围在[-π/2, π/2]之间</returns>
        public static Fixed64 Asin(Fixed64 value)
        {
            return Half_PI - Acos(value);
        }
    }
}
