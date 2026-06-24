using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TPow
    {
        private const int LOOP_TIMES = 100;
        private const int B_MIN_NUMBER = -10;
        private const int B_MAX_NUMBER = 10;
        private const int E_MIN_NUMBER = -4;
        private const int E_MAX_NUMBER = 4;

        [Fact]
        public void Random()
        {
            for (int i = 0; i < LOOP_TIMES; i++)
            {
                var b1 = System.Random.Shared.Next(B_MIN_NUMBER, B_MAX_NUMBER);
                var b2 = System.Random.Shared.Next(B_MIN_NUMBER, 0);
                var b3 = System.Random.Shared.NextDouble() * b1;
                var b4 = System.Random.Shared.NextDouble() * b2;
                var e1 = System.Random.Shared.Next(1, E_MAX_NUMBER);
                var e2 = System.Random.Shared.Next(E_MIN_NUMBER, 0);
                var e3 = System.Random.Shared.NextDouble() * e1;
                var e4 = System.Random.Shared.NextDouble() * e2;

                var fb1 = new Fixed32(b1);
                var fb2 = new Fixed32(b2);
                var fb3 = new Fixed32(b3);
                var fb4 = new Fixed32(b4);
                var fe1 = new Fixed32(e1);
                var fe2 = new Fixed32(e2);
                var fe3 = new Fixed32(e3);
                var fe4 = new Fixed32(e4);

                Assert.Equal(Math.Pow(b1, e1), Fixed32.Pow(fb1, e1).ToDouble(),  TOLERANCE);
                Assert.Equal(Math.Pow(b1, e1), Fixed32.Pow(fb1, fe1).ToDouble(), TOLERANCE);
                Assert.Equal(Math.Pow(b1, e2), Fixed32.Pow(fb1, e2).ToDouble(),  TOLERANCE);
                Assert.Equal(Math.Pow(b1, e2), Fixed32.Pow(fb1, fe2).ToDouble(), TOLERANCE);
                Assert.Equal(Math.Pow(b1, e3), Fixed32.Pow(fb1, fe3).ToDouble(), TOLERANCE);
                Assert.Equal(Math.Pow(b1, e4), Fixed32.Pow(fb1, fe4).ToDouble(), TOLERANCE);
            }
        }
    }
}
