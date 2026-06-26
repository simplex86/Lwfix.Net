using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TCeil
    {
        [Fact]
        public void Zero()
        {
            var result = Fixed32.Ceil(new Fixed32(0));
            Assert.Equal(0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void One()
        {
            var result = Fixed32.Ceil(new Fixed32(1));
            Assert.Equal(1, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MinusOne()
        {
            var result = Fixed32.Ceil(new Fixed32(-1));
            Assert.Equal(-1, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void PositiveHalf()
        {
            var result = Fixed32.Ceil(new Fixed32(0.5));
            Assert.Equal(1, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void NegativeHalf()
        {
            var result = Fixed32.Ceil(new Fixed32(-0.5));
            Assert.Equal(0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void JustAboveOne()
        {
            var result = Fixed32.Ceil(new Fixed32(1.001));
            Assert.Equal(2, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void JustBelowMinusOne()
        {
            var result = Fixed32.Ceil(new Fixed32(-1.999));
            Assert.Equal(-1, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MaxValue()
        {
            var result = Fixed32.Ceil(Fixed32.MaxValue);
            // Ceil(MaxValue) = (MaxValue + One).Floor() 溢出到PositiveInfinity
            Assert.True(result.IsPositiveInfinity());
        }

        [Fact]
        public void MinValue()
        {
            var result = Fixed32.Ceil(Fixed32.MinValue);
            // MinValue有小数位, Ceil = (MinValue + One).Floor() = -2147483647.0
            Assert.Equal(-2147483647.0, result.ToDouble(), TOLERANCE);
        }
    }
}
