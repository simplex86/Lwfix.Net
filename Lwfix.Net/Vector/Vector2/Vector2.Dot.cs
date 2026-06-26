namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 点乘
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 点乘
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static T Dot(FVector2<T> lhs, FVector2<T> rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }
    }
}
