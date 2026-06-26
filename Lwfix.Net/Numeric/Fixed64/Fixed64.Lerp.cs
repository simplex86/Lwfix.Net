namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 插值
    /// <para>包含定点数的插值方法，如线性插值、平滑插值等</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 线性插值
        /// </summary>
        /// <param name="value1">起始值</param>
        /// <param name="value2">结束值</param>
        /// <param name="amount">插值因子，范围通常在[0, 1]之间</param>
        /// <returns>插值结果</returns>
        public static Fixed64 Lerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)
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
        public static Fixed64 ClampLerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)
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
        public static Fixed64 InverseLerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)
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
        public static Fixed64 SmoothStep(Fixed64 value1, Fixed64 value2, Fixed64 amount)
        {
            return FMath.SmoothStep(value1, value2, amount);
        }
    }
}
