using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
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
