using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TFloor
    {
        [Fact]
        public void Zero()
        {
            var result = Fixed32.Floor(new Fixed32(0));
            Assert.Equal(0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void One()
        {
            var result = Fixed32.Floor(new Fixed32(1));
            Assert.Equal(1, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MinusOne()
        {
            var result = Fixed32.Floor(new Fixed32(-1));
            Assert.Equal(-1, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void PositiveHalf()
        {
            var result = Fixed32.Floor(new Fixed32(0.5));
            Assert.Equal(0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void NegativeHalf()
        {
            var result = Fixed32.Floor(new Fixed32(-0.5));
            Assert.Equal(-1, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void JustBelowTwo()
        {
            var result = Fixed32.Floor(new Fixed32(1.999));
            Assert.Equal(1, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void JustBelowMinusOne()
        {
            var result = Fixed32.Floor(new Fixed32(-1.001));
            Assert.Equal(-2, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MaxValue()
        {
            var result = Fixed32.Floor(Fixed32.MaxValue);
            // MaxValue的rawvalue = 0x7FFFFFFFFFFFFFFE, & INTEGRAL_MASK = 0x7FFFFFFF00000000 = 2147483647.0
            Assert.Equal(2147483647.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MinValue()
        {
            var result = Fixed32.Floor(Fixed32.MinValue);
            // MinValue的rawvalue = 0x8000000000000002, & INTEGRAL_MASK = 0x8000000000000000 = NaN
            // Floor特殊处理返回MinValue本身
            Assert.Equal(Fixed32.MinValue, result);
        }
    }
}
