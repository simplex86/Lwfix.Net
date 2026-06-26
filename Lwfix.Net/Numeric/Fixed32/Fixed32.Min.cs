namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 最小值
    /// <para>包含定点数的最小值计算方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 求两个数的最小值
        /// <para>计算两个定点数中的较小值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>两个数中的较小值</returns>
        public static Fixed32 Min(Fixed32 a, Fixed32 b)
        {
            return FMath.Min(a, b);
        }

        /// <summary>
        /// 求三个数的最小值
        /// <para>计算三个定点数中的最小值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <param name="c">第三个定点数</param>
        /// <returns>三个数中的最小值</returns>
        public static Fixed32 Min(Fixed32 a, Fixed32 b, Fixed32 c)
        {
            return FMath.Min(a, b, c);
        }

        /// <summary>
        /// 求四个数的最小值
        /// <para>计算四个定点数中的最小值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <param name="c">第三个定点数</param>
        /// <param name="d">第四个定点数</param>
        /// <returns>四个数中的最小值</returns>
        public static Fixed32 Min(Fixed32 a, Fixed32 b, Fixed32 c, Fixed32 d)
        {
            return FMath.Min(a, b, c, d);
        }

        /// <summary>
        /// 求多个数的最小值
        /// <para>计算多个定点数中的最小值</para>
        /// </summary>
        /// <param name="fixeds">定点数数组</param>
        /// <returns>多个数中的最小值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果数组为空，抛出异常</item>
        /// <item>如果数组包含NaN，返回NaN</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Min(params Fixed32[] fixeds)
        {
            return FMath.Min(fixeds);
        }
    }
}
