namespace SimplexLab.Fixed
{
    /// <summary>
    /// 函数库 - 余弦
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static T Cos<T>(T radian) where T : struct, IFixed<T>
        {
            return T.Cos(radian);
        }

        /// <summary>
        /// 快速计算余弦
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static T FastCos<T>(T radian) where T : struct, IFixed<T>
        {
            return T.FastCos(radian);
        }

        /// <summary>
        /// 反余弦
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Acos<T>(T value) where T : struct, IFixed<T>
        {
            return T.Acos(value);
        }
    }
}
