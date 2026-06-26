namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 除法
    /// <para>包含定点数的除法运算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 除法运算符（定点数除整数）
        /// <para>将定点数除以整数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>相除后的结果</returns>
        /// <exception cref="DivideByZeroException">当除数为0时抛出</exception>
        public static Fixed32 operator /(Fixed32 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Div(a.rawvalue, b_rawvalue, out var _);
        }

        /// <summary>
        /// 除法运算符（整数除定点数）
        /// <para>将整数除以定点数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>相除后的结果</returns>
        /// <exception cref="DivideByZeroException">当除数为0时抛出</exception>
        public static Fixed32 operator /(int a, Fixed32 b)
        {
            var a_rawvalue = Int32ToRaw(a);
            return Div(a_rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 除法运算符（定点数除定点数）
        /// <para>将第一个定点数除以第二个定点数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>相除后的结果</returns>
        /// <exception cref="DivideByZeroException">当除数为0时抛出</exception>
        public static Fixed32 operator /(Fixed32 a, Fixed32 b)
        {
            return Div(a.rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 计算除法，并检查溢出
        /// <para>执行除法运算并检查是否发生溢出</para>
        /// </summary>
        /// <param name="a">被除数的原始值</param>
        /// <param name="b">除数的原始值</param>
        /// <param name="overflow">是否发生溢出</param>
        /// <returns>除法结果</returns>
        private static Fixed32 Div(long a, long b, out bool overflow)
        {
            overflow = false;

            // 预处理特殊边界值
            if (PreprocessDiv(a, b, out var r))
            {
                return r;
            }

            // 快速路径：如果除数是1，直接返回被除数
            if (b == One.rawvalue)
            {
                return FromRaw(a);
            }
            // 快速路径：如果除数是-1，直接返回被除数的相反数
            if (b == NegativeOne.rawvalue)
            {
                return FromRaw(-a);
            }

            // 提取符号位
            var am = a >> (TOTAL_BITS - 1);
            var bm = b >> (TOTAL_BITS - 1);

            // 计算绝对值
            var remainder = (ulong)((a + am) ^ am); // 余数
            var divisor = (ulong)((b + bm) ^ bm); // 除数
            var quotient = 0UL; // 商

            // 如果 divisor 是 2 的幂，直接右移来进行除法运算；
            // 否则，进行逐位除法
            if ((divisor & (divisor - 1)) == 0)
            {
                var shift = GetTrailingZeroCount(divisor) - FRACTIONAL_BITS - 1;
                quotient = shift >= 0 ? remainder >> shift : remainder << -shift;
                remainder = remainder & (divisor - 1);
            }
            else
            {
                var bitptr = TOTAL_BITS / 2 + 1;

                // 优化：如果除数末尾有连续的0，先右移除数
                while ((divisor & 0xF) == 0 && bitptr >= 4)
                {
                    divisor >>= 4;
                    bitptr -= 4;
                }

                // 逐位计算商
                while (remainder != 0 && bitptr >= 0)
                {
                    var shift = GetLeadingZeroCount(remainder);
                    if (shift > bitptr) shift = bitptr;
                    
                    remainder <<= shift;
                    bitptr -= shift;

                    var quot = remainder / divisor;
                    remainder = remainder % divisor;
                    quotient += quot << bitptr;

                    // 检查溢出
                    if ((quot & ~(FULL_BIT_MASK >> bitptr)) != 0)
                    {
                        overflow = true;
                        return IsSigns(a, b) ? PositiveInfinity : NegativeInfinity;
                    }

                    remainder <<= 1;
                    bitptr -= 1;
                }
            }

            // 计算最终结果
            var result = (long)(quotient >> 1);
            if ((quotient & 1) != 0 && remainder >= (divisor >> 1))
            {
                result += 1; // 进位
            }
            // 符号相反，则取负
            if (!IsSigns(a, b))
            {
                result = -result;
            }

            return FromRaw(result);
        }

        /// <summary>
        /// 获取前导零的数量
        /// <para>计算64位无符号整数的前导零个数</para>
        /// </summary>
        /// <param name="n">要计算的64位无符号整数</param>
        /// <returns>前导零的数量</returns>
        private static int GetLeadingZeroCount(ulong n)
        {
            if (n == 0) return 64;

            var count = 0;
            {
                while ((n & 0xF000000000000000) == 0) { count += 4; n <<= 4; }
                while ((n & 0x8000000000000000) == 0) { count += 1; n <<= 1; }
            }
            return count;
        }

        /// <summary>
        /// 获取尾部零的数量
        /// <para>计算64位无符号整数的尾部零个数</para>
        /// </summary>
        /// <param name="n">要计算的64位无符号整数</param>
        /// <returns>尾部零的数量</returns>
        private static int GetTrailingZeroCount(ulong n)
        {
            if (n == 0) return 64;

            var count = 0;
            {
                while ((n & 0xF) == 0) { count += 4; n >>= 4; }
                while ((n & 0x1) == 0) { count += 1; n >>= 1; }
            }
            return count;
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理特殊情况下的除法操作，如NaN、无穷大等</para>
        /// </summary>
        /// <param name="a">被除数的原始值</param>
        /// <param name="b">除数的原始值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessDiv(long a, long b, out Fixed32 r)
        {
            // 任意有一个数是NaN，得NaN
            if (a.IsNaN() || b.IsNaN()) { r = NaN; return true; }
            // 零除以零，得NaN；正数除以零，得正无穷；负数除以零，得负无穷
            if (b.IsZero())
            {
                if (a.IsZero()) { r = NaN; return true; }
                r = a > 0 ? PositiveInfinity : NegativeInfinity; 
                return true;
            }
            // 任何数除以无穷大，得零；无穷大除以无穷大，得NaN
            if (b.IsPositiveInfinity() || b.IsNegativeInfinity())
            {
                r = (a.IsPositiveInfinity() || a.IsNegativeInfinity()) ? NaN : Zero;
                return true;
            }
            // 正无穷，除以正数得正无穷；除以负数得负无穷
            if (a.IsPositiveInfinity()) { r = b > 0 ? PositiveInfinity : NegativeInfinity; return true; }
            // 负无穷，除以正数得负无穷；除以负数得正无穷
            if (a.IsNegativeInfinity()) { r = b > 0 ? NegativeInfinity : PositiveInfinity; return true; }

            r = Zero;
            return false;
        }
    }
}
