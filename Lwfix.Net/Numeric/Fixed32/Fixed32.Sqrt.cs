namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 平方根
    /// <para>包含定点数的平方根计算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 半位宽
        /// <para>用于平方根计算的中间变量</para>
        /// </summary>
        private const byte HALF_TOTAL_BITS = TOTAL_BITS / 2;

        /// <summary>
        /// 计算平方根
        /// <para>使用二进制搜索算法计算定点数的平方根</para>
        /// </summary>
        /// <returns>平方根值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// <item>如果输入是0，返回NaN</item>
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
            if (IsZero()) return NaN;

            var val = (ulong)rawvalue;
            var bit = 1UL << (TOTAL_BITS - 2);
            while (bit > val) bit >>= 2;

            var res = 0UL;
            for (int i = 0; i < 2; i++)
            {
                while (bit != 0)
                {
                    if (val >= res + bit)
                    {
                        val -= res + bit;
                        res = (res >> 1) + bit;
                    }
                    else
                    {
                        res >>= 1;
                    }
                    bit >>= 2;
                }

                if (i == 0)
                {
                    if (val > (1UL << HALF_TOTAL_BITS) - 1)
                    {
                        val -= res;
                        val = (val << HALF_TOTAL_BITS) - 0x80000000UL;
                        res = (res << HALF_TOTAL_BITS) + 0x80000000UL;
                    }
                    else
                    {
                        val <<= HALF_TOTAL_BITS;
                        res <<= HALF_TOTAL_BITS;
                    }

                    bit = 1UL << (HALF_TOTAL_BITS - 2);
                }
            }

            if (val > res) res++;
            return FromRaw((long)res);
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
        /// <item>如果输入是0，返回NaN</item>
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
