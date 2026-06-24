using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TClamp
    {
        private readonly static List<double[]> normal_numbers =
        [
            [65.506, 24.1307, 31.2347],
            [-16.3083, -86.05, - 28.2577],
            [-631299.896, 95260.5, 909539.5],
            [-660730.7668, -906813.78, - 788217.62],
            [395260.552, 610316.5062, 979026.3581],
            [230618.374, -840202.24, 906813.78]
        ];
        private const double TOLERANCE = 10e-7;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f1 = new Fixed32(n[0]);
                var f2 = new Fixed32(n[1]);
                var f3 = new Fixed32(n[2]);

                Assert.Equal(Math.Clamp(n[0], n[1], n[2]), Fixed32.Clamp(f1, f2, f3).ToDouble(), TOLERANCE);
            }
        }
    }
}
