using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 乘法 - 溢出
    /// </summary>
    public partial class TMul
    {
        private readonly static List<double[]> overflow_numbers =
        [
            [13386646.16, 216453.32],
            [-1444062.296, 11008233.9],
            [1374573.04, -12692913.069],
            [-660737.7668, -78820.62],
        ];

        [Fact]
        public void Overflow()
        {
            foreach (var n in overflow_numbers)
            {
                var u = new Fixed32(n[0]);
                var v = new Fixed32(n[1]);

                Assert.True(Fixed32.IsInfinity(u * v));
            }
        }
    }
}
