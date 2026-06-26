namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 立方根
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 计算立方根，使用公式 ∛x = sign * e^(ln(|x|)/3)
        /// </summary>
        public Fixed64 Cbrt()
        {
            if (IsNaN()) return NaN;
            if (IsZero()) return Zero;

            // 负数的立方根：∛(-x) = -∛(x)
            var isNegative = IsNegative();
            var absValue = isNegative ? -this : this;
            var absResult = (absValue.Log() / 3).Exp();
            return isNegative ? -absResult : absResult;
        }

        /// <summary>
        /// 计算立方根
        /// </summary>
        public static Fixed64 Cbrt(Fixed64 n)
        {
            return n.Cbrt();
        }
    }
}
