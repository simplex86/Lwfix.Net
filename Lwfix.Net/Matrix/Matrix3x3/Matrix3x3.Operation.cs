using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 3x3矩阵
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMatrix3x3<T> operator +(FMatrix3x3<T> lhs, FMatrix3x3<T> rhs)
        {
            var m11 = lhs.M11 + rhs.M11;
            var m12 = lhs.M12 + rhs.M12;
            var m13 = lhs.M13 + rhs.M13;
            var m21 = lhs.M21 + rhs.M21;
            var m22 = lhs.M22 + rhs.M22;
            var m23 = lhs.M23 + rhs.M23;
            var m31 = lhs.M31 + rhs.M31;
            var m32 = lhs.M32 + rhs.M32;
            var m33 = lhs.M33 + rhs.M33;

            return new FMatrix3x3<T>(m11, m12, m13,
                                     m21, m22, m23,
                                     m31, m32, m33);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMatrix3x3<T> operator -(FMatrix3x3<T> lhs, FMatrix3x3<T> rhs)
        {
            var m11 = lhs.M11 - rhs.M11;
            var m12 = lhs.M12 - rhs.M12;
            var m13 = lhs.M13 - rhs.M13;
            var m21 = lhs.M21 - rhs.M21;
            var m22 = lhs.M22 - rhs.M22;
            var m23 = lhs.M23 - rhs.M23;
            var m31 = lhs.M31 - rhs.M31;
            var m32 = lhs.M32 - rhs.M32;
            var m33 = lhs.M33 - rhs.M33;

            return new FMatrix3x3<T>(m11, m12, m13,
                                     m21, m22, m23,
                                     m31, m32, m33);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMatrix3x3<T> operator *(FMatrix3x3<T> lhs, FMatrix3x3<T> rhs)
        {
            var m11 = lhs.M11 * rhs.M11 + lhs.M12 * rhs.M21 + lhs.M13 * rhs.M31;
            var m12 = lhs.M11 * rhs.M12 + lhs.M12 * rhs.M22 + lhs.M13 * rhs.M32;
            var m13 = lhs.M11 * rhs.M13 + lhs.M12 * rhs.M23 + lhs.M13 * rhs.M33;
            var m21 = lhs.M21 * rhs.M11 + lhs.M22 * rhs.M21 + lhs.M23 * rhs.M31;
            var m22 = lhs.M21 * rhs.M12 + lhs.M22 * rhs.M22 + lhs.M23 * rhs.M32;
            var m23 = lhs.M21 * rhs.M13 + lhs.M22 * rhs.M23 + lhs.M23 * rhs.M33;
            var m31 = lhs.M31 * rhs.M11 + lhs.M32 * rhs.M21 + lhs.M33 * rhs.M31;
            var m32 = lhs.M31 * rhs.M12 + lhs.M32 * rhs.M22 + lhs.M33 * rhs.M32;
            var m33 = lhs.M31 * rhs.M13 + lhs.M32 * rhs.M23 + lhs.M33 * rhs.M33;

            return new FMatrix3x3<T>(m11, m12, m13,
                                     m21, m22, m23,
                                     m31, m32, m33);
        }
    }
}
