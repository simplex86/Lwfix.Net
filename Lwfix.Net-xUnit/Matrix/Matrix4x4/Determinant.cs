using Xunit;
using SimplexLab.Fixed;
using System;

namespace LwfixTest.Fixed.Matrix
{
    public partial class TMatrix4x4
    {
        [Fact]
        public void Determinant_Identity_IsOne()
        {
            var identity = FMatrix4x4<Fixed32>.Identity;
            var det = FMatrix4x4<Fixed32>.Determinant(identity);

            Assert.Equal(1.0, det.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Determinant_Zero_IsZero()
        {
            var zero = FMatrix4x4<Fixed32>.Zero;
            var det = FMatrix4x4<Fixed32>.Determinant(zero);

            Assert.Equal(0.0, det.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Determinant_InstanceProperty_MatchesStatic()
        {
            var m = new FMatrix4x4<Fixed32>(
                new Fixed32(2), new Fixed32(0), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(3), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(4), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(0), new Fixed32(5));

            var staticDet = FMatrix4x4<Fixed32>.Determinant(m);
            var instanceDet = m.Determinanted;

            Assert.Equal(staticDet.ToDouble(), instanceDet.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Determinant_KnownValue()
        {
            // Diagonal matrix: det = 2 * 3 * 4 * 5 = 120
            var m = new FMatrix4x4<Fixed32>(
                new Fixed32(2), new Fixed32(0), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(3), new Fixed32(0), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(4), new Fixed32(0),
                new Fixed32(0), new Fixed32(0), new Fixed32(0), new Fixed32(5));

            var det = FMatrix4x4<Fixed32>.Determinant(m);

            Assert.Equal(120.0, det.ToDouble(), TOLERANCE);
        }
    }
}
