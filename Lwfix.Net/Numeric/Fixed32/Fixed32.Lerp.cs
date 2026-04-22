namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 插值
    /// <para>包含定点数的插值方法，如线性插值、平滑插值等</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 线性插值
        /// <para>在两个值之间进行线性插值</para>
        /// </summary>
        /// <param name="value1">起始值</param>
        /// <param name="value2">结束值</param>
        /// <param name="amount">插值因子，范围通常在[0, 1]之间</param>
        /// <returns>插值结果</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用公式：value1 + (value2 - value1) * amount</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Lerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)
        {
            return FMath.Lerp(value1, value2, amount);
        }

        /// <summary>
        /// 钳制线性插值
        /// <para>在两个值之间进行线性插值，并将插值因子钳制在[0, 1]范围内</para>
        /// </summary>
        /// <param name="value1">起始值</param>
        /// <param name="value2">结束值</param>
        /// <param name="amount">插值因子</param>
        /// <returns>插值结果</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>先将amount钳制在[0, 1]范围内</item>
        /// <item>然后使用公式：value1 + (value2 - value1) * amount</item>
        /// </list>
        /// </remarks>
        public static Fixed32 ClampLerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)
        {
            return FMath.ClampLerp(value1, value2, amount);
        }

        /// <summary>
        /// 反向线性插值
        /// <para>计算一个值在两个值之间的插值因子</para>
        /// </summary>
        /// <param name="value1">起始值</param>
        /// <param name="value2">结束值</param>
        /// <param name="amount">要计算插值因子的值</param>
        /// <returns>插值因子，范围在[0, 1]之间</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用公式：(amount - value1) / (value2 - value1)</item>
        /// <item>如果value1等于value2，返回0</item>
        /// </list>
        /// </remarks>
        public static Fixed32 InverseLerp(Fixed32 value1, Fixed32 value2, Fixed32 amount)
        {
            return FMath.InverseLerp(value1, value2, amount);
        }

        /// <summary>
        /// 平滑插值
        /// <para>在两个值之间进行平滑插值，插值因子会被平滑处理</para>
        /// </summary>
        /// <param name="value1">起始值</param>
        /// <param name="value2">结束值</param>
        /// <param name="amount">插值因子，范围通常在[0, 1]之间</param>
        /// <returns>插值结果</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>首先将amount钳制在[0, 1]范围内</item>
        /// <item>然后使用三次平滑函数：3t² - 2t³</item>
        /// <item>最后进行线性插值</item>
        /// </list>
        /// </remarks>
        public static Fixed32 SmoothStep(Fixed32 value1, Fixed32 value2, Fixed32 amount)
        {
            return FMath.SmoothStep(value1, value2, amount);
        }
    }
}
