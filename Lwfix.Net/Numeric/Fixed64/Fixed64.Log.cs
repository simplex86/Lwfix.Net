using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 对数
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 自然对数（e为底），使用 Log2() * ln(2) 计算
        /// </summary>
        public Fixed64 Log()
        {
            if (ProprocessLog(rawvalue, out var r))
            {
                return r;
            }

            return Log2() * Ln2;
        }

        /// <summary>
        /// 自然对数（e为底）
        /// </summary>
        public static Fixed64 Log(Fixed64 n)
        {
            return n.Log();
        }

        /// <summary>
        /// 以2为底的对数，使用二进制搜索算法
        /// </summary>
        public Fixed64 Log2()
        {
            if (ProprocessLog(rawvalue, out var r))
            {
                return r;
            }

            var b = Int128.One << (FRACTIONAL_BITS - 1);
            var y = (Int128)0;

            var rawX = rawvalue;
            while (rawX < One.rawvalue)
            {
                rawX <<= 1;
                y -= One.rawvalue;
            }

            while (rawX >= Two.rawvalue)
            {
                rawX >>= 1;
                y += One.rawvalue;
            }

            var z = FromRaw(rawX);

            for (int i = 0; i < FRACTIONAL_BITS; i++)
            {
                z = z * z;
                if (z.rawvalue >= Two.rawvalue)
                {
                    z = FromRaw(z.rawvalue >> 1);
                    y += b;
                }
                b >>= 1;
            }

            return FromRaw(y);
        }

        /// <summary>
        /// 以2为底的对数
        /// </summary>
        public static Fixed64 Log2(Fixed64 n)
        {
            return n.Log2();
        }

        /// <summary>
        /// 以10为底的对数，使用 Log() / ln(10) 计算
        /// </summary>
        public Fixed64 Log10()
        {
            return Log() / Ln10;
        }

        /// <summary>
        /// 以10为底的对数
        /// </summary>
        public static Fixed64 Log10(Fixed64 n)
        {
            return n.Log10();
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        private bool ProprocessLog(Int128 n, out Fixed64 r)
        {
            if (n.IsNaN() || n < 0) { r = NaN; return true; }
            if (n.IsZero()) { r = NegativeInfinity; return true; }
            if (n.IsPositiveInfinity()) { r = PositiveInfinity; return true; }

            r = Zero;
            return false;
        }
    }
}
