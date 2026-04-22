namespace SimplexLab.Fixed
{
    /// <summary>
    /// 3x3矩阵
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
    {
        public T M11;
        public T M12;
        public T M13;
        public T M21;
        public T M22;
        public T M23;
        public T M31;
        public T M32;
        public T M33;

        /// <summary>
        /// 
        /// </summary>
        public static FMatrix3x3<T> Zero { get; } = new FMatrix3x3<T>(T.Zero, T.Zero, T.Zero,
                                                                    T.Zero, T.Zero, T.Zero,
                                                                    T.Zero, T.Zero, T.Zero);
        /// <summary>
        /// 
        /// </summary>
        public static FMatrix3x3<T> Identity { get; } = new FMatrix3x3<T>(T.One,  T.Zero, T.Zero,
                                                                        T.Zero, T.One,  T.Zero,
                                                                        T.Zero, T.Zero, T.One);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m13"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        /// <param name="m23"></param>
        /// <param name="m31"></param>
        /// <param name="m32"></param>
        /// <param name="m33"></param>
        public FMatrix3x3(T m11, T m12, T m13,
                          T m21, T m22, T m23,
                          T m31, T m32, T m33)
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        public FMatrix3x3(FMatrix3x3<T> matrix)
        {
            M11 = matrix.M11;
            M12 = matrix.M12;
            M13 = matrix.M13;
            M21 = matrix.M21;
            M22 = matrix.M22;
            M23 = matrix.M23;
            M31 = matrix.M31;
            M32 = matrix.M32;
            M33 = matrix.M33;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(@$"{M11} {M12} {M13}\n
                                    {M21} {M22} {M23}\n
                                    {M31} {M32} {M33}\n");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return M11.GetHashCode() ^
                   M12.GetHashCode() ^
                   M13.GetHashCode() ^
                   M21.GetHashCode() ^
                   M22.GetHashCode() ^
                   M23.GetHashCode() ^
                   M31.GetHashCode() ^
                   M32.GetHashCode() ^
                   M33.GetHashCode();
        }
    }
}
