using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 平滑阻尼
    /// <para>包含定点数的平滑阻尼方法，用于实现值的平滑过渡</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>0.48 常量，用于SmoothDamp计算</summary>
        private readonly static Fixed64 P48  = FromRaw((Int128)(0.48 * FRACTIONAL_MULTIPLIER + 0.5));
        /// <summary>0.235 常量，用于SmoothDamp计算</summary>
        private readonly static Fixed64 P235 = FromRaw((Int128)(0.235 * FRACTIONAL_MULTIPLIER + 0.5));

        /// <summary>
        /// 平滑阻尼
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="currentVelocity">当前速度（引用传递，会被修改）</param>
        /// <param name="smoothTime">平滑时间，值越小过渡越快</param>
        /// <param name="maxSpeed">最大速度</param>
        /// <param name="deltaTime">时间步长</param>
        /// <returns>平滑过渡后的值</returns>
        public static Fixed64 SmoothDamp(Fixed64 current, Fixed64 target, ref Fixed64 currentVelocity, Fixed64 smoothTime, Fixed64 maxSpeed, Fixed64 deltaTime)
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
        /// 平滑阻尼（使用默认时间步长0.01）
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="currentVelocity">当前速度（引用传递，会被修改）</param>
        /// <param name="smoothTime">平滑时间，值越小过渡越快</param>
        /// <param name="maxSpeed">最大速度</param>
        /// <returns>平滑过渡后的值</returns>
        public static Fixed64 SmoothDamp(Fixed64 current, Fixed64 target, ref Fixed64 currentVelocity, Fixed64 smoothTime, Fixed64 maxSpeed)
        {
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, TPN2);
        }

        /// <summary>
        /// 平滑阻尼（使用默认最大速度和时间步长）
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="currentVelocity">当前速度（引用传递，会被修改）</param>
        /// <param name="smoothTime">平滑时间，值越小过渡越快</param>
        /// <returns>平滑过渡后的值</returns>
        public static Fixed64 SmoothDamp(Fixed64 current, Fixed64 target, ref Fixed64 currentVelocity, Fixed64 smoothTime)
        {
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, MinValue, TPN2);
        }
    }
}
