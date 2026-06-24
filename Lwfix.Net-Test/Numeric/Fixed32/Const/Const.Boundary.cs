using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TConst
    {
        private const double TOLERANCE = 10e-7;

        [Fact]
        public void Boundary()
        {
            // Zero.ToDouble() should equal 0.0
            Assert.Equal(0.0, Fixed32.Zero.ToDouble(), TOLERANCE);

            // One.ToDouble() should equal 1.0
            Assert.Equal(1.0, Fixed32.One.ToDouble(), TOLERANCE);

            // PI.ToDouble() should be close to Math.PI
            Assert.Equal(Math.PI, Fixed32.PI.ToDouble(), TOLERANCE);

            // E.ToDouble() should be close to Math.E
            Assert.Equal(Math.E, Fixed32.E.ToDouble(), TOLERANCE);

            // Half.ToDouble() should equal 0.5
            Assert.Equal(0.5, Fixed32.Half.ToDouble(), TOLERANCE);

            // NaN.IsNaN() should be true
            Assert.True(Fixed32.NaN.IsNaN());

            // PositiveInfinity.IsPositiveInfinity() should be true
            Assert.True(Fixed32.PositiveInfinity.IsPositiveInfinity());

            // NegativeInfinity.IsNegativeInfinity() should be true
            Assert.True(Fixed32.NegativeInfinity.IsNegativeInfinity());

            // MaxValue > Zero should be true
            Assert.True(Fixed32.MaxValue > Fixed32.Zero);

            // MinValue < Zero should be true
            Assert.True(Fixed32.MinValue < Fixed32.Zero);
        }
    }
}
