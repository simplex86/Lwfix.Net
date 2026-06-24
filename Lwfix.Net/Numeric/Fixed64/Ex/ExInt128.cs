using System;

namespace SimplexLab.Fixed
{
    internal static class ExInt128
    {
        public static bool IsNaN(this Int128 rawvalue)
        {
            return rawvalue == Fixed64.NaN.rawvalue;
        }

        public static bool IsZero(this Int128 rawvalue)
        {
            return rawvalue == Fixed64.Zero.rawvalue;
        }

        public static bool IsNegativeOne(this Int128 rawvalue)
        {
            return rawvalue == Fixed64.NegativeOne.rawvalue;
        }

        public static bool IsOne(this Int128 rawvalue)
        {
            return rawvalue == Fixed64.One.rawvalue;
        }

        public static bool IsMin(this Int128 rawvalue)
        {
            return rawvalue == Fixed64.MinValue.rawvalue;
        }

        public static bool IsMax(this Int128 rawvalue)
        {
            return rawvalue == Fixed64.MaxValue.rawvalue;
        }

        public static bool IsMinMax(this Int128 rawvalue)
        {
            return rawvalue.IsMin() ||
                   rawvalue.IsMax();
        }

        public static bool IsPositiveInfinity(this Int128 rawvalue)
        {
            return rawvalue == Fixed64.PositiveInfinity.rawvalue;
        }

        public static bool IsNegativeInfinity(this Int128 rawvalue)
        {
            return rawvalue == Fixed64.NegativeInfinity.rawvalue;
        }

        public static bool IsInfinity(this Int128 rawvalue)
        {
            return rawvalue.IsPositiveInfinity() ||
                   rawvalue.IsNegativeInfinity();
        }

        public static bool IsFractional(this Int128 rawvalue)
        {
            return (rawvalue & Fixed64.FRACTIONAL_MASK) != 0;
        }

        public static bool IsPureFractional(this Int128 rawvalue)
        {
            return rawvalue > Fixed64.NegativeOne.rawvalue &&
                   rawvalue < Fixed64.One.rawvalue;
        }
    }
}
