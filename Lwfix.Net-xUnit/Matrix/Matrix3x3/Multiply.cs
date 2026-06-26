using Xunit;
using SimplexLab.Lwfix;
using System;

namespace SimplexLab.Lwfix.Test.Matrix
{
    public partial class TMatrix3x3
    {
        [Fact]
        public void Multiply_IdentityTimesIdentity_IsIdentity()
        {
            var identity = FMatrix3x3<Fixed32>.Identity;
            var result = identity * identity;

            Assert.Equal(1.0, result.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M32.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.M33.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MultiplyPoint_TranslationMatrix()
        {
            var translation = FMatrix3x3<Fixed32>.Translate(new Fixed32(3), new Fixed32(4));
            var point = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(1));

            var result = FMatrix3x3<Fixed32>.MultiplyPoint(point, translation);

            Assert.Equal(4.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MultiplyVector_RotationMatrix()
        {
            // Rotate 90 degrees (PI/2 radians): (1,0) -> (0,1)
            var rotation = FMatrix3x3<Fixed32>.Rotate(new Fixed32(Math.PI / 2));
            var vector = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(0));

            var result = FMatrix3x3<Fixed32>.MultiplyVector(vector, rotation);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MultiplyPoint_Identity_IsSamePoint()
        {
            var identity = FMatrix3x3<Fixed32>.Identity;
            var point = new FVector2<Fixed32>(new Fixed32(5), new Fixed32(7));

            var result = FMatrix3x3<Fixed32>.MultiplyPoint(point, identity);

            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(7.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MultiplyVector_Identity_IsSameVector()
        {
            var identity = FMatrix3x3<Fixed32>.Identity;
            var vector = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));

            var result = FMatrix3x3<Fixed32>.MultiplyVector(vector, identity);

            Assert.Equal(3.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.Y.ToDouble(), TOLERANCE);
        }
    }
}
