using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 倒数
    /// <para>包含定点数的倒数计算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 计算倒数
        /// <para>计算定点数的倒数</para>
        /// </summary>
        /// <returns>倒数</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是0，返回正无穷</item>
        /// <item>如果输入是无穷大，返回0</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用牛顿迭代法计算倒数：x_{n+1} = x_n * (2 - a * x_n)</item>
        /// <item>初始估计使用 64 位硬件除法：(2^63 / |a|) &lt;&lt; 1 ≈ 2^64 / |a|，精度约 63 位</item>
        /// <item>迭代使用 UInt128 中间运算，避免 Q32.32 乘法截断导致的精度损失</item>
        /// <item>1 次迭代即可达到 Q32.32 满精度（误差 &lt; 1 ULP）</item>
        /// </list>
        /// </remarks>
        public Fixed32 Reciprocal()
        {
            if (PreprocessReciprocal(rawvalue, out var r))
            {
                return r;
            }

            var a = rawvalue;
            var negative = a < 0;
            var a_abs = (ulong)(negative ? -a : a);

            // 溢出检查：|a_raw| <= 2 时，1/|a| 超出 Q32.32 表示范围
            // 因为 MaxRawValue = 2^63 - 2，而 1/|a| 的 raw 值 = 2^64 / |a_raw|
            // 当 |a_raw| < 3 时，2^64 / |a_raw| > 2^63 - 2 = MaxRawValue
            if (a_abs < 3)
            {
                return negative ? NegativeInfinity : PositiveInfinity;
            }

            // 初始估计：x0 ≈ 2^64 / a_abs
            // 使用 (2^63 / a_abs) << 1 避免 2^64 溢出
            // 一次 64 位硬件除法，精度约 63 位（绝对误差 ≤ 2）
            ulong x = ((1UL << 63) / a_abs) << 1;

            // 牛顿迭代：x = x * (2 - a * x)
            // 迭代公式（raw 值）：x_new = x * (2^33 - (a*x >> 32)) >> 32
            // 使用 UInt128 避免中间乘积截断，保证牛顿迭代二次收敛
            {
                var prod = (UInt128)a_abs * (UInt128)x;       // a*x（128 位，约 2^64）
                var t = (ulong)(prod >> FRACTIONAL_BITS);      // a*x >> 32（约 2^32）
                var factor = (2UL << FRACTIONAL_BITS) - t;     // 2^33 - t（约 2^32）
                var prod2 = (UInt128)x * (UInt128)factor;      // x * factor（128 位）
                x = (ulong)(prod2 >> FRACTIONAL_BITS);          // x_new（64 位）
            }

            // 溢出检查：结果超出 Q32.32 最大值
            if (x > (ulong)S_MAX_RAW_VALUE)
            {
                return negative ? NegativeInfinity : PositiveInfinity;
            }

            var result = FromRaw((long)x);
            return negative ? -result : result;
        }

        /// <summary>
        /// 计算倒数
        /// <para>静态方法，计算定点数的倒数</para>
        /// </summary>
        /// <param name="n">要计算倒数的定点数</param>
        /// <returns>倒数</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是0，返回正无穷</item>
        /// <item>如果输入是无穷大，返回0</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Reciprocal(Fixed32 n)
        {
            return FMath.Reciprocal(n);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理倒数函数的特殊输入情况</para>
        /// </summary>
        /// <param name="n">原始存储值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessReciprocal(long n, out Fixed32 r)
        {
            if (n.IsNaN()) { r = NaN; return true; }
            if (n.IsZero()) { r = PositiveInfinity; return true; }
            if (n.IsInfinity()) { r = Zero; return true; }

            r = Zero;
            return false;
        }
    }
}
