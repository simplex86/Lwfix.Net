namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 数学
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T Clamp<T>(T value, T min, T max) where T : struct, IFixed<T>
        {
            if (value.IsNaN()) return T.NaN;

            if (min.IsNaN()) min = T.NegativeInfinity;
            if (max.IsNaN()) max = T.PositiveInfinity;

            if (min > max)
            {
                throw new System.ArgumentException($"{min} cannot be greater than {max}");
            }

            if (value < min)   return min;
            if (value > max)   return max;

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Clamp01<T>(T value) where T : struct, IFixed<T>
        {
            return Clamp(value, T.Zero, T.One);
        }
    }
}
