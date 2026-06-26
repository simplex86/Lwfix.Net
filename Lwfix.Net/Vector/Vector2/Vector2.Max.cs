namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 最大值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static FVector2<T> Max(FVector2<T> lhs, FVector2<T> rhs)
        {
            var x = T.Max(lhs.X, rhs.X);
            var y = T.Max(lhs.Y, rhs.Y);
            return new FVector2<T>(x, y);
        }
    }
}
