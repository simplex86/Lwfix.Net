using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TCeil
    {
        [Fact]
        public void NaN()
        {
            Assert.True(Fixed32.Ceil(Fixed32.NaN).IsNaN());
        }

        [Fact]
        public void Infinity()
        {
            Assert.True(Fixed32.Ceil(Fixed32.PositiveInfinity).IsPositiveInfinity());
            Assert.True(Fixed32.Ceil(Fixed32.NegativeInfinity).IsNegativeInfinity());
        }
    }
}
