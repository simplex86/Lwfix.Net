using Xunit;
using SimplexLab.Fixed;
using System;

namespace LwfixTest.Fixed.Matrix
{
    public partial class TMatrix4x4
    {
        [Fact]
        public void Inverse_Identity_IsIdentity()
        {
            var identity = FMatrix4x4<Fixed32>.Identity;
            var inverse = FMatrix4x4<Fixed32>.Inverse(identity);

            Assert.Equal(1.0, inverse.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M14.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, inverse.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M24.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M32.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, inverse.M33.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M34.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M41.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M42.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M43.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, inverse.M44.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Inverse_MatrixTimesInverse_IsIdentity()
        {
            // Diagonal matrix with known inverse
            var m = new FMatrix4x4<Fixed32>(
                new Fixed32(2), new Fixed32(0), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(3), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(4), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(0), new Fixed32(5));

            var inverse = FMatrix4x4<Fixed32>.Inverse(m);
            var product = m * inverse;

            Assert.Equal(1.0, product.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M14.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, product.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M24.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M32.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, product.M33.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M34.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M41.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M42.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M43.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, product.M44.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Inverse_KnownDiagonalMatrix()
        {
            var m = new FMatrix4x4<Fixed32>(
                new Fixed32(2), new Fixed32(0), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(4), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(5), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(0), new Fixed32(10));

            var inverse = FMatrix4x4<Fixed32>.Inverse(m);

            Assert.Equal(0.5, inverse.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M14.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M21.ToDouble(), TOLERANCE);
            Assert.Equal(0.25, inverse.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M24.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M32.ToDouble(), TOLERANCE);
            Assert.Equal(0.2, inverse.M33.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M34.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M41.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M42.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M43.ToDouble(), TOLERANCE);
            Assert.Equal(0.1, inverse.M44.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Inverse_InstanceProperty_MatchesStatic()
        {
            var m = new FMatrix4x4<Fixed32>(
                new Fixed32(2), new Fixed32(0), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(3), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(4), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(0), new Fixed32(5));

            var staticResult = FMatrix4x4<Fixed32>.Inverse(m);
            var instanceResult = m.Inversed;

            Assert.Equal(staticResult.M11.ToDouble(), instanceResult.M11.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M22.ToDouble(), instanceResult.M22.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M33.ToDouble(), instanceResult.M33.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M44.ToDouble(), instanceResult.M44.ToDouble(), TOLERANCE);
        }
    }
}
