using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TSqrt64
    {
        private readonly static List<double> normal_numbers =
        [
            31.23479409344165,
            86.05775761556997,
            906813.7862607994,
            979026.3581211731,
            100909.43195481248,
        ];
        private const double TOLERANCE = 10e-6;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f = new Fixed64(n);
                Assert.Equal(Math.Sqrt(n), Fixed64.Sqrt(f).ToDouble(), TOLERANCE);
            }
        }
    }
}
