using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void RotateTowards_SameDirection_ReturnsSameVector()
        {
            var current = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var target = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.RotateTowards(current, target, new Fixed32(1), new Fixed32(1));

            Assert.Equal(1.0, result.X.ToDouble(), 0.01);
            Assert.Equal(0.0, result.Y.ToDouble(), 0.01);
            Assert.Equal(0.0, result.Z.ToDouble(), 0.01);
        }

        [Fact]
        public void RotateTowards_ZeroRotationDelta_NoDirectionChange()
        {
            // With zero rotation delta, direction should not change
            // maxMagnitudeDelta = 1 keeps the magnitude at 1
            var current = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var target = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var result = FVector3<Fixed32>.RotateTowards(current, target, Fixed32.Zero, new Fixed32(1));

            // Result should still point in X direction (no rotation applied)
            Assert.True(result.X.ToDouble() > 0.5,
                $"Result X should be positive (direction unchanged), but was {result.X.ToDouble()}");
        }

        [Fact]
        public void RotateTowards_LargeDelta_ReachesTarget()
        {
            // With large enough deltas, should reach target direction
            var current = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var target = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var result = FVector3<Fixed32>.RotateTowards(current, target, new Fixed32(Math.PI), new Fixed32(1));

            // Should be close to target direction (0, 1, 0)
            Assert.Equal(0.0, result.X.ToDouble(), 0.1);
            Assert.Equal(1.0, result.Y.ToDouble(), 0.1);
            Assert.Equal(0.0, result.Z.ToDouble(), 0.1);
        }
    }
}
