using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Angle_RightAngle()
        {
            var from = new FVector2<Fixed32>(new Fixed32(1), Fixed32.Zero);
            var to = new FVector2<Fixed32>(Fixed32.Zero, new Fixed32(1));
            var result = FVector2<Fixed32>.Angle(from, to);
            Assert.Equal(90.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Angle_SameDirection()
        {
            var from = new FVector2<Fixed32>(new Fixed32(1), Fixed32.Zero);
            var to = new FVector2<Fixed32>(new Fixed32(1), Fixed32.Zero);
            var result = FVector2<Fixed32>.Angle(from, to);
            Assert.Equal(0.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Angle_OppositeDirection()
        {
            var from = new FVector2<Fixed32>(new Fixed32(1), Fixed32.Zero);
            var to = new FVector2<Fixed32>(new Fixed32(-1), Fixed32.Zero);
            var result = FVector2<Fixed32>.Angle(from, to);
            Assert.Equal(180.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void SignedAngle_Positive()
        {
            var from = new FVector2<Fixed32>(new Fixed32(1), Fixed32.Zero);
            var to = new FVector2<Fixed32>(Fixed32.Zero, new Fixed32(1));
            var result = FVector2<Fixed32>.SignedAngle(from, to);
            Assert.Equal(90.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void SignedAngle_Negative()
        {
            var from = new FVector2<Fixed32>(Fixed32.Zero, new Fixed32(1));
            var to = new FVector2<Fixed32>(new Fixed32(1), Fixed32.Zero);
            var result = FVector2<Fixed32>.SignedAngle(from, to);
            Assert.Equal(-90.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Angle_RandomCompareWithSystemMath()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double angle1 = rand.NextDouble() * 2 * Math.PI;
                double angle2 = rand.NextDouble() * 2 * Math.PI;
                double fx = Math.Cos(angle1);
                double fy = Math.Sin(angle1);
                double tx = Math.Cos(angle2);
                double ty = Math.Sin(angle2);

                var from = new FVector2<Fixed32>(new Fixed32(fx), new Fixed32(fy));
                var to = new FVector2<Fixed32>(new Fixed32(tx), new Fixed32(ty));
                var fangle = FVector2<Fixed32>.Angle(from, to);

                double dot = fx * tx + fy * ty;
                double magFrom = Math.Sqrt(fx * fx + fy * fy);
                double magTo = Math.Sqrt(tx * tx + ty * ty);
                double cosAngle = Math.Clamp(dot / (magFrom * magTo), -1.0, 1.0);
                double expected = Math.Acos(cosAngle) * (180.0 / Math.PI);
                Assert.Equal(expected, fangle.ToDouble(), TOLERANCE);
            }
        }
    }
}
