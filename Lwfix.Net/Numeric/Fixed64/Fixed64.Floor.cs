namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 向下取整
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 向下取整，计算小于或等于当前值的最大整数
        /// </summary>
        public Fixed64 Floor()
        {
            if (IsNaN()) return NaN;
            if (IsNegativeInfinity()) return NegativeInfinity;
            if (IsPositiveInfinity()) return PositiveInfinity;

            var result = rawvalue & INTEGRAL_MASK;
            // MinValue的rawvalue & INTEGRAL_MASK会得到NaN的rawvalue，需要特殊处理
            if (result == NaN.rawvalue) return MinValue;

            return FromRaw(result);
        }

        /// <summary>
        /// 向下取整
        /// </summary>
        public static Fixed64 Floor(Fixed64 n)
        {
            return FMath.Floor(n);
        }

        /// <summary>
        /// 向下取整到整数
        /// </summary>
        public int FloorToInt()
        {
            return Floor().ToInt();
        }

        /// <summary>
        /// 向下取整到整数
        /// </summary>
        public static int FloorToInt(Fixed64 n)
        {
            return FMath.FloorToInt(n);
        }
    }
}
