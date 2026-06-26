namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 向上取整
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 向上取整，计算大于或等于当前值的最小整数
        /// </summary>
        public Fixed64 Ceil()
        {
            if (IsNaN()) return NaN;
            if (IsPositiveInfinity()) return PositiveInfinity;

            return IsFractional() ? (this + One).Floor() : this;
        }

        /// <summary>
        /// 向上取整
        /// </summary>
        public static Fixed64 Ceil(Fixed64 n)
        {
            return FMath.Ceil(n);
        }

        /// <summary>
        /// 向上取整到整数
        /// </summary>
        public int CeilToInt()
        {
            return Ceil().ToInt();
        }

        /// <summary>
        /// 向上取整到整数
        /// </summary>
        public static int CeilToInt(Fixed64 n)
        {
            return FMath.CeilToInt(n);
        }
    }
}
