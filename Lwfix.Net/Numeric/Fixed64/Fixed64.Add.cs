using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 加法
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 加法运算符（定点数加整数）
        /// </summary>
        public static Fixed64 operator +(Fixed64 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Add(a.rawvalue, b_rawvalue, out var _);
        }

        /// <summary>
        /// 加法运算符（整数加定点数）
        /// </summary>
        public static Fixed64 operator +(int a, Fixed64 b)
        {
            return b + a;
        }

        /// <summary>
        /// 加法运算符（定点数加定点数）
        /// </summary>
        public static Fixed64 operator +(Fixed64 a, Fixed64 b)
        {
            return Add(a.rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 相加并检查溢出
        /// </summary>
        private static Fixed64 Add(Int128 a, Int128 b, out bool overflow)
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
                return a > Int128.Zero ? PositiveInfinity : NegativeInfinity;
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
        /// </summary>
        private static bool PreprocessAdd(Int128 a, Int128 b, out Fixed64 r)
        {
            // NaN加任何数，得NaN
            if (a.IsNaN() || b.IsNaN()) { r = NaN; return true; }
            // 正负无穷相加，得NaN
            if (a.IsPositiveInfinity() && b.IsNegativeInfinity()) { r = NaN; return true; }
            if (a.IsNegativeInfinity() && b.IsPositiveInfinity()) { r = NaN; return true; }
            // 最大值加正数，得正无穷
            if (a.IsMax() && b > Int128.Zero) { r = PositiveInfinity; return true; }
            if (b.IsMax() && a > Int128.Zero) { r = PositiveInfinity; return true; }
            // 最小值加负数，得负无穷
            if (a.IsMin() && b < Int128.Zero) { r = NegativeInfinity; return true; }
            if (b.IsMin() && a < Int128.Zero) { r = NegativeInfinity; return true; }
            // 正无穷加任何数，得正无穷
            if (a.IsPositiveInfinity() || b.IsPositiveInfinity()) { r = PositiveInfinity; return true; }
            // 负无穷加任何数，得负无穷
            if (a.IsNegativeInfinity() || b.IsNegativeInfinity()) { r = NegativeInfinity; return true; }

            r = Zero;
            return false;
        }

        /// <summary>
        /// 计算加法，并检查溢出
        /// </summary>
        private static Int128 OverflowAdd(Int128 a, Int128 b, ref bool overflow)
        {
            var r = a + b;
            // 检查溢出：如果两个数符号相同但结果符号不同，则发生溢出
            overflow |= ((~(a ^ b) & (a ^ r)) & Int128.MinValue) != Int128.Zero;

            return r;
        }
    }
}
