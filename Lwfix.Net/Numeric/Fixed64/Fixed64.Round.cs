using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 四舍五入
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 四舍五入到最近的整数，半值采用银行家舍入法
        /// </summary>
        public Fixed64 Round()
        {
            if (IsNaN()) return NaN;
            if (IsPositiveInfinity()) return PositiveInfinity;
            if (IsNegativeInfinity()) return NegativeInfinity;

            var frac = rawvalue & FRACTIONAL_MASK;

            // Q64.64的半值点为1<<63
            if (frac < (Int128.One << 63)) return Floor();
            if (frac > (Int128.One << 63)) return Ceil();

            // 半值时向偶数取整
            return (rawvalue & One.rawvalue) == 0 ? Floor()
                                                  : Ceil();
        }

        /// <summary>
        /// 四舍五入
        /// </summary>
        public static Fixed64 Round(Fixed64 n)
        {
            return FMath.Round(n);
        }

        /// <summary>
        /// 四舍五入到整数
        /// </summary>
        public int RoundToInt()
        {
            return Round().ToInt();
        }

        /// <summary>
        /// 四舍五入到整数
        /// </summary>
        public static int RoundToInt(Fixed64 n)
        {
            return FMath.RoundToInt(n);
        }
    }
}
