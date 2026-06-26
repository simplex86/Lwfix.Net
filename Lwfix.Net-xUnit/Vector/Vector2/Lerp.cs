using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Lerp_Midpoint()
        {
            var a = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            var b = new FVector2<Fixed32>(new Fixed32(10), new Fixed32(10));
            var result = FVector2<Fixed32>.Lerp(a, b, new Fixed32(0.5));
            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Lerp_AtStart()
        {
            var a = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            var b = new FVector2<Fixed32>(new Fixed32(10), new Fixed32(10));
            var result = FVector2<Fixed32>.Lerp(a, b, Fixed32.Zero);
            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Lerp_AtEnd()
        {
            var a = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            var b = new FVector2<Fixed32>(new Fixed32(10), new Fixed32(10));
            var result = FVector2<Fixed32>.Lerp(a, b, Fixed32.One);
            Assert.Equal(10.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ClampLerp_ClampsToMax()
        {
            var a = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            var b = new FVector2<Fixed32>(new Fixed32(10), new Fixed32(10));
            var result = FVector2<Fixed32>.ClampLerp(a, b, new Fixed32(2.0));
            Assert.Equal(10.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ClampLerp_ClampsToMin()
        {
            var a = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            var b = new FVector2<Fixed32>(new Fixed32(10), new Fixed32(10));
            var result = FVector2<Fixed32>.ClampLerp(a, b, new Fixed32(-1.0));
            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Lerp_RandomCompareWithSystemMath()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double ax = rand.NextDouble() * 200 - 100;
                double ay = rand.NextDouble() * 200 - 100;
                double bx = rand.NextDouble() * 200 - 100;
                double by = rand.NextDouble() * 200 - 100;
                double t = rand.NextDouble();

                var fa = new FVector2<Fixed32>(new Fixed32(ax), new Fixed32(ay));
                var fb = new FVector2<Fixed32>(new Fixed32(bx), new Fixed32(by));
                var ft = new Fixed32(t);
                var result = FVector2<Fixed32>.Lerp(fa, fb, ft);

                double expectedX = ax + (bx - ax) * t;
                double expectedY = ay + (by - ay) * t;
                Assert.Equal(expectedX, result.X.ToDouble(), TOLERANCE);
                Assert.Equal(expectedY, result.Y.ToDouble(), TOLERANCE);
            }
        }
    }
}
