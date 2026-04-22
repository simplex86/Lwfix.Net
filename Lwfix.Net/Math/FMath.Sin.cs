namespace SimplexLab.Fixed
{
    /// <summary>
    /// 函数库 - 正弦
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 正弦
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static T Sin<T>(T radian) where T : struct, IFixed<T>
        {
            return T.Sin(radian);
        }

        /// <summary>
        /// 快速正弦
        /// 注：误差大于Sin函数
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static T FastSin<T>(T radian) where T : struct, IFixed<T>
        {
            return T.FastSin(radian);
        }

        /// <summary>
        /// 反正弦
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Asin<T>(T value) where T : struct, IFixed<T>
        {
            return T.Asin(value);
        }
    }
}
