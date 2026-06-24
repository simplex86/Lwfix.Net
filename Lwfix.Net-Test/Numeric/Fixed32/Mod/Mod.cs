using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 取模 - 常规
    /// </summary>
    public partial class TMod
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
        private const double TOLERANCE = 10e-5;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var u = n[0];
                var v = n[1];

                var t = new Fixed32(u);
                var w = new Fixed32(v);

                Assert.Equal(u % v, (t % w).ToDouble(), TOLERANCE);
            }
        }              
    }
}
