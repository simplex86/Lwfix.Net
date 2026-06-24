using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TCos
    {
        [Fact]
        public void Infinity()
        {
            Assert.True(double.IsNaN(Math.Cos(double.PositiveInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Cos(Fixed32.PositiveInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.FastCos(Fixed32.PositiveInfinity)));

            Assert.True(double.IsNaN(Math.Cos(double.NegativeInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Cos(Fixed32.NegativeInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.FastCos(Fixed32.NegativeInfinity)));
        }
    }
}
