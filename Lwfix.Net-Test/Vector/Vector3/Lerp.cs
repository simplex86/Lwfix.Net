using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Lerp_Midpoint_ReturnsMiddle()
        {
            var a = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var b = new FVector3<Fixed32>(new Fixed32(10), new Fixed32(10), new Fixed32(10));
            var t = Fixed32.Half;
            var result = FVector3<Fixed32>.Lerp(a, b, t);

            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Lerp_AtZero_ReturnsStart()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(10), new Fixed32(20), new Fixed32(30));
            var result = FVector3<Fixed32>.Lerp(a, b, Fixed32.Zero);

            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Lerp_AtOne_ReturnsEnd()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(10), new Fixed32(20), new Fixed32(30));
            var result = FVector3<Fixed32>.Lerp(a, b, Fixed32.One);

            Assert.Equal(10.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(20.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(30.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ClampLerp_TGreaterThanOne_ClampsToEnd()
        {
            var a = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var b = new FVector3<Fixed32>(new Fixed32(10), new Fixed32(10), new Fixed32(10));
            var t = new Fixed32(2.0);
            var result = FVector3<Fixed32>.ClampLerp(a, b, t);

            Assert.Equal(10.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ClampLerp_TLessThanZero_ClampsToStart()
        {
            var a = new FVector3<Fixed32>(new Fixed32(5), new Fixed32(5), new Fixed32(5));
            var b = new FVector3<Fixed32>(new Fixed32(10), new Fixed32(10), new Fixed32(10));
            var t = new Fixed32(-1.0);
            var result = FVector3<Fixed32>.ClampLerp(a, b, t);

            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Slerp_PerpendicularVectors_Midpoint()
        {
            var from = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var to = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var t = Fixed32.Half;
            var result = FVector3<Fixed32>.Slerp(from, to, t);

            // At t=0.5 between perpendicular unit vectors, the result should be
            // at 45 degrees: (sin(45°), cos(45°), 0) normalized
            var expectedComponent = Math.Sin(Math.PI / 4);
            Assert.Equal(expectedComponent, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(expectedComponent, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
