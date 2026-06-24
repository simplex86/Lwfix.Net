using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 加法 - 溢出
    /// </summary>
    public partial class TAdd
    {
        private readonly static List<double[]> overflow_numbers =
        [
            [2133786646.16, 2116943553.32],
            [1404748062.296, 1102082233.9],
            [1374526073.04, 1269291313.069],
        ];

        [Fact]
        public void Overflow()
        {
            foreach (var n in overflow_numbers)
            {
                var u = n[0];
                var v = n[1];

                var t = new Fixed32(u);
                var w = new Fixed32(v);

                // 正数相加后溢出
                Assert.True(Fixed32.IsPositiveInfinity(t + w));

                var p = new Fixed32(-u);
                var q = new Fixed32(-v);

                // 负数相加后溢出
                Assert.True(Fixed32.IsNegativeInfinity(p + q));
            }
        }
    }
}
