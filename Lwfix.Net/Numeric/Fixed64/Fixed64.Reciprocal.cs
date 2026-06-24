using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 倒数
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 计算倒数
        /// </summary>
        public Fixed64 Reciprocal()
        {
            if (PreprocessReciprocal(rawvalue, out var r))
            {
                return r;
            }

            return One / this;
        }

        /// <summary>
        /// 计算倒数
        /// </summary>
        public static Fixed64 Reciprocal(Fixed64 n)
        {
            return FMath.Reciprocal(n);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// </summary>
        private static bool PreprocessReciprocal(Int128 n, out Fixed64 r)
        {
            if (n.IsNaN()) { r = NaN; return true; }
            if (n.IsZero()) { r = PositiveInfinity; return true; }
            if (n.IsInfinity()) { r = Zero; return true; }

            r = Zero;
            return false;
        }
    }
}
