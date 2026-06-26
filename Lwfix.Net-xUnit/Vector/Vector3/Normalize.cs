using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Normalize_345_MagnitudeIs1()
        {
            var v = new FVector3<Fixed32>(new Fixed32(3), new Fixed32(4), Fixed32.Zero);
            var result = FVector3<Fixed32>.Normalize(v);

            // (3/5, 4/5, 0)
            Assert.Equal(0.6, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.8, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);

            var mag = result.Magnitude;
            Assert.Equal(1.0, mag.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Normalize_UnitX_ReturnsSame()
        {
            var v = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.Normalize(v);

            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Normalize_Zero_ReturnsZero()
        {
            var v = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.Normalize(v);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
