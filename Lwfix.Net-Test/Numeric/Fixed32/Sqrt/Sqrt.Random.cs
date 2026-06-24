using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TSqrt
    {
        private const int LOOP_TIMES = 10000;
        private const int MAX_NUMBER = 1000000;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < LOOP_TIMES; i++)
            {
                var n1 = System.Random.Shared.Next(1, MAX_NUMBER);
                var n2 = System.Random.Shared.NextDouble() * n1;

                var f1 = new Fixed32(n1);
                var f2 = new Fixed32(n2);

                Assert.Equal(Math.Sqrt(n1), Fixed32.Sqrt(f1).ToDouble(), TOLERANCE);
                Assert.Equal(Math.Sqrt(n2), Fixed32.Sqrt(f2).ToDouble(), TOLERANCE);
            }
        }
    }
}
