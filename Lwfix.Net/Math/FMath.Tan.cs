namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 正切
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 正切
        /// 注：将radian规范化到[-π/2, π/2]范围内，其值越接近(±π/2)误差越大。
        /// 经测试，与(±π/2)差值小于0.0017时，误差将大于0.1
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static T Tan<T>(T radian) where T : struct, IFixed<T>
        {
            return T.Tan(radian);
        }

        /// <summary>
        /// 快速计算正切
        /// 注：将radian规范化到[-π/2, π/2]范围内，其值越接近(±π/2)误差越大。
        /// 误差大于Tan函数，但计算速度比Tan函数更快
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static T FastTan<T>(T radian) where T : struct, IFixed<T>
        {
            return T.FastTan(radian);
        }

        /// <summary>
        /// 反余切
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Atan<T>(T value) where T : struct, IFixed<T>
        {
            return T.Atan(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static T Atan2<T>(T y, T x) where T : struct, IFixed<T>
        {
            return T.Atan2(x, y);
        }
    }
}
