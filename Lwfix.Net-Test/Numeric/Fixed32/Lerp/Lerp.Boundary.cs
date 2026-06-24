using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TLerp
    {
        [Fact]
        public void Boundary()
        {
            var a = new Fixed32(3.0);
            var b = new Fixed32(7.0);

            // Lerp(a, b, 0) = a
            Assert.Equal(a.ToDouble(), Fixed32.Lerp(a, b, Fixed32.Zero).ToDouble(), TOLERANCE);

            // Lerp(a, b, 1) = b
            Assert.Equal(b.ToDouble(), Fixed32.Lerp(a, b, Fixed32.One).ToDouble(), TOLERANCE);

            // Lerp(a, a, t) = a (same start and end)
            var t = new Fixed32(0.37);
            Assert.Equal(a.ToDouble(), Fixed32.Lerp(a, a, t).ToDouble(), TOLERANCE);

            // Lerp(0, 10, 0.5) = 5
            Assert.Equal(5.0, Fixed32.Lerp(Fixed32.Zero, new Fixed32(10.0), Fixed32.Half).ToDouble(), TOLERANCE);

            // Lerp(-10, 10, 0.5) = 0
            Assert.Equal(0.0, Fixed32.Lerp(new Fixed32(-10.0), new Fixed32(10.0), Fixed32.Half).ToDouble(), TOLERANCE);

            // ClampLerp(a, b, -1) = a (clamped)
            Assert.Equal(a.ToDouble(), Fixed32.ClampLerp(a, b, Fixed32.NegativeOne).ToDouble(), TOLERANCE);

            // ClampLerp(a, b, 2) = b (clamped)
            Assert.Equal(b.ToDouble(), Fixed32.ClampLerp(a, b, new Fixed32(2.0)).ToDouble(), TOLERANCE);

            // InverseLerp(a, b, a) = 0
            Assert.Equal(0.0, Fixed32.InverseLerp(a, b, a).ToDouble(), TOLERANCE);

            // InverseLerp(a, b, b) = 1
            Assert.Equal(1.0, Fixed32.InverseLerp(a, b, b).ToDouble(), TOLERANCE);

            // InverseLerp(0, 10, 5) = 0.5
            Assert.Equal(0.5, Fixed32.InverseLerp(Fixed32.Zero, new Fixed32(10.0), new Fixed32(5.0)).ToDouble(), TOLERANCE);
        }
    }
}
