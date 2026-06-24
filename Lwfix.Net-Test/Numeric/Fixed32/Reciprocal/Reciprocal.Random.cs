using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TReciprocal
    {
        private const int LOOP_TIMES = 100;
        private const int NEGATIVE_MIN_NUMBER = -1000000;
        private const int POSITIVE_MAX_NUMBER = 1000000;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < LOOP_TIMES; i++)
            {
                var n1 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(1, POSITIVE_MAX_NUMBER);
                var n2 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(NEGATIVE_MIN_NUMBER, 0);
                var f1 = new Fixed32(n1);
                var f2 = new Fixed32(n2);

                Assert.Equal(1.0 / n1, Fixed32.Reciprocal(f1).ToDouble(), TOLERANCE);
                Assert.Equal(1.0 / n2, Fixed32.Reciprocal(f2).ToDouble(), TOLERANCE);
            }
        }
    }
}
