using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void MoveTowards_Halfway_ReturnsMidpoint()
        {
            var current = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var target = new FVector3<Fixed32>(new Fixed32(10), Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.MoveTowards(current, target, new Fixed32(5));

            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MoveTowards_Overshoot_ReturnsTarget()
        {
            var current = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var target = new FVector3<Fixed32>(new Fixed32(10), Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.MoveTowards(current, target, new Fixed32(100));

            Assert.Equal(10.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MoveTowards_AtTarget_StaysAtTarget()
        {
            var current = new FVector3<Fixed32>(new Fixed32(10), Fixed32.Zero, Fixed32.Zero);
            var target = new FVector3<Fixed32>(new Fixed32(10), Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.MoveTowards(current, target, new Fixed32(5));

            Assert.Equal(10.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
