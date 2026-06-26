namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 叉乘
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 叉乘
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static T Cross(FVector2<T> lhs, FVector2<T> rhs)
        {
            return lhs.X * rhs.Y - lhs.Y * rhs.X;
        }
    }
}
