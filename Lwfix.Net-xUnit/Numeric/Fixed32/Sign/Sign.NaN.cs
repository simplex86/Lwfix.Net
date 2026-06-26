using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TSign
    {
        [Fact]
        public void NaN()
        {
            Assert.Throws<ArithmeticException>(() => Fixed32.Sign(Fixed32.NaN));
        }

        [Fact]
        public void Infinity()
        {
            Assert.Equal(1, Fixed32.Sign(Fixed32.PositiveInfinity));
            Assert.Equal(-1, Fixed32.Sign(Fixed32.NegativeInfinity));
        }
    }
}
