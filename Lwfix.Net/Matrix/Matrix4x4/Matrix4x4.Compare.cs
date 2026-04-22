namespace SimplexLab.Fixed
{
    /// <summary>
    /// 4x4矩阵 - 比较
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        // 
        public static bool operator ==(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
        {
            return lhs.M11 == rhs.M11 &&
                   lhs.M12 == rhs.M12 &&
                   lhs.M13 == rhs.M13 &&
                   lhs.M14 == rhs.M14 &&
                   lhs.M21 == rhs.M21 &&
                   lhs.M22 == rhs.M22 &&
                   lhs.M23 == rhs.M23 &&
                   lhs.M24 == rhs.M24 &&
                   lhs.M31 == rhs.M31 &&
                   lhs.M32 == rhs.M32 &&
                   lhs.M33 == rhs.M33 &&
                   lhs.M34 == rhs.M34 &&
                   lhs.M41 == rhs.M41 &&
                   lhs.M42 == rhs.M42 &&
                   lhs.M43 == rhs.M43 &&
                   lhs.M44 == rhs.M44;
        }

        /// <summary>
        /// 是否不等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(FMatrix4x4<T> lhs, FMatrix4x4<T> rhs)
        {
            return !(lhs == rhs);
        }
    }
}
