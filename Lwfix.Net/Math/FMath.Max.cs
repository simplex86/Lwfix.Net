namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 函数库 - 最大值
    /// </summary>
    public partial class FMath
    {
        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Max<T>(T a, T b) where T : struct, IFixed<T>
        {
            if (a.IsNaN() || b.IsNaN()) return T.NaN;
            return a > b ? a : b;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static T Max<T>(T a, T b, T c) where T : struct, IFixed<T>
        {
            return Max(a, Max(b, c));
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static T Max<T>(T a, T b, T c, T d) where T : struct, IFixed<T>
        {
            return Max(a, Max(b, c, d));
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="fixeds"></param>
        /// <returns></returns>
        public static T Max<T>(params T[] fixeds) where T : struct, IFixed<T>
        {
            if (fixeds == null || fixeds.Length < 2)
            {
                return T.NaN;
            }

            var min = Max(fixeds[0], fixeds[1]);
            for (int i = 2; i < fixeds.Length; i++)
            {
                min = Max(min, fixeds[i]);
            }

            return min;
        }
    }
}
