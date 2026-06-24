using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TTan
    {
        [Fact]
        public void NaN()
        {
            Assert.True(double.IsNaN(Math.Tan(double.NaN)));
            Assert.True(Fixed32.IsNaN(Fixed32.Tan(Fixed32.NaN)));
            Assert.True(Fixed32.IsNaN(Fixed32.FastTan(Fixed32.NaN)));
        }
    }
}
