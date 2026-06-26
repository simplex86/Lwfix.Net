namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 3x3矩阵 - 行列式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 行列式
        /// </summary>
        public T Determinant => Determinate(this);

        /// <summary>
        /// 行列式
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static T Determinate(FMatrix3x3<T> matrix)
        {
            return matrix.M11 * matrix.M22 * matrix.M33 -
                   matrix.M11 * matrix.M23 * matrix.M32 -
                   matrix.M12 * matrix.M21 * matrix.M33 +
                   matrix.M12 * matrix.M23 * matrix.M31 +
                   matrix.M13 * matrix.M21 * matrix.M32 -
                   matrix.M13 * matrix.M22 * matrix.M31;
        }
    }
}
