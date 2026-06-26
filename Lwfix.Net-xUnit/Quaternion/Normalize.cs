using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Quaternion
{
    public partial class TQuaternion
    {
        [Fact]
        public void Normalize_Identity_ReturnsIdentity()
        {
            var result = FQuaternion<Fixed32>.Normalize(FQuaternion<Fixed32>.Identity);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Normalize_NonUnitQuaternion_ReturnsUnitQuaternion()
        {
            // q = (1, 2, 3, 4), |q| = sqrt(30)
            var q = new FQuaternion<Fixed32>(
                new Fixed32(1),
                new Fixed32(2),
                new Fixed32(3),
                new Fixed32(4));

            var result = FQuaternion<Fixed32>.Normalize(q);

            // |result|^2 should be 1
            var dot = FQuaternion<Fixed32>.Dot(result, result);
            Assert.Equal(1.0, dot.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Normalize_NonUnitQuaternion_PreservesDirection()
        {
            // q = (0, 0, 0, 2) - should normalize to (0, 0, 0, 1)
            var q = new FQuaternion<Fixed32>(
                Fixed32.Zero,
                Fixed32.Zero,
                Fixed32.Zero,
                new Fixed32(2));

            var result = FQuaternion<Fixed32>.Normalize(q);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Normalize_ZeroQuaternion_ReturnsIdentity()
        {
            var q = new FQuaternion<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);

            var result = FQuaternion<Fixed32>.Normalize(q);

            // Zero quaternion normalizes to Identity per implementation
            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }
    }
}
