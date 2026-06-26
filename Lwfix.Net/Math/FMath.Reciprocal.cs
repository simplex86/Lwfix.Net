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
        /// <param name="n"></param>
        /// <returns></returns>
        public static T Reciprocal<T>(T n) where T : struct, IFixed<T>
        {
            return n.Reciprocal();
        }
    }
}
