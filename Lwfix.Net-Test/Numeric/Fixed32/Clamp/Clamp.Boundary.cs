using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TClamp
    {
        [Fact]
        public void Boundary()
        {
            // Clamp(0.5, 0, 1) = 0.5 (within range)
            var half = new Fixed32(0.5);
            Assert.Equal(0.5, Fixed32.Clamp(half, Fixed32.Zero, Fixed32.One).ToDouble(), TOLERANCE);

            // Clamp(-0.5, 0, 1) = 0 (below min)
            var negHalf = new Fixed32(-0.5);
            Assert.Equal(Fixed32.Zero, Fixed32.Clamp(negHalf, Fixed32.Zero, Fixed32.One));

            // Clamp(1.5, 0, 1) = 1 (above max)
            var oneHalf = new Fixed32(1.5);
            Assert.Equal(Fixed32.One, Fixed32.Clamp(oneHalf, Fixed32.Zero, Fixed32.One));

            // Clamp(0, 0, 1) = 0 (at min boundary)
            Assert.Equal(Fixed32.Zero, Fixed32.Clamp(Fixed32.Zero, Fixed32.Zero, Fixed32.One));

            // Clamp(1, 0, 1) = 1 (at max boundary)
            Assert.Equal(Fixed32.One, Fixed32.Clamp(Fixed32.One, Fixed32.Zero, Fixed32.One));

            // Clamp01(0.5) = 0.5
            Assert.Equal(0.5, Fixed32.Clamp01(half).ToDouble(), TOLERANCE);

            // Clamp01(-0.5) = 0
            Assert.Equal(Fixed32.Zero, Fixed32.Clamp01(negHalf));

            // Clamp01(1.5) = 1
            Assert.Equal(Fixed32.One, Fixed32.Clamp01(oneHalf));
        }
    }
}
