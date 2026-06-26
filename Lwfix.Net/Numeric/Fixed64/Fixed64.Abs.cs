namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 数学
    /// <para>包含定点数的绝对值计算方法</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 获取当前定点数的绝对值
        /// <para>返回非负数，与原数同号</para>
        /// </summary>
        /// <returns>当前定点数的绝对值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果当前值是NaN，返回NaN</item>
        /// <item>如果当前值是正数，返回原值</item>
        /// <item>如果当前值是负无穷大，返回正无穷大</item>
        /// <item>如果当前值是最小值（MinValue），返回最大值（MaxValue）</item>
        /// </list>
        /// </remarks>
        public Fixed64 Abs()
        {
            if (IsNaN()) return NaN;
            if (IsPositive()) return this;
            if (IsNegativeInfinity()) return PositiveInfinity;
            if (IsMin()) return MaxValue;

            var mask = rawvalue >> 127;
            return FromRaw((rawvalue + mask) ^ mask);
        }

        /// <summary>
        /// 获取给定定点数的绝对值
        /// <para>返回非负数，与原数同号</para>
        /// </summary>
        /// <param name="n">要计算绝对值的定点数</param>
        /// <returns>给定定点数的绝对值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入值是NaN，返回NaN</item>
        /// <item>如果输入值是正数，返回原值</item>
        /// <item>如果输入值是负无穷大，返回正无穷大</item>
        /// <item>如果输入值是最小值（MinValue），返回最大值（MaxValue）</item>
        /// </list>
        /// </remarks>
        public static Fixed64 Abs(Fixed64 n)
        {
            return FMath.Abs(n);
        }
    }
}
