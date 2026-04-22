namespace SimplexLab.Fixed
{
    /// <summary>
    /// 四元数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        public T X;
        public T Y;
        public T Z;
        public T W;

        /// <summary>
        /// 
        /// </summary>
        public static FQuaternion<T> Identity { get; } = new FQuaternion<T>(T.Zero, T.Zero, T.Zero, T.One);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public FQuaternion(T x, T y, T z, T w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format($"({X}, {Y}, {Z}, {W})");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Z.GetHashCode() >> 2 ^ W.GetHashCode() >> 1;
        }
    }
}
