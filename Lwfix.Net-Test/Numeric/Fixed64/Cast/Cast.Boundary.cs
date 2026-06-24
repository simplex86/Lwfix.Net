using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TCast64
    {
        private const double TOLERANCE = 10e-7;

        [Fact]
        public void FromRawToRawRoundtrip()
        {
            var value = Fixed64.FromRaw(12345);
            Assert.Equal((System.Int128)12345, Fixed64.ToRaw(value));
        }

        [Fact]
        public void NewFromInt()
        {
            var value = new Fixed64(5);
            Assert.Equal(5.0, value.ToDouble());
        }

        [Fact]
        public void NewFromDouble()
        {
            var value = new Fixed64(3.5);
            Assert.Equal(3.5, value.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ToInt_Truncation()
        {
            Assert.Equal(5, new Fixed64(5.7).ToInt());
        }

        [Fact]
        public void ToInt_NegativeTruncation()
        {
            // ToInt uses arithmetic right shift, which floors toward negative infinity
            Assert.Equal(-6, new Fixed64(-5.7).ToInt());
        }

        [Fact]
        public void Integral()
        {
            Assert.Equal(3.0, new Fixed64(3.7).Integral().ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Fractional()
        {
            Assert.Equal(0.7, new Fixed64(3.7).Fractional().ToDouble(), TOLERANCE);
        }

        [Fact]
        public void NaN_ToDouble()
        {
            Assert.Equal(double.NaN, Fixed64.NaN.ToDouble());
        }

        [Fact]
        public void PositiveInfinity_ToDouble()
        {
            Assert.Equal(double.PositiveInfinity, Fixed64.PositiveInfinity.ToDouble());
        }

        [Fact]
        public void ToByte()
        {
            Assert.Equal((byte)255, new Fixed64(255).ToByte());
            Assert.Equal((byte)0, new Fixed64(0).ToByte());
        }

        [Fact]
        public void ToShort()
        {
            Assert.Equal((short)1000, new Fixed64(1000).ToShort());
            Assert.Equal((short)-1, new Fixed64(-1).ToShort());
        }

        [Fact]
        public void ToLong()
        {
            Assert.Equal(123456789L, new Fixed64(123456789).ToLong());
            Assert.Equal(-123456789L, new Fixed64(-123456789).ToLong());
        }

        [Fact]
        public void ToFloat()
        {
            Assert.Equal(3.5f, new Fixed64(3.5).ToFloat(), TOLERANCE);
        }

        [Fact]
        public void ExplicitCast_Operators()
        {
            Assert.Equal((byte)5, (byte)new Fixed64(5));
            Assert.Equal((short)5, (short)new Fixed64(5));
            Assert.Equal(5, (int)new Fixed64(5));
            Assert.Equal(5L, (long)new Fixed64(5));
            Assert.Equal(3.5f, (float)new Fixed64(3.5), TOLERANCE);
            Assert.Equal(3.5, (double)new Fixed64(3.5), TOLERANCE);
        }

        [Fact]
        public void ImplicitCast_FromInt()
        {
            Fixed64 value = 5;
            Assert.Equal(5.0, value.ToDouble());
        }
    }
}
