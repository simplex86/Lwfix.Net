using Xunit;
using SimplexLab.Fixed;
using System;

namespace LwfixTest.Fixed.Matrix
{
    public partial class TMatrix4x4
    {
        [Fact]
        public void Multiply_IdentityTimesIdentity_IsIdentity()
        {
            var identity = FMatrix4x4<Fixed32>.Identity;
            var result = identity * identity;

            Assert.Equal(1.0, result.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M14.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M24.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M32.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.M33.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M34.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M41.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M42.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M43.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.M44.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MultiplyPoint_TranslationMatrix()
        {
            var translation = FMatrix4x4<Fixed32>.Translate(new Fixed32(3), new Fixed32(4), new Fixed32(5));
            var point = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(1), new Fixed32(1));

            var result = FMatrix4x4<Fixed32>.MultiplyPoint(point, translation);

            Assert.Equal(4.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(6.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MultiplyVector_RotationMatrixZ()
        {
            // Rotate 90 degrees around Z: (1,0,0) -> (0,1,0)
            var rotation = FMatrix4x4<Fixed32>.RotateZ(new Fixed32(Math.PI / 2));
            var vector = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(0), new Fixed32(0));

            var result = FMatrix4x4<Fixed32>.MultiplyVector(vector, rotation);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MultiplyPoint_Identity_IsSamePoint()
        {
            var identity = FMatrix4x4<Fixed32>.Identity;
            var point = new FVector3<Fixed32>(new Fixed32(5), new Fixed32(7), new Fixed32(9));

            var result = FMatrix4x4<Fixed32>.MultiplyPoint(point, identity);

            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(7.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(9.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MultiplyVector_Identity_IsSameVector()
        {
            var identity = FMatrix4x4<Fixed32>.Identity;
            var vector = new FVector3<Fixed32>(new Fixed32(3), new Fixed32(4), new Fixed32(5));

            var result = FMatrix4x4<Fixed32>.MultiplyVector(vector, identity);

            Assert.Equal(3.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
