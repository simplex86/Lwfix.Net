using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Quaternion
{
    public partial class TQuaternion
    {
        private const double TOLERANCE = 10e-3;

        [Fact]
        public void Identity_IsCorrect()
        {
            var identity = FQuaternion<Fixed32>.Identity;

            Assert.Equal(0.0, identity.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, identity.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Identity_MultiplyQuaternion_ReturnsSameQuaternion()
        {
            var q = new FQuaternion<Fixed32>(
                new Fixed32(0.1),
                new Fixed32(0.2),
                new Fixed32(0.3),
                new Fixed32(0.9));

            var result = FQuaternion<Fixed32>.Identity * q;

            Assert.Equal(q.X.ToDouble(), result.X.ToDouble(), TOLERANCE);
            Assert.Equal(q.Y.ToDouble(), result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(q.Z.ToDouble(), result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(q.W.ToDouble(), result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Quaternion_MultiplyIdentity_ReturnsSameQuaternion()
        {
            var q = new FQuaternion<Fixed32>(
                new Fixed32(0.1),
                new Fixed32(0.2),
                new Fixed32(0.3),
                new Fixed32(0.9));

            var result = q * FQuaternion<Fixed32>.Identity;

            Assert.Equal(q.X.ToDouble(), result.X.ToDouble(), TOLERANCE);
            Assert.Equal(q.Y.ToDouble(), result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(q.Z.ToDouble(), result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(q.W.ToDouble(), result.W.ToDouble(), TOLERANCE);
        }
    }
}
