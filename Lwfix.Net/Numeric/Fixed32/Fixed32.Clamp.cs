namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 钳制
    /// <para>包含定点数的钳制操作方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 钳制值到指定范围
        /// <para>将值钳制在指定的最小值和最大值之间</para>
        /// </summary>
        /// <param name="value">要钳制的值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>钳制后的值，范围在[min, max]之间</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果value小于min，返回min</item>
        /// <item>如果value大于max，返回max</item>
        /// <item>否则返回value</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Clamp(Fixed32 value, Fixed32 min, Fixed32 max)
        {
            return FMath.Clamp(value, min, max);
        }

        /// <summary>
        /// 钳制值到[0, 1]范围
        /// <para>将值钳制在[0, 1]范围内</para>
        /// </summary>
        /// <param name="value">要钳制的值</param>
        /// <returns>钳制后的值，范围在[0, 1]之间</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果value小于0，返回0</item>
        /// <item>如果value大于1，返回1</item>
        /// <item>否则返回value</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Clamp01(Fixed32 value)
        {
            return FMath.Clamp01(value);
        }
    }
}
