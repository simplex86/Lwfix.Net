using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Magnitude_345_Returns5()
        {
            var v = new FVector3<Fixed32>(new Fixed32(3), new Fixed32(4), Fixed32.Zero);
            var result = v.Magnitude;

            // sqrt(9 + 16 + 0) = sqrt(25) = 5
            Assert.Equal(5.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Magnitude_UnitX_Returns1()
        {
            var v = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var result = v.Magnitude;

            Assert.Equal(1.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Magnitude_Zero_Returns0()
        {
            var v = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var result = v.Magnitude;

            Assert.Equal(0.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void SqrMagnitude_345_Returns25()
        {
            var v = new FVector3<Fixed32>(new Fixed32(3), new Fixed32(4), Fixed32.Zero);
            var result = v.SqrMagnitude;

            // 3*3 + 4*4 + 0*0 = 25
            Assert.Equal(25.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ClampMagnitude_ClampsToMax()
        {
            var v = new FVector3<Fixed32>(new Fixed32(3), new Fixed32(4), Fixed32.Zero);
            var result = FVector3<Fixed32>.ClampMagnitude(v, new Fixed32(2.5));

            // Original magnitude is 5, clamped to 2.5 => (3/5*2.5, 4/5*2.5, 0) = (1.5, 2, 0)
            Assert.Equal(1.5, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ClampMagnitude_BelowMax_NoChange()
        {
            var v = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.ClampMagnitude(v, new Fixed32(5));

            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
