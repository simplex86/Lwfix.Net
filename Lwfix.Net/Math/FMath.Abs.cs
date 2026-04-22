namespace SimplexLab.Fixed
{
    /// <summary>
    /// 数学库 - 绝对值
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Abs(int n)
        {
            return (n >=0) ? n : -n;
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long Abs(long n)
        {
            return (n >= 0) ? n : -n;
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Abs<T>(T n) where T : struct, IFixed<T>
        {
            return n.Abs();
        }
    }
}
