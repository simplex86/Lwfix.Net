using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 取余
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 取余运算符（定点数取余整数）
        /// </summary>
        public static Fixed64 operator %(Fixed64 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Mod(a.rawvalue, b_rawvalue);
        }

        /// <summary>
        /// 取余运算符（整数取余定点数）
        /// </summary>
        public static Fixed64 operator %(int a, Fixed64 b)
        {
            var a_rawvalue = Int32ToRaw(a);
            return Mod(a_rawvalue, b.rawvalue);
        }

        /// <summary>
        /// 取余运算符（定点数取余定点数）
        /// </summary>
        public static Fixed64 operator %(Fixed64 a, Fixed64 b)
        {
            return Mod(a.rawvalue, b.rawvalue);
        }

        /// <summary>
        /// 取余
        /// </summary>
        private static Fixed64 Mod(Int128 a, Int128 b)
        {
            if (PreprocessMod(a, b, out var r))
            {
                return r;
            }

            return FromRaw(a % b);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        private static bool PreprocessMod(Int128 a, Int128 b, out Fixed64 r)
        {
            if (a.IsNaN() || b.IsNaN()) { r = NaN; return true; }
            if (a.IsInfinity()) { r = NaN; return true; }
            if (b.IsZero()) { r = NaN; return true; }
            if (a.IsMin()) { r = Zero; return true; }
            if (b.IsNegativeOne()) { r = Zero; return true; }
            if (b.IsMinMax() || b.IsInfinity()) { r = FromRaw(a); return true; }

            r = Zero;
            return false;
        }
    }
}
