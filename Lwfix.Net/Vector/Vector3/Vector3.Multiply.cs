namespace SimplexLab.Fixed
{
    /// <summary>
    /// 三维向量 - 三重积
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 三重积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static FVector3<T> Multiply3(FVector3<T> a, FVector3<T> b, FVector3<T> c)
        {
            var t = Cross(a, b);
            return Cross(c, t);
        }
    }
}
