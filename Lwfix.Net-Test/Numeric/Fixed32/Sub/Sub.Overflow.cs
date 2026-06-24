using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 减法 - 溢出
    /// </summary>
    public partial class TSub
    {
        private readonly static List<double[]> overflow_numbers =
        [
            [-2133786646.16, 2116943553.32],
            [-1404748062.296, 1102082233.9],
            [-1374526073.04, 1269291313.069],
            [2133786646.16, -2116943553.32],
            [1404748062.296, -1102082233.9],
            [1374526073.04, -1269291313.069],
        ];

        [Fact]
        public void Overflow()
        {
            foreach (var n in overflow_numbers)
            {
                var u = new Fixed32(n[0]);
                var v = new Fixed32(n[1]);

                Assert.True(Fixed32.IsInfinity(u - v));
            }
        }
    }
}
