using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Normalize_KnownValue()
        {
            var v = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));
            var result = FVector2<Fixed32>.Normalize(v);
            Assert.Equal(1.0, result.Magnitude.ToDouble(), TOLERANCE);
            Assert.Equal(3.0 / 5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(4.0 / 5.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Normalize_UnitVector()
        {
            var v = new FVector2<Fixed32>(Fixed32.One, Fixed32.Zero);
            var result = FVector2<Fixed32>.Normalize(v);
            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Normalize_ZeroVector()
        {
            var v = new FVector2<Fixed32>(Fixed32.Zero, Fixed32.Zero);
            var result = FVector2<Fixed32>.Normalize(v);
            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void NormalizedProperty()
        {
            var v = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));
            var result = v.Normalized;
            Assert.Equal(1.0, result.Magnitude.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Normalize_RandomCompareWithSystemMath()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double x = rand.NextDouble() * 200 - 100;
                double y = rand.NextDouble() * 200 - 100;

                var fv = new FVector2<Fixed32>(new Fixed32(x), new Fixed32(y));
                var result = FVector2<Fixed32>.Normalize(fv);

                double mag = Math.Sqrt(x * x + y * y);
                if (mag == 0)
                {
                    Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
                    Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
                }
                else
                {
                    Assert.Equal(x / mag, result.X.ToDouble(), TOLERANCE);
                    Assert.Equal(y / mag, result.Y.ToDouble(), TOLERANCE);
                }
            }
        }
    }
}
