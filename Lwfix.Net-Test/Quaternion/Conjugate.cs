using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Quaternion
{
    public partial class TQuaternion
    {
        [Fact]
        public void Conjugate_Identity_ReturnsIdentity()
        {
            var result = FQuaternion<Fixed32>.Conjugate(FQuaternion<Fixed32>.Identity);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Conjugate_KnownQuaternion_ReturnsNegatedXYZ()
        {
            // Conjugate of (1, 2, 3, 4) = (-1, -2, -3, 4)
            var q = new FQuaternion<Fixed32>(
                new Fixed32(1),
                new Fixed32(2),
                new Fixed32(3),
                new Fixed32(4));

            var result = FQuaternion<Fixed32>.Conjugate(q);

            Assert.Equal(-1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(-2.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(-3.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Conjugate_NegativeQuaternion_ReturnsPositiveXYZ()
        {
            // Conjugate of (-1, -2, -3, 4) = (1, 2, -3... wait) = (1, 2, 3, 4)
            var q = new FQuaternion<Fixed32>(
                new Fixed32(-1),
                new Fixed32(-2),
                new Fixed32(-3),
                new Fixed32(4));

            var result = FQuaternion<Fixed32>.Conjugate(q);

            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.W.ToDouble(), TOLERANCE);
        }
    }
}
