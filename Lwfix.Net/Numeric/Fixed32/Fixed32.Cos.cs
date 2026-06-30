using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 余弦
    /// <para>包含定点数的余弦函数实现，包括标准实现和快速实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 计算余弦值
        /// <para>使用正弦函数计算定点数的余弦值</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>余弦值，范围在[-1, 1]之间</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷或负无穷，返回NaN</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Cos(Fixed32 radian)
        {
            if (PreprocessCos(radian, out var r))
            {
                return r;
            }

            // 优化（P1-1）：原路径 Sin(radian + Half_PI) 会再次执行 PreprocessSin（冗余 NaN/∞ 检查）。
            // 此处直接复用 SinFromNormalized，跳过冗余 Preprocess，数学等价：
            // radian 有限 ⇒ radian + Half_PI 有限，PreprocessSin 必返回 false。
            var normalized = NormalizeRadian(radian + Half_PI);
            return SinFromNormalized(normalized);
        }

        /// <summary>
        /// 快速计算余弦值
        /// <para>使用快速正弦函数计算定点数的余弦值，速度比Cos函数快，但精度较低</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>余弦值，范围在[-1, 1]之间</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷或负无穷，返回NaN</item>
        /// </list>
        /// 注意：该方法的误差大于Cos函数
        /// </remarks>
        public static Fixed32 FastCos(Fixed32 radian)
        {
            if (PreprocessCos(radian, out var r))
            {
                return r;
            }

            // 优化（P1-1）：同 Cos，跳过 FastSin 内冗余 PreprocessSin。
            var normalized = NormalizeRadian(radian + Half_PI);
            return FastSinFromNormalized(normalized);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理余弦函数的特殊输入情况</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessCos(Fixed32 radian, out Fixed32 r)
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
        /// <para>计算定点数的反余弦值</para>
        /// </summary>
        /// <param name="value">余弦值，范围在[-1, 1]之间</param>
        /// <returns>反余弦值，范围在[0, π]之间</returns>
        /// <exception cref="ArgumentOutOfRangeException">当value不在[-1, 1]范围内时抛出</exception>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是0，返回π/2</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Acos(Fixed32 value)
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
