namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 三维向量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public T X { get; set; } = T.Zero;
        /// <summary>
        /// 
        /// </summary>
        public T Y { get; set; } = T.Zero;
        /// <summary>
        /// 
        /// </summary>
        public T Z { get; set; } = T.Zero;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public FVector3(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public FVector3(FVector3<T> other)
        {
            this.X = other.X;
            this.Y = other.Y;
            this.Z = other.Z;
        }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[{X},{Y},{Z}]";

        /// <summary>
        /// 获取Hash码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Z.GetHashCode() >> 2;
        }
    }
}
