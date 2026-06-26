namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 三维向量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// (0, 0, 0)
        /// </summary>
        public readonly static FVector3<T> Zero = new FVector3<T>();

        /// <summary>
        /// (1, 1, 1)
        /// </summary>
        public readonly static FVector3<T> One = new FVector3<T>(T.One, T.One, T.One);

        /// <summary>
        /// (0, 1, 0)
        /// </summary>
        public readonly static FVector3<T> Up = new FVector3<T>(T.Zero, T.One, T.Zero);

        /// <summary>
        /// (0, -1, 0)
        /// </summary>
        public readonly static FVector3<T> Down = new FVector3<T>(T.Zero, T.NegativeOne, T.Zero);

        /// <summary>
        /// (-1, 0, 0)
        /// </summary>
        public readonly static FVector3<T> Left = new FVector3<T>(T.NegativeOne, T.Zero, T.Zero);

        /// <summary>
        /// (1, 0, 0)
        /// </summary>
        public readonly static FVector3<T> Right = new FVector3<T>(T.One, T.Zero, T.Zero);

        /// <summary>
        /// (0, 0, 1)
        /// </summary>
        public readonly static FVector3<T> Forward = new FVector3<T>(T.Zero, T.Zero, T.One);

        /// <summary>
        /// (0, 0, -1)
        /// </summary>
        public readonly static FVector3<T> Back = new FVector3<T>(T.Zero, T.Zero, T.NegativeOne);
    }
}
