using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void MoveTowards_Halfway()
        {
            var current = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            var target = new FVector2<Fixed32>(new Fixed32(10), Fixed32.Zero);
            var result = FVector2<Fixed32>.MoveTowards(current, target, new Fixed32(5));
            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MoveTowards_OvershootReturnsTarget()
        {
            var current = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            var target = new FVector2<Fixed32>(new Fixed32(3), Fixed32.Zero);
            var result = FVector2<Fixed32>.MoveTowards(current, target, new Fixed32(100));
            Assert.Equal(3.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MoveTowards_SamePointReturnsSamePoint()
        {
            var point = new FVector2<Fixed32>(new Fixed32(5), new Fixed32(7));
            var result = FVector2<Fixed32>.MoveTowards(point, point, new Fixed32(10));
            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(7.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MoveTowards_RandomCompareWithSystemMath()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double cx = rand.NextDouble() * 200 - 100;
                double cy = rand.NextDouble() * 200 - 100;
                double tx = rand.NextDouble() * 200 - 100;
                double ty = rand.NextDouble() * 200 - 100;
                double maxDist = rand.NextDouble() * 50;

                var current = new FVector2<Fixed32>(new Fixed32(cx), new Fixed32(cy));
                var target = new FVector2<Fixed32>(new Fixed32(tx), new Fixed32(ty));
                var result = FVector2<Fixed32>.MoveTowards(current, target, new Fixed32(maxDist));

                double dx = tx - cx;
                double dy = ty - cy;
                double dist = Math.Sqrt(dx * dx + dy * dy);
                double expectedX, expectedY;
                if (dist <= maxDist || dist == 0)
                {
                    expectedX = tx;
                    expectedY = ty;
                }
                else
                {
                    expectedX = cx + dx / dist * maxDist;
                    expectedY = cy + dy / dist * maxDist;
                }
                Assert.Equal(expectedX, result.X.ToDouble(), TOLERANCE);
                Assert.Equal(expectedY, result.Y.ToDouble(), TOLERANCE);
            }
        }
    }
}
