namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - Unity风格数学方法
    /// <para>包含类似于Unity引擎的数学方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 向目标值移动
        /// <para>将当前值向目标值移动指定的最大距离</para>
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="maxDelta">最大移动距离</param>
        /// <returns>移动后的值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果当前值与目标值的差的绝对值小于等于maxDelta，直接返回目标值</item>
        /// <item>否则，向目标值方向移动maxDelta距离</item>
        /// </list>
        /// </remarks>
        public static Fixed32 MoveTowards(Fixed32 current, Fixed32 target, Fixed32 maxDelta)
        {
            if (Abs(target - current) <= maxDelta) return target;
            return current + Sign(target - current) * maxDelta;
        }

        /// <summary>
        /// 向目标角度移动
        /// <para>将当前角度向目标角度移动指定的最大角度</para>
        /// </summary>
        /// <param name="current">当前角度（度）</param>
        /// <param name="target">目标角度（度）</param>
        /// <param name="maxDelta">最大移动角度</param>
        /// <returns>移动后的角度</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>计算两个角度之间的最小差</item>
        /// <item>使用MoveTowards方法进行移动</item>
        /// </list>
        /// </remarks>
        public static Fixed32 MoveTowardsAngle(Fixed32 current, Fixed32 target, Fixed32 maxDelta)
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
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>计算t除以length的商的整数部分</item>
        /// <item>用t减去商乘以length，得到余数</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Repeat(Fixed32 t, Fixed32 length)
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
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>计算目标角度与当前角度的差</item>
        /// <item>将差值限制在[0, 360)范围内</item>
        /// <item>如果差值大于180，返回差值减360</item>
        /// <item>否则直接返回差值</item>
        /// </list>
        /// </remarks>
        public static Fixed32 DeltaAngle(Fixed32 current, Fixed32 target)
        {
            var num = Repeat(target - current, N360);
            return (num > N180) ? num - N360  : num;
        }
    }
}
