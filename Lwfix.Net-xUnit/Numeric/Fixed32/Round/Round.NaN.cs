using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TRound
    {
        [Fact]
        public void NaN()
        {
            Assert.True(Fixed32.Round(Fixed32.NaN).IsNaN());
        }

        [Fact]
        public void Infinity()
        {
            Assert.True(Fixed32.Round(Fixed32.PositiveInfinity).IsPositiveInfinity());
            Assert.True(Fixed32.Round(Fixed32.NegativeInfinity).IsNegativeInfinity());
        }
    }
}
