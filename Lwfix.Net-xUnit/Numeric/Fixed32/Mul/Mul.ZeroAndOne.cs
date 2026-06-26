using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TMul
    {
        [Fact]
        public void ZeroAndOne()
        {
            // anything * Zero = Zero
            Assert.Equal(Fixed32.Zero, new Fixed32(5) * Fixed32.Zero);
            Assert.Equal(Fixed32.Zero, new Fixed32(-5) * Fixed32.Zero);

            // anything * One = itself
            Assert.Equal(5.0, (new Fixed32(5) * Fixed32.One).ToDouble(), TOLERANCE);
            Assert.Equal(-5.0, (new Fixed32(-5) * Fixed32.One).ToDouble(), TOLERANCE);

            // Zero * Zero = Zero
            Assert.Equal(Fixed32.Zero, Fixed32.Zero * Fixed32.Zero);

            // One * One = One
            Assert.Equal(Fixed32.One, Fixed32.One * Fixed32.One);

            // NegativeOne * NegativeOne = One
            Assert.Equal(Fixed32.One, Fixed32.NegativeOne * Fixed32.NegativeOne);

            // Half * Two = One
            Assert.Equal(1.0, (Fixed32.Half * Fixed32.Two).ToDouble(), TOLERANCE);
        }
    }
}
