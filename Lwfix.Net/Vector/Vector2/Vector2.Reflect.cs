namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 反射
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 反射
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static FVector2<T> Reflect(FVector2<T> direction, FVector2<T> normal)
        {
            var t = -2 * Dot(normal, direction);
            var x = t * normal.X + direction.X;
            var y = t * normal.Y + direction.Y;

            return new FVector2<T>(x, y);
        }
    }
}
