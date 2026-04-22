namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 减法
    /// <para>包含定点数的减法运算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 减法运算符（定点数减整数）
        /// <para>将定点数减去整数</para>
        /// </summary>
        /// <param name="a">被减数（定点数）</param>
        /// <param name="b">减数（整数）</param>
        /// <returns>相减后的结果</returns>
        public static Fixed32 operator -(Fixed32 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Sub(a.rawvalue, b_rawvalue, out var _);
        }

        /// <summary>
        /// 减法运算符（整数减定点数）
        /// <para>将整数减去定点数</para>
        /// </summary>
        /// <param name="a">被减数（整数）</param>
        /// <param name="b">减数（定点数）</param>
        /// <returns>相减后的结果</returns>
        public static Fixed32 operator -(int a, Fixed32 b)
        {
            var a_rawvalue = Int32ToRaw(a);
            return Sub(a_rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 减法运算符（定点数减定点数）
        /// <para>将第一个定点数减去第二个定点数</para>
        /// </summary>
        /// <param name="a">被减数</param>
        /// <param name="b">减数</param>
        /// <returns>相减后的结果</returns>
        public static Fixed32 operator -(Fixed32 a, Fixed32 b)
        {
            return Sub(a.rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 相减并检查溢出
        /// <para>执行减法运算并检查是否发生溢出</para>
        /// </summary>
        /// <param name="a">被减数的原始值</param>
        /// <param name="b">减数的原始值</param>
        /// <param name="overflow">是否发生溢出</param>
        /// <returns>减法结果</returns>
        private static Fixed32 Sub(long a, long b, out bool overflow)
        {
            overflow = false;

            // 预处理特殊边界值
            if (PreprocessSub(a, b, out var r))
            {
                return r;
            }

            // 执行减法并检查溢出
            var c = OverflowSub(a, b, ref overflow);
            
            // 处理溢出情况
            if (overflow)
            {
                return a > 0 ? PositiveInfinity : NegativeInfinity;
            }
            
            // 处理极值情况
            if (c < MinValue.rawvalue)
            {
                return NegativeInfinity;
            }
            if (c > MaxValue.rawvalue)
            {
                return PositiveInfinity;
            }

            return FromRaw(c);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理特殊情况下的减法操作，如NaN、无穷大等</para>
        /// </summary>
        /// <param name="a">被减数的原始值</param>
        /// <param name="b">减数的原始值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessSub(long a, long b, out Fixed32 r)
        {
            // NaN减任何数，得NaN
            if (a.IsNaN() || b.IsNaN()) { r = NaN; return true; }
            // 正无穷减正无穷，得NaN
            if (a.IsPositiveInfinity() && b.IsPositiveInfinity()) { r = NaN; return true; }
            // 负无穷减负无穷，得NaN
            if (a.IsNegativeInfinity() && b.IsNegativeInfinity()) { r = NaN; return true; }
            // 负无穷减任何数 或 任何数减正无穷，得负无穷
            if (a.IsNegativeInfinity() || b.IsPositiveInfinity()) { r = NegativeInfinity; return true; }
            // 正无穷减任何数 或 任何数减负无穷，得正无穷
            if (a.IsPositiveInfinity() || b.IsNegativeInfinity()) { r = PositiveInfinity; return true; }
            // 最小值减正数，得负无穷
            if (a.IsMin() && b > 0) { r = NegativeInfinity; return true; }
            // 最大值减负数，得正无穷
            if (a.IsMax() && b < 0) { r = PositiveInfinity; return true; }
            // 正数减最小值，得正无穷
            if (b.IsMin() && a > 0) { r = PositiveInfinity; return true; }
            // 负数减最大值，得负无穷
            if (b.IsMax() && a < 0) { r = NegativeInfinity; return true; }

            r = Zero;
            return false;
        }

        /// <summary>
        /// 计算减法，并检查溢出
        /// <para>执行减法运算并检查是否发生溢出</para>
        /// </summary>
        /// <param name="a">被减数</param>
        /// <param name="b">减数</param>
        /// <param name="overflow">是否发生溢出</param>
        /// <returns>减法结果</returns>
        private static long OverflowSub(long a, long b, ref bool overflow)
        {
            var r = a - b;
            // 检查溢出：如果被减数和减数符号不同但结果符号与被减数不同，则发生溢出
            overflow |= (((a ^ b) & (a ^ r)) & long.MinValue) != 0;

            return r;
        }

        /// <summary>
        /// 取反运算符
        /// <para>返回定点数的相反数</para>
        /// </summary>
        /// <param name="n">要取反的定点数</param>
        /// <returns>取反后的结果</returns>
        public static Fixed32 operator -(Fixed32 n)
        {
            if (n.IsNaN()) return NaN;
            if (n.IsZero()) return Zero;
            if (n.IsPositiveInfinity()) return NegativeInfinity;
            if (n.IsNegativeInfinity()) return PositiveInfinity;

            return FromRaw(-n.rawvalue);
        }
    }
}
