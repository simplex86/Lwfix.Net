namespace SimplexLab.Fixed
{
    /// <summary>
    /// 三维向量 - 叉乘
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 叉乘
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static FVector3<T> Cross(FVector3<T> lhs, FVector3<T> rhs)
        {
            var x = lhs.Y * rhs.Z - lhs.Z * rhs.Y;
            var y = lhs.Z * rhs.X - lhs.X * rhs.Z;
            var z = lhs.X * rhs.Y - lhs.Y * rhs.X;
            return new FVector3<T>(x, y, z);
        }
    }
}
