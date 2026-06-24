using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Rotate_90Degrees()
        {
            var v = new FVector2<Fixed32>(Fixed32.One, Fixed32.Zero);
            var result = FVector2<Fixed32>.Rotate(v, Fixed32.PI / Fixed32.Two);
            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Rotate_180Degrees()
        {
            var v = new FVector2<Fixed32>(Fixed32.One, Fixed32.Zero);
            var result = FVector2<Fixed32>.Rotate(v, Fixed32.PI);
            Assert.Equal(-1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Rotate_RandomCompareWithSystemMath()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double x = rand.NextDouble() * 200 - 100;
                double y = rand.NextDouble() * 200 - 100;
                double radians = rand.NextDouble() * 2 * Math.PI;

                var v = new FVector2<Fixed32>(new Fixed32(x), new Fixed32(y));
                var result = FVector2<Fixed32>.Rotate(v, new Fixed32(radians));

                double cos = Math.Cos(radians);
                double sin = Math.Sin(radians);
                double expectedX = x * cos - y * sin;
                double expectedY = x * sin + y * cos;
                Assert.Equal(expectedX, result.X.ToDouble(), TOLERANCE);
                Assert.Equal(expectedY, result.Y.ToDouble(), TOLERANCE);
            }
        }
    }
}
