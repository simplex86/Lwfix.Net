using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TTan
    {
        [Fact]
        public void Infinity()
        {
            Assert.True(double.IsNaN(Math.Tan(double.PositiveInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Tan(Fixed32.PositiveInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.FastTan(Fixed32.PositiveInfinity)));

            Assert.True(double.IsNaN(Math.Tan(double.NegativeInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Tan(Fixed32.NegativeInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.FastTan(Fixed32.NegativeInfinity)));
        }
    }
}
