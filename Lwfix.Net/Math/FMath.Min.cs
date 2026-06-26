namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 函数库 - 最小值
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 求两个数的最小值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Min<T>(T a, T b) where T : struct, IFixed<T>
        {
            if (a.IsNaN() || b.IsNaN()) return T.NaN;
            return a < b ? a : b;
        }

        /// <summary>
        /// 求三个数的最小值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static T Min<T>(T a, T b, T c) where T : struct, IFixed<T>
        {
            return Min(a, Min(b, c));
        }

        /// <summary>
        /// 求四个数的最小值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static T Min<T>(T a, T b, T c, T d) where T : struct, IFixed<T>
        {
            return Min(a, Min(b, c, d));
        }

        /// <summary>
        /// 求多个数的最小值
        /// </summary>
        /// <param name="fixeds"></param>
        /// <returns></returns>
        public static T Min<T>(params T[] fixeds) where T : struct, IFixed<T>
        {
            if (fixeds == null || fixeds.Length < 2)
            {
                return T.NaN;
            }

            var min = Min(fixeds[0], fixeds[1]);
            for (int i=2; i<fixeds.Length; i++)
            {
                min = Min(min, fixeds[i]);
            }

            return min;
        }
    }
}
