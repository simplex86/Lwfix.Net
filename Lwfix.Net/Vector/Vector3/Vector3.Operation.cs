namespace SimplexLab.Fixed
{
    /// <summary>
    /// 三维向量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 相加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FVector3<T> operator +(FVector3<T> a, FVector3<T> b)
        {
            return new FVector3<T>(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// 相减
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FVector3<T> operator -(FVector3<T> a, FVector3<T> b)
        {
            return new FVector3<T>(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FVector3<T> operator *(FVector3<T> a, FVector3<T> b)
        {
            return new FVector3<T>(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FVector3<T> operator /(FVector3<T> a, FVector3<T> b)
        {
            return new FVector3<T>(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        /// <summary>
        /// 取反
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static FVector3<T> operator -(FVector3<T> a)
        {
            return new FVector3<T>(-a.X, -a.Y, -a.Z);
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static FVector3<T> operator *(FVector3<T> a, T d)
        {
            return new FVector3<T>(a.X * d, a.Y * d, a.Z * d);
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="d"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static FVector3<T> operator *(T d, FVector3<T> a)
        {
            return a * d;
        }

        /// <summary>
        /// 数除
        /// </summary>
        /// <param name="a"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static FVector3<T> operator /(FVector3<T> a, T d)
        {
            return new FVector3<T>(a.X / d, a.Y / d, a.Z / d);
        }
    }
}
