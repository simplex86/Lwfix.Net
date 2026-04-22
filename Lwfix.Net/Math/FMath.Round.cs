namespace SimplexLab.Fixed
{
    /// <summary>
    /// 数学库
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Round<T>(T n) where T : struct, IFixed<T>
        {
            return n.Round();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int RoundToInt<T>(T n) where T : struct, IFixed<T>
        {
            return n.RoundToInt();
        }
    }
}
