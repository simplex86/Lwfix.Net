using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 4x4矩阵 - 投影
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        /// <param name="zNear"></param>
        /// <param name="zFar"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FMatrix4x4<T> Frustum(T left, T right, T bottom, T top, T zNear, T zFar)
        {
            FMatrix4x4<T> matrix = Zero;

            matrix[0] = T.Two * zNear / (right - left);
            matrix[8] = (right + left) / (right - left);
            matrix[5] = T.Two * zNear / (top - bottom);
            matrix[9] = (top + bottom) / (top - bottom);
            matrix[10] = -(zFar + zNear) / (zFar - zNear);
            matrix[14] = -(T.Two * zFar * zNear) / (zFar - zNear);
            matrix[11] = T.NegativeOne;

            return matrix;
        }

        /// <summary>
        /// 正交投影矩阵
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        /// <param name="zNear"></param>
        /// <param name="zFar"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMatrix4x4<T> Ortho(T left, T right, T bottom, T top, T zNear, T zFar)
        {
            FMatrix4x4<T> matrix = Zero;

            matrix[0] = T.Two / (right - left);
            matrix[5] = T.Two / (top - bottom);
            matrix[10] = -T.Two / (zFar - zNear);
            matrix[15] = T.One;

            //result[12] = -(right + left) / (right - left);
            //result[13] = -(top + bottom) / (top - bottom);
            matrix[14] = -(zFar + zNear) / (zFar - zNear);

            return matrix;
        }

        /// <summary>
        /// 透视投影矩阵
        /// </summary>
        /// <param name="fov"></param>
        /// <param name="aspect"></param>
        /// <param name="zNear"></param>
        /// <param name="zFar"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMatrix4x4<T> Perspective(T fov, T aspect, T zNear, T zFar)
        {
            var rad = T.DegreeToRadian(fov * T.Half);
            var fax = T.One / T.Tan(rad);

            FMatrix4x4<T> matrix = Zero;
            matrix[0] = fax / aspect;
            matrix[5] = fax;
            matrix[10] = -(zFar + zNear) / (zFar - zNear);
            matrix[14] = -(T.Two * zFar * zNear) / (zFar - zNear);
            matrix[11] = T.One;

            return matrix;
        }
    }
}
