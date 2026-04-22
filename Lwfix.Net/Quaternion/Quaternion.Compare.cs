namespace SimplexLab.Fixed
{
    /// <summary>
    /// 四元数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(FQuaternion<T> lhs, FQuaternion<T> rhs) => IsEqualUsingDot(Dot(lhs, rhs));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(FQuaternion<T> lhs, FQuaternion<T> rhs) => !(lhs == rhs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other) => other is FQuaternion<T> other1 && this.Equals(other1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(FQuaternion<T> other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);

        /// <summary>
        /// 两个四元数是否相等（点积等于1）
        /// </summary>
        /// <param name="dot"></param>
        /// <returns></returns>
        private static bool IsEqualUsingDot(T dot) => dot == T.One;
    }
}
