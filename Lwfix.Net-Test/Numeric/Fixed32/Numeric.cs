using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public class TNumeric
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
                var f1 = new Fixed32(a1);
                Assert.Equal(a1, f1.ToInt());

                // 单精度
                var a2 = System.Random.Shared.NextSingle();
                var f2 = new Fixed32(a2);
                Assert.Equal(a2, f2.ToDouble(), TOLERANCE);

                // 双精度
                var a3 = System.Random.Shared.NextDouble();
                var f3 = new Fixed32(a3);
                Assert.Equal(a3, f3.ToDouble(), TOLERANCE);
            }
        }
    }
}
