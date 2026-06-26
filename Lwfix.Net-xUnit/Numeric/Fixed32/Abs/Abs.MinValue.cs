using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
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
