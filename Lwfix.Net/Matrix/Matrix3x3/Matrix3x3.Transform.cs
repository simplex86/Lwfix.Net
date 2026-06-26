namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 3x3矩阵 - 变换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 获取平移矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static FMatrix3x3<T> Translate(T x, T y)
        {
            var matrix = new FMatrix3x3<T>(1, 0, 0,
                                           0, 1, 0,
                                           x, y, 1);
            return matrix;
        }

        /// <summary>
        /// 获取旋转矩阵
        /// </summary>
        /// <param name="radians">与 X 轴的夹角（单位：弧度）</param>
        /// <returns></returns>
        public static FMatrix3x3<T> Rotate(T radians)
        {
            var s = T.Sin(radians);
            var c = T.Cos(radians);

            var matrix = new FMatrix3x3<T>( c, s, 0,
                                           -s, c, 0,
                                            0, 0, 1);
            return matrix;
        }

        /// <summary>
        /// 获取缩放矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static FMatrix3x3<T> Scale(T x, T y)
        {
            var matrix = new FMatrix3x3<T>(x, 0, 0,
                                           0, y, 0,
                                           0, 0, 1);
            return matrix;
        }

        /// <summary>
        /// 获取平移旋转矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix3x3<T> TR(T x, T y, T radians)
        {
            var t = Translate(x, y);
            var r = Rotate(radians);

            return r * t;
        }

        /// <summary>
        /// 获取平移旋转缩放矩阵
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="radians"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public static FMatrix3x3<T> TRS(T tx, T ty, T radians, T sx, T sy)
        {
            var t = Translate(tx, ty);
            var r = Rotate(radians);
            var s = Scale(sx, sy);

            return s * r * t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static FMatrix3x3<T> AngleAxis(T angle, FVector3<T> axis)
        {
            var x = axis.X;
            var y = axis.Y;
            var z = axis.Z;
            var n1 = T.Sin(angle);
            var n2 = T.Cos(angle);
            var n3 = x * x;
            var n4 = y * y;
            var n5 = z * z;
            var n6 = x * y;
            var n7 = x * z;
            var n8 = y * z;

            var result = new FMatrix3x3<T>()
            {
                M11 = n3 + (n2 * (T.One - n3)),
                M12 = n6 - (n2 * n6) + (n1 * z),
                M13 = n7 - (n2 * n7) - (n1 * y),
                M21 = n6 - (n2 * n6) - (n1 * z),
                M22 = n4 + (n2 * (T.One - n4)),
                M23 = n8 - (n2 * n8) + (n1 * x),
                M31 = n7 - (n2 * n7) + (n1 * y),
                M32 = n8 - (n2 * n8) - (n1 * x),
                M33 = n5 + (n2 * (T.One - n5)),
            };

            return result;
        }
    }
}
