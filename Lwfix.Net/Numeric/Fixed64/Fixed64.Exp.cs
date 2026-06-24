using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - e的幂
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 计算e的幂，分解 x = k*ln(2) + r，计算 e^r 的泰勒级数后乘以 2^k
        /// </summary>
        public Fixed64 Exp()
        {
            if (PreprocessExp(rawvalue, out var r))
            {
                return r;
            }

            // 分解 x = k * ln(2) + r，其中 |r| ≤ 0.5 * ln(2)
            var k = (this / Ln2).Round();
            var residual = this - k * Ln2;

            // 计算 e^r 的泰勒级数展开
            var ter = One;
            var sum = One;
            var idx = 0;
            while (ter != Zero)
            {
                idx++;
                ter = ter * residual / idx;
                sum += ter;
            }

            var s = k.IsNegative();
            var t = k.Abs().ToInt();

            // t可达63，1<<t会溢出int，直接构造raw值
            var pow = FromRaw(Int128.One << (t + FRACTIONAL_BITS));
            if (s) pow = pow.Reciprocal();

            // e^x = e^(k * ln(2) + r) = 2^k * e^r
            return pow * sum;
        }

        /// <summary>
        /// 计算e的幂
        /// </summary>
        public static Fixed64 Exp(Fixed64 m)
        {
            return m.Exp();
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        private bool PreprocessExp(Int128 n, out Fixed64 r)
        {
            if (n.IsNaN()) { r = NaN; return true; }
            if (n.IsZero()) { r = One; return true; }
            if (n.IsPositiveInfinity()) { r = PositiveInfinity; return true; }
            if (n.IsNegativeInfinity()) { r = Zero; return true; }

            r = Zero;
            return false;
        }
    }
}
