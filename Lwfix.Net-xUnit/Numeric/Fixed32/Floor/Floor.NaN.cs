using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TFloor
    {
        [Fact]
        public void NaN()
        {
            Assert.True(Fixed32.Floor(Fixed32.NaN).IsNaN());
        }

        [Fact]
        public void Infinity()
        {
            Assert.True(Fixed32.Floor(Fixed32.PositiveInfinity).IsPositiveInfinity());
            Assert.True(Fixed32.Floor(Fixed32.NegativeInfinity).IsNegativeInfinity());
        }
    }
}
