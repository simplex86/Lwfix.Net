namespace SimplexLab.Fixed
{
    /// <summary>
    /// 三维向量 - 最小值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static FVector3<T> Min(FVector3<T> lhs, FVector3<T> rhs)
        {
            var x = T.Min(lhs.X, rhs.X);
            var y = T.Min(lhs.Y, rhs.Y);
            var z = T.Min(lhs.Z, rhs.Z);
            return new FVector3<T>(x, y, z);
        }
    }
}
