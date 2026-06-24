using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 减法 - 常规
    /// </summary>
    public partial class TSub
    {
        private const int RANDOM_LOOP_TIMES = 100;

        private readonly static int MIN_NUMBER = Fixed32.MinValue.ToInt() / 2;
        private readonly static int MAX_NUMBER = Fixed32.MaxValue.ToInt() / 2;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < RANDOM_LOOP_TIMES; i++)
            {
                var n1 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(MIN_NUMBER, MAX_NUMBER);
                var n2 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(MIN_NUMBER, MAX_NUMBER);

                var f1 = new Fixed32(n1);
                var f2 = new Fixed32(n2);

                Assert.Equal(n1 - n2, (f1 - f2).ToDouble(), TOLERANCE);
            }
        }
    }
}
