using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TClamp
    {
        [Fact]
        public void Infinity()
        {
            // Clamp(PositiveInfinity, 0, 1) = 1
            Assert.Equal(Fixed32.One, Fixed32.Clamp(Fixed32.PositiveInfinity, Fixed32.Zero, Fixed32.One));

            // Clamp(NegativeInfinity, 0, 1) = 0
            Assert.Equal(Fixed32.Zero, Fixed32.Clamp(Fixed32.NegativeInfinity, Fixed32.Zero, Fixed32.One));

            // Clamp(0, NegativeInfinity, PositiveInfinity) = 0
            Assert.Equal(Fixed32.Zero, Fixed32.Clamp(Fixed32.Zero, Fixed32.NegativeInfinity, Fixed32.PositiveInfinity));

            // Clamp(5, 0, PositiveInfinity) = 5
            var five = new Fixed32(5);
            Assert.Equal(five, Fixed32.Clamp(five, Fixed32.Zero, Fixed32.PositiveInfinity));

            // Clamp(-5, NegativeInfinity, 0) = -5
            var negFive = new Fixed32(-5);
            Assert.Equal(negFive, Fixed32.Clamp(negFive, Fixed32.NegativeInfinity, Fixed32.Zero));
        }
    }
}
