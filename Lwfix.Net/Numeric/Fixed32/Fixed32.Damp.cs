namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 平滑阻尼
    /// <para>包含定点数的平滑阻尼方法，用于实现值的平滑过渡</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 0.48 常量
        /// <para>用于SmoothDamp计算的系数</para>
        /// </summary>
        private readonly static Fixed32 P48  = FromRaw(2061584301);
        /// <summary>
        /// 0.235 常量
        /// <para>用于SmoothDamp计算的系数</para>
        /// </summary>
        private readonly static Fixed32 P235 = FromRaw(1009317315);

        /// <summary>
        /// 平滑阻尼
        /// <para>平滑地将一个值过渡到目标值</para>
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="currentVelocity">当前速度（引用传递，会被修改）</param>
        /// <param name="smoothTime">平滑时间，值越小过渡越快</param>
        /// <param name="maxSpeed">最大速度</param>
        /// <param name="deltaTime">时间步长</param>
        /// <returns>平滑过渡后的值</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用阻尼算法平滑过渡值</item>
        /// <item>通过调整速度来实现平滑效果</item>
        /// <item>限制最大速度以防止过渡过快</item>
        /// </list>
        /// </remarks>
        public static Fixed32 SmoothDamp(Fixed32 current, Fixed32 target, ref Fixed32 currentVelocity, Fixed32 smoothTime, Fixed32 maxSpeed, Fixed32 deltaTime)
        {
            smoothTime = Max(TPN4, smoothTime);
            var max = maxSpeed * smoothTime;

            var num1 = Two / smoothTime;
            var num2 = num1 * deltaTime;
            var num3 = Reciprocal(One + num2 + P48 * num2 * num2 + P235 * num2 * num2 * num2);
            var num4 = Clamp(current - target, -max, max);
            var num5 = target;
            var num6 = (currentVelocity + num1 * num4) * deltaTime;

            target = current - num4;
            currentVelocity = (currentVelocity - num1 * num6) * num3;

            var num7 = target + (num4 + num6) * num3;
            if ((num5 - current > Zero) == (num7 > num5))
            {
                num7 = num5;
                currentVelocity = (num7 - num5) / deltaTime;
            }

            return num7;
        }

        /// <summary>
        /// 平滑阻尼
        /// <para>平滑地将一个值过渡到目标值（使用默认的时间步长）</para>
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="currentVelocity">当前速度（引用传递，会被修改）</param>
        /// <param name="smoothTime">平滑时间，值越小过渡越快</param>
        /// <param name="maxSpeed">最大速度</param>
        /// <returns>平滑过渡后的值</returns>
        /// <remarks>
        /// 默认时间步长为0.01
        /// </remarks>
        public static Fixed32 SmoothDamp(Fixed32 current, Fixed32 target, ref Fixed32 currentVelocity, Fixed32 smoothTime, Fixed32 maxSpeed)
        {
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, TPN2);
        }

        /// <summary>
        /// 平滑阻尼
        /// <para>平滑地将一个值过渡到目标值（使用默认的最大速度和时间步长）</para>
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="currentVelocity">当前速度（引用传递，会被修改）</param>
        /// <param name="smoothTime">平滑时间，值越小过渡越快</param>
        /// <returns>平滑过渡后的值</returns>
        /// <remarks>
        /// 默认最大速度为最小值，默认时间步长为0.01
        /// </remarks>
        public static Fixed32 SmoothDamp(Fixed32 current, Fixed32 target, ref Fixed32 currentVelocity, Fixed32 smoothTime)
        {
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, MinValue, TPN2);
        }
    }
}
