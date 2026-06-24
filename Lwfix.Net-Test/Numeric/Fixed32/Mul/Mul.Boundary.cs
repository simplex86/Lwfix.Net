using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TMul
    {
        [Fact]
        public void Boundary()
        {
            // Mul by zero
            Assert.Equal(Fixed32.Zero, new Fixed32(5) * Fixed32.Zero);

            // Mul by one
            Assert.Equal(5, (new Fixed32(5) * Fixed32.One).ToDouble(), TOLERANCE);

            // Mul by negative one
            Assert.Equal(-5, (new Fixed32(5) * Fixed32.NegativeOne).ToDouble(), TOLERANCE);

            // Mul by two
            Assert.Equal(6, (new Fixed32(3) * Fixed32.Two).ToDouble(), TOLERANCE);

            // Zero * anything should be Zero
            Assert.Equal(Fixed32.Zero, Fixed32.Zero * new Fixed32(123.456));

            // One * One should equal One
            Assert.Equal(Fixed32.One, Fixed32.One * Fixed32.One);

            // NegativeOne * NegativeOne should equal One
            Assert.Equal(Fixed32.One, Fixed32.NegativeOne * Fixed32.NegativeOne);

            // Half * Two should equal One
            Assert.Equal(Fixed32.One, Fixed32.Half * Fixed32.Two);
        }
    }
}
