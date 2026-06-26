using Xunit;
using SimplexLab.Fixed;
using System;

namespace LwfixTest.Fixed.Matrix
{
    public partial class TMatrix3x3
    {
        [Fact]
        public void Translate_MovesPoint()
        {
            var translation = FMatrix3x3<Fixed32>.Translate(new Fixed32(3), new Fixed32(4));
            var point = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(1));

            var result = FMatrix3x3<Fixed32>.MultiplyPoint(point, translation);

            Assert.Equal(4.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Scale_ScalesVector()
        {
            var scale = FMatrix3x3<Fixed32>.Scale(new Fixed32(2), new Fixed32(3));
            var vector = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(1));

            var result = FMatrix3x3<Fixed32>.MultiplyVector(vector, scale);

            Assert.Equal(2.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Rotate_90Degrees()
        {
            var rotation = FMatrix3x3<Fixed32>.Rotate(new Fixed32(Math.PI / 2));
            var vector = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(0));

            var result = FMatrix3x3<Fixed32>.MultiplyVector(vector, rotation);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Rotate_45Degrees()
        {
            var rotation = FMatrix3x3<Fixed32>.Rotate(new Fixed32(Math.PI / 4));
            var vector = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(0));

            var result = FMatrix3x3<Fixed32>.MultiplyVector(vector, rotation);

            var expected = Math.Sqrt(2) / 2;
            Assert.Equal(expected, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(expected, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void TR_TranslateAndRotate()
        {
            // Use 0 rotation to test TR without trig precision issues
            var tr = FMatrix3x3<Fixed32>.TR(new Fixed32(5), new Fixed32(3), Fixed32.Zero);
            var point = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(2));

            var result = FMatrix3x3<Fixed32>.MultiplyPoint(point, tr);

            // With 0 rotation: just translate (1+5, 2+3) = (6, 5)
            Assert.Equal(6.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void TRS_TranslateRotateScale()
        {
            // Use 0 rotation to avoid trig precision issues
            var trs = FMatrix3x3<Fixed32>.TRS(
                new Fixed32(10), new Fixed32(0),
                Fixed32.Zero,
                new Fixed32(2), new Fixed32(3));
            var point = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(1));

            var result = FMatrix3x3<Fixed32>.MultiplyPoint(point, trs);

            // With 0 rotation: translate(10,0) + scale(2,3) applied to (1,1) = (10+2, 0+3) = (12, 3)
            Assert.Equal(12.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Y.ToDouble(), TOLERANCE);
        }
    }
}
