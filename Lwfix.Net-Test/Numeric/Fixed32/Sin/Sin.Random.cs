using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TSin
    {
        private const int LOOP_TIMES = 10000;
        private const int MIN_NUMBER = -360;
        private const int MAX_NUMBER = 360;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < LOOP_TIMES; i++)
            {
                var n1 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(MIN_NUMBER, MAX_NUMBER);
                var f1 = new Fixed32(n1);
                var r1 = (n1 / 180) * Math.PI;
                var r2 = Fixed32.DegreeToRadian(f1);

                var s1 = Math.Sin(r1);
                var s2 = Fixed32.Sin(r2);
                var s3 = Fixed32.FastSin(r2);

                Assert.Equal(s1, s2.ToDouble(), TOLERANCE);
                Assert.Equal(s1, s3.ToDouble(), FAST_TOLERANCE);
            }
        }
    }
}
