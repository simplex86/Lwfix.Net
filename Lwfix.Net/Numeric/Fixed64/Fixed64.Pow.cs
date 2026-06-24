using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 幂
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 计算整数次幂，使用快速幂算法
        /// </summary>
        public Fixed64 Pow(int n)
        {
            var n_rawvalue = Int32ToRaw(n);
            if (PreprocessLog(rawvalue, n_rawvalue, out var r))
            {
                return r;
            }

            var m = this;
            if (n < 0)
            {
                m = Reciprocal();
                n = -n;
            }

            var c = One;
            while (n > 0)
            {
                if (n % 2 == 1)
                {
                    c *= m;
                }
                m *= m;
                n /= 2;
            }

            return c;
        }

        /// <summary>
        /// 计算整数次幂
        /// </summary>
        public static Fixed64 Pow(Fixed64 m, int n)
        {
            return FMath.Pow(m, n);
        }

        /// <summary>
        /// 计算任意次幂，小数指数使用 m^n = e^(n*ln(m))
        /// </summary>
        public Fixed64 Pow(Fixed64 n)
        {
            if (PreprocessLog(rawvalue, n.rawvalue, out var r))
            {
                return r;
            }

            if (n.IsFractional())
            {
                if (IsNegative()) return NaN;
                return (n * Log()).Exp();
            }

            return Pow(n.ToInt());
        }

        /// <summary>
        /// 计算任意次幂
        /// </summary>
        public static Fixed64 Pow(Fixed64 m, Fixed64 n)
        {
            return FMath.Pow(m, n);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        private static bool PreprocessLog(Int128 m, Int128 n, out Fixed64 r)
        {
            // 有NaN参与的运算，都等于NaN
            if (m.IsNaN() || n.IsNaN()) { r = NaN; return true; }
            // 任何数（非NaN）的0次幂，都等于1
            if (n.IsZero()) { r = One; return true; }
            // 负数的小数次幂，等于NaN
            if (m < 0 && n.IsFractional()) { r = NaN; return true; }
            if (m.IsZero()) { r = (n < 0) ? PositiveInfinity : Zero; return true; }
            // 1的任何次幂都等于1
            if (m.IsOne()) { r = One; return true; }
            if (m.IsNegativeOne() && (n.IsInfinity())) { r = One; return true; }
            if (m.IsPureFractional())
            {
                if (n.IsPositiveInfinity()) { r = Zero; return true; }
                if (n.IsNegativeInfinity()) { r = PositiveInfinity; return true; }
            }
            else
            {
                if (n.IsPositiveInfinity()) { r = PositiveInfinity; return true; }
                if (n.IsNegativeInfinity()) { r = Zero; return true; }
            }

            r = Zero;
            return false;
        }

        /// <summary>
        /// 计算2的定点数次幂
        /// </summary>
        public Fixed64 Pow2(Fixed64 x)
        {
            var s = x.IsNegative();
            x = x.Abs();

            var integer = x.ToInt();
            x = x.Fractional();

            // 计算 e^r 的泰勒级数展开
            var ter = One;
            var sum = One;
            var idx = 0;
            while (ter != Zero)
            {
                idx++;
                ter = ter * x * Ln2 / new Fixed64(idx);
                sum += ter;
            }

            sum = FromRaw(sum.rawvalue << integer);
            if (s) sum = sum.Reciprocal();

            return sum;
        }

        /// <summary>
        /// 是否为2的幂
        /// </summary>
        public bool IsPowerOfTwo()
        {
            if (rawvalue <= 0) return false;
            return (rawvalue & (rawvalue - 1)) == 0;
        }

        /// <summary>
        /// 是否为2的幂
        /// </summary>
        public static bool IsPowerOfTwo(Fixed64 value)
        {
            return FMath.IsPowerOfTwo(value);
        }

        /// <summary>
        /// 最接近的2的幂
        /// </summary>
        public Fixed64 ClosestPowerOfTwo()
        {
            if (rawvalue <= 0)
            {
                return One;
            }

            var raw = (UInt128)rawvalue;
            var pos = TOTAL_BITS - 1;

            // 找到最高有效位的位置
            while (pos >= 0 && (raw & ((UInt128)1 << pos)) == 0)
            {
                pos--;
            }

            var k = pos - FRACTIONAL_BITS;
            var lower = (Int128)((UInt128)1 << (k + FRACTIONAL_BITS));

            // 检查上界2^(k+1)是否可表示
            var valid = (k + FRACTIONAL_BITS + 1) < TOTAL_BITS;
            var upper = valid ? (Int128)((UInt128)1 << (k + FRACTIONAL_BITS + 1)) : -1;

            // 比较距离选择最近值
            if (valid)
            {
                var diffLower = rawvalue - lower;
                var diffUpper = upper - rawvalue;

                return (diffLower < diffUpper) ? FromRaw(lower)
                                               : FromRaw(upper);
            }

            return FromRaw(lower);
        }

        /// <summary>
        /// 最接近的2的幂
        /// </summary>
        public static Fixed64 ClosestPowerOfTwo(Fixed64 value)
        {
            return FMath.ClosestPowerOfTwo(value);
        }

        /// <summary>
        /// 下一个2的幂
        /// </summary>
        public Fixed64 NextPowerOfTwo()
        {
            var raw = (UInt128)rawvalue;
            var pos = TOTAL_BITS - 1;

            // 找到最高有效位的位置
            while (pos >= 0 && (raw & ((UInt128)1 << pos)) == 0)
            {
                pos--;
            }

            return FromRaw(Int128.One << (pos + 1));
        }

        /// <summary>
        /// 下一个2的幂
        /// </summary>
        public static Fixed64 NextPowerOfTwo(Fixed64 value)
        {
            return FMath.NextPowerOfTwo(value);
        }
    }
}
