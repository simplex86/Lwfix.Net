namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 3x3矩阵
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 点与矩阵相乘
        /// </summary>
        /// <param name="point"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FVector2<T> MultiplyPoint(FVector2<T> point, FMatrix3x3<T> matrix)
        {
            var x = (point.X * matrix.M11) + (point.Y * matrix.M21) + matrix.M31;
            var y = (point.X * matrix.M12) + (point.Y * matrix.M22) + matrix.M32;

            return new FVector2<T>(x, y);
        }

        /// <summary>
        /// 向量与矩阵相乘
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FVector2<T> MultiplyVector(FVector2<T> vector, FMatrix3x3<T> matrix)
        {
            var x = (vector.X * matrix.M11) + (vector.Y * matrix.M21);
            var y = (vector.X * matrix.M12) + (vector.Y * matrix.M22);

            return new FVector2<T>(x, y);
        }
    }
}
