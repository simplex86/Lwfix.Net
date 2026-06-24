using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 除法 - 常规，检验准确性
    /// </summary>
    public partial class TDiv
    {
        private const int RANDOM_LOOP_TIMES = 100;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < RANDOM_LOOP_TIMES; i++)
            {
                var n1 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue);
                var n2 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue);

                var f1 = new Fixed32(n1);
                var f2 = new Fixed32(n2);

                Assert.Equal(n1 / n2, (f1 / f2).ToDouble(), TOLERANCE);
            }
        }
    }
}
