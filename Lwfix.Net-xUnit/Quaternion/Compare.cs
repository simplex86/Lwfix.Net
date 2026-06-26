using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Quaternion
{
    public partial class TQuaternion
    {
        [Fact]
        public void Operator_Equality_IdentityAndIdentity_ReturnsTrue()
        {
            var q1 = FQuaternion<Fixed32>.Identity;
            var q2 = FQuaternion<Fixed32>.Identity;

            Assert.True(q1 == q2);
        }

        [Fact]
        public void Operator_Inequality_IdentityAndIdentity_ReturnsFalse()
        {
            var q1 = FQuaternion<Fixed32>.Identity;
            var q2 = FQuaternion<Fixed32>.Identity;

            Assert.False(q1 != q2);
        }

        [Fact]
        public void Operator_Equality_DifferentQuaternions_ReturnsFalse()
        {
            var q1 = FQuaternion<Fixed32>.Identity;
            var q2 = new FQuaternion<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);

            Assert.False(q1 == q2);
        }

        [Fact]
        public void Operator_Inequality_DifferentQuaternions_ReturnsTrue()
        {
            var q1 = FQuaternion<Fixed32>.Identity;
            var q2 = new FQuaternion<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);

            Assert.True(q1 != q2);
        }

        [Fact]
        public void Operator_Equality_SameInstance_ReturnsTrue()
        {
            // Identity is exactly representable in fixed-point
            var q = FQuaternion<Fixed32>.Identity;
            var copy = q;

            Assert.True(q == copy);
        }

        [Fact]
        public void Operator_Equality_SameRotation_ReturnsTrue()
        {
            // Two identical quaternion values should be equal
            // Note: IsEqualUsingDot checks dot == 1 exactly,
            // so only exactly-representable unit quaternions pass ==
            var q = FQuaternion<Fixed32>.Identity;
            var same = new FQuaternion<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero, Fixed32.One);

            Assert.True(q == same);
        }

        [Fact]
        public void Equals_Object_SameQuaternion_ReturnsTrue()
        {
            var q1 = FQuaternion<Fixed32>.Identity;
            object q2 = FQuaternion<Fixed32>.Identity;

            Assert.True(q1.Equals(q2));
        }

        [Fact]
        public void Equals_Object_DifferentQuaternion_ReturnsFalse()
        {
            var q1 = FQuaternion<Fixed32>.Identity;
            object q2 = new FQuaternion<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);

            Assert.False(q1.Equals(q2));
        }

        [Fact]
        public void Equals_Object_Null_ReturnsFalse()
        {
            var q = FQuaternion<Fixed32>.Identity;

            Assert.False(q.Equals(null));
        }
    }
}
