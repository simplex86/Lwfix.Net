using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 减法
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 减法运算符（定点数减整数）
        /// </summary>
        public static Fixed64 operator -(Fixed64 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Sub(a.rawvalue, b_rawvalue, out var _);
        }

        /// <summary>
        /// 减法运算符（整数减定点数）
        /// </summary>
        public static Fixed64 operator -(int a, Fixed64 b)
        {
            var a_rawvalue = Int32ToRaw(a);
            return Sub(a_rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 减法运算符（定点数减定点数）
        /// </summary>
        public static Fixed64 operator -(Fixed64 a, Fixed64 b)
        {
            return Sub(a.rawvalue, b.rawvalue, out var _);
        }

        /// <summary>
        /// 相减并检查溢出
        /// </summary>
        private static Fixed64 Sub(Int128 a, Int128 b, out bool overflow)
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
        private static bool PreprocessSub(Int128 a, Int128 b, out Fixed64 r)
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
            if (a.IsMin() && b > Int128.Zero) { r = NegativeInfinity; return true; }
            // 最大值减负数，得正无穷
            if (a.IsMax() && b < Int128.Zero) { r = PositiveInfinity; return true; }
            // 正数减最小值，得正无穷
            if (b.IsMin() && a > Int128.Zero) { r = PositiveInfinity; return true; }
            // 负数减最大值，得负无穷
            if (b.IsMax() && a < Int128.Zero) { r = NegativeInfinity; return true; }

            r = Zero;
            return false;
        }

        /// <summary>
        /// 计算减法，并检查溢出
        /// </summary>
        private static Int128 OverflowSub(Int128 a, Int128 b, ref bool overflow)
        {
            var r = a - b;
            // 检查溢出：如果被减数和减数符号不同但结果符号与被减数不同，则发生溢出
            overflow |= (((a ^ b) & (a ^ r)) & Int128.MinValue) != Int128.Zero;

            return r;
        }

        /// <summary>
        /// 取反运算符
        /// </summary>
        public static Fixed64 operator -(Fixed64 n)
        {
            if (n.IsNaN()) return NaN;
            if (n.IsZero()) return Zero;
            if (n.IsPositiveInfinity()) return NegativeInfinity;
            if (n.IsNegativeInfinity()) return PositiveInfinity;

            return FromRaw(-n.rawvalue);
        }
    }
}
