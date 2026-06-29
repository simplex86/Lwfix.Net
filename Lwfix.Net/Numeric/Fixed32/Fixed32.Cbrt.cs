using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 立方根
    /// <para>包含定点数的立方根计算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 计算立方根
        /// <para>使用牛顿迭代法计算定点数的立方根</para>
        /// <para>算法说明：
        /// <list type="bullet">
        /// <item>目标：result_raw = ∛(s_raw) * 2^(64/3) = (s_raw * 2^64)^(1/3)</item>
        /// <item>牛顿迭代：x_{n+1} = (2*x_n + S/x_n^2) / 3，二次收敛</item>
        /// <item>初始估计：硬件 Math.Cbrt（IEEE 754，约 52 位精度）</item>
        /// <item>S/x^2 用两次硬件 ulong 除法精确计算，无精度损失</item>
        /// <item>1 次牛顿迭代后初始误差被抵消，残余误差仅来自除法取整</item>
        /// </list></para>
        /// </summary>
        /// <returns>立方根值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// <item>如果输入是负无穷，返回负无穷</item>
        /// <item>如果输入是0，返回0</item>
        /// </list>
        /// 注意：
        /// <list type="bullet">
        /// <item>支持负数的立方根计算：∛(-x) = -∛(x)</item>
        /// <item>结果精度取决于定点数的位宽</item>
        /// </list>
        /// </remarks>
        public Fixed32 Cbrt()
        {
            if (IsNaN()) return NaN;
            if (IsZero()) return Zero;
            if (IsPositiveInfinity()) return PositiveInfinity;
            if (IsNegativeInfinity()) return NegativeInfinity;

            // 负数的立方根：∛(-x) = -∛(x)
            var isNegative = IsNegative();
            var s_raw = isNegative ? (ulong)(-rawvalue) : (ulong)rawvalue;

            // 初始估计：x0 ≈ (s_raw * 2^64)^(1/3)
            // 使用硬件 double cbrt（IEEE 754，约 52 位精度，跨平台一致）
            ulong x = (ulong)(Math.Cbrt((double)s_raw * (double)(1UL << 32) * (double)(1UL << 32)) + 0.5);

            // 牛顿迭代：x = (2*x + S/x^2) / 3
            // S/x^2 的 raw 值 = s_raw * 2^64 / x^2
            // 拆分为两次硬件 ulong 除法（避免 128 位除法）：
            //   Step 1: s_over_x = s_raw * 2^32 / x  （S/x 的 raw 值）
            //   Step 2: s_over_x2 = s_over_x * 2^32 / x  （S/x^2 的 raw 值）

            // Step 1: s_over_x = s_raw * 2^32 / x
            // 用硬件 ulong 除法精确计算：
            //   s_raw * 2^32 / x = (s_raw / x) * 2^32 + (s_raw % x) * 2^32 / x
            ulong q1 = s_raw / x;    // s_raw / x 的整数部分（≈ S^(2/3)）
            ulong r1 = s_raw % x;     // 余数（r1 < x）

            // 小数部分：r1 * 2^32 / x（结果 < 2^32，因为 r1 < x）
            // r1 * 2^32 可能溢出 ulong（当 r1 >= 2^32 时），需分情况处理
            ulong frac1;
            if (r1 < (1UL << 32))
            {
                frac1 = (r1 << 32) / x;
            }
            else
            {
                // r1 * 2^32 溢出，拆分为两次 16 位移位：
                // r1 * 2^32 = (r1 * 2^16) * 2^16
                ulong temp = r1 << 16;         // r1 < 2^48，temp < 2^64，不溢出
                ulong q1a = temp / x;           // q1a < 2^16
                ulong rem1a = temp % x;         // rem1a < x
                ulong q1b = (rem1a << 16) / x;  // rem1a < 2^48，rem1a << 16 < 2^64，不溢出
                frac1 = (q1a << 16) + q1b;
            }
            ulong s_over_x = (q1 << 32) + frac1;

            // Step 2: s_over_x2 = s_over_x * 2^32 / x
            // 同样的精确除法模式
            ulong q2 = s_over_x / x;   // s_over_x / x 的整数部分（≈ S^(1/3)）
            ulong r2 = s_over_x % x;    // 余数（r2 < x）

            ulong frac2;
            if (r2 < (1UL << 32))
            {
                frac2 = (r2 << 32) / x;
            }
            else
            {
                ulong temp = r2 << 16;
                ulong q2a = temp / x;
                ulong rem2a = temp % x;
                ulong q2b = (rem2a << 16) / x;
                frac2 = (q2a << 16) + q2b;
            }
            ulong s_over_x2 = (q2 << 32) + frac2;

            // 牛顿步：x = (2*x + S/x^2) / 3（四舍五入）
            // 初始估计误差 e0 被抵消（一阶项为零），残余误差仅来自除法取整
            x = (2 * x + s_over_x2 + 1) / 3;

            return FromRaw(isNegative ? -(long)x : (long)x);
        }

        /// <summary>
        /// 计算立方根
        /// <para>静态方法，计算定点数的立方根</para>
        /// </summary>
        /// <param name="n">要计算立方根的定点数</param>
        /// <returns>立方根值</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用牛顿迭代法：x_{n+1} = (2*x_n + S/x_n^2) / 3</item>
        /// <item>初始估计使用硬件 Math.Cbrt</item>
        /// <item>S/x^2 用硬件 ulong 除法精确计算</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Cbrt(Fixed32 n)
        {
            return n.Cbrt();
        }
    }
}
