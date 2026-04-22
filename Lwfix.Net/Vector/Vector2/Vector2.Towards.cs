namespace SimplexLab.Fixed
{
    /// <summary>
    /// 二维向量 - 移动
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 移动到
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static FVector2<T> MoveTowards(FVector2<T> current, FVector2<T> target, T maxDistanceDelta)
        {
            var x = target.X - current.X;
            var y = target.Y - current.Y;

            var d = (x * x + y * y).Sqrt();
            if (d.IsZero() || maxDistanceDelta.IsPositive() && d <= maxDistanceDelta)
            {
                return target;
            }

            x = current.X + x / d * maxDistanceDelta;
            y = current.Y + y / d * maxDistanceDelta;

            return new FVector2<T>(x, y);
        }
    }
}
