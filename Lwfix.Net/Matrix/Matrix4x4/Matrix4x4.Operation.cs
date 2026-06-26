namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 4x4矩阵 - 运算符
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> operator +(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
        {
            var m11 = lhs.M11 + rhs.M11;
            var m12 = lhs.M12 + rhs.M12;
            var m13 = lhs.M13 + rhs.M13;
            var m14 = lhs.M14 + rhs.M14;
            var m21 = lhs.M21 + rhs.M21;
            var m22 = lhs.M22 + rhs.M22;
            var m23 = lhs.M23 + rhs.M23;
            var m24 = lhs.M24 + rhs.M24;
            var m31 = lhs.M31 + rhs.M31;
            var m32 = lhs.M32 + rhs.M32;
            var m33 = lhs.M33 + rhs.M33;
            var m34 = lhs.M34 + rhs.M34;
            var m41 = lhs.M41 + rhs.M41;
            var m42 = lhs.M42 + rhs.M42;
            var m43 = lhs.M43 + rhs.M43;
            var m44 = lhs.M44 + rhs.M44;

            return new FMatrix4x4<T>(m11, m12, m13, m14,
                                     m21, m22, m23, m24,
                                     m31, m32, m33, m34,
                                     m41, m42, m43, m44);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> operator -(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
        {
            var m11 = lhs.M11 - rhs.M11;
            var m12 = lhs.M12 - rhs.M12;
            var m13 = lhs.M13 - rhs.M13;
            var m14 = lhs.M14 - rhs.M14;
            var m21 = lhs.M21 - rhs.M21;
            var m22 = lhs.M22 - rhs.M22;
            var m23 = lhs.M23 - rhs.M23;
            var m24 = lhs.M24 - rhs.M24;
            var m31 = lhs.M31 - rhs.M31;
            var m32 = lhs.M32 - rhs.M32;
            var m33 = lhs.M33 - rhs.M33;
            var m34 = lhs.M34 - rhs.M34;
            var m41 = lhs.M41 - rhs.M41;
            var m42 = lhs.M42 - rhs.M42;
            var m43 = lhs.M43 - rhs.M43;
            var m44 = lhs.M44 - rhs.M44;

            return new FMatrix4x4<T>(m11, m12, m13, m14,
                                     m21, m22, m23, m24,
                                     m31, m32, m33, m34,
                                     m41, m42, m43, m44);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> operator *(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
        {
            var m11 = lhs.M11 * rhs.M11 + lhs.M12 * rhs.M21 + lhs.M13 * rhs.M31 + lhs.M14 * rhs.M41;
            var m12 = lhs.M11 * rhs.M12 + lhs.M12 * rhs.M22 + lhs.M13 * rhs.M32 + lhs.M14 * rhs.M42;
            var m13 = lhs.M11 * rhs.M13 + lhs.M12 * rhs.M23 + lhs.M13 * rhs.M33 + lhs.M14 * rhs.M43;
            var m14 = lhs.M11 * rhs.M14 + lhs.M12 * rhs.M24 + lhs.M13 * rhs.M34 + lhs.M14 * rhs.M44;
            var m21 = lhs.M21 * rhs.M11 + lhs.M22 * rhs.M21 + lhs.M23 * rhs.M31 + lhs.M24 * rhs.M41;
            var m22 = lhs.M21 * rhs.M12 + lhs.M22 * rhs.M22 + lhs.M23 * rhs.M32 + lhs.M24 * rhs.M42;
            var m23 = lhs.M21 * rhs.M13 + lhs.M22 * rhs.M23 + lhs.M23 * rhs.M33 + lhs.M24 * rhs.M43;
            var m24 = lhs.M21 * rhs.M14 + lhs.M22 * rhs.M24 + lhs.M23 * rhs.M34 + lhs.M24 * rhs.M44;
            var m31 = lhs.M31 * rhs.M11 + lhs.M32 * rhs.M21 + lhs.M33 * rhs.M31 + lhs.M34 * rhs.M41;
            var m32 = lhs.M31 * rhs.M12 + lhs.M32 * rhs.M22 + lhs.M33 * rhs.M32 + lhs.M34 * rhs.M42;
            var m33 = lhs.M31 * rhs.M13 + lhs.M32 * rhs.M23 + lhs.M33 * rhs.M33 + lhs.M34 * rhs.M43;
            var m34 = lhs.M31 * rhs.M14 + lhs.M32 * rhs.M24 + lhs.M33 * rhs.M34 + lhs.M34 * rhs.M44;
            var m41 = lhs.M41 * rhs.M11 + lhs.M42 * rhs.M21 + lhs.M43 * rhs.M31 + lhs.M44 * rhs.M41;
            var m42 = lhs.M41 * rhs.M12 + lhs.M42 * rhs.M22 + lhs.M43 * rhs.M32 + lhs.M44 * rhs.M42;
            var m43 = lhs.M41 * rhs.M13 + lhs.M42 * rhs.M23 + lhs.M43 * rhs.M33 + lhs.M44 * rhs.M43;
            var m44 = lhs.M41 * rhs.M14 + lhs.M42 * rhs.M24 + lhs.M43 * rhs.M34 + lhs.M44 * rhs.M44;

            return new FMatrix4x4<T>(m11, m12, m13, m14,
                                     m21, m22, m23, m24,
                                     m31, m32, m33, m34,
                                     m41, m42, m43, m44);
        }
    }
}
