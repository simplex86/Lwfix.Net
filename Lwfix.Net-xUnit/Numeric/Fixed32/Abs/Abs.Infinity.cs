using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TAbs
    {
        [Fact]
        public void Infinity()
        {
            Assert.True(double.IsPositiveInfinity(Math.Abs(double.NegativeInfinity)));
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.Abs(Fixed32.NegativeInfinity)));
        }
    }
}
