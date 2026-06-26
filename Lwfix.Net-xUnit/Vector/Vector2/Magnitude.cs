using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Magnitude_KnownValue()
        {
            var v = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));
            Assert.Equal(5.0, v.Magnitude.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Magnitude_UnitVector()
        {
            var v = new FVector2<Fixed32>(Fixed32.One, Fixed32.Zero);
            Assert.Equal(1.0, v.Magnitude.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Magnitude_ZeroVector()
        {
            var v = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            Assert.Equal(0.0, v.Magnitude.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void SqrMagnitude_KnownValue()
        {
            var v = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));
            Assert.Equal(25.0, v.SqrMagnitude.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ClampMagnitude_ClampsToMax()
        {
            var v = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));
            var clamped = FVector2<Fixed32>.ClampMagnitude(v, new Fixed32(2.0));
            Assert.Equal(2.0, clamped.Magnitude.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ClampMagnitude_NoClampNeeded()
        {
            var v = new FVector2<Fixed32>(new Fixed32(1), Fixed32.Zero);
            var clamped = FVector2<Fixed32>.ClampMagnitude(v, new Fixed32(5.0));
            Assert.Equal(1.0, clamped.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, clamped.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Magnitude_RandomCompareWithSystemMath()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double x = rand.NextDouble() * 200 - 100;
                double y = rand.NextDouble() * 200 - 100;

                var fv = new FVector2<Fixed32>(new Fixed32(x), new Fixed32(y));
                double expected = Math.Sqrt(x * x + y * y);
                Assert.Equal(expected, fv.Magnitude.ToDouble(), TOLERANCE);
            }
        }
    }
}
