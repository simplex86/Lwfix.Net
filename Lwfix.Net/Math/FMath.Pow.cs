namespace SimplexLab.Lwfix
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
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Pow<T>(int m, int n) where T : struct, IFixed<T>
        {
            return Pow((T)m, (T)n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Pow<T>(T m, int n) where T : struct, IFixed<T>
        {
            return Pow(m, (T)n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Pow<T>(long m, long n) where T : struct, IFixed<T>
        {
            return Pow((T)m, (T)n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Pow<T>(T m, T n) where T : struct, IFixed<T>
        {
            return m.Pow(n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsPowerOfTwo<T>(T n) where T : struct, IFixed<T>
        {
            return n.IsPowerOfTwo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ClosestPowerOfTwo<T>(T value) where T : struct, IFixed<T>
        {
            return value.ClosestPowerOfTwo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T NextPowerOfTwo<T>(T value) where T : struct, IFixed<T>
        {
            return value.NextPowerOfTwo();
        }
    }
}
