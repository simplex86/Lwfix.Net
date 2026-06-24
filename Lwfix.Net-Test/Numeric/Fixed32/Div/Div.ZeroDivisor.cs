using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TDiv
    {
        [Fact]
        public void ZeroDivisor()
        {
            // Positive number divided by zero should be PositiveInfinity or NaN
            var posOverZero = new Fixed32(5) / Fixed32.Zero;
            Assert.True(posOverZero.IsPositiveInfinity() || posOverZero.IsNaN());

            // Zero divided by zero should be NaN
            var zeroOverZero = Fixed32.Zero / Fixed32.Zero;
            Assert.True(zeroOverZero.IsNaN());

            // Negative number divided by zero should be NegativeInfinity or NaN
            var negOverZero = new Fixed32(-5) / Fixed32.Zero;
            Assert.True(negOverZero.IsNegativeInfinity() || negOverZero.IsNaN());
        }
    }
}
