namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 加法
    /// <para>包含定点数的加法运算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 加法运算符（定点数加整数）
        /// <para>将定点数与整数相加</para>
        /// </summary>
        /// <param name="a">定点数</param>
        /// <param name="b">整数</param>
        /// <returns>相加后的结果</returns>
        public static Fixed32 operator +(Fixed32 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Add(a.rawvalue, b_rawvalue, out var _);
        }

        /// <summary>
        /// 加法运算符（整数加定点数）
        /// <para>将整数与定点数相加</para>
        /// </summary>
        /// <param name="a">整数</param>
        /// <param name="b">定点数</param>
        /// <returns>相加后的结果</returns>
        public static Fixed32 operator +(int a, Fixed32 b)
        {
            return b + a;
        }

        /// <summary>
        /// 加法运算符（定点数加定点数）
        /// <para>将两个定点数相加</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>相加后的结果</returns>
        public static Fixed32 operator +(Fixed32 a, Fixed32 b)
        {
            return Add(a.rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 相加并检查溢出
        /// <para>执行加法运算并检查是否发生溢出</para>
        /// </summary>
        /// <param name="a">第一个操作数的原始值</param>
        /// <param name="b">第二个操作数的原始值</param>
        /// <param name="overflow">是否发生溢出</param>
        /// <returns>加法结果</returns>
        private static Fixed32 Add(long a, long b, out bool overflow)
        {
            overflow = false;

            // 预处理特殊边界值
            if (PreprocessAdd(a, b, out var r))
            {
                return r;
            }
            
            // 执行加法并检查溢出
            var c = OverflowAdd(a, b, ref overflow);
            
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
        /// <para>处理特殊情况下的加法操作，如NaN、无穷大等</para>
        /// </summary>
        /// <param name="a">第一个操作数的原始值</param>
        /// <param name="b">第二个操作数的原始值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessAdd(long a, long b, out Fixed32 r)
        {
            // NaN加任何数，得NaN
            if (a.IsNaN() || b.IsNaN()) { r = NaN; return true; }
            // 正负无穷相加，得NaN
            if (a.IsPositiveInfinity() && b.IsNegativeInfinity()) { r = NaN; return true; }
            if (a.IsNegativeInfinity() && b.IsPositiveInfinity()) { r = NaN; return true; }
            // 最大值加正数，得正无穷
            if (a.IsMax() && b > 0) { r = PositiveInfinity; return true; }
            if (b.IsMax() && a > 0) { r = PositiveInfinity; return true; }
            // 最小值加负数，得负无穷
            if (a.IsMin() && b < 0) { r = NegativeInfinity; return true; }
            if (b.IsMin() && a < 0) { r = NegativeInfinity; return true; }
            // 正无穷加任何数，得正无穷
            if (a.IsPositiveInfinity() || b.IsPositiveInfinity()) { r = PositiveInfinity; return true; }
            // 负无穷加任何数，得负无穷
            if (a.IsNegativeInfinity() || b.IsNegativeInfinity()) { r = NegativeInfinity; return true; }

            r = Zero;
            return false;
        }

        /// <summary>
        /// 计算加法，并检查溢出
        /// <para>执行加法运算并检查是否发生溢出</para>
        /// </summary>
        /// <param name="a">第一个操作数</param>
        /// <param name="b">第二个操作数</param>
        /// <param name="overflow">是否发生溢出</param>
        /// <returns>加法结果</returns>
        private static long OverflowAdd(long a, long b, ref bool overflow)
        {
            var r = a + b;
            // 检查溢出：如果两个数符号相同但结果符号不同，则发生溢出
            overflow |= ((~(a ^ b) & (a ^ r)) & long.MinValue) != 0;

            return r;
        }
    }
}
