namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 数学库 - 平方根
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 平方根
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Sqrt<T>(int n) where T : struct, IFixed<T>
        {
            var f = (T)n;
            return f.Sqrt();
        }

        /// <summary>
        /// 平方根
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Sqrt<T>(long n) where T : struct, IFixed<T>
        {
            var f = (T)n;
            return f.Sqrt();
        }

        /// <summary>
        /// 平方根
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Sqrt<T>(T n) where T : struct, IFixed<T>
        {
            return n.Sqrt();
        }
    }
}
