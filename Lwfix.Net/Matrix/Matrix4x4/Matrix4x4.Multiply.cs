namespace SimplexLab.Fixed
{
    /// <summary>
    /// 4x4矩阵
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 点与矩阵相乘
        /// </summary>
        /// <param name="point"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FVector3<T> MultiplyPoint(FVector3<T> point, FMatrix4x4<T> matrix)
        {
            var x = (point.X * matrix.M11) + (point.Y * matrix.M21) + (point.Z * matrix.M31) + matrix.M41;
            var y = (point.X * matrix.M12) + (point.Y * matrix.M22) + (point.Z * matrix.M32) + matrix.M42;
            var z = (point.X * matrix.M13) + (point.Y * matrix.M23) + (point.Z * matrix.M33) + matrix.M43;

            return new FVector3<T>(x, y, z);
        }

        /// <summary>
        /// 向量与矩阵相乘
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FVector3<T> MultiplyVector(FVector3<T> vector, FMatrix4x4<T> matrix)
        {
            var x = (vector.X * matrix.M11) + (vector.Y * matrix.M21) + (vector.Z * matrix.M31);
            var y = (vector.X * matrix.M12) + (vector.Y * matrix.M22) + (vector.Z * matrix.M32);
            var z = (vector.X * matrix.M13) + (vector.Y * matrix.M23) + (vector.Z * matrix.M33);

            return new FVector3<T>(x, y, z);
        }
    }
}
