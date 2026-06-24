using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 倒数 - NaN与特殊值
    /// </summary>
    public partial class TReciprocal
    {
        [Fact]
        public void NaN()
        {
            // Reciprocal(NaN) is NaN
            Assert.True(Fixed32.IsNaN(Fixed32.Reciprocal(Fixed32.NaN)));

            // Reciprocal(PositiveInfinity) is Zero
            Assert.Equal(0.0, Fixed32.Reciprocal(Fixed32.PositiveInfinity).ToDouble(), TOLERANCE);

            // Reciprocal(NegativeInfinity) is Zero
            Assert.Equal(0.0, Fixed32.Reciprocal(Fixed32.NegativeInfinity).ToDouble(), TOLERANCE);

            // Reciprocal(0) is PositiveInfinity
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.Reciprocal(Fixed32.Zero)));

            // Reciprocal(1) = 1
            Assert.Equal(1.0, Fixed32.Reciprocal(Fixed32.One).ToDouble(), TOLERANCE);

            // Reciprocal(-1) = -1
            Assert.Equal(-1.0, Fixed32.Reciprocal(Fixed32.NegativeOne).ToDouble(), TOLERANCE);

            // Reciprocal(2) = 0.5
            Assert.Equal(0.5, Fixed32.Reciprocal(new Fixed32(2)).ToDouble(), TOLERANCE);

            // Reciprocal(-2) = -0.5
            Assert.Equal(-0.5, Fixed32.Reciprocal(new Fixed32(-2)).ToDouble(), TOLERANCE);

            // Reciprocal(MinValue) is a very small negative number (not -Infinity, since MinValue is not Zero)
            var result = Fixed32.Reciprocal(Fixed32.MinValue);
            Assert.True(result.IsNegative() && !result.IsNegativeInfinity());
        }
    }
}
