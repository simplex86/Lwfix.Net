using Xunit;
using SimplexLab.Lwfix;
using System;

namespace SimplexLab.Lwfix.Test.Matrix
{
    public partial class TMatrix4x4
    {
        [Fact]
        public void Transpose_Identity_IsIdentity()
        {
            var identity = FMatrix4x4<Fixed32>.Identity;
            var transposed = FMatrix4x4<Fixed32>.Transpose(identity);

            Assert.Equal(1.0, transposed.M11.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M12.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M13.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M14.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M21.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, transposed.M22.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M23.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M24.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M31.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M32.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, transposed.M33.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M34.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M41.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M42.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, transposed.M43.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, transposed.M44.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Transpose_KnownMatrix()
        {
            // | 1  2  3  4 |^T   | 1  5  9  13 |
            // | 5  6  7  8 |   =  | 2  6  10 14 |
            // | 9  10 11 12|      | 3  7  11 15 |
            // | 13 14 15 16|      | 4  8  12 16 |
            var m = new FMatrix4x4<Fixed32>(
                new Fixed32(1),  new Fixed32(2),  new Fixed32(3),  new Fixed32(4),
                new Fixed32(5),  new Fixed32(6),  new Fixed32(7),  new Fixed32(8),
                new Fixed32(9),  new Fixed32(10), new Fixed32(11), new Fixed32(12),
                new Fixed32(13), new Fixed32(14), new Fixed32(15), new Fixed32(16));

            var result = FMatrix4x4<Fixed32>.Transpose(m);

            Assert.Equal(1.0,  result.M11.ToDouble(), TOLERANCE);
            Assert.Equal(5.0,  result.M12.ToDouble(), TOLERANCE);
            Assert.Equal(9.0,  result.M13.ToDouble(), TOLERANCE);
            Assert.Equal(13.0, result.M14.ToDouble(), TOLERANCE);
            Assert.Equal(2.0,  result.M21.ToDouble(), TOLERANCE);
            Assert.Equal(6.0,  result.M22.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.M23.ToDouble(), TOLERANCE);
            Assert.Equal(14.0, result.M24.ToDouble(), TOLERANCE);
            Assert.Equal(3.0,  result.M31.ToDouble(), TOLERANCE);
            Assert.Equal(7.0,  result.M32.ToDouble(), TOLERANCE);
            Assert.Equal(11.0, result.M33.ToDouble(), TOLERANCE);
            Assert.Equal(15.0, result.M34.ToDouble(), TOLERANCE);
            Assert.Equal(4.0,  result.M41.ToDouble(), TOLERANCE);
            Assert.Equal(8.0,  result.M42.ToDouble(), TOLERANCE);
            Assert.Equal(12.0, result.M43.ToDouble(), TOLERANCE);
            Assert.Equal(16.0, result.M44.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Transpose_DoubleTranspose_IsOriginal()
        {
            var m = new FMatrix4x4<Fixed32>(
                new Fixed32(1),  new Fixed32(2),  new Fixed32(3),  new Fixed32(4),
                new Fixed32(5),  new Fixed32(6),  new Fixed32(7),  new Fixed32(8),
                new Fixed32(9),  new Fixed32(10), new Fixed32(11), new Fixed32(12),
                new Fixed32(13), new Fixed32(14), new Fixed32(15), new Fixed32(16));

            var doubleTransposed = FMatrix4x4<Fixed32>.Transpose(FMatrix4x4<Fixed32>.Transpose(m));

            Assert.Equal(m.M11.ToDouble(), doubleTransposed.M11.ToDouble(), TOLERANCE);
            Assert.Equal(m.M12.ToDouble(), doubleTransposed.M12.ToDouble(), TOLERANCE);
            Assert.Equal(m.M13.ToDouble(), doubleTransposed.M13.ToDouble(), TOLERANCE);
            Assert.Equal(m.M14.ToDouble(), doubleTransposed.M14.ToDouble(), TOLERANCE);
            Assert.Equal(m.M21.ToDouble(), doubleTransposed.M21.ToDouble(), TOLERANCE);
            Assert.Equal(m.M22.ToDouble(), doubleTransposed.M22.ToDouble(), TOLERANCE);
            Assert.Equal(m.M23.ToDouble(), doubleTransposed.M23.ToDouble(), TOLERANCE);
            Assert.Equal(m.M24.ToDouble(), doubleTransposed.M24.ToDouble(), TOLERANCE);
            Assert.Equal(m.M31.ToDouble(), doubleTransposed.M31.ToDouble(), TOLERANCE);
            Assert.Equal(m.M32.ToDouble(), doubleTransposed.M32.ToDouble(), TOLERANCE);
            Assert.Equal(m.M33.ToDouble(), doubleTransposed.M33.ToDouble(), TOLERANCE);
            Assert.Equal(m.M34.ToDouble(), doubleTransposed.M34.ToDouble(), TOLERANCE);
            Assert.Equal(m.M41.ToDouble(), doubleTransposed.M41.ToDouble(), TOLERANCE);
            Assert.Equal(m.M42.ToDouble(), doubleTransposed.M42.ToDouble(), TOLERANCE);
            Assert.Equal(m.M43.ToDouble(), doubleTransposed.M43.ToDouble(), TOLERANCE);
            Assert.Equal(m.M44.ToDouble(), doubleTransposed.M44.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Transpose_InstanceProperty_MatchesStatic()
        {
            var m = new FMatrix4x4<Fixed32>(
                new Fixed32(1),  new Fixed32(2),  new Fixed32(3),  new Fixed32(4),
                new Fixed32(5),  new Fixed32(6),  new Fixed32(7),  new Fixed32(8),
                new Fixed32(9),  new Fixed32(10), new Fixed32(11), new Fixed32(12),
                new Fixed32(13), new Fixed32(14), new Fixed32(15), new Fixed32(16));

            var staticResult = FMatrix4x4<Fixed32>.Transpose(m);
            var instanceResult = m.Transposed;

            Assert.Equal(staticResult.M11.ToDouble(), instanceResult.M11.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M12.ToDouble(), instanceResult.M12.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M13.ToDouble(), instanceResult.M13.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M14.ToDouble(), instanceResult.M14.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M21.ToDouble(), instanceResult.M21.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M22.ToDouble(), instanceResult.M22.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M23.ToDouble(), instanceResult.M23.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M24.ToDouble(), instanceResult.M24.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M31.ToDouble(), instanceResult.M31.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M32.ToDouble(), instanceResult.M32.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M33.ToDouble(), instanceResult.M33.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M34.ToDouble(), instanceResult.M34.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M41.ToDouble(), instanceResult.M41.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M42.ToDouble(), instanceResult.M42.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M43.ToDouble(), instanceResult.M43.ToDouble(), TOLERANCE);
            Assert.Equal(staticResult.M44.ToDouble(), instanceResult.M44.ToDouble(), TOLERANCE);
        }
    }
}
