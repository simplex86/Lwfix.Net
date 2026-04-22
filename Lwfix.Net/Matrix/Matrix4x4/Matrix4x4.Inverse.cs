namespace SimplexLab.Fixed
{
    /// <summary>
    /// 4x4矩阵 - 逆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public FMatrix4x4<T> Inversed => Inverse(this);

        /// <summary>
        /// 逆矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> Inverse(FMatrix4x4<T> matrix)
        {
            //                                       -1
            // If you have matrix M, inverse Matrix M   can compute
            //
            //     -1       1      
            //    M   = --------- A
            //            det(M)
            //
            // A is adjugate (adjoint) of M, where,
            //
            //      T
            // A = C
            //
            // C is Cofactor matrix of M, where,
            //           i + j
            // C   = (-1)      * det(M  )
            //  ij                    ij
            //
            //     [ a b c d ]
            // M = [ e f g h ]
            //     [ i j k l ]
            //     [ m n o p ]
            //
            // First Row
            //           2 | f g h |
            // C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
            //  11         | n o p |
            //
            //           3 | e g h |
            // C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
            //  12         | m o p |
            //
            //           4 | e f h |
            // C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
            //  13         | m n p |
            //
            //           5 | e f g |
            // C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
            //  14         | m n o |
            //
            // Second Row
            //           3 | b c d |
            // C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
            //  21         | n o p |
            //
            //           4 | a c d |
            // C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
            //  22         | m o p |
            //
            //           5 | a b d |
            // C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
            //  23         | m n p |
            //
            //           6 | a b c |
            // C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
            //  24         | m n o |
            //
            // Third Row
            //           4 | b c d |
            // C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
            //  31         | n o p |
            //
            //           5 | a c d |
            // C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
            //  32         | m o p |
            //
            //           6 | a b d |
            // C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
            //  33         | m n p |
            //
            //           7 | a b c |
            // C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
            //  34         | m n o |
            //
            // Fourth Row
            //           5 | b c d |
            // C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
            //  41         | j k l |
            //
            //           6 | a c d |
            // C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
            //  42         | i k l |
            //
            //           7 | a b d |
            // C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
            //  43         | i j l |
            //
            //           8 | a b c |
            // C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
            //  44         | i j k |
            
            FMatrix4x4<T> result;

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

            T a11 = f * kp_lo - g * jp_ln + h * jo_kn;
            T a12 = -(e * kp_lo - g * ip_lm + h * io_km);
            T a13 = e * jp_ln - f * ip_lm + h * in_jm;
            T a14 = -(e * jo_kn - f * io_km + g * in_jm);

            var det = a * a11 + b * a12 + c * a13 + d * a14;

            if (det.IsZero())
            {
                result.M11 = T.PositiveInfinity;
                result.M12 = T.PositiveInfinity;
                result.M13 = T.PositiveInfinity;
                result.M14 = T.PositiveInfinity;
                result.M21 = T.PositiveInfinity;
                result.M22 = T.PositiveInfinity;
                result.M23 = T.PositiveInfinity;
                result.M24 = T.PositiveInfinity;
                result.M31 = T.PositiveInfinity;
                result.M32 = T.PositiveInfinity;
                result.M33 = T.PositiveInfinity;
                result.M34 = T.PositiveInfinity;
                result.M41 = T.PositiveInfinity;
                result.M42 = T.PositiveInfinity;
                result.M43 = T.PositiveInfinity;
                result.M44 = T.PositiveInfinity;

            }
            else
            {
                var invDet = det.Reciprocal();

                result.M11 = a11 * invDet;
                result.M21 = a12 * invDet;
                result.M31 = a13 * invDet;
                result.M41 = a14 * invDet;

                result.M12 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
                result.M22 = (a * kp_lo - c * ip_lm + d * io_km) * invDet;
                result.M32 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
                result.M42 = (a * jo_kn - b * io_km + c * in_jm) * invDet;

                var gp_ho = g * p - h * o;
                var fp_hn = f * p - h * n;
                var fo_gn = f * o - g * n;
                var ep_hm = e * p - h * m;
                var eo_gm = e * o - g * m;
                var en_fm = e * n - f * m;

                result.M13 = (b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
                result.M23 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
                result.M33 = (a * fp_hn - b * ep_hm + d * en_fm) * invDet;
                result.M43 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

                var gl_hk = g * l - h * k;
                var fl_hj = f * l - h * j;
                var fk_gj = f * k - g * j;
                var el_hi = e * l - h * i;
                var ek_gi = e * k - g * i;
                var ej_fi = e * j - f * i;

                result.M14 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
                result.M24 = (a * gl_hk - c * el_hi + d * ek_gi) * invDet;
                result.M34 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
                result.M44 = (a * fk_gj - b * ek_gi + c * ej_fi) * invDet;
            }

            return result;
        }
    }
}
