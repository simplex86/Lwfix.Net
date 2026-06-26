using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    /// <summary>
    /// 加法 - 边界
    /// </summary>
    public partial class TAdd
    {
        [Fact]
        public void Boundary()
        {
            // Add(Zero, Zero) should equal Zero
            Assert.Equal(Fixed32.Zero, Fixed32.Zero + Fixed32.Zero);

            // Add(One, Zero) should equal One
            Assert.Equal(Fixed32.One, Fixed32.One + Fixed32.Zero);

            // Add(Zero, One) should equal One
            Assert.Equal(Fixed32.One, Fixed32.Zero + Fixed32.One);

            // Add(One, NegativeOne) should equal Zero
            Assert.Equal(Fixed32.Zero, Fixed32.One + Fixed32.NegativeOne);

            // Add(Half, Half) should equal One
            Assert.Equal(Fixed32.One.ToDouble(), (Fixed32.Half + Fixed32.Half).ToDouble(), TOLERANCE);

            // Add(MaxValue, NegativeOne) should be close to MaxValue - 1
            Assert.Equal(Fixed32.MaxValue.ToDouble() - 1, (Fixed32.MaxValue + Fixed32.NegativeOne).ToDouble(), TOLERANCE);

            // Add(MinValue, One) should be close to MinValue + 1
            Assert.Equal(Fixed32.MinValue.ToDouble() + 1, (Fixed32.MinValue + Fixed32.One).ToDouble(), TOLERANCE);
        }
    }
}
