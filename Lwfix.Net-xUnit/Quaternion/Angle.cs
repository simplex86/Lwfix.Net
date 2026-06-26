using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Quaternion
{
    public partial class TQuaternion
    {
        [Fact]
        public void Angle_IdentityIdentity_ReturnsZero()
        {
            var angle = FQuaternion<Fixed32>.Angle(
                FQuaternion<Fixed32>.Identity,
                FQuaternion<Fixed32>.Identity);

            Assert.Equal(0.0, angle.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Angle_SameRotation_ReturnsZero()
        {
            var q = FQuaternion<Fixed32>.Identity;

            var angle = FQuaternion<Fixed32>.Angle(q, q);

            Assert.Equal(0.0, angle.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Angle_90DegreeRotation_Returns90()
        {
            // 90° rotation around X
            var q1 = FQuaternion<Fixed32>.Identity;
            var q2 = FQuaternion<Fixed32>.Euler(new Fixed32(90), Fixed32.Zero, Fixed32.Zero);

            var angle = FQuaternion<Fixed32>.Angle(q1, q2);

            Assert.Equal(90.0, angle.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Angle_180DegreeRotation_Returns180()
        {
            var q1 = FQuaternion<Fixed32>.Identity;
            var q2 = FQuaternion<Fixed32>.Euler(new Fixed32(180), Fixed32.Zero, Fixed32.Zero);

            var angle = FQuaternion<Fixed32>.Angle(q1, q2);

            Assert.Equal(180.0, angle.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Angle_Symmetric_ReturnsSameBothWays()
        {
            var q1 = FQuaternion<Fixed32>.Euler(new Fixed32(30), Fixed32.Zero, Fixed32.Zero);
            var q2 = FQuaternion<Fixed32>.Euler(Fixed32.Zero, new Fixed32(45), Fixed32.Zero);

            var angle1 = FQuaternion<Fixed32>.Angle(q1, q2);
            var angle2 = FQuaternion<Fixed32>.Angle(q2, q1);

            Assert.Equal(angle1.ToDouble(), angle2.ToDouble(), TOLERANCE);
        }
    }
}
