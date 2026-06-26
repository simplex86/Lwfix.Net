using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TAbs64
    {
        private readonly static List<double> normal_numbers =
        [
            31.23479409344165,
            -86.05775761556997,
            -906813.7862607994,
            979026.3581211731,
            -100909.43195481248,
        ];
        private const double TOLERANCE = 10e-7;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f = new Fixed64(n);
                Assert.Equal(Math.Abs(n), FMath.Abs(f).ToDouble(), TOLERANCE);
            }
        }

        [Fact]
        public void Boundary()
        {
            // Abs(Zero) = Zero
            Assert.Equal(Fixed64.Zero, Fixed64.Abs(Fixed64.Zero));

            // Abs(PositiveInfinity) = PositiveInfinity
            Assert.True(Fixed64.IsPositiveInfinity(Fixed64.Abs(Fixed64.PositiveInfinity)));

            // Abs(MinValue) = MaxValue (two's complement edge case)
            Assert.Equal(Fixed64.MaxValue, Fixed64.Abs(Fixed64.MinValue));

            // Abs(One) = One
            Assert.Equal(Fixed64.One, Fixed64.Abs(Fixed64.One));

            // Abs(NegativeOne) = One
            Assert.Equal(Fixed64.One, Fixed64.Abs(Fixed64.NegativeOne));

            // Abs(Half) = Half
            Assert.Equal(Fixed64.Half, Fixed64.Abs(Fixed64.Half));
        }
    }
}
