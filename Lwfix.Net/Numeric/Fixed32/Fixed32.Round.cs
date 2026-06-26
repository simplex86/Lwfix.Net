namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 四舍五入
    /// <para>包含定点数的四舍五入操作方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 四舍五入
        /// <para>将当前定点数四舍五入到最近的整数</para>
        /// </summary>
        /// <returns>四舍五入后的值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// <item>如果输入是负无穷，返回负无穷</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>获取小数部分</item>
        /// <item>如果小数部分小于0.5，向下取整</item>
        /// <item>如果小数部分大于0.5，向上取整</item>
        /// <item>如果小数部分等于0.5，向偶数取整</item>
        /// </list>
        /// </remarks>
        public Fixed32 Round()
        {
            if (IsNaN()) return NaN;
            if (IsPositiveInfinity()) return PositiveInfinity;
            if (IsNegativeInfinity()) return NegativeInfinity;

            var frac = rawvalue & FRACTIONAL_MASK;

            if (frac < 0x80000000) return Floor();
            if (frac > 0x80000000) return Ceil();

            return (rawvalue & One.rawvalue) == 0 ? Floor()
                                                  : Ceil();
        }

        /// <summary>
        /// 四舍五入
        /// <para>静态方法，将定点数四舍五入到最近的整数</para>
        /// </summary>
        /// <param name="n">要四舍五入的值</param>
        /// <returns>四舍五入后的值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// <item>如果输入是负无穷，返回负无穷</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Round(Fixed32 n)
        {
            return FMath.Round(n);
        }

        /// <summary>
        /// 四舍五入到整数
        /// <para>将当前定点数四舍五入到最近的整数，并转换为int类型</para>
        /// </summary>
        /// <returns>四舍五入后的整数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回0</item>
        /// <item>如果输入是正无穷，返回int.MaxValue</item>
        /// <item>如果输入是负无穷，返回int.MinValue</item>
        /// </list>
        /// </remarks>
        public int RoundToInt()
        {
            return Round().ToInt();
        }

        /// <summary>
        /// 四舍五入到整数
        /// <para>静态方法，将定点数四舍五入到最近的整数，并转换为int类型</para>
        /// </summary>
        /// <param name="n">要四舍五入的值</param>
        /// <returns>四舍五入后的整数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回0</item>
        /// <item>如果输入是正无穷，返回int.MaxValue</item>
        /// <item>如果输入是负无穷，返回int.MinValue</item>
        /// </list>
        /// </remarks>
        public static int RoundToInt(Fixed32 n)
        {
            return FMath.RoundToInt(n);
        }
    }
}
