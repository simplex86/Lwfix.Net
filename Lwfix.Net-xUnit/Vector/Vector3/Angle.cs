using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Angle_PerpendicularVectors_Returns90()
        {
            var from = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var to = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var result = FVector3<Fixed32>.Angle(from, to);

            Assert.Equal(90.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Angle_SameVectors_Returns0()
        {
            var from = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var to = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.Angle(from, to);

            Assert.Equal(0.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Angle_OppositeVectors_Returns180()
        {
            var from = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var to = new FVector3<Fixed32>(Fixed32.NegativeOne, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.Angle(from, to);

            Assert.Equal(180.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void SignedAngle_PositiveRotation_ReturnsPositive()
        {
            var from = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var to = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var axis = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.One);
            var result = FVector3<Fixed32>.SignedAngle(from, to, axis);

            // From +X to +Y around +Z is a positive 90 degree rotation
            Assert.Equal(90.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void SignedAngle_NegativeRotation_ReturnsNegative()
        {
            var from = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var to = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var axis = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.One);
            var result = FVector3<Fixed32>.SignedAngle(from, to, axis);

            // From +Y to +X around +Z is a negative 90 degree rotation
            Assert.Equal(-90.0, result.ToDouble(), TOLERANCE);
        }
    }
}
