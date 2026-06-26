namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 旋转
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 向量旋转radians（弧度）
        /// </summary>
        /// <param name="vector">向量</param>
        /// <param name="radians">旋转的弧度，以逆时针方向为正</param>
        /// <returns></returns>
        public static FVector2<T> Rotate(FVector2<T> vector, T radians)
        {
            var s = T.Sin(radians);
            var c = T.Cos(radians);
            var x = vector.X * c - vector.Y * s;
            var y = vector.X * s + vector.Y * c;

            return new FVector2<T>(x, y);
        }
    }
}
