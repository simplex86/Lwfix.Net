namespace SimplexLab.Fixed
{
    /// <summary>
    /// 4x4矩阵 - 行列式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 行列式
        /// </summary>
        public T Determinanted => Determinant(this);

        /// <summary>
        /// 行列式
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static T Determinant(FMatrix4x4<T> matrix)
        {
            // | a b c d |     | f g h |     | e g h |     | e f h |     | e f g |
            // | e f g h | = a | j k l | - b | i k l | + c | i j l | - d | i j k |
            // | i j k l |     | n o p |     | m o p |     | m n p |     | m n o |
            // | m n o p |
            //
            //   | f g h |
            // a | j k l | = a ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
            //   | n o p |
            //
            //   | e g h |     
            // b | i k l | = b ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
            //   | m o p |     
            //
            //   | e f h |
            // c | i j l | = c ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
            //   | m n p |
            //
            //   | e f g |
            // d | i j k | = d ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
            //   | m n o |
            //
            // Cost of operation
            // 17 adds and 28 muls.
            //
            // add: 6 + 8 + 3 = 17
            // mul: 12 + 16 = 28

            T a = matrix.M11, b = matrix.M12, c = matrix.M13, d = matrix.M14;
            T e = matrix.M21, f = matrix.M22, g = matrix.M23, h = matrix.M24;
            T i = matrix.M31, j = matrix.M32, k = matrix.M33, l = matrix.M34;
            T m = matrix.M41, n = matrix.M42, o = matrix.M43, p = matrix.M44;

            T kp_lo = k * p - l * o;
            T jp_ln = j * p - l * n;
            T jo_kn = j * o - k * n;
            T ip_lm = i * p - l * m;
            T io_km = i * o - k * m;
            T in_jm = i * n - j * m;

            return a * (f * kp_lo - g * jp_ln + h * jo_kn) -
                   b * (e * kp_lo - g * ip_lm + h * io_km) +
                   c * (e * jp_ln - f * ip_lm + h * in_jm) -
                   d * (e * jo_kn - f * io_km + g * in_jm);
        }
    }
}
