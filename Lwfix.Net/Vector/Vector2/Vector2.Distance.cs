namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 距离
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 两点间的距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Distance(FVector2<T> a, FVector2<T> b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return (dx * dx + dy * dy).Sqrt();
        }
    }
}
