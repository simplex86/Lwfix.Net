namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// (0, 0)
        /// </summary>
        public readonly static FVector2<T> Zero = new FVector2<T>();

        /// <summary>
        /// (1, 1)
        /// </summary>
        public readonly static FVector2<T> One = new FVector2<T>(T.One, T.One);

        /// <summary>
        /// (0, 1)
        /// </summary>
        public readonly static FVector2<T> Up = new FVector2<T>(T.Zero, T.One);

        /// <summary>
        /// (0, -1)
        /// </summary>
        public readonly static FVector2<T> Down = new FVector2<T>(T.Zero, T.NegativeOne);

        /// <summary>
        /// (-1, 0)
        /// </summary>
        public readonly static FVector2<T> Left = new FVector2<T>(T.NegativeOne, T.Zero);

        /// <summary>
        /// (1, 0)
        /// </summary>
        public readonly static FVector2<T> Right = new FVector2<T>(T.One, T.Zero);
    }
}
