namespace SimplexLab.Fixed
{
    /// <summary>
    /// 三维向量 - 比较
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(FVector3<T> lhs, FVector3<T> rhs)
        {
            return lhs.X == rhs.X &&
                   lhs.Y == rhs.Y && 
                   lhs.Z == rhs.Z;
        }

        /// <summary>
        /// 是否不等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(FVector3<T> lhs, FVector3<T> rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            return other is FVector3<T> o && Equals(o);
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(FVector3<T> other)
        {
            return this == other;
        }

    }
}
