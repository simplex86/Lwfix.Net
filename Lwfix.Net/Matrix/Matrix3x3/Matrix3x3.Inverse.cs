using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 3x3矩阵 - 逆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 逆矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMatrix3x3<T> Inverse(FMatrix3x3<T> matrix)
        {
            var det = matrix.Determinant;

            var num11 = matrix.M22 * matrix.M33 - matrix.M23 * matrix.M32;
            var num12 = matrix.M13 * matrix.M32 - matrix.M12 * matrix.M33;
            var num13 = matrix.M12 * matrix.M23 - matrix.M22 * matrix.M13;

            var num21 = matrix.M23 * matrix.M31 - matrix.M33 * matrix.M21;
            var num22 = matrix.M11 * matrix.M33 - matrix.M31 * matrix.M13;
            var num23 = matrix.M13 * matrix.M21 - matrix.M23 * matrix.M11;

            var num31 = matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22;
            var num32 = matrix.M12 * matrix.M31 - matrix.M32 * matrix.M11;
            var num33 = matrix.M11 * matrix.M22 - matrix.M21 * matrix.M12;

            var result = new FMatrix3x3<T>();

            if (det == 0)
            {
                result.M11 = T.PositiveInfinity;
                result.M12 = T.PositiveInfinity;
                result.M13 = T.PositiveInfinity;
                result.M21 = T.PositiveInfinity;
                result.M22 = T.PositiveInfinity;
                result.M23 = T.PositiveInfinity;
                result.M31 = T.PositiveInfinity;
                result.M32 = T.PositiveInfinity;
                result.M33 = T.PositiveInfinity;
            }
            else
            {
                result.M11 = num11 / det;
                result.M12 = num12 / det;
                result.M13 = num13 / det;
                result.M21 = num21 / det;
                result.M22 = num22 / det;
                result.M23 = num23 / det;
                result.M31 = num31 / det;
                result.M32 = num32 / det;
                result.M33 = num33 / det;
            }

            return result;
        }
    }
}
