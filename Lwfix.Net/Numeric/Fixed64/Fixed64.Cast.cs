using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 类型转换
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        public static Fixed64 FromRaw(Int128 value)
        {
            return new Fixed64() { rawvalue = value };
        }

        public static Int128 ToRaw(Fixed64 value)
        {
            return value.rawvalue;
        }

        private static Int128 Int32ToRaw(int value)
        {
            var raw = (Int128)value << FRACTIONAL_BITS;
            return raw < S_MIN_RAW_VALUE ? S_MIN_RAW_VALUE : (raw > S_MAX_RAW_VALUE ? S_MAX_RAW_VALUE : raw);
        }

        private static Int128 DoubleToRaw(double value)
        {
            var raw = (Int128)(value * FRACTIONAL_MULTIPLIER + 0.5);
            return raw < S_MIN_RAW_VALUE ? S_MIN_RAW_VALUE : (raw > S_MAX_RAW_VALUE ? S_MAX_RAW_VALUE : raw);
        }

        public static explicit operator byte(Fixed64 n) => n.ToByte();
        public static explicit operator short(Fixed64 n) => n.ToShort();
        public static explicit operator int(Fixed64 n) => n.ToInt();
        public static explicit operator long(Fixed64 n) => n.ToLong();
        public static explicit operator float(Fixed64 n) => n.ToFloat();
        public static explicit operator double(Fixed64 n) => n.ToDouble();

        public static implicit operator Fixed64(byte n) => new Fixed64(n);
        public static implicit operator Fixed64(short n) => new Fixed64(n);
        public static implicit operator Fixed64(int n) => new Fixed64(n);
        public static explicit operator Fixed64(long n) => new Fixed64((double)n);
        public static explicit operator Fixed64(float n) => new Fixed64(n);
        public static explicit operator Fixed64(double n) => new Fixed64(n);

        public byte ToByte()
        {
            return IsNaN() ? (byte)0 : (byte)ToLong();
        }

        public short ToShort()
        {
            return IsNaN() ? (short)0 : (short)ToLong();
        }

        public int ToInt()
        {
            return IsNaN() ? 0 : (int)ToLong();
        }

        public long ToLong()
        {
            if (IsNaN()) return 0L;
            return (long)(rawvalue >> FRACTIONAL_BITS);
        }

        public float ToFloat()
        {
            if (IsNaN()) return float.NaN;
            return (float)ToDouble();
        }

        public double ToDouble()
        {
            if (IsNaN()) return double.NaN;
            if (IsPositiveInfinity()) return double.PositiveInfinity;
            if (IsNegativeInfinity()) return double.NegativeInfinity;

            return (double)rawvalue / FRACTIONAL_MULTIPLIER;
        }

        public override string ToString()
        {
            if (IsNaN()) return "NaN";
            if (IsPositiveInfinity()) return "+∞";
            if (IsNegativeInfinity()) return "-∞";

            return IsFractional() ? ToDouble().ToString()
                                  : ToLong().ToString();
        }

        public Fixed64 Integral()
        {
            if (IsNaN()) return NaN;

            var result = rawvalue & INTEGRAL_MASK;
            if (result == NaN.rawvalue) return MinValue;

            return FromRaw(result);
        }

        public static Fixed64 Integral(Fixed64 n)
        {
            return n.Integral();
        }

        public Fixed64 Fractional()
        {
            if (IsNaN()) return NaN;
            return FromRaw(rawvalue & FRACTIONAL_MASK);
        }

        public static Fixed64 Fractional(Fixed64 n)
        {
            return n.Fractional();
        }
    }
}
