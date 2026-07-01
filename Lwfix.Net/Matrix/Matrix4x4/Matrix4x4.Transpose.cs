using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 4x4矩阵 - 转置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public FMatrix4x4<T> Transposed => Transpose(this);

        /// <summary>
        /// 转置矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMatrix4x4<T> Transpose(FMatrix4x4<T> matrix)
        {
            var transpose = new FMatrix4x4<T>()
            {
                M11 = matrix.M11,
                M12 = matrix.M21,
                M13 = matrix.M31,
                M14 = matrix.M41,
                M21 = matrix.M12,
                M22 = matrix.M22,
                M23 = matrix.M32,
                M24 = matrix.M42,
                M31 = matrix.M13,
                M32 = matrix.M23,
                M33 = matrix.M33,
                M34 = matrix.M43,
                M41 = matrix.M14,
                M42 = matrix.M24,
                M43 = matrix.M34,
                M44 = matrix.M44,
            };

            return transpose;
        }
    }
}
