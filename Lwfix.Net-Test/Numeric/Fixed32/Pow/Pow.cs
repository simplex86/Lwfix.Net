using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TPow
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
        private const double TOLERANCE = 10e-3;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var fb = new Fixed32(n[0]);
                var fe = new Fixed32(n[1]);

                Assert.Equal(Math.Pow(n[0], n[1]), Fixed32.Pow(fb, fe).ToDouble(),  TOLERANCE);
            }
        }
    }
}
