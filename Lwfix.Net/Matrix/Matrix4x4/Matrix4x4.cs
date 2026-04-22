using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 4x4矩阵
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        public T M11;
        public T M12;
        public T M13;
        public T M14;
        public T M21;
        public T M22;
        public T M23;
        public T M24;
        public T M31;
        public T M32;
        public T M33;
        public T M34;
        public T M41;
        public T M42;
        public T M43;
        public T M44;

        /// <summary>
        /// 
        /// </summary>
        public static FMatrix4x4<T> Zero { get; } = new FMatrix4x4<T>(T.Zero, T.Zero, T.Zero, T.Zero,
                                                                      T.Zero, T.Zero, T.Zero, T.Zero,
                                                                      T.Zero, T.Zero, T.Zero, T.Zero,
                                                                      T.Zero, T.Zero, T.Zero, T.Zero);
        /// <summary>
        /// 
        /// </summary>
        public static FMatrix4x4<T> Identity { get; } = new FMatrix4x4<T>(T.One,  T.Zero, T.Zero, T.Zero,
                                                                          T.Zero, T.One,  T.Zero, T.Zero,
                                                                          T.Zero, T.Zero, T.One,  T.Zero,
                                                                          T.Zero, T.Zero, T.Zero, T.One);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m13"></param>
        /// <param name="m14"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        /// <param name="m23"></param>
        /// <param name="m24"></param>
        /// <param name="m31"></param>
        /// <param name="m32"></param>
        /// <param name="m33"></param>
        /// <param name="m34"></param>
        /// <param name="m41"></param>
        /// <param name="m42"></param>
        /// <param name="m43"></param>
        /// <param name="m44"></param>
        public FMatrix4x4(T m11, T m12, T m13, T m14,
                          T m21, T m22, T m23, T m24,
                          T m31, T m32, T m33, T m34,
                          T m41, T m42, T m43, T m44)
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M14 = m14;
            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M24 = m24;
            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
            this.M34 = m34;
            this.M41 = m41;
            this.M42 = m42;
            this.M43 = m43;
            this.M44 = m44;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        public FMatrix4x4(FMatrix4x4<T> matrix)
        {
            M11 = matrix.M11;
            M12 = matrix.M12;
            M13 = matrix.M13;
            M14 = matrix.M14;
            M21 = matrix.M21;
            M22 = matrix.M22;
            M23 = matrix.M23;
            M24 = matrix.M24;
            M31 = matrix.M31;
            M32 = matrix.M32;
            M33 = matrix.M33;
            M34 = matrix.M34;
            M41 = matrix.M41;
            M42 = matrix.M42;
            M43 = matrix.M43;
            M44 = matrix.M44;
        }

        public T this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return M11;
                    case 1: return M21;
                    case 2: return M31;
                    case 3: return M41;
                    case 4: return M12;
                    case 5: return M22;
                    case 6: return M32;
                    case 7: return M42;
                    case 8: return M13;
                    case 9: return M23;
                    case 10:return M33;
                    case 11:return M43;
                    case 12:return M14;
                    case 13:return M24;
                    case 14:return M34;
                    case 15:return M44;
                    default: throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:  M11 = value; break;
                    case 1:  M21 = value; break;
                    case 2:  M31 = value; break;
                    case 3:  M41 = value; break;
                    case 4:  M12 = value; break;
                    case 5:  M22 = value; break;
                    case 6:  M32 = value; break;
                    case 7:  M42 = value; break;
                    case 8:  M13 = value; break;
                    case 9:  M23 = value; break;
                    case 10: M33 = value; break;
                    case 11: M43 = value; break;
                    case 12: M14 = value; break;
                    case 13: M24 = value; break;
                    case 14: M34 = value; break;
                    case 15: M44 = value; break;
                    default: throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(@$"{M11} {M12} {M13} {M14}\n
                                    {M21} {M22} {M23} {M24}\n
                                    {M31} {M32} {M33} {M34}\n
                                    {M41} {M42} {M43} {M44}\n");
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
                   M14.GetHashCode() ^
                   M21.GetHashCode() ^
                   M22.GetHashCode() ^
                   M23.GetHashCode() ^
                   M24.GetHashCode() ^
                   M31.GetHashCode() ^
                   M32.GetHashCode() ^
                   M33.GetHashCode() ^
                   M34.GetHashCode() ^
                   M41.GetHashCode() ^
                   M42.GetHashCode() ^
                   M43.GetHashCode() ^
                   M44.GetHashCode();
        }
    }
}
