using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Perpendicular_RightUnit()
        {
            var v = new FVector2<Fixed32>(Fixed32.One, Fixed32.Zero);
            var result = FVector2<Fixed32>.Perpendicular(v);
            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Perpendicular_UpUnit()
        {
            var v = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.One);
            var result = FVector2<Fixed32>.Perpendicular(v);
            Assert.Equal(-1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Perpendicular_DotWithOriginalIsZero()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double x = rand.NextDouble() * 200 - 100;
                double y = rand.NextDouble() * 200 - 100;

                var v = new FVector2<Fixed32>(new Fixed32(x), new Fixed32(y));
                var perp = FVector2<Fixed32>.Perpendicular(v);
                var dot = FVector2<Fixed32>.Dot(v, perp);
                Assert.Equal(0.0, dot.ToDouble(), TOLERANCE);
            }
        }
    }
}
