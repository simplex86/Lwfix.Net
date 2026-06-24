using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Reflect_OffHorizontalSurface()
        {
            var direction = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(-1));
            var normal = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.One);
            var result = FVector2<Fixed32>.Reflect(direction, normal);
            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Reflect_WithNormalizedNormal()
        {
            var direction = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(-1));
            var normal = new FVector2<Fixed32>(Fixed32.Zero, new Fixed32(1)).Normalized;
            var result = FVector2<Fixed32>.Reflect(direction, normal);
            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Reflect_RandomCompareWithSystemMath()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double dx = rand.NextDouble() * 200 - 100;
                double dy = rand.NextDouble() * 200 - 100;
                double nx = rand.NextDouble() * 200 - 100;
                double ny = rand.NextDouble() * 200 - 100;

                // Normalize the normal
                double nmag = Math.Sqrt(nx * nx + ny * ny);
                if (nmag == 0) continue;
                nx /= nmag;
                ny /= nmag;

                var direction = new FVector2<Fixed32>(new Fixed32(dx), new Fixed32(dy));
                var normal = new FVector2<Fixed32>(new Fixed32(nx), new Fixed32(ny));
                var result = FVector2<Fixed32>.Reflect(direction, normal);

                // System.Math: reflect = direction - 2 * dot(normal, direction) * normal
                double dot = nx * dx + ny * dy;
                double expectedX = dx - 2 * dot * nx;
                double expectedY = dy - 2 * dot * ny;
                Assert.Equal(expectedX, result.X.ToDouble(), TOLERANCE);
                Assert.Equal(expectedY, result.Y.ToDouble(), TOLERANCE);
            }
        }
    }
}
