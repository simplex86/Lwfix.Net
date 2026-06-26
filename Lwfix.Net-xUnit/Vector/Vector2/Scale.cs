using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Scale_KnownValues()
        {
            var a = new FVector2<Fixed32>(new Fixed32(2), new Fixed32(3));
            var b = new FVector2<Fixed32>(new Fixed32(4), new Fixed32(5));
            var result = FVector2<Fixed32>.Scale(a, b);
            Assert.Equal(8.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(15.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Scale_WithOne()
        {
            var a = new FVector2<Fixed32>(Fixed32.One, Fixed32.One);
            var b = new FVector2<Fixed32>(new Fixed32(2), new Fixed32(3));
            var result = FVector2<Fixed32>.Scale(a, b);
            Assert.Equal(2.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Scale_RandomCompareWithSystemMath()
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
                var result = FVector2<Fixed32>.Scale(fa, fb);

                Assert.Equal(ax * bx, result.X.ToDouble(), TOLERANCE);
                Assert.Equal(ay * by, result.Y.ToDouble(), TOLERANCE);
            }
        }
    }
}
