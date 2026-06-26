using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    /// <summary>
    /// 减法 - 边界
    /// </summary>
    public partial class TSub
    {
        [Fact]
        public void Boundary()
        {
            // Sub(Zero, Zero) should equal Zero
            Assert.Equal(Fixed32.Zero, Fixed32.Zero - Fixed32.Zero);

            // Sub(One, Zero) should equal One
            Assert.Equal(Fixed32.One, Fixed32.One - Fixed32.Zero);

            // Sub(One, One) should equal Zero
            Assert.Equal(Fixed32.Zero, Fixed32.One - Fixed32.One);

            // Sub(Zero, One) should equal NegativeOne
            Assert.Equal(Fixed32.NegativeOne, Fixed32.Zero - Fixed32.One);

            // Sub(One, NegativeOne) should equal Two
            Assert.Equal(Fixed32.Two, Fixed32.One - Fixed32.NegativeOne);

            // Sub(Half, Half) should equal Zero
            Assert.Equal(Fixed32.Zero.ToDouble(), (Fixed32.Half - Fixed32.Half).ToDouble(), TOLERANCE);
        }
    }
}
