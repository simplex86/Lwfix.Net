using Xunit;
using SimplexLab.Lwfix;
using System;

namespace SimplexLab.Lwfix.Test.Matrix
{
    public partial class TMatrix4x4
    {
        private const double TOLERANCE = 10e-3;

        [Fact]
        public void Identity_HasOnesOnDiagonal()
        {
            var identity = FMatrix4x4<Fixed32>.Identity;

            Assert.Equal(1.0, identity.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M14.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, identity.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M24.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M32.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, identity.M33.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M34.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M41.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M42.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, identity.M43.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, identity.M44.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Zero_HasAllZeros()
        {
            var zero = FMatrix4x4<Fixed32>.Zero;

            Assert.Equal(0.0, zero.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M14.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M21.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M24.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M32.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M33.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M34.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M41.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M42.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M43.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, zero.M44.ToDouble(), TOLERANCE);
        }
    }
}
