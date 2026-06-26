using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TCast
    {
        private const double TOLERANCE = 10e-7;

        [Fact]
        public void FromRawToRawRoundtrip()
        {
            var value = Fixed32.FromRaw(12345);
            Assert.Equal(12345, Fixed32.ToRaw(value));
        }

        [Fact]
        public void NewFromInt()
        {
            var value = new Fixed32(5);
            Assert.Equal(5.0, value.ToDouble());
        }

        [Fact]
        public void NewFromDouble()
        {
            var value = new Fixed32(3.5);
            Assert.Equal(3.5, value.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ToInt_Truncation()
        {
            Assert.Equal(5, new Fixed32(5.7).ToInt());
        }

        [Fact]
        public void ToInt_NegativeTruncation()
        {
            // ToInt uses arithmetic right shift, which floors toward negative infinity
            Assert.Equal(-6, new Fixed32(-5.7).ToInt());
        }

        [Fact]
        public void Integral()
        {
            Assert.Equal(3.0, new Fixed32(3.7).Integral().ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Fractional()
        {
            Assert.Equal(0.7, new Fixed32(3.7).Fractional().ToDouble(), TOLERANCE);
        }

        [Fact]
        public void NaN_ToDouble()
        {
            Assert.Equal(double.NaN, Fixed32.NaN.ToDouble());
        }

        [Fact]
        public void PositiveInfinity_ToDouble()
        {
            Assert.Equal(double.PositiveInfinity, Fixed32.PositiveInfinity.ToDouble());
        }
    }
}
