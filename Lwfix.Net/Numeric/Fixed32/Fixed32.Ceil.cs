namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 向上取整
    /// <para>包含定点数的向上取整操作方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 向上取整
        /// <para>计算大于或等于当前定点数的最小整数</para>
        /// </summary>
        /// <returns>向上取整后的值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>如果有小数部分，加1后向下取整</item>
        /// <item>否则直接返回原值</item>
        /// </list>
        /// </remarks>
        public Fixed32 Ceil()
        {
            if (IsNaN()) return NaN;
            if (IsPositiveInfinity()) return PositiveInfinity;

            return IsFractional() ? (this + One).Floor() : this;
        }

        /// <summary>
        /// 向上取整
        /// <para>静态方法，计算大于或等于定点数的最小整数</para>
        /// </summary>
        /// <param name="n">要向上取整的值</param>
        /// <returns>向上取整后的值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Ceil(Fixed32 n)
        {
            return FMath.Ceil(n);
        }

        /// <summary>
        /// 向上取整到整数
        /// <para>计算大于或等于当前定点数的最小整数，并转换为int类型</para>
        /// </summary>
        /// <returns>向上取整后的整数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回0</item>
        /// <item>如果输入是正无穷，返回int.MaxValue</item>
        /// </list>
        /// </remarks>
        public int CeilToInt()
        {
            return Ceil().ToInt();
        }

        /// <summary>
        /// 向上取整到整数
        /// <para>静态方法，计算大于或等于定点数的最小整数，并转换为int类型</para>
        /// </summary>
        /// <param name="n">要向上取整的值</param>
        /// <returns>向上取整后的整数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回0</item>
        /// <item>如果输入是正无穷，返回int.MaxValue</item>
        /// </list>
        /// </remarks>
        public static int CeilToInt(Fixed32 n)
        {
            return FMath.CeilToInt(n);
        }
    }
}
