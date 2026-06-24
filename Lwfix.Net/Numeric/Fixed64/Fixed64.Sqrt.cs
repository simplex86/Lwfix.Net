using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 平方根
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 半位宽，用于平方根计算的中间变量
        /// </summary>
        private const byte HALF_TOTAL_BITS = TOTAL_BITS / 2;

        /// <summary>
        /// 计算平方根，使用二进制搜索算法
        /// </summary>
        public Fixed64 Sqrt()
        {
            if (IsNaN()) return NaN;
            if (IsPositiveInfinity()) return PositiveInfinity;
            if (IsZero()) return Zero;
            if (IsNegative()) return NaN;

            var val = (UInt128)rawvalue;
            var bit = (UInt128)1 << (TOTAL_BITS - 2);
            while (bit > val) bit >>= 2;

            var res = (UInt128)0;
            for (int i = 0; i < 2; i++)
            {
                while (bit != 0)
                {
                    if (val >= res + bit)
                    {
                        val -= res + bit;
                        res = (res >> 1) + bit;
                    }
                    else
                    {
                        res >>= 1;
                    }
                    bit >>= 2;
                }

                if (i == 0)
                {
                    if (val > ((UInt128)1 << HALF_TOTAL_BITS) - 1)
                    {
                        val -= res;
                        val = (val << HALF_TOTAL_BITS) - ((UInt128)1 << (HALF_TOTAL_BITS - 1));
                        res = (res << HALF_TOTAL_BITS) + ((UInt128)1 << (HALF_TOTAL_BITS - 1));
                    }
                    else
                    {
                        val <<= HALF_TOTAL_BITS;
                        res <<= HALF_TOTAL_BITS;
                    }
                    bit = (UInt128)1 << (HALF_TOTAL_BITS - 2);
                }
            }

            if (val > res) res++;
            return FromRaw((Int128)res);
        }

        /// <summary>
        /// 计算平方根
        /// </summary>
        public static Fixed64 Sqrt(Fixed64 n)
        {
            return n.Sqrt();
        }
    }
}
