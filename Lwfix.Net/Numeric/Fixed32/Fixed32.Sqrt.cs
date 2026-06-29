using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 平方根
    /// <para>包含定点数的平方根计算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 计算平方根
        /// <para>使用牛顿迭代法计算定点数的平方根</para>
        /// <para>算法说明：
        /// <list type="bullet">
        /// <item>目标：result_raw = sqrt(s_raw) * 2^16（其中 s_raw 为输入原始值）</item>
        /// <item>牛顿迭代：x_{n+1} = (x_n + S/x_n) / 2，二次收敛</item>
        /// <item>初始估计：硬件 Math.Sqrt（IEEE 754，约 52 位精度）</item>
        /// <item>S/x 用硬件 ulong 除法精确计算，无精度损失</item>
        /// <item>1 次牛顿迭代后误差 &lt; 1 ULP（初始误差被牛顿步抵消）</item>
        /// </list></para>
        /// </summary>
        /// <returns>平方根值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// <item>如果输入是0，返回0</item>
        /// </list>
        /// 注意：
        /// <list type="bullet">
        /// <item>只支持非负数的平方根计算</item>
        /// <item>结果精度取决于定点数的位宽</item>
        /// </list>
        /// </remarks>
        public Fixed32 Sqrt()
        {
            if (IsNaN()) return NaN;
            if (IsPositiveInfinity()) return PositiveInfinity;
            if (IsZero()) return Zero;
            if (IsNegative()) return NaN;

            var s_raw = (ulong)rawvalue;

            // 初始估计：x0 ≈ sqrt(s_raw) * 2^16（即结果的 raw 值）
            // 使用硬件 double sqrt（SQRTPD 指令，IEEE 754，约 52 位精度，跨平台一致）
            ulong x = (ulong)(Math.Sqrt((double)s_raw) * 65536.0 + 0.5);

            // 牛顿迭代：x = (x + S/x) / 2
            // S/x 的 raw 值 = s_raw * 2^32 / x_raw
            // 用硬件 ulong 除法精确计算（避免 128 位除法和倒数精度问题）：
            //   s_raw * 2^32 / x = (s_raw / x) * 2^32 + (s_raw % x) * 2^32 / x

            ulong q = s_raw / x;   // s_raw / x 的整数部分（≈ x_raw / 2^32）
            ulong r = s_raw % x;    // 余数（r < x）

            // 小数部分：r * 2^32 / x（结果 < 2^32，因为 r < x）
            // r * 2^32 可能溢出 ulong（当 r >= 2^32 时），需分情况处理
            ulong frac;
            if (r < (1UL << 32))
            {
                // r * 2^32 在 ulong 范围内，直接除
                frac = (r << 32) / x;
            }
            else
            {
                // r * 2^32 溢出，拆分为两次 16 位移位：
                // r * 2^32 = (r * 2^16) * 2^16
                ulong temp = r << 16;        // r < 2^48，temp < 2^64，不溢出
                ulong q1 = temp / x;          // q1 < 2^16（因为 temp < x * 2^16）
                ulong rem1 = temp % x;        // rem1 < x
                ulong q2 = (rem1 << 16) / x;  // rem1 < 2^48，rem1 << 16 < 2^64，不溢出
                frac = (q1 << 16) + q2;
            }

            var s_over_x = (q << 32) + frac;

            // 牛顿步：x = (x + S/x) / 2
            // 初始估计误差 e0 被抵消：(x_true + e0 + x_true - e0) / 2 = x_true
            // 残余误差仅来自除法取整（< 1 raw 单位）
            x = (x + s_over_x) >> 1;

            return FromRaw((long)x);
        }

        /// <summary>
        /// 计算平方根
        /// <para>静态方法，计算定点数的平方根</para>
        /// </summary>
        /// <param name="n">要计算平方根的定点数</param>
        /// <returns>平方根值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// <item>如果输入是0，返回0</item>
        /// </list>
        /// 注意：
        /// <list type="bullet">
        /// <item>只支持非负数的平方根计算</item>
        /// <item>结果精度取决于定点数的位宽</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Sqrt(Fixed32 n)
        {
            return n.Sqrt();
        }
    }
}
