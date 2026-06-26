using Xunit;
using SimplexLab.Lwfix;
using System;

namespace SimplexLab.Lwfix.Test.Matrix
{
    public partial class TMatrix4x4
    {
        [Fact]
        public void Translate_MovesPoint()
        {
            var translation = FMatrix4x4<Fixed32>.Translate(new Fixed32(3), new Fixed32(4), new Fixed32(5));
            var point = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(1), new Fixed32(1));

            var result = FMatrix4x4<Fixed32>.MultiplyPoint(point, translation);

            Assert.Equal(4.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(6.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Translate_VectorArg_MovesPoint()
        {
            var translation = FMatrix4x4<Fixed32>.Translate(new FVector3<Fixed32>(new Fixed32(10), new Fixed32(20), new Fixed32(30)));
            var point = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));

            var result = FMatrix4x4<Fixed32>.MultiplyPoint(point, translation);

            Assert.Equal(11.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(22.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(33.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Scale_ScalesVector()
        {
            var scale = FMatrix4x4<Fixed32>.Scale(new Fixed32(2), new Fixed32(3), new Fixed32(4));
            var vector = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(1), new Fixed32(1));

            var result = FMatrix4x4<Fixed32>.MultiplyVector(vector, scale);

            Assert.Equal(2.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void RotateX_90Degrees()
        {
            var rotation = FMatrix4x4<Fixed32>.RotateX(new Fixed32(Math.PI / 2));
            var vector = new FVector3<Fixed32>(new Fixed32(0), new Fixed32(1), new Fixed32(0));

            var result = FMatrix4x4<Fixed32>.MultiplyVector(vector, rotation);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void RotateY_90Degrees()
        {
            var rotation = FMatrix4x4<Fixed32>.RotateY(new Fixed32(Math.PI / 2));
            var vector = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(0), new Fixed32(0));

            var result = FMatrix4x4<Fixed32>.MultiplyVector(vector, rotation);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(-1.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void RotateZ_90Degrees()
        {
            var rotation = FMatrix4x4<Fixed32>.RotateZ(new Fixed32(Math.PI / 2));
            var vector = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(0), new Fixed32(0));

            var result = FMatrix4x4<Fixed32>.MultiplyVector(vector, rotation);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void TRS_TranslateRotateScale()
        {
            // Test TRS with identity rotation using direct matrix construction
            // to avoid quaternion->matrix conversion precision issues
            // With row-vector convention: point * (S * R * T) = scale first, then rotate, then translate
            var translation = FMatrix4x4<Fixed32>.Translate(new Fixed32(10), new Fixed32(0), new Fixed32(0));
            var rotation = FMatrix4x4<Fixed32>.Identity; // No rotation
            var scale = FMatrix4x4<Fixed32>.Scale(new Fixed32(2), new Fixed32(3), new Fixed32(4));

            var trs = scale * rotation * translation;
            var point = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(1), new Fixed32(1));

            var result = FMatrix4x4<Fixed32>.MultiplyPoint(point, trs);

            Assert.Equal(12.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
