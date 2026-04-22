namespace SimplexLab.Fixed
{
    /// <summary>
    /// 数学库 - 立方根
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 立方根
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Cbrt<T>(T n) where T : struct, IFixed<T>
        {
            return n.Cbrt();
        }
    }
}
