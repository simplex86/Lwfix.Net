using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 乘法
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 乘法运算符（定点数乘整数）
        /// </summary>
        public static Fixed64 operator *(Fixed64 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Mul(a.rawvalue, b_rawvalue, out var _);
        }

        /// <summary>
        /// 乘法运算符（整数乘定点数）
        /// </summary>
        public static Fixed64 operator *(int a, Fixed64 b)
        {
            return b * a;
        }

        /// <summary>
        /// 乘法运算符（定点数乘定点数）
        /// </summary>
        public static Fixed64 operator *(Fixed64 a, Fixed64 b)
        {
            return Mul(a.rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 计算乘法，并检查溢出
        /// </summary>
        private static Fixed64 Mul(Int128 a, Int128 b, out bool overflow)
        {
            overflow = false;

            // 预处理特殊边界值
            if (PreprocessMul(a, b, out var r))
            {
                return r;
            }

            // 快速路径：如果其中一个操作数是0，直接返回0
            if (a.IsZero() || b.IsZero())
            {
                return Zero;
            }

            // 快速路径：如果其中一个操作数是1，直接返回另一个操作数
            if (a.IsOne())
            {
                return FromRaw(b);
            }
            if (b.IsOne())
            {
                return FromRaw(a);
            }

            // 分解为整数部分和小数部分
            var aint = (long)(a >> FRACTIONAL_BITS);        // 整数部分（有符号64位）
            var bint = (long)(b >> FRACTIONAL_BITS);        // 整数部分（有符号64位）
            var afrac = (UInt128)(a & FRACTIONAL_MASK);      // 小数部分（无符号64位）
            var bfrac = (UInt128)(b & FRACTIONAL_MASK);      // 小数部分（无符号64位）

            // 各部分乘积均不超过128位
            var term1 = (Int128)aint * (Int128)bint;         // 整数 * 整数
            var term2 = (Int128)aint * (Int128)bfrac;        // 整数 * 小数
            var term3 = (Int128)bint * (Int128)afrac;        // 整数 * 小数
            // 小数 * 小数用UInt128存储，避免两个~2^64的值相乘超过Int128.MaxValue(≈2^127)导致溢出
            var term4 = afrac * bfrac;                        // UInt128

            // 合并结果：result = term1 << 64 + term2 + term3 + (term4 >> 64)
            // 先右移再转Int128，高64位最大约2^64，不会溢出
            var c = OverflowAdd((Int128)(term4 >> FRACTIONAL_BITS), term3, ref overflow);
            c = OverflowAdd(c, term2, ref overflow);
            c = OverflowAdd(c, term1 << INTEGRAL_BITS, ref overflow);

            // 检查符号和溢出情况
            var signs = IsSigns(a, b);
            if (signs)
            {
                // 同号相乘，结果应为正
                if (c < Int128.Zero || (overflow && a > Int128.Zero)) return PositiveInfinity;
            }
            else
            {
                // 异号相乘，结果应为负
                if (c > Int128.Zero) return NegativeInfinity;
            }

            // 检查进位：整数乘积的高位若非0或-1（符号扩展），则溢出
            var carry = term1 >> FRACTIONAL_BITS;
            if (carry != Int128.Zero && carry != Int128.NegativeOne)
            {
                return signs ? PositiveInfinity : NegativeInfinity;
            }

            return FromRaw(c);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        private static bool PreprocessMul(Int128 a, Int128 b, out Fixed64 r)
        {
            // NaN乘以任何数，都等于NaN
            if (a.IsNaN() || b.IsNaN()) { r = NaN; return true; }
            // 正无穷，乘以正数得正无穷，乘以负数得负无穷
            if (a.IsPositiveInfinity())
            {
                if (b.IsZero()) { r = NaN; return true; }
                r = b > Int128.Zero ? PositiveInfinity : NegativeInfinity;
                return true;
            }
            if (b.IsPositiveInfinity())
            {
                if (a.IsZero()) { r = NaN; return true; }
                r = a > Int128.Zero ? PositiveInfinity : NegativeInfinity;
                return true;
            }
            // 负无穷，乘以正数得负无穷，乘以负数得正无穷
            if (a.IsNegativeInfinity())
            {
                if (b.IsZero()) { r = NaN; return true; }
                r = b < Int128.Zero ? PositiveInfinity : NegativeInfinity;
                return true;
            }
            if (b.IsNegativeInfinity())
            {
                if (a.IsZero()) { r = NaN; return true; }
                r = a < Int128.Zero ? PositiveInfinity : NegativeInfinity;
                return true;
            }

            r = Zero;
            return false;
        }
    }
}
