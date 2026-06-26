namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 乘法
    /// <para>包含定点数的乘法运算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 乘法运算符（定点数乘整数）
        /// <para>将定点数与整数相乘</para>
        /// </summary>
        /// <param name="a">定点数</param>
        /// <param name="b">整数</param>
        /// <returns>相乘后的结果</returns>
        public static Fixed32 operator *(Fixed32 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Mul(a.rawvalue, b_rawvalue, out var _);
        }

        /// <summary>
        /// 乘法运算符（整数乘定点数）
        /// <para>将整数与定点数相乘</para>
        /// </summary>
        /// <param name="a">整数</param>
        /// <param name="b">定点数</param>
        /// <returns>相乘后的结果</returns>
        public static Fixed32 operator *(int a, Fixed32 b)
        {
            return b * a;
        }

        /// <summary>
        /// 乘法运算符（定点数乘定点数）
        /// <para>将两个定点数相乘</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>相乘后的结果</returns>
        public static Fixed32 operator *(Fixed32 a, Fixed32 b)
        {
            return Mul(a.rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 计算乘法，并检查溢出
        /// <para>执行乘法运算并检查是否发生溢出</para>
        /// </summary>
        /// <param name="a">第一个操作数的原始值</param>
        /// <param name="b">第二个操作数的原始值</param>
        /// <param name="overflow">是否发生溢出</param>
        /// <returns>乘法结果</returns>
        private static Fixed32 Mul(long a, long b, out bool overflow)
        {
            overflow = false;

            // 预处理特殊边界值
            if (PreprocessMul(a, b, out var r))
            {
                return r;
            }

            // 快速路径：如果其中一个操作数是0，直接返回0
            if (a == 0 || b == 0)
            {
                return Zero;
            }

            // 快速路径：如果其中一个操作数是1，直接返回另一个操作数
            if (a == One.rawvalue)
            {
                return FromRaw(b);
            }
            if (b == One.rawvalue)
            {
                return FromRaw(a);
            }

            // 分解整数部分和小数部分
            var aint = a >> FRACTIONAL_BITS;        // a的整数部分
            var bint = b >> FRACTIONAL_BITS;        // b的整数部分
            var afrac = (ulong)(a & FRACTIONAL_MASK); // a的小数部分
            var bfrac = (ulong)(b & FRACTIONAL_MASK); // b的小数部分

            // 计算各部分的乘积
            var term1 = aint * bint;                // 整数部分相乘
            var term2 = aint * (long)bfrac;         // a整数部分 * b小数部分
            var term3 = bint * (long)afrac;         // b整数部分 * a小数部分
            var term4 = afrac * bfrac;              // 小数部分相乘

            // 合并结果，处理溢出
            var c = OverflowAdd((long)(term4 >> FRACTIONAL_BITS), term3, ref overflow);
            c = OverflowAdd(c, term2, ref overflow);
            c = OverflowAdd(c, term1 << INTEGRAL_BITS, ref overflow);

            // 检查符号和溢出情况
            var signs = IsSigns(a, b); // 符号相同
            if (signs)
            {
                // 同号相乘，结果应为正
                if (c < 0 || (overflow && a > 0)) return PositiveInfinity;
            }
            else
            {
                // 异号相乘，结果应为负
                if (c > 0) return NegativeInfinity;
            }

            // 检查进位情况
            var carry = term1 >> FRACTIONAL_BITS;
            if (carry != 0 && carry != -1)
            {
                return signs ? PositiveInfinity : NegativeInfinity;
            }

            return FromRaw(c);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理特殊情况下的乘法操作，如NaN、无穷大等</para>
        /// </summary>
        /// <param name="a">第一个操作数的原始值</param>
        /// <param name="b">第二个操作数的原始值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessMul(long a, long b, out Fixed32 r)
        {
            // NaN乘以任何数，都等于NaN
            if (a.IsNaN() || b.IsNaN()) { r = NaN; return true; }
            // 正无穷，乘以正数得正无穷，乘以负数得负无穷
            if (a.IsPositiveInfinity()) 
            {
                if (b.IsZero()) { r = NaN; return true; }
                r = b > 0 ? PositiveInfinity : NegativeInfinity;
                return true; 
            }
            if (b.IsPositiveInfinity()) 
            {
                if (a.IsZero()) { r = NaN; return true; }
                r = a > 0 ? PositiveInfinity : NegativeInfinity; 
                return true; 
            }
            // 负无穷，乘以正数得负无穷，乘以负数得正无穷
            if (a.IsNegativeInfinity())
            {
                if (b.IsZero()) { r = NaN; return true; }
                r = b < 0 ? PositiveInfinity : NegativeInfinity;
                return true;
            }
            if (b.IsNegativeInfinity())
            {
                if (a.IsZero()) { r = NaN; return true; }
                r = a < 0 ? PositiveInfinity : NegativeInfinity;
                return true;
            }

            r = Zero;
            return false;
        }
    }
}
