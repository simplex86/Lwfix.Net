namespace SimplexLab.Fixed
{
    /// <summary>
    /// 数学库
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 以e为底的对数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Log<T>(int n) where T : struct, IFixed<T>
        {
            var f = (T)n;
            return f.Log();
        }

        /// <summary>
        /// 以e为底的对数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Log<T>(long n) where T : struct, IFixed<T>
        {
            var f = (T)n;
            return f.Log();
        }

        /// <summary>
        /// 以e为底的对数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Log<T>(T n) where T : struct, IFixed<T>
        {
            return n.Log();
        }

        /// <summary>
        /// 以2为底的对数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Log2<T>(int n) where T : struct, IFixed<T>
        {
            var f = (T)n;
            return f.Log2();
        }

        /// <summary>
        /// 以2为底的对数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Log2<T>(long n) where T : struct, IFixed<T>
        {
            var f = (T)n;
            return f.Log2();
        }

        /// <summary>
        /// 以2为底的对数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Log2<T>(T n) where T : struct, IFixed<T>
        {
            return n.Log2();
        }

        /// <summary>
        /// 以10为底的对数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Log10<T>(int n) where T : struct, IFixed<T>
        {
            var f = (T)n;
            return f.Log10();
        }

        /// <summary>
        /// 以10为底的对数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Log10<T>(long n) where T : struct, IFixed<T>
        {
            var f = (T)n;
            return f.Log10();
        }

        /// <summary>
        /// 以10为底的对数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Log10<T>(T n) where T : struct, IFixed<T>
        {
            return n.Log10();
        }
    }
}
