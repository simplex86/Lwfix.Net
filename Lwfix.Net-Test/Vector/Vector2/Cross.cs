using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Cross_PerpendicularVectors()
        {
            var a = new FVector2<Fixed32>(Fixed32.One, Fixed32.Zero);
            var b = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.One);
            var result = FVector2<Fixed32>.Cross(a, b);
            Assert.Equal(1.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Cross_ParallelVectors()
        {
            var a = new FVector2<Fixed32>(Fixed32.One, Fixed32.Zero);
            var b = new FVector2<Fixed32>(Fixed32.One, Fixed32.Zero);
            var result = FVector2<Fixed32>.Cross(a, b);
            Assert.Equal(0.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Cross_RandomCompareWithSystemMath()
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
                var result = FVector2<Fixed32>.Cross(fa, fb);

                double expected = ax * by - ay * bx;
                Assert.Equal(expected, result.ToDouble(), TOLERANCE);
            }
        }
    }
}
