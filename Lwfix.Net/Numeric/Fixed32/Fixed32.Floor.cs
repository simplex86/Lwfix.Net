namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 向下取整
    /// <para>包含定点数的向下取整操作方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 向下取整
        /// <para>计算小于或等于当前定点数的最大整数</para>
        /// </summary>
        /// <returns>向下取整后的值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是负无穷，返回负无穷</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用位掩码获取整数部分</item>
        /// </list>
        /// </remarks>
        public Fixed32 Floor()
        {
            if (IsNaN()) return NaN;
            if (IsNegativeInfinity()) return NegativeInfinity;
            if (IsPositiveInfinity()) return PositiveInfinity;

            var result = rawvalue & INTEGRAL_MASK;
            // MinValue的rawvalue & INTEGRAL_MASK会得到NaN的rawvalue，需要特殊处理
            if (result == NaN.rawvalue) return MinValue;

            return FromRaw(result);
        }

        /// <summary>
        /// 向下取整
        /// <para>静态方法，计算小于或等于定点数的最大整数</para>
        /// </summary>
        /// <param name="n">要向下取整的值</param>
        /// <returns>向下取整后的值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是负无穷，返回负无穷</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Floor(Fixed32 n)
        {
            return FMath.Floor(n);
        }

        /// <summary>
        /// 向下取整到整数
        /// <para>计算小于或等于当前定点数的最大整数，并转换为int类型</para>
        /// </summary>
        /// <returns>向下取整后的整数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回0</item>
        /// <item>如果输入是负无穷，返回int.MinValue</item>
        /// </list>
        /// </remarks>
        public int FloorToInt()
        {
            return Floor().ToInt();
        }

        /// <summary>
        /// 向下取整到整数
        /// <para>静态方法，计算小于或等于定点数的最大整数，并转换为int类型</para>
        /// </summary>
        /// <param name="n">要向下取整的值</param>
        /// <returns>向下取整后的整数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回0</item>
        /// <item>如果输入是负无穷，返回int.MinValue</item>
        /// </list>
        /// </remarks>
        public static int FloorToInt(Fixed32 n)
        {
            return FMath.FloorToInt(n);
        }
    }
}
