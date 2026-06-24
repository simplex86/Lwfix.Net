using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 乘法 - 随机
    /// </summary>
    public partial class TMul
    {
        private const int RANDOM_LOOP_TIMES = 100;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < RANDOM_LOOP_TIMES; i++)
            {
                var n1 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(-10000, 10000);
                var n2 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(-10000, 10000);

                var f1 = new Fixed32(n1);
                var f2 = new Fixed32(n2);

                Assert.Equal(n1 * n2, (f1 * f2).ToDouble(), TOLERANCE);
            }
        }
    }
}
