using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector2
    {
        private const double TOLERANCE = 10e-5;

        [Fact]
        public void Dot_KnownValues()
        {
            var a = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));
            var b = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(0));
            var result = FVector2<Fixed32>.Dot(a, b);
            Assert.Equal(3.0, result.ToDouble(), TOLERANCE);

            var c = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(2));
            var d = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));
            var result2 = FVector2<Fixed32>.Dot(c, d);
            Assert.Equal(11.0, result2.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Dot_PerpendicularVectors()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(0));
            var b = new FVector2<Fixed32>(new Fixed32(0), new Fixed32(1));
            var result = FVector2<Fixed32>.Dot(a, b);
            Assert.Equal(0.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Dot_RandomCompareWithSystemMath()
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
                var fdot = FVector2<Fixed32>.Dot(fa, fb);

                double expected = ax * bx + ay * by;
                Assert.Equal(expected, fdot.ToDouble(), TOLERANCE);
            }
        }
    }
}
