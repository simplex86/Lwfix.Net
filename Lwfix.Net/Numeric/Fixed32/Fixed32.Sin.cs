using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 正弦
    /// <para>包含定点数的正弦函数实现，包括标准实现和快速实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>1/(3!) 常量</summary>
        private static readonly Fixed32 C3  = FromRaw(715827882); // 1/(3!) 
        /// <summary>1/(5!) 常量</summary>
        private static readonly Fixed32 C5  = FromRaw(35791394);  // 1/(5!) 
        /// <summary>1/(7!) 常量</summary>
        private static readonly Fixed32 C7  = FromRaw(852176);    // 1/(7!) 
        /// <summary>1/(9!) 常量</summary>
        private static readonly Fixed32 C9  = FromRaw(11836);     // 1/(9!) 
        /// <summary>1/(11!) 常量</summary>
        private static readonly Fixed32 C11 = FromRaw(108);       // 1/(11!)

        /// <summary>
        /// 计算正弦值
        /// <para>使用泰勒展开计算定点数的正弦值</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正弦值，范围在[-1, 1]之间</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷或负无穷，返回NaN</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Sin(Fixed32 radian)
        {
            if (PreprocessSin(radian,out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian);
            return SinFromNormalized(normalized);
        }

        /// <summary>
        /// 对已归一化到 [0, 2π) 的角度计算正弦值
        /// <para>跳过 Preprocess/Normalize，直接做象限缩减 + 泰勒展开</para>
        /// </summary>
        /// <param name="normalized">已归一化到 [0, 2π) 的角度</param>
        /// <returns>正弦值</returns>
        /// <remarks>
        /// 优化（P1-1）：提取此辅助方法，使 <see cref="Cos"/> 与 <see cref="SinCos"/> 可共享
        /// 象限缩减 + 泰勒展开路径，避免重复的 Preprocess 调用。
        /// </remarks>
        private static Fixed32 SinFromNormalized(Fixed32 normalized)
        {
            var referenced = ReduceRadian4Sin(normalized, out var sign);
            var result = TaylorEvaluate4Sin(referenced);
            return sign ? -result : result;
        }

        /// <summary>
        /// 同时计算正弦和余弦值（真融合）
        /// <para>一次归一化，sin/cos 共享象限缩减 + 泰勒展开路径</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>(正弦值, 余弦值)</returns>
        /// <remarks>
        /// 优化（P1-2）：原 FMath.SinCos 为假融合（分别调 Sin 与 Cos，各自 Preprocess + Normalize）。
        /// 此处：Preprocess 1 次 + Normalize 1 次，cos 复用归一化结果（+Half_PI 后单次条件减法）。
        /// 数学等价性：
        /// <list type="bullet">
        /// <item>sin ≡ Sin(radian)：NormalizeRadian + SinFromNormalized</item>
        /// <item>cos ≡ Cos(radian) = Sin(radian + Half_PI) = SinFromNormalized(NormalizeRadian(radian + Half_PI))</item>
        /// <item>由模运算恒等式 (a+b) mod m ≡ ((a mod m)+b) mod m，且 normalized∈[0, Two_PI) ⇒
        /// normalized+Half_PI ∈ [Half_PI, Two_PI+Half_PI) ⊂ [0, 2·Two_PI)，单次减法即完成归一化，
        /// 与 NormalizeRadian(radian+Half_PI) 比特级等价。</item>
        /// </list>
        /// 精度不衰减、平台一致（纯整数运算）。
        /// </remarks>
        public static (Fixed32 sin, Fixed32 cos) SinCos(Fixed32 radian)
        {
            if (PreprocessSin(radian, out var r))
            {
                return (r, r);
            }

            var normalized = NormalizeRadian(radian);
            var sin = SinFromNormalized(normalized);

            var cosAngle = normalized + Half_PI;
            if (cosAngle >= Two_PI) cosAngle -= Two_PI;
            var cos = SinFromNormalized(cosAngle);

            return (sin, cos);
        }

        /// <summary>
        /// 将角度缩减到[0, π/2]区间
        /// <para>利用正弦函数的对称性，将角度缩减到[0, π/2]区间以减少计算误差</para>
        /// </summary>
        /// <param name="radian">角度（弧度），已归一化到[0, 2π)</param>
        /// <param name="sign">正弦值的符号</param>
        /// <returns>缩减到[0, π/2]区间的角度</returns>
        /// <remarks>
        /// 优化：比较链替代 (radian.raw &lt;&lt; 1) / PI.raw 的硬件除法（idiv）。
        /// 边界与原始 floor(2r/PI.raw) 完全一致：用 r2 = r&lt;&lt;1 与 PI.raw 的倍数比较。
        /// 注意：Two_PI.raw ≠ 2×PI.raw、Half_PI.raw ≠ PI.raw/2（常量截断误差），
        /// 故边界统一用 PI.raw 的倍数（PI.raw+PI.raw、3×PI.raw），保证比特级等价。
        /// </remarks>
        internal static Fixed32 ReduceRadian4Sin(Fixed32 radian, out bool sign)
        {
            var r2 = radian.rawvalue << 1;

            if (r2 < PI.rawvalue)
            {
                // 第一象限 [0, π/2)
                sign = false;
                return radian;
            }
            if (r2 < PI.rawvalue + PI.rawvalue)
            {
                // 第二象限 [π/2, π)
                sign = false;
                return PI - radian;
            }
            if (r2 < PI.rawvalue + PI.rawvalue + PI.rawvalue)
            {
                // 第三象限 [π, 3π/2)
                sign = true;
                return radian - PI;
            }

            // 第四象限 [3π/2, 2π)
            sign = true;
            return Two_PI - radian;
        }

        /// <summary>
        /// 使用泰勒展开计算正弦值
        /// <para>在[0, π/2]区间内使用泰勒展开计算正弦值</para>
        /// </summary>
        /// <param name="x">角度（弧度），范围在[0, π/2]之间</param>
        /// <returns>正弦值</returns>
        /// <remarks>
        /// 优化：UInt128 原始乘法替代 Mul（4 项分解 + 溢出检查 + 符号判定）。
        /// 输入 x ∈ [0, π/2]，所有中间值非负且远小于 long.MaxValue，无溢出风险。
        /// 精度等价：Mul 的 4 项分解对正值 ≡ (a×b)&gt;&gt;32 截断，与 UInt128 路径一致；
        /// 系数 C3..C11 均为正，乘积非负。平台一致（纯整数运算）。
        /// </remarks>
        private static Fixed32 TaylorEvaluate4Sin(Fixed32 x)
        {
            // 输入 x ∈ [0, π/2]，raw ∈ [0, 6746518852]，非负
            var x1Raw  = x.rawvalue;
            var x2Raw  = (long)((UInt128)(ulong)x1Raw * (ulong)x1Raw >> 32);
            var x3Raw  = (long)((UInt128)(ulong)x1Raw * (ulong)x2Raw >> 32);
            var x5Raw  = (long)((UInt128)(ulong)x3Raw * (ulong)x2Raw >> 32);
            var x7Raw  = (long)((UInt128)(ulong)x5Raw * (ulong)x2Raw >> 32);
            var x9Raw  = (long)((UInt128)(ulong)x7Raw * (ulong)x2Raw >> 32);
            var x11Raw = (long)((UInt128)(ulong)x9Raw * (ulong)x2Raw >> 32);

            var t3Raw  = (long)((UInt128)(ulong)x3Raw  * (ulong)C3.rawvalue  >> 32);
            var t5Raw  = (long)((UInt128)(ulong)x5Raw  * (ulong)C5.rawvalue  >> 32);
            var t7Raw  = (long)((UInt128)(ulong)x7Raw  * (ulong)C7.rawvalue  >> 32);
            var t9Raw  = (long)((UInt128)(ulong)x9Raw  * (ulong)C9.rawvalue  >> 32);
            var t11Raw = (long)((UInt128)(ulong)x11Raw * (ulong)C11.rawvalue >> 32);

            return FromRaw(x1Raw - t3Raw + t5Raw - t7Raw + t9Raw - t11Raw);
        }

        /// <summary>
        /// 快速计算正弦值
        /// <para>使用查表法快速计算正弦值，速度比Sin函数快，但精度较低</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正弦值，范围在[-1, 1]之间</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷或负无穷，返回NaN</item>
        /// </list>
        /// 注意：该方法的误差大于Sin函数
        /// </remarks>
        public static Fixed32 FastSin(Fixed32 radian)
        {
            if (PreprocessSin(radian, out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian);
            return FastSinFromNormalized(normalized);
        }

        /// <summary>
        /// 对已归一化到 [0, 2π) 的角度用查表法快速计算正弦值
        /// <para>跳过 Preprocess/Normalize，直接做象限缩减 + LUT 查表</para>
        /// </summary>
        /// <param name="normalized">已归一化到 [0, 2π) 的角度</param>
        /// <returns>正弦值</returns>
        /// <remarks>
        /// 优化（P1-1）：提取此辅助方法，使 <see cref="FastCos"/> 可共享查表路径，
        /// 避免重复的 Preprocess 调用。
        /// </remarks>
        private static Fixed32 FastSinFromNormalized(Fixed32 normalized)
        {
            var referenced = ReduceRadian4Sin(normalized, out var sign);

            var index = referenced.rawvalue >> 15;
            if (index >= SinLut.Length) index = SinLut.Length - 1;

            var nearest = SinLut[index];
            if (sign) nearest = -nearest;

            return FromRaw(nearest);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理正弦函数的特殊输入情况</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessSin(Fixed32 radian, out Fixed32 r)
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
        /// <para>使用余弦函数计算反正弦值</para>
        /// </summary>
        /// <param name="value">正弦值，范围在[-1, 1]之间</param>
        /// <returns>反正弦值，范围在[-π/2, π/2]之间</returns>
        public static Fixed32 Asin(Fixed32 value)
        {
            return Half_PI - Acos(value);
        }
    }
}
