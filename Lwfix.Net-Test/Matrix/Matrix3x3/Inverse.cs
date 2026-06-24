using Xunit;
using SimplexLab.Fixed;
using System;

namespace Test.Matrix
{
    public partial class TMatrix3x3
    {
        [Fact]
        public void Inverse_Identity_IsIdentity()
        {
            var identity = FMatrix3x3<Fixed32>.Identity;
            var inverse = FMatrix3x3<Fixed32>.Inverse(identity);

            Assert.Equal(1.0, inverse.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, inverse.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M32.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, inverse.M33.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Inverse_MatrixTimesInverse_IsIdentity()
        {
            // | 1 2 3 |
            // | 0 1 4 |
            // | 5 6 0 |  det = 1(0-24) - 2(0-20) + 3(0-5) = -24 + 40 - 15 = 1
            var m = new FMatrix3x3<Fixed32>(
                new Fixed32(1), new Fixed32(2), new Fixed32(3),
                new Fixed32(0), new Fixed32(1), new Fixed32(4),
                new Fixed32(5), new Fixed32(6), new Fixed32(0));

            var inverse = FMatrix3x3<Fixed32>.Inverse(m);
            var product = m * inverse;

            Assert.Equal(1.0, product.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, product.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, product.M32.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, product.M33.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Inverse_KnownMatrix()
        {
            // | 2 0 0 |
            // | 0 3 0 |  inverse = | 0.5  0    0  |
            // | 0 0 4 |             | 0    1/3  0  |
            //                        | 0    0    0.25|
            var m = new FMatrix3x3<Fixed32>(
                new Fixed32(2), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(3), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(4));

            var inverse = FMatrix3x3<Fixed32>.Inverse(m);

            Assert.Equal(0.5, inverse.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0 / 3.0, inverse.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, inverse.M32.ToDouble(), TOLERANCE);
            Assert.Equal(0.25, inverse.M33.ToDouble(), TOLERANCE);
        }
    }
}
