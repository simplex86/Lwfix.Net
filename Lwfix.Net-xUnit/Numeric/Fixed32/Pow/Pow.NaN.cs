using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    /// <summary>
    /// 幂运算 - NaN与特殊值
    /// </summary>
    public partial class TPow
    {
        [Fact]
        public void NaN()
        {
            // Pow(NaN, 2) is NaN
            Assert.True(Fixed32.IsNaN(Fixed32.Pow(Fixed32.NaN, new Fixed32(2))));

            // Pow(2, NaN) is NaN
            Assert.True(Fixed32.IsNaN(Fixed32.Pow(new Fixed32(2), Fixed32.NaN)));

            // Pow(NaN, NaN) is NaN
            Assert.True(Fixed32.IsNaN(Fixed32.Pow(Fixed32.NaN, Fixed32.NaN)));

            // Pow(PositiveInfinity, 1) is PositiveInfinity
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.Pow(Fixed32.PositiveInfinity, Fixed32.One)));

            // Pow(NegativeInfinity, 1) is NegativeInfinity
            Assert.True(Fixed32.IsNegativeInfinity(Fixed32.Pow(Fixed32.NegativeInfinity, Fixed32.One)));

            // Pow(1, PositiveInfinity) = 1
            Assert.Equal(1.0, Fixed32.Pow(Fixed32.One, Fixed32.PositiveInfinity).ToDouble(), TOLERANCE);

            // Pow(1, NegativeInfinity) = 1
            Assert.Equal(1.0, Fixed32.Pow(Fixed32.One, Fixed32.NegativeInfinity).ToDouble(), TOLERANCE);

            // Pow(0, 0) = 1 (convention)
            Assert.Equal(1.0, Fixed32.Pow(Fixed32.Zero, Fixed32.Zero).ToDouble(), TOLERANCE);

            // Pow(anything, 0) = 1
            Assert.Equal(1.0, Fixed32.Pow(new Fixed32(5), Fixed32.Zero).ToDouble(), TOLERANCE);
            Assert.Equal(1.0, Fixed32.Pow(new Fixed32(-3), Fixed32.Zero).ToDouble(), TOLERANCE);

            // Pow(0, negative) = PositiveInfinity
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.Pow(Fixed32.Zero, new Fixed32(-1))));

            // Pow(negative, 0.5) is NaN (imaginary result)
            Assert.True(Fixed32.IsNaN(Fixed32.Pow(new Fixed32(-2), new Fixed32(0.5))));

            // Pow(2, -1) = 0.5
            Assert.Equal(0.5, Fixed32.Pow(new Fixed32(2), new Fixed32(-1)).ToDouble(), TOLERANCE);
        }
    }
}
