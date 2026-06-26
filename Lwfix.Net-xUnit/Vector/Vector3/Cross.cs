using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Cross_XAndY_ReturnsZ()
        {
            var a = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var b = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var result = FVector3<Fixed32>.Cross(a, b);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Cross_YAndX_ReturnsNegativeZ()
        {
            var a = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var b = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.Cross(a, b);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(-1.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Cross_ParallelVectors_ReturnsZero()
        {
            var a = new FVector3<Fixed32>(new Fixed32(2), new Fixed32(3), new Fixed32(4));
            var b = new FVector3<Fixed32>(new Fixed32(4), new Fixed32(6), new Fixed32(8));
            var result = FVector3<Fixed32>.Cross(a, b);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
