using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TAbs
    {
        [Fact]
        public void Boundary()
        {
            // Abs(Zero) = Zero
            Assert.Equal(Fixed32.Zero, Fixed32.Abs(Fixed32.Zero));

            // Abs(PositiveInfinity) = PositiveInfinity
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.Abs(Fixed32.PositiveInfinity)));

            // Abs(MinValue) = MaxValue (two's complement edge case)
            Assert.Equal(Fixed32.MaxValue, Fixed32.Abs(Fixed32.MinValue));

            // Abs(One) = One
            Assert.Equal(Fixed32.One, Fixed32.Abs(Fixed32.One));

            // Abs(NegativeOne) = One
            Assert.Equal(Fixed32.One, Fixed32.Abs(Fixed32.NegativeOne));

            // Abs(Half) = Half
            Assert.Equal(Fixed32.Half, Fixed32.Abs(Fixed32.Half));
        }
    }
}
