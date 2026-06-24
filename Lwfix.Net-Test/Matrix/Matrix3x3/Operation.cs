using Xunit;
using SimplexLab.Fixed;
using System;

namespace Test.Matrix
{
    public partial class TMatrix3x3
    {
        [Fact]
        public void Add_ElementWise()
        {
            var a = new FMatrix3x3<Fixed32>(
                new Fixed32(1), new Fixed32(2), new Fixed32(3),
                new Fixed32(4), new Fixed32(5), new Fixed32(6),
                new Fixed32(7), new Fixed32(8), new Fixed32(9));

            var b = new FMatrix3x3<Fixed32>(
                new Fixed32(9), new Fixed32(8), new Fixed32(7),
                new Fixed32(6), new Fixed32(5), new Fixed32(4),
                new Fixed32(3), new Fixed32(2), new Fixed32(1));

            var result = a + b;

            Assert.Equal(10.0, result.M11.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.M12.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.M13.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.M21.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.M22.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.M23.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.M31.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.M32.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.M33.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Subtract_ElementWise()
        {
            var a = new FMatrix3x3<Fixed32>(
                new Fixed32(10), new Fixed32(20), new Fixed32(30),
                new Fixed32(40), new Fixed32(50), new Fixed32(60),
                new Fixed32(70), new Fixed32(80), new Fixed32(90));

            var b = new FMatrix3x3<Fixed32>(
                new Fixed32(1), new Fixed32(2), new Fixed32(3),
                new Fixed32(4), new Fixed32(5), new Fixed32(6),
                new Fixed32(7), new Fixed32(8), new Fixed32(9));

            var result = a - b;

            Assert.Equal(9.0, result.M11.ToDouble(), TOLERANCE);
            Assert.Equal(18.0, result.M12.ToDouble(), TOLERANCE);
            Assert.Equal(27.0, result.M13.ToDouble(), TOLERANCE);
            Assert.Equal(36.0, result.M21.ToDouble(), TOLERANCE);
            Assert.Equal(45.0, result.M22.ToDouble(), TOLERANCE);
            Assert.Equal(54.0, result.M23.ToDouble(), TOLERANCE);
            Assert.Equal(63.0, result.M31.ToDouble(), TOLERANCE);
            Assert.Equal(72.0, result.M32.ToDouble(), TOLERANCE);
            Assert.Equal(81.0, result.M33.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Add_IdentityAndZero_IsIdentity()
        {
            var identity = FMatrix3x3<Fixed32>.Identity;
            var zero = FMatrix3x3<Fixed32>.Zero;
            var result = identity + zero;

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
        public void Subtract_IdentityFromSelf_IsZero()
        {
            var identity = FMatrix3x3<Fixed32>.Identity;
            var result = identity - identity;

            Assert.Equal(0.0, result.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M21.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M32.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.M33.ToDouble(), TOLERANCE);
        }
    }
}
