using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Quaternion
{
    public partial class TQuaternion
    {
        [Fact]
        public void Lerp_IdentityToIdentity_ReturnsIdentity()
        {
            var result = FQuaternion<Fixed32>.Lerp(
                FQuaternion<Fixed32>.Identity,
                FQuaternion<Fixed32>.Identity,
                new Fixed32(0.5));

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Lerp_AtZero_ReturnsFrom()
        {
            var from = FQuaternion<Fixed32>.Identity;
            var to = FQuaternion<Fixed32>.Euler(new Fixed32(90), Fixed32.Zero, Fixed32.Zero);

            var result = FQuaternion<Fixed32>.Lerp(from, to, Fixed32.Zero);

            Assert.Equal(from.X.ToDouble(), result.X.ToDouble(), TOLERANCE);
            Assert.Equal(from.Y.ToDouble(), result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(from.Z.ToDouble(), result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(from.W.ToDouble(), result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Lerp_AtOne_ReturnsTo()
        {
            var from = FQuaternion<Fixed32>.Identity;
            var to = FQuaternion<Fixed32>.Euler(new Fixed32(90), Fixed32.Zero, Fixed32.Zero);

            var result = FQuaternion<Fixed32>.Lerp(from, to, Fixed32.One);

            Assert.Equal(to.X.ToDouble(), result.X.ToDouble(), TOLERANCE);
            Assert.Equal(to.Y.ToDouble(), result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(to.Z.ToDouble(), result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(to.W.ToDouble(), result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ClampLerp_ClampsT()
        {
            var from = FQuaternion<Fixed32>.Identity;
            var to = FQuaternion<Fixed32>.Euler(new Fixed32(90), Fixed32.Zero, Fixed32.Zero);

            var result = FQuaternion<Fixed32>.ClampLerp(from, to, new Fixed32(1.5));

            // t is clamped to 1, so result should equal 'to'
            Assert.Equal(to.X.ToDouble(), result.X.ToDouble(), TOLERANCE);
            Assert.Equal(to.Y.ToDouble(), result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(to.Z.ToDouble(), result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(to.W.ToDouble(), result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Slerp_IdentityToIdentity_ReturnsIdentity()
        {
            var result = FQuaternion<Fixed32>.Slerp(
                FQuaternion<Fixed32>.Identity,
                FQuaternion<Fixed32>.Identity,
                new Fixed32(0.5));

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Slerp_IdentityToIdentity_AtZero_ReturnsIdentity()
        {
            var result = FQuaternion<Fixed32>.Slerp(
                FQuaternion<Fixed32>.Identity,
                FQuaternion<Fixed32>.Identity,
                Fixed32.Zero);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Slerp_IdentityToIdentity_AtOne_ReturnsIdentity()
        {
            var result = FQuaternion<Fixed32>.Slerp(
                FQuaternion<Fixed32>.Identity,
                FQuaternion<Fixed32>.Identity,
                Fixed32.One);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Slerp_BetweenDifferentRotations_Interpolates()
        {
            var from = FQuaternion<Fixed32>.Identity;
            var to = FQuaternion<Fixed32>.Euler(new Fixed32(90), Fixed32.Zero, Fixed32.Zero);

            var result = FQuaternion<Fixed32>.Slerp(from, to, new Fixed32(0.5));

            // Result should be between from and to (not equal to either)
            // The angle between from and result should be less than between from and to
            var angleFromTo = FQuaternion<Fixed32>.Angle(from, to);
            var angleFromResult = FQuaternion<Fixed32>.Angle(from, result);

            Assert.True(angleFromResult.ToDouble() < angleFromTo.ToDouble(),
                $"Slerp at t=0.5 should produce a rotation closer to 'from' than 'to'");
        }

        [Fact]
        public void ClampSlerp_ClampsT()
        {
            var from = FQuaternion<Fixed32>.Identity;
            var to = FQuaternion<Fixed32>.Identity;

            var result = FQuaternion<Fixed32>.ClampSlerp(from, to, new Fixed32(-0.5));

            // Both are Identity, so result should be Identity
            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }
    }
}
