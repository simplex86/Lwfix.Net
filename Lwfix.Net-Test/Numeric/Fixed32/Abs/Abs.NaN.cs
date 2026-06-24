using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TAbs
    {
        [Fact]
        public void NaN()
        {
            Assert.True(double.IsNaN(Math.Abs(double.NaN)));
            Assert.True(Fixed32.IsNaN(Fixed32.Abs(Fixed32.NaN)));
        }
    }
}
