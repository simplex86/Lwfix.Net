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
        public static T Ceil<T>(T n) where T : struct, IFixed<T>
        {
            return n.Ceil();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int CeilToInt<T>(T n) where T : struct, IFixed<T>
        {
            return n.CeilToInt();
        }
    }
}
