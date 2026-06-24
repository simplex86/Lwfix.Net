namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 最大值
    /// <para>包含定点数的最大值计算方法</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 求两个数的最大值
        /// <para>计算两个定点数中的较大值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>两个数中的较大值</returns>
        public static Fixed64 Max(Fixed64 a, Fixed64 b)
        {
            return FMath.Max(a, b);
        }

        /// <summary>
        /// 求三个数的最大值
        /// <para>计算三个定点数中的最大值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <param name="c">第三个定点数</param>
        /// <returns>三个数中的最大值</returns>
        public static Fixed64 Max(Fixed64 a, Fixed64 b, Fixed64 c)
        {
            return FMath.Max(a, b, c);
        }

        /// <summary>
        /// 求四个数的最大值
        /// <para>计算四个定点数中的最大值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <param name="c">第三个定点数</param>
        /// <param name="d">第四个定点数</param>
        /// <returns>四个数中的最大值</returns>
        public static Fixed64 Max(Fixed64 a, Fixed64 b, Fixed64 c, Fixed64 d)
        {
            return FMath.Max(a, b, c, d);
        }

        /// <summary>
        /// 求多个数的最大值
        /// <para>计算多个定点数中的最大值</para>
        /// </summary>
        /// <param name="fixeds">定点数数组</param>
        /// <returns>多个数中的最大值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果数组为空，抛出异常</item>
        /// <item>如果数组包含NaN，返回NaN</item>
        /// </list>
        /// </remarks>
        public static Fixed64 Max(params Fixed64[] fixeds)
        {
            return FMath.Max(fixeds);
        }
    }
}
