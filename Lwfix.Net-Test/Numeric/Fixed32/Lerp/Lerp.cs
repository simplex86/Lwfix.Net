using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TLerp
    {
        private readonly static List<double[]> normal_numbers =
        [
            [65.506, 24.1307, 0.2347],
            [-16.3083, -86.05, 0.2577],
            [-631299.896, 95260.5, 0.5],
            [-660730.7668, -906813.78, 0.62],
            [39526.552, 61031.5062, 1.35],
            [23061.374, -84020.24, -1.78]
        ];
        private const double TOLERANCE = 10e-5;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var n1 = n[0];
                var n2 = n[1];
                var n3 = n[2];
                var n4 = Math.Clamp(n3, 0, 1);

                var f1 = new Fixed32(n1);
                var f2 = new Fixed32(n2);
                var f3 = new Fixed32(n3);

                Assert.Equal(double.Lerp(n1, n2, n3), Fixed32.Lerp(f1, f2, f3).ToDouble(), TOLERANCE);
                Assert.Equal(double.Lerp(n1, n2, n4), Fixed32.ClampLerp(f1, f2, f3).ToDouble(), TOLERANCE);
            }
        }
    }
}
