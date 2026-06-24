using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 取模 - NaN与特殊值
    /// </summary>
    public partial class TMod
    {
        [Fact]
        public void NaN()
        {
            var w = new Fixed32(7); // 任意非特殊值

            // Mod(NaN, x) is NaN
            Assert.True(Fixed32.IsNaN(Fixed32.NaN % w));

            // Mod(x, NaN) is NaN
            Assert.True(Fixed32.IsNaN(w % Fixed32.NaN));

            // Mod(PositiveInfinity, x) is NaN
            Assert.True(Fixed32.IsNaN(Fixed32.PositiveInfinity % w));

            // Mod(x, PositiveInfinity) = x
            Assert.Equal(w.ToDouble(), (w % Fixed32.PositiveInfinity).ToDouble(), TOLERANCE);

            // Mod(x, NegativeInfinity) = x
            Assert.Equal(w.ToDouble(), (w % Fixed32.NegativeInfinity).ToDouble(), TOLERANCE);

            // Mod(0, x) = 0 (when x != 0)
            Assert.Equal(0.0, (Fixed32.Zero % w).ToDouble(), TOLERANCE);

            // Mod(x, 0) is NaN
            var divByZero = w % Fixed32.Zero;
            Assert.True(Fixed32.IsNaN(divByZero));

            // Mod(5, 3) = 2
            Assert.Equal(2.0, (new Fixed32(5) % new Fixed32(3)).ToDouble(), TOLERANCE);

            // Mod(-5, 3) = -2 (C# remainder semantics)
            Assert.Equal(-2.0, (new Fixed32(-5) % new Fixed32(3)).ToDouble(), TOLERANCE);

            // Mod(5, -3) = 2
            Assert.Equal(2.0, (new Fixed32(5) % new Fixed32(-3)).ToDouble(), TOLERANCE);
        }
    }
}
