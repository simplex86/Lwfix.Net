using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TExp
    {
        [Fact]
        public void NaN()
        {
            Assert.True(Fixed32.Exp(Fixed32.NaN).IsNaN());
        }

        [Fact]
        public void Infinity()
        {
            Assert.True(Fixed32.Exp(Fixed32.PositiveInfinity).IsPositiveInfinity());
        }

        [Fact]
        public void NegativeInfinity()
        {
            Assert.Equal(Fixed32.Zero, Fixed32.Exp(Fixed32.NegativeInfinity));
        }

        [Fact]
        public void Zero()
        {
            Assert.Equal(Fixed32.One, Fixed32.Exp(Fixed32.Zero));
        }

        [Fact]
        public void One()
        {
            Assert.Equal(Math.E, Fixed32.Exp(Fixed32.One).ToDouble(), TOLERANCE);
        }

        [Fact]
        public void NegativeOne()
        {
            Assert.Equal(1.0 / Math.E, Fixed32.Exp(Fixed32.NegativeOne).ToDouble(), TOLERANCE);
        }
    }
}
