namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 数学
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 角度规范化到[0, 2π]
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static T NormalizeRadian<T>(T radian) where T : struct, IFixed<T>
        {
            return T.NormalizeRadian(radian);
        }
        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static T DegreeToRadian<T>(T degree) where T : struct, IFixed<T>
        {
            return T.DegreeToRadian(degree);
        }

        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static T RadianToDegree<T>(T radian) where T : struct, IFixed<T>
        {
            return T.RadianToDegree(radian);
        }
    }
}
