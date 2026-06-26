using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Quaternion
{
    public partial class TQuaternion
    {
        [Fact]
        public void Inverse_Identity_ReturnsIdentity()
        {
            var result = FQuaternion<Fixed32>.Inverse(FQuaternion<Fixed32>.Identity);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Inverse_MultiplyByOriginal_ReturnsIdentity()
        {
            // q = normalized(1, 2, 3, 4)
            var q = new FQuaternion<Fixed32>(
                new Fixed32(1),
                new Fixed32(2),
                new Fixed32(3),
                new Fixed32(4));

            var normalized = FQuaternion<Fixed32>.Normalize(q);
            var inverse = FQuaternion<Fixed32>.Inverse(normalized);
            var result = normalized * inverse;

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Inverse_KnownQuaternion_ReturnsCorrectInverse()
        {
            // q = (0, 0, sin(45°), cos(45°)) - 90° rotation around Z
            var sin45 = Fixed32.Sin(new Fixed32(45) * Fixed32.DegToRad * Fixed32.Half);
            var cos45 = Fixed32.Cos(new Fixed32(45) * Fixed32.DegToRad * Fixed32.Half);
            var q = new FQuaternion<Fixed32>(Fixed32.Zero, Fixed32.Zero, sin45, cos45);

            var inverse = FQuaternion<Fixed32>.Inverse(q);
            var result = q * inverse;

            // q * Inverse(q) ≈ Identity
            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }
    }
}
