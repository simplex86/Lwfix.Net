using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TClamp
    {
        [Fact]
        public void NaN()
        {
            var n = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue);
            var f = new Fixed32(n);

            Assert.Equal(Math.Clamp(n, double.NaN, double.MaxValue), Fixed32.Clamp(f, Fixed32.NaN, Fixed32.MaxValue).ToDouble(), TOLERANCE);
            Assert.Equal(Math.Clamp(n, double.MinValue, double.NaN), Fixed32.Clamp(f, Fixed32.MinValue, Fixed32.NaN).ToDouble(), TOLERANCE);

            Assert.True(Fixed32.IsNaN(Fixed32.Clamp(Fixed32.NaN, Fixed32.MinValue, Fixed32.MaxValue)));
        }
    }
}
