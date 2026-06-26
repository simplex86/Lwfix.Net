using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TMax
    {
        [Fact]
        public void NaN()
        {
            var k = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue);
            var f = new Fixed32(k);

            // Max(NaN, x) = NaN
            Assert.True(Fixed32.IsNaN(Fixed32.Max(Fixed32.NaN, f)));
            // Max(x, NaN) = NaN
            Assert.True(Fixed32.IsNaN(Fixed32.Max(f, Fixed32.NaN)));

            // Max(PositiveInfinity, x) = PositiveInfinity
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.Max(Fixed32.PositiveInfinity, f)));
            // Max(x, PositiveInfinity) = PositiveInfinity
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.Max(f, Fixed32.PositiveInfinity)));

            // Max(NegativeInfinity, x) = x
            Assert.Equal(k, Fixed32.Max(Fixed32.NegativeInfinity, f).ToDouble(), TOLERANCE);
            // Max(x, NegativeInfinity) = x
            Assert.Equal(k, Fixed32.Max(f, Fixed32.NegativeInfinity).ToDouble(), TOLERANCE);

            // Max(MaxValue, MinValue) = MaxValue
            Assert.Equal(Fixed32.MaxValue, Fixed32.Max(Fixed32.MaxValue, Fixed32.MinValue));

            // Max(x, x) = x (equal values)
            Assert.Equal(f, Fixed32.Max(f, f));
        }
    }
}
