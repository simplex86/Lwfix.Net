namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - Unity风格数学方法
    /// <para>包含类似于Unity引擎的数学方法</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 向目标值移动
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="maxDelta">最大移动距离</param>
        /// <returns>移动后的值</returns>
        public static Fixed64 MoveTowards(Fixed64 current, Fixed64 target, Fixed64 maxDelta)
        {
            if (Abs(target - current) <= maxDelta) return target;
            return current + Sign(target - current) * maxDelta;
        }

        /// <summary>
        /// 向目标角度移动
        /// </summary>
        /// <param name="current">当前角度（度）</param>
        /// <param name="target">目标角度（度）</param>
        /// <param name="maxDelta">最大移动角度</param>
        /// <returns>移动后的角度</returns>
        public static Fixed64 MoveTowardsAngle(Fixed64 current, Fixed64 target, Fixed64 maxDelta)
        {
            target = current + DeltaAngle(current, target);
            return MoveTowards(current, target, maxDelta);
        }

        /// <summary>
        /// 重复值
        /// <para>将值限制在指定范围内，超出范围则循环</para>
        /// </summary>
        /// <param name="t">要重复的值</param>
        /// <param name="length">范围长度</param>
        /// <returns>重复后的值，范围在[0, length)之间</returns>
        public static Fixed64 Repeat(Fixed64 t, Fixed64 length)
        {
            return t - Floor(t / length) * length;
        }

        /// <summary>
        /// 两个角度之间的最小差（度）
        /// <para>计算两个角度之间的最小差值，范围在[-180, 180]之间</para>
        /// </summary>
        /// <param name="current">当前角度（度）</param>
        /// <param name="target">目标角度（度）</param>
        /// <returns>最小角度差，范围在[-180, 180]之间</returns>
        public static Fixed64 DeltaAngle(Fixed64 current, Fixed64 target)
        {
            var num = Repeat(target - current, N360);
            return (num > N180) ? num - N360 : num;
        }
    }
}
