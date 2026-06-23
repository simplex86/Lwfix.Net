namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 立方根
    /// <para>包含定点数的立方根计算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 计算立方根
        /// <para>计算定点数的立方根</para>
        /// </summary>
        /// <returns>立方根值</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用公式：∛x = e^(ln(x)/3)</item>
        /// <item>先计算自然对数，除以3，再计算指数</item>
        /// </list>
        /// </remarks>
        public Fixed32 Cbrt()
        {
            if (IsNaN()) return NaN;
            if (IsZero()) return Zero;

            // 负数的立方根：∛(-x) = -∛(x)
            var isNegative = IsNegative();
            var absValue = isNegative ? -this : this;
            var absResult = (absValue.Log() / 3).Exp();
            return isNegative ? -absResult : absResult;
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
        /// <item>使用公式：∛x = e^(ln(x)/3)</item>
        /// <item>先计算自然对数，除以3，再计算指数</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Cbrt(Fixed32 n)
        {
            return n.Cbrt();
        }
    }
}
