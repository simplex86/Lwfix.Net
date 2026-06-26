using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TPow64
    {
        private readonly static List<double[]> normal_numbers =
        [
            [5.506, 4.13],
            [-16.308, -3.25],
            [15.5, -1.09],
            [44.92, -4.78],
            [-312.896, 2.5],
            [-66.7668, -3.62],
            [3.552, 2.52],
            [1.374, -8.24]
        ];
        private const double TOLERANCE = 10e-5;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var fb = new Fixed64(n[0]);
                var fe = new Fixed64(n[1]);

                Assert.Equal(Math.Pow(n[0], n[1]), Fixed64.Pow(fb, fe).ToDouble(), TOLERANCE);
            }
        }

        [Fact]
        public void PowInt()
        {
            // 整数次幂
            var cases = new (double b, int e)[]
            {
                (2.0, 10),
                (3.0, 5),
                (1.5, 3),
                (2.0, -3),
                (5.0, 0),
                (-2.0, 4),
                (-2.0, 3),
            };

            foreach (var (b, e) in cases)
            {
                var fb = new Fixed64(b);
                Assert.Equal(Math.Pow(b, e), Fixed64.Pow(fb, e).ToDouble(), TOLERANCE);
            }
        }
    }
}
