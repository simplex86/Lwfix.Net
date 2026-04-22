namespace SimplexLab.Fixed
{
    /// <summary>
    /// 函数库 - 符号
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 符号
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Sign<T>(T n) where T : struct, IFixed<T>
        {
            return n.Sign();
        }

        /// <summary>
        /// 符号是否相同
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsSigns<T>(T a, T b) where T : struct, IFixed<T>
        {
            return T.IsSigns(a, b);
        }
    }
}
