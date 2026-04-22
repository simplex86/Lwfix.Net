namespace SimplexLab.Fixed
{
    /// <summary>
    /// 三维向量 - 距离
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 两点间的距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Distance(FVector3<T> a, FVector3<T> b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            var dz = a.Z - b.Z;
            return (dx * dx + dy * dy + dz * dz).Sqrt();
        }
    }
}
