using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Quaternion
{
    public partial class TQuaternion
    {
        [Fact]
        public void Dot_IdentityIdentity_ReturnsOne()
        {
            var dot = FQuaternion<Fixed32>.Dot(
                FQuaternion<Fixed32>.Identity,
                FQuaternion<Fixed32>.Identity);

            Assert.Equal(1.0, dot.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Dot_KnownQuaternions_ReturnsCorrectValue()
        {
            // q1 = (1, 0, 0, 0), q2 = (0, 1, 0, 0)
            // Dot = 0*0 + 1*1 + 0*0 + 0*0 = 1
            var q1 = new FQuaternion<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var q2 = new FQuaternion<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero, Fixed32.Zero);

            var dot = FQuaternion<Fixed32>.Dot(q1, q2);

            Assert.Equal(0.0, dot.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Dot_SameQuaternion_ReturnsSquaredMagnitude()
        {
            // q = (1, 2, 3, 4), Dot(q, q) = 1+4+9+16 = 30
            var q = new FQuaternion<Fixed32>(
                new Fixed32(1),
                new Fixed32(2),
                new Fixed32(3),
                new Fixed32(4));

            var dot = FQuaternion<Fixed32>.Dot(q, q);

            Assert.Equal(30.0, dot.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Dot_OrthogonalQuaternions_ReturnsZero()
        {
            // q1 = (1, 0, 0, 0), q2 = (0, 0, 0, 1)
            // Dot = 1*0 + 0*0 + 0*0 + 0*1 = 0
            var q1 = new FQuaternion<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var q2 = new FQuaternion<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero, Fixed32.One);

            var dot = FQuaternion<Fixed32>.Dot(q1, q2);

            Assert.Equal(0.0, dot.ToDouble(), TOLERANCE);
        }
    }
}
