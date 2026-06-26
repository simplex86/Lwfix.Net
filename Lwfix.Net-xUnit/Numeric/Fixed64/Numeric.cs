using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public class TNumeric64
    {
        private const int LOOP_TIMES = 100;
        private const double TOLERANCE = 10e-6;

        [Fact]
        public void Normal()
        {
            for (int i = 0; i < LOOP_TIMES; i++)
            {
                // 整数
                var a1 = System.Random.Shared.Next();
                var f1 = new Fixed64(a1);
                Assert.Equal(a1, f1.ToInt());

                // 单精度
                var a2 = System.Random.Shared.NextSingle();
                var f2 = new Fixed64(a2);
                Assert.Equal(a2, f2.ToDouble(), TOLERANCE);

                // 双精度
                var a3 = System.Random.Shared.NextDouble();
                var f3 = new Fixed64(a3);
                Assert.Equal(a3, f3.ToDouble(), TOLERANCE);
            }
        }
    }
}
