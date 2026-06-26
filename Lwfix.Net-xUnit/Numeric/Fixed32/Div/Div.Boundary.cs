using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    /// <summary>
    /// 除法 - 边界
    /// </summary>
    public partial class TDiv
    {
        [Fact]
        public void DivByZero()
        {
            var result = new Fixed32(5) / Fixed32.Zero;
            Assert.True(result.IsPositiveInfinity() || result.IsNaN());
        }

        [Fact]
        public void ZeroDivByNonZero()
        {
            var result = Fixed32.Zero / new Fixed32(5);
            Assert.Equal(Fixed32.Zero, result);
        }

        [Fact]
        public void DivByOne()
        {
            var result = new Fixed32(5) / Fixed32.One;
            Assert.Equal(5, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void DivByNegativeOne()
        {
            var result = new Fixed32(5) / Fixed32.NegativeOne;
            Assert.Equal(-5, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void MaxValueDivByOne()
        {
            var result = Fixed32.MaxValue / Fixed32.One;
            Assert.Equal(Fixed32.MaxValue, result);
        }

        [Fact]
        public void OneDivByOne()
        {
            var result = Fixed32.One / Fixed32.One;
            Assert.Equal(Fixed32.One, result);
        }

        [Fact]
        public void SelfDivision()
        {
            var result = new Fixed32(7) / new Fixed32(7);
            Assert.Equal(Fixed32.One, result);
        }
    }
}
