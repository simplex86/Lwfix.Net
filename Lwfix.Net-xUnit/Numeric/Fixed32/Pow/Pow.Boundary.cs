using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TPow
    {
        [Fact]
        public void Boundary()
        {
            // Pow(2, 0) should equal One (any number to the 0th power is 1)
            Assert.Equal(Fixed32.One, Fixed32.Pow(Fixed32.Two, 0));

            // Pow(2, 1) should equal Two
            Assert.Equal(Fixed32.Two, Fixed32.Pow(Fixed32.Two, 1));

            // Pow(2, 2) should equal new Fixed32(4)
            Assert.Equal(new Fixed32(4), Fixed32.Pow(Fixed32.Two, 2));

            // Pow(2, -1) should equal Half
            Assert.Equal(Fixed32.Half, Fixed32.Pow(Fixed32.Two, -1));

            // Pow(0, 0) should equal One (convention)
            Assert.Equal(Fixed32.One, Fixed32.Pow(Fixed32.Zero, 0));

            // Pow(0, 1) should equal Zero
            Assert.Equal(Fixed32.Zero, Fixed32.Pow(Fixed32.Zero, 1));

            // Pow(-1, 2) should equal One
            Assert.Equal(Fixed32.One, Fixed32.Pow(Fixed32.NegativeOne, 2));

            // Pow(-1, 3) should equal NegativeOne
            Assert.Equal(Fixed32.NegativeOne, Fixed32.Pow(Fixed32.NegativeOne, 3));

            // Pow(One, 100) should equal One
            Assert.Equal(Fixed32.One, Fixed32.Pow(Fixed32.One, 100));

            // Pow(Zero, -1) should be PositiveInfinity
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.Pow(Fixed32.Zero, -1)));
        }
    }
}
