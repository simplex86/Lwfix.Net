using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TMin
    {
        [Fact]
        public void NaN()
        {
            var k = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue);
            var f = new Fixed32(k);

            // Min(NaN, x) = NaN
            Assert.True(Fixed32.IsNaN(Fixed32.Min(Fixed32.NaN, f)));
            // Min(x, NaN) = NaN
            Assert.True(Fixed32.IsNaN(Fixed32.Min(f, Fixed32.NaN)));

            // Min(NegativeInfinity, x) = NegativeInfinity
            Assert.True(Fixed32.IsNegativeInfinity(Fixed32.Min(Fixed32.NegativeInfinity, f)));
            // Min(x, NegativeInfinity) = NegativeInfinity
            Assert.True(Fixed32.IsNegativeInfinity(Fixed32.Min(f, Fixed32.NegativeInfinity)));

            // Min(x, PositiveInfinity) = x
            Assert.Equal(k, Fixed32.Min(f, Fixed32.PositiveInfinity).ToDouble(), TOLERANCE);
            // Min(PositiveInfinity, x) = x
            Assert.Equal(k, Fixed32.Min(Fixed32.PositiveInfinity, f).ToDouble(), TOLERANCE);

            // Min(MaxValue, MinValue) = MinValue
            Assert.Equal(Fixed32.MinValue, Fixed32.Min(Fixed32.MaxValue, Fixed32.MinValue));

            // Min(x, x) = x (equal values)
            Assert.Equal(f, Fixed32.Min(f, f));
        }
    }
}
