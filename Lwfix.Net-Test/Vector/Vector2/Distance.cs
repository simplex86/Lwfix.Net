using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Distance_KnownValues()
        {
            var a = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            var b = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));
            var result = FVector2<Fixed32>.Distance(a, b);
            Assert.Equal(5.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Distance_SamePoint()
        {
            var a = new FVector2<Fixed32>(new Fixed32(5), new Fixed32(7));
            var result = FVector2<Fixed32>.Distance(a, a);
            Assert.Equal(0.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Distance_Horizontal()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), Fixed32.Zero);
            var b = new FVector2<Fixed32>(new Fixed32(-1), Fixed32.Zero);
            var result = FVector2<Fixed32>.Distance(a, b);
            Assert.Equal(2.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Distance_RandomCompareWithSystemMath()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double ax = rand.NextDouble() * 200 - 100;
                double ay = rand.NextDouble() * 200 - 100;
                double bx = rand.NextDouble() * 200 - 100;
                double by = rand.NextDouble() * 200 - 100;

                var fa = new FVector2<Fixed32>(new Fixed32(ax), new Fixed32(ay));
                var fb = new FVector2<Fixed32>(new Fixed32(bx), new Fixed32(by));
                var fdist = FVector2<Fixed32>.Distance(fa, fb);

                double expected = Math.Sqrt(Math.Pow(ax - bx, 2) + Math.Pow(ay - by, 2));
                Assert.Equal(expected, fdist.ToDouble(), TOLERANCE);
            }
        }
    }
}
