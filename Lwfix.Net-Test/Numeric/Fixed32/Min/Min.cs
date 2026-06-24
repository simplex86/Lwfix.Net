using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TMin
    {
        private readonly static List<double[]> normal_numbers =
        [
            [65.506, 24.1307],
            [-16.3083, -28.2577],
            [15.5667775, -11.09],
            [44.92, -98.7889],
            [-631299.896, 909539.5],
            [-660730.7668, -788217.62],
            [395260.552, 610316.5062],
            [230618.374, -840202.24]
        ];
        private const double TOLERANCE = 10e-7;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f1 = new Fixed32(n[0]);
                var f2 = new Fixed32(n[1]);

                Assert.Equal(Math.Min(n[0], n[1]), Fixed32.Min(f1, f2).ToDouble(), TOLERANCE);
            }
        }
    }
}
