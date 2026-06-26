namespace SimplexLab.Lwfix
{
    internal static class ExInt64
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsNaN(this long rawvalue)
        {
            return rawvalue == Fixed32.NaN.rawvalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsZero(this long rawvalue)
        {
            return rawvalue == Fixed32.Zero.rawvalue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsNegativeOne(this long rawvalue)
        {
            return rawvalue == Fixed32.NegativeOne.rawvalue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsOne(this long rawvalue)
        {
            return rawvalue == Fixed32.One.rawvalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsMin(this long rawvalue)
        {
            return rawvalue == Fixed32.MinValue.rawvalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsMax(this long rawvalue)
        {
            return rawvalue == Fixed32.MaxValue.rawvalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsMinMax(this long rawvalue)
        {
            return rawvalue.IsMin() ||
                   rawvalue.IsMax();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsPositiveInfinity(this long rawvalue)
        {
            return rawvalue == Fixed32.PositiveInfinity.rawvalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsNegativeInfinity(this long rawvalue)
        {
            return rawvalue == Fixed32.NegativeInfinity.rawvalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsInfinity(this long rawvalue)
        {
            return rawvalue.IsPositiveInfinity() || 
                   rawvalue.IsNegativeInfinity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsFractional(this long rawvalue)
        {
            return (rawvalue & Fixed32.FRACTIONAL_MASK) != 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawvalue"></param>
        /// <returns></returns>
        public static bool IsPureFractional(this long rawvalue)
        {
            return rawvalue > Fixed32.NegativeOne.rawvalue && 
                   rawvalue < Fixed32.One.rawvalue;
        }
    }
}
