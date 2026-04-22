namespace SimplexLab.Fixed
{
    /// <summary>
    /// 3x3矩阵 - 转置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FMatrix3x3<T> Transpose(FMatrix3x3<T> matrix)
        {
            var result = new FMatrix3x3<T>()
            {
                M11 = matrix.M11,
                M12 = matrix.M21,
                M13 = matrix.M31,
                M21 = matrix.M12,
                M22 = matrix.M22,
                M23 = matrix.M32,
                M31 = matrix.M13,
                M32 = matrix.M23,
                M33 = matrix.M33,
            };
            return result;
        }
    }
}
