using Xunit;
using SimplexLab.Lwfix;
using System;

namespace SimplexLab.Lwfix.Test.Matrix
{
    public partial class TMatrix3x3
    {
        [Fact]
        public void Determinant_Identity_IsOne()
        {
            var identity = FMatrix3x3<Fixed32>.Identity;
            var det = FMatrix3x3<Fixed32>.Determinate(identity);

            Assert.Equal(1.0, det.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Determinant_Zero_IsZero()
        {
            var zero = FMatrix3x3<Fixed32>.Zero;
            var det = FMatrix3x3<Fixed32>.Determinate(zero);

            Assert.Equal(0.0, det.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Determinant_InstanceProperty_MatchesStatic()
        {
            var m = new FMatrix3x3<Fixed32>(
                new Fixed32(1), new Fixed32(2), new Fixed32(3),
                new Fixed32(4), new Fixed32(5), new Fixed32(6),
                new Fixed32(7), new Fixed32(8), new Fixed32(0));

            var staticDet = FMatrix3x3<Fixed32>.Determinate(m);
            var instanceDet = m.Determinant;

            Assert.Equal(staticDet.ToDouble(), instanceDet.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Determinant_KnownValue()
        {
            // | 1 2 3 |
            // | 4 5 6 | = 1(5*0-6*8) - 2(4*0-6*7) + 3(4*8-5*7)
            // | 7 8 0 |   = 1(-48) - 2(-42) + 3(-3) = -48 + 84 - 9 = 27
            var m = new FMatrix3x3<Fixed32>(
                new Fixed32(1), new Fixed32(2), new Fixed32(3),
                new Fixed32(4), new Fixed32(5), new Fixed32(6),
                new Fixed32(7), new Fixed32(8), new Fixed32(0));

            var det = FMatrix3x3<Fixed32>.Determinate(m);

            Assert.Equal(27.0, det.ToDouble(), TOLERANCE);
        }
    }
}
