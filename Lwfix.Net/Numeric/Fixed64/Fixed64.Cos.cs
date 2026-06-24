using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 余弦
    /// <para>包含定点数的余弦函数实现，包括标准实现和快速实现</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 计算余弦值
        /// <para>使用正弦函数计算定点数的余弦值</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>余弦值，范围在[-1, 1]之间</returns>
        public static Fixed64 Cos(Fixed64 radian)
        {
            if (PreprocessCos(radian, out var r))
            {
                return r;
            }

            return Sin(radian + Half_PI);
        }

        /// <summary>
        /// 快速计算余弦值
        /// <para>使用快速正弦函数计算定点数的余弦值，速度比Cos函数快，但精度较低</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>余弦值，范围在[-1, 1]之间</returns>
        public static Fixed64 FastCos(Fixed64 radian)
        {
            if (PreprocessCos(radian, out var r))
            {
                return r;
            }

            return FastSin(radian + Half_PI);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessCos(Fixed64 radian, out Fixed64 r)
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
        /// 计算反余弦值
        /// </summary>
        /// <param name="value">余弦值，范围在[-1, 1]之间</param>
        /// <returns>反余弦值，范围在[0, π]之间</returns>
        /// <exception cref="ArgumentOutOfRangeException">当value不在[-1, 1]范围内时抛出</exception>
        public static Fixed64 Acos(Fixed64 value)
        {
            if (value < NegativeOne || value > One)
            {
                throw new ArgumentOutOfRangeException("value", "Must between NegativeOne and One");
            }

            if (value.IsZero()) return Half_PI;

            var result = Atan(Sqrt(One - value * value) / value);
            return value.IsNegative() ? result + PI : result;
        }
    }
}
