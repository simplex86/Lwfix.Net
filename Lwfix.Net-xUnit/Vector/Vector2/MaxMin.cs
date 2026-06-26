using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Max_KnownValues()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(5));
            var b = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(2));
            var result = FVector2<Fixed32>.Max(a, b);
            Assert.Equal(3.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Min_KnownValues()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(5));
            var b = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(2));
            var result = FVector2<Fixed32>.Min(a, b);
            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Max_RandomCompareWithSystemMath()
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
                var result = FVector2<Fixed32>.Max(fa, fb);

                Assert.Equal(Math.Max(ax, bx), result.X.ToDouble(), TOLERANCE);
                Assert.Equal(Math.Max(ay, by), result.Y.ToDouble(), TOLERANCE);
            }
        }

        [Fact]
        public void Min_RandomCompareWithSystemMath()
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
                var result = FVector2<Fixed32>.Min(fa, fb);

                Assert.Equal(Math.Min(ax, bx), result.X.ToDouble(), TOLERANCE);
                Assert.Equal(Math.Min(ay, by), result.Y.ToDouble(), TOLERANCE);
            }
        }
    }
}
