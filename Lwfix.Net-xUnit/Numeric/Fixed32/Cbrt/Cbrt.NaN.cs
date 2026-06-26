using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TCbrt
    {
        [Fact]
        public void NaN()
        {
            Assert.True(Fixed32.Cbrt(Fixed32.NaN).IsNaN());
        }

        [Fact]
        public void Zero()
        {
            Assert.Equal(Fixed32.Zero, Fixed32.Cbrt(Fixed32.Zero));
        }

        [Fact]
        public void PositiveInfinity()
        {
            Assert.True(Fixed32.Cbrt(Fixed32.PositiveInfinity).IsPositiveInfinity());
        }

        [Fact]
        public void NegativeInfinity()
        {
            Assert.True(Fixed32.Cbrt(Fixed32.NegativeInfinity).IsNegativeInfinity());
        }

        [Fact]
        public void One()
        {
            Assert.Equal(Fixed32.One, Fixed32.Cbrt(Fixed32.One));
        }

        [Fact]
        public void NegativeOne()
        {
            Assert.Equal(Fixed32.NegativeOne, Fixed32.Cbrt(Fixed32.NegativeOne));
        }
    }
}
