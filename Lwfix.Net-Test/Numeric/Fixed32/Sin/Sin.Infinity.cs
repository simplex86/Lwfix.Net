using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TSin
    {
        [Fact]
        public void Infinity()
        {
            Assert.True(double.IsNaN(Math.Sin(double.PositiveInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Sin(Fixed32.PositiveInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.FastSin(Fixed32.PositiveInfinity)));

            Assert.True(double.IsNaN(Math.Sin(double.NegativeInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Sin(Fixed32.NegativeInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.FastSin(Fixed32.NegativeInfinity)));
        }
    }
}
