using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TSqrt
    {
        [Fact]
        public void NaN()
        {
            Assert.True(Fixed32.Sqrt(Fixed32.NaN).IsNaN());
        }

        [Fact]
        public void Infinity()
        {
            Assert.True(Fixed32.Sqrt(Fixed32.PositiveInfinity).IsPositiveInfinity());
        }

        [Fact]
        public void Zero()
        {
            Assert.Equal(Fixed32.Zero, Fixed32.Sqrt(Fixed32.Zero));
        }

        [Fact]
        public void One()
        {
            Assert.Equal(Fixed32.One, Fixed32.Sqrt(Fixed32.One));
        }

        [Fact]
        public void Negative()
        {
            // Sqrt of negative should return NaN
            Assert.True(Fixed32.Sqrt(new Fixed32(-1.0)).IsNaN());
            Assert.True(Fixed32.Sqrt(Fixed32.NegativeOne).IsNaN());
            Assert.True(Fixed32.Sqrt(Fixed32.MinValue).IsNaN());
        }
    }
}
