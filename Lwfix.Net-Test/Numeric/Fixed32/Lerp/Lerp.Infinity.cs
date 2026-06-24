using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 加法 - 极值
    /// </summary>
    public partial class TLerp
    {
        [Fact]
        public void Infinity()
        {
            var amount = System.Random.Shared.NextDouble();
            var famount = new Fixed32(amount);

            Assert.True(double.IsNaN(double.Lerp(double.PositiveInfinity, double.NegativeInfinity, amount)));
            Assert.True(Fixed32.IsNaN(Fixed32.Lerp(Fixed32.PositiveInfinity, Fixed32.NegativeInfinity, famount)));

            Assert.True(double.IsNaN(double.Lerp(double.NegativeInfinity, double.PositiveInfinity, amount)));
            Assert.True(Fixed32.IsNaN(Fixed32.Lerp(Fixed32.NegativeInfinity, Fixed32.PositiveInfinity, famount)));

            Assert.True(double.IsPositiveInfinity(double.Lerp(double.PositiveInfinity, double.PositiveInfinity, amount)));
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.Lerp(Fixed32.PositiveInfinity, Fixed32.PositiveInfinity, famount)));

            Assert.True(double.IsNegativeInfinity(double.Lerp(double.NegativeInfinity, double.NegativeInfinity, amount)));
            Assert.True(Fixed32.IsNegativeInfinity(Fixed32.Lerp(Fixed32.NegativeInfinity, Fixed32.NegativeInfinity, famount)));

            Assert.True(double.IsNaN(double.Lerp(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Lerp(Fixed32.PositiveInfinity, Fixed32.PositiveInfinity, Fixed32.PositiveInfinity)));
            Assert.True(double.IsNaN(double.Lerp(double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Lerp(Fixed32.PositiveInfinity, Fixed32.PositiveInfinity, Fixed32.NegativeInfinity)));

            Assert.True(double.IsNaN(double.Lerp(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Lerp(Fixed32.NegativeInfinity, Fixed32.NegativeInfinity, Fixed32.NegativeInfinity)));
            Assert.True(double.IsNaN(double.Lerp(double.NegativeInfinity, double.NegativeInfinity, double.PositiveInfinity)));
            Assert.True(Fixed32.IsNaN(Fixed32.Lerp(Fixed32.NegativeInfinity, Fixed32.NegativeInfinity, Fixed32.PositiveInfinity)));
        }
    }
}
