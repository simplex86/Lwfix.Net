using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 常量
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        #region values

        private static readonly Fixed64 S_ADDITIVE_IDENTITY = new Fixed64(0);
        private static readonly Fixed64 S_MULTIPLICATIVE_IDENTITY = new Fixed64(1);
        private static readonly Fixed64 S_ZERO = new Fixed64(0);
        private static readonly Fixed64 S_HALF = FromRaw(Int128.One << 63);
        private static readonly Fixed64 S_ONE = new Fixed64(1);
        private static readonly Fixed64 S_NEGATIVE_ONE = new Fixed64(-1);
        private static readonly Fixed64 S_TWO = new Fixed64(2);
        private static readonly Fixed64 S_LN2 = FromRaw((Int128)(0.6931471805599453 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_LN10 = FromRaw((Int128)(2.302585092994046 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_NaN = FromRaw(Int128.MinValue);
        private static readonly Fixed64 S_EPSILON = FromRaw(EPSILON_VALUE);
        private static readonly Fixed64 S_E = FromRaw((Int128)(2.718281828459045 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_PI = FromRaw((Int128)(3.141592653589793 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_HALF_PI = FromRaw((Int128)(1.5707963267948966 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_QUARTER_PI = FromRaw((Int128)(0.7853981633974483 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_TWO_PI = FromRaw((Int128)(6.283185307179586 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_TPN1 = FromRaw((Int128)(0.1 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_TPN2 = FromRaw((Int128)(0.01 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_TPN3 = FromRaw((Int128)(0.001 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_TPN4 = FromRaw((Int128)(0.0001 * FRACTIONAL_MULTIPLIER + 0.5));
        private static readonly Fixed64 S_N180 = new Fixed64(180);
        private static readonly Fixed64 S_N360 = new Fixed64(360);
        private static readonly Fixed64 S_POSITIVE_INFINITY = FromRaw(Int128.MaxValue);
        private static readonly Fixed64 S_NEGATIVE_INFINITY = FromRaw(Int128.MinValue + 1);

        #endregion

        #region properties

        public static Fixed64 MaxValue => FromRaw(S_MAX_RAW_VALUE);
        public static Fixed64 MinValue => FromRaw(S_MIN_RAW_VALUE);
        public static Fixed64 AdditiveIdentity => S_ADDITIVE_IDENTITY;
        public static Fixed64 MultiplicativeIdentity => S_MULTIPLICATIVE_IDENTITY;
        public static Fixed64 Zero => S_ZERO;
        public static Fixed64 Half => S_HALF;
        public static Fixed64 One => S_ONE;
        public static Fixed64 NegativeOne => S_NEGATIVE_ONE;
        public static Fixed64 Two => S_TWO;
        public static Fixed64 Ln2 => S_LN2;
        public static Fixed64 Ln10 => S_LN10;
        public static Fixed64 NaN => S_NaN;
        public static Fixed64 Epsilon => S_EPSILON;
        public static Fixed64 E => S_E;
        public static Fixed64 PI => S_PI;
        public static Fixed64 Half_PI => S_HALF_PI;
        public static Fixed64 Quarter_PI => S_QUARTER_PI;
        public static Fixed64 Two_PI => S_TWO_PI;
        public static Fixed64 TPN1 => S_TPN1;
        public static Fixed64 TPN2 => S_TPN2;
        public static Fixed64 TPN3 => S_TPN3;
        public static Fixed64 TPN4 => S_TPN4;
        public static Fixed64 N180 => S_N180;
        public static Fixed64 N360 => S_N360;
        public static Fixed64 PositiveInfinity => S_POSITIVE_INFINITY;
        public static Fixed64 NegativeInfinity => S_NEGATIVE_INFINITY;

        #endregion
    }
}
