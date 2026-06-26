using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TAbs
    {
        [Fact]
        public void MinValue()
        {
            // In two's complement, Abs(MinValue) = MaxValue
            Assert.Equal(Fixed32.MaxValue, Fixed32.Abs(Fixed32.MinValue));
        }
    }
}
