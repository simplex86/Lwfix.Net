using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 除法
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 除法运算符（定点数除整数）
        /// </summary>
        public static Fixed64 operator /(Fixed64 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Div(a.rawvalue, b_rawvalue, out var _);
        }

        /// <summary>
        /// 除法运算符（整数除定点数）
        /// </summary>
        public static Fixed64 operator /(int a, Fixed64 b)
        {
            var a_rawvalue = Int32ToRaw(a);
            return Div(a_rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 除法运算符（定点数除定点数）
        /// </summary>
        public static Fixed64 operator /(Fixed64 a, Fixed64 b)
        {
            return Div(a.rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 计算除法，并检查溢出
        /// </summary>
        private static Fixed64 Div(Int128 a, Int128 b, out bool overflow)
        {
            overflow = false;

            // 预处理特殊边界值
            if (PreprocessDiv(a, b, out var r))
            {
                return r;
            }

            // 快速路径：如果除数是1，直接返回被除数
            if (b.IsOne())
            {
                return FromRaw(a);
            }
            // 快速路径：如果除数是-1，直接返回被除数的相反数
            if (b.IsNegativeOne())
            {
                return FromRaw(-a);
            }

            // 提取符号位（算术右移127位，正数得0，负数得-1）
            var am = a >> (TOTAL_BITS - 1);
            var bm = b >> (TOTAL_BITS - 1);

            // 计算绝对值
            var remainder = (UInt128)((a + am) ^ am);
            var divisor = (UInt128)((b + bm) ^ bm);
            var quotient = UInt128.Zero;

            // 如果 divisor 是 2 的幂，直接右移来进行除法运算；
            // 否则，进行逐位除法
            if ((divisor & (divisor - 1U)) == 0U)
            {
                var shift = GetTrailingZeroCount(divisor) - FRACTIONAL_BITS - 1;
                quotient = shift >= 0 ? remainder >> shift : remainder << -shift;
                remainder = remainder & (divisor - 1U);
            }
            else
            {
                var bitptr = TOTAL_BITS / 2 + 1;

                // 优化：如果除数末尾有连续的0，先右移除数
                while (((ulong)divisor & 0xF) == 0 && bitptr >= 4)
                {
                    divisor >>= 4;
                    bitptr -= 4;
                }

                // 逐位计算商
                while (remainder != 0U && bitptr >= 0)
                {
                    var shift = GetLeadingZeroCount(remainder);
                    if (shift > bitptr) shift = bitptr;

                    remainder <<= shift;
                    bitptr -= shift;

                    var quot = remainder / divisor;
                    remainder = remainder % divisor;
                    quotient += quot << bitptr;

                    // 检查溢出
                    if ((quot & ~(FULL_BIT_MASK >> bitptr)) != 0U)
                    {
                        overflow = true;
                        return IsSigns(a, b) ? PositiveInfinity : NegativeInfinity;
                    }

                    remainder <<= 1;
                    bitptr -= 1;
                }
            }

            // 计算最终结果
            var result = (Int128)(quotient >> 1);
            if ((quotient & 1U) != 0U && remainder >= (divisor >> 1))
            {
                result += Int128.One; // 四舍五入进位
            }
            // 符号相反，则取负
            if (!IsSigns(a, b))
            {
                result = -result;
            }

            return FromRaw(result);
        }

        /// <summary>
        /// 获取前导零的数量（128位）
        /// </summary>
        private static int GetLeadingZeroCount(UInt128 n)
        {
            if (n == 0U) return 128;

            var count = 0;
            // 先检查高64位，为0则检查低64位
            var hi = (ulong)(n >> 64);
            if (hi == 0)
            {
                count = 64;
                hi = (ulong)n;
            }
            while ((hi & 0xF000000000000000) == 0) { count += 4; hi <<= 4; }
            while ((hi & 0x8000000000000000) == 0) { count += 1; hi <<= 1; }
            return count;
        }

        /// <summary>
        /// 获取尾部零的数量（128位）
        /// </summary>
        private static int GetTrailingZeroCount(UInt128 n)
        {
            if (n == 0U) return 128;

            var count = 0;
            // 先检查低64位，为0则检查高64位
            var lo = (ulong)n;
            if (lo == 0)
            {
                count = 64;
                lo = (ulong)(n >> 64);
            }
            while ((lo & 0xF) == 0) { count += 4; lo >>= 4; }
            while ((lo & 0x1) == 0) { count += 1; lo >>= 1; }
            return count;
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        private static bool PreprocessDiv(Int128 a, Int128 b, out Fixed64 r)
        {
            // 任意有一个数是NaN，得NaN
            if (a.IsNaN() || b.IsNaN()) { r = NaN; return true; }
            // 零除以零，得NaN；正数除以零，得正无穷；负数除以零，得负无穷
            if (b.IsZero())
            {
                if (a.IsZero()) { r = NaN; return true; }
                r = a > Int128.Zero ? PositiveInfinity : NegativeInfinity;
                return true;
            }
            // 任何数除以无穷大，得零；无穷大除以无穷大，得NaN
            if (b.IsPositiveInfinity() || b.IsNegativeInfinity())
            {
                r = (a.IsPositiveInfinity() || a.IsNegativeInfinity()) ? NaN : Zero;
                return true;
            }
            // 正无穷，除以正数得正无穷；除以负数得负无穷
            if (a.IsPositiveInfinity()) { r = b > Int128.Zero ? PositiveInfinity : NegativeInfinity; return true; }
            // 负无穷，除以正数得负无穷；除以负数得正无穷
            if (a.IsNegativeInfinity()) { r = b > Int128.Zero ? NegativeInfinity : PositiveInfinity; return true; }

            r = Zero;
            return false;
        }
    }
}
