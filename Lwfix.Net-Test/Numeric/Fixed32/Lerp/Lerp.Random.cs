using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TLerp
    {
        private const int LOOP_TIMES = 100;
        private const int MIN_NUMBER = -100000;
        private const int MAX_NUMBER =  100000;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < LOOP_TIMES; i++)
            {
                var n1 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(MIN_NUMBER, MAX_NUMBER);
                var n2 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(MIN_NUMBER, MAX_NUMBER);
                var n3 = System.Random.Shared.NextDouble();

                var f1 = new Fixed32(n1);
                var f2 = new Fixed32(n2);
                var f3 = new Fixed32(n3);

                Assert.Equal(double.Lerp(n1, n2, n3), Fixed32.Lerp(f1, f2, f3).ToDouble(), TOLERANCE);
            }
        }
    }
}
