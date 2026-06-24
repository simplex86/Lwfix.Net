using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Quaternion
{
    public partial class TQuaternion
    {
        [Fact]
        public void Euler_ZeroAngles_ReturnsIdentity()
        {
            var result = FQuaternion<Fixed32>.Euler(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Euler_90DegreesAroundX_ReturnsCorrectRotation()
        {
            // Euler(90, 0, 0) - rotation around X axis
            var result = FQuaternion<Fixed32>.Euler(new Fixed32(90), Fixed32.Zero, Fixed32.Zero);

            // Expected: sin(45°) ≈ 0.7071 for X, cos(45°) ≈ 0.7071 for W
            var expectedSin = Math.Sin(Math.PI / 4);
            var expectedCos = Math.Cos(Math.PI / 4);

            Assert.Equal(expectedSin, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(expectedCos, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Euler_90DegreesAroundY_ReturnsCorrectRotation()
        {
            // Euler(0, 90, 0) - rotation around Y axis
            var result = FQuaternion<Fixed32>.Euler(Fixed32.Zero, new Fixed32(90), Fixed32.Zero);

            var expectedSin = Math.Sin(Math.PI / 4);
            var expectedCos = Math.Cos(Math.PI / 4);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(expectedSin, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(expectedCos, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Euler_90DegreesAroundZ_ReturnsCorrectRotation()
        {
            // Euler(0, 0, 90) - rotation around Z axis
            var result = FQuaternion<Fixed32>.Euler(Fixed32.Zero, Fixed32.Zero, new Fixed32(90));

            var expectedSin = Math.Sin(Math.PI / 4);
            var expectedCos = Math.Cos(Math.PI / 4);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(expectedSin, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(expectedCos, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Euler_Vector3Overload_ReturnsSameAsScalar()
        {
            var euler = new FVector3<Fixed32>(new Fixed32(30), new Fixed32(45), new Fixed32(60));
            var result1 = FQuaternion<Fixed32>.Euler(euler);
            var result2 = FQuaternion<Fixed32>.Euler(euler.X, euler.Y, euler.Z);

            Assert.Equal(result1.X.ToDouble(), result2.X.ToDouble(), TOLERANCE);
            Assert.Equal(result1.Y.ToDouble(), result2.Y.ToDouble(), TOLERANCE);
            Assert.Equal(result1.Z.ToDouble(), result2.Z.ToDouble(), TOLERANCE);
            Assert.Equal(result1.W.ToDouble(), result2.W.ToDouble(), TOLERANCE);
        }
    }
}
