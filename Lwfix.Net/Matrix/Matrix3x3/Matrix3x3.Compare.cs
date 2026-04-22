namespace SimplexLab.Fixed
{
    /// <summary>
    /// 3x3矩阵 - 比较
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(FMatrix3x3<T> a, FMatrix3x3<T> b)
        {
            return a.M11 == b.M11 &&
                   a.M12 == b.M12 &&
                   a.M13 == b.M13 &&
                   a.M21 == b.M21 &&
                   a.M22 == b.M22 &&
                   a.M23 == b.M23 &&
                   a.M31 == b.M31 &&
                   a.M32 == b.M32 &&
                   a.M33 == b.M33;
        }

        /// <summary>
        /// 是否不等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(FMatrix3x3<T> a, FMatrix3x3<T> b)
        {
            return !(a == b);
        }
    }
}
