using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TCbrt
    {
        private const int LOOP_TIMES = 100;
        private const double TOLERANCE = 10e-5;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < LOOP_TIMES; i++)
            {
                var n1 = System.Random.Shared.Next(1, 1000000);
                var n2 = System.Random.Shared.NextDouble() * n1;
                var n3 = -n1;
                var n4 = -n2;

                var f1 = new Fixed32(n1);
                var f2 = new Fixed32(n2);
                var f3 = new Fixed32(n3);
                var f4 = new Fixed32(n4);

                Assert.Equal(Math.Cbrt(n1), Fixed32.Cbrt(f1).ToDouble(), TOLERANCE);
                Assert.Equal(Math.Cbrt(n2), Fixed32.Cbrt(f2).ToDouble(), TOLERANCE);
                Assert.Equal(Math.Cbrt(n3), Fixed32.Cbrt(f3).ToDouble(), TOLERANCE);
                Assert.Equal(Math.Cbrt(n4), Fixed32.Cbrt(f4).ToDouble(), TOLERANCE);
            }
        }
    }
}
