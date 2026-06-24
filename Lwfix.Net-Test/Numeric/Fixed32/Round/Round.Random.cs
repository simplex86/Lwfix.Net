using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TRound
    {
        private const int LOOP_TIMES = 100;
        private const int NEGATIVE_MIN_NUMBER = -1000000;
        private const int POSITIVE_MAX_NUMBER = 1000000;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < LOOP_TIMES; i++)
            {
                var n = System.Random.Shared.NextDouble() * System.Random.Shared.Next(NEGATIVE_MIN_NUMBER, POSITIVE_MAX_NUMBER);
                var f = new Fixed32(n);

                Assert.Equal(Math.Round(n), Fixed32.Round(f).ToDouble(), TOLERANCE);
            }
        }
    }
}
