using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TCos
    {
        private const int LOOP_TIMES = 10000;
        private const int MIN_NUMBER = -3600;
        private const int MAX_NUMBER = 3600;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < LOOP_TIMES; i++)
            {
                var n1 = System.Random.Shared.NextSingle() * System.Random.Shared.Next(MIN_NUMBER, MAX_NUMBER);
                var f1 = new Fixed32(n1);
                var r1 = (n1 / 180) * Math.PI;
                var r2 = Fixed32.DegreeToRadian(f1);

                var c1 = Math.Cos(r1);
                var c2 = Fixed32.Cos(r2);
                var c3 = Fixed32.FastCos(r2);

                Assert.Equal(c1, c2.ToDouble(), TOLERANCE);
                Assert.Equal(c1, c3.ToDouble(), FAST_TOLERANCE);
            }
        }
    }
}
