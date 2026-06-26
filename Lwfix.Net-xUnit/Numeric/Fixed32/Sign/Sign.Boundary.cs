using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TSign
    {
        [Fact]
        public void Zero()
        {
            Assert.Equal(0, Fixed32.Sign(Fixed32.Zero));
        }

        [Fact]
        public void One()
        {
            Assert.Equal(1, Fixed32.Sign(Fixed32.One));
        }

        [Fact]
        public void NegativeOne()
        {
            Assert.Equal(-1, Fixed32.Sign(Fixed32.NegativeOne));
        }

        [Fact]
        public void PositiveHalf()
        {
            Assert.Equal(1, Fixed32.Sign(new Fixed32(0.5)));
        }

        [Fact]
        public void NegativeHalf()
        {
            Assert.Equal(-1, Fixed32.Sign(new Fixed32(-0.5)));
        }

        [Fact]
        public void MaxValue()
        {
            Assert.Equal(1, Fixed32.Sign(Fixed32.MaxValue));
        }

        [Fact]
        public void MinValue()
        {
            Assert.Equal(-1, Fixed32.Sign(Fixed32.MinValue));
        }
    }
}
