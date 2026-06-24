using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 除法 - 溢出
    /// </summary>
    public partial class TDiv
    {
        private readonly static List<double[]> overflow_numbers =
        [
            [2133786646.16, 0.05],
            [1404748062.296, -0.1],
            [-1374526073.04, -0.01],
        ];

        [Fact]
        public void Overflow()
        {
            foreach (var n in overflow_numbers)
            {
                var u = new Fixed32(n[0]);
                var v = new Fixed32(n[1]);

                Assert.True(Fixed32.IsInfinity(u / v));
            }
        }
    }
}
