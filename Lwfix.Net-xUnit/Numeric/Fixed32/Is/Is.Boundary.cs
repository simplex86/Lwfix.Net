using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TIs
    {
        [Fact]
        public void IsNaN()
        {
            Assert.True(Fixed32.NaN.IsNaN());
            Assert.False(Fixed32.Zero.IsNaN());
        }

        [Fact]
        public void IsZero()
        {
            Assert.True(Fixed32.Zero.IsZero());
            Assert.False(Fixed32.One.IsZero());
        }

        [Fact]
        public void IsPositive()
        {
            Assert.True(Fixed32.One.IsPositive());
            Assert.False(Fixed32.NegativeOne.IsPositive());
            Assert.True(Fixed32.Zero.IsPositive());
        }

        [Fact]
        public void IsNegative()
        {
            Assert.True(Fixed32.NegativeOne.IsNegative());
            Assert.False(Fixed32.One.IsNegative());
            Assert.False(Fixed32.Zero.IsNegative());
        }

        [Fact]
        public void IsFractional()
        {
            Assert.True(new Fixed32(1.5).IsFractional());
            Assert.False(Fixed32.One.IsFractional());
        }

        [Fact]
        public void IsPositiveInfinity()
        {
            Assert.True(Fixed32.PositiveInfinity.IsPositiveInfinity());
        }

        [Fact]
        public void IsNegativeInfinity()
        {
            Assert.True(Fixed32.NegativeInfinity.IsNegativeInfinity());
        }

        [Fact]
        public void IsInfinity()
        {
            Assert.True(Fixed32.PositiveInfinity.IsInfinity());
            Assert.True(Fixed32.NegativeInfinity.IsInfinity());
        }

        [Fact]
        public void IsMax()
        {
            Assert.True(Fixed32.MaxValue.IsMax());
        }

        [Fact]
        public void IsMin()
        {
            Assert.True(Fixed32.MinValue.IsMin());
        }
    }
}
