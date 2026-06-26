using Xunit;
using SimplexLab.Lwfix;
using System;

namespace SimplexLab.Lwfix.Test.Matrix
{
    public partial class TMatrix3x3
    {
        [Fact]
        public void Transpose_Identity_IsIdentity()
        {
            var identity = FMatrix3x3<Fixed32>.Identity;
            var transposed = FMatrix3x3<Fixed32>.Transpose(identity);

            Assert.Equal(1.0, transposed.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, transposed.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M32.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, transposed.M33.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Transpose_KnownMatrix()
        {
            // | 1 2 3 |^T   | 1 4 7 |
            // | 4 5 6 |   =  | 2 5 8 |
            // | 7 8 9 |      | 3 6 9 |
            var m = new FMatrix3x3<Fixed32>(
                new Fixed32(1), new Fixed32(2), new Fixed32(3),
                new Fixed32(4), new Fixed32(5), new Fixed32(6),
                new Fixed32(7), new Fixed32(8), new Fixed32(9));

            var result = FMatrix3x3<Fixed32>.Transpose(m);

            Assert.Equal(1.0, result.M11.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.M12.ToDouble(), TOLERANCE);
            Assert.Equal(7.0, result.M13.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.M21.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.M22.ToDouble(), TOLERANCE);
            Assert.Equal(8.0, result.M23.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.M31.ToDouble(), TOLERANCE);
            Assert.Equal(6.0, result.M32.ToDouble(), TOLERANCE);
            Assert.Equal(9.0, result.M33.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Transpose_DoubleTranspose_IsOriginal()
        {
            var m = new FMatrix3x3<Fixed32>(
                new Fixed32(1), new Fixed32(2), new Fixed32(3),
                new Fixed32(4), new Fixed32(5), new Fixed32(6),
                new Fixed32(7), new Fixed32(8), new Fixed32(9));

            var doubleTransposed = FMatrix3x3<Fixed32>.Transpose(FMatrix3x3<Fixed32>.Transpose(m));

            Assert.Equal(m.M11.ToDouble(), doubleTransposed.M11.ToDouble(), TOLERANCE);
            Assert.Equal(m.M12.ToDouble(), doubleTransposed.M12.ToDouble(), TOLERANCE);
            Assert.Equal(m.M13.ToDouble(), doubleTransposed.M13.ToDouble(), TOLERANCE);
            Assert.Equal(m.M21.ToDouble(), doubleTransposed.M21.ToDouble(), TOLERANCE);
            Assert.Equal(m.M22.ToDouble(), doubleTransposed.M22.ToDouble(), TOLERANCE);
            Assert.Equal(m.M23.ToDouble(), doubleTransposed.M23.ToDouble(), TOLERANCE);
            Assert.Equal(m.M31.ToDouble(), doubleTransposed.M31.ToDouble(), TOLERANCE);
            Assert.Equal(m.M32.ToDouble(), doubleTransposed.M32.ToDouble(), TOLERANCE);
            Assert.Equal(m.M33.ToDouble(), doubleTransposed.M33.ToDouble(), TOLERANCE);
        }
    }
}
