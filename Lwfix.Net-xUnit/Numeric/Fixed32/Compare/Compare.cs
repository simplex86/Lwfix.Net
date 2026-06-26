using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TCompare
    {
        [Fact]
        public void NaN_Equality()
        {
            var nan = Fixed32.NaN;
            var one = Fixed32.One;

            // NaN == anything should be false
            Assert.False(nan == nan);
            Assert.False(nan == one);
            Assert.False(one == nan);

            // NaN != anything should be true
            Assert.True(nan != nan);
            Assert.True(nan != one);
            Assert.True(one != nan);
        }

        [Fact]
        public void NaN_Relational()
        {
            var nan = Fixed32.NaN;
            var one = Fixed32.One;

            // NaN > anything should be false
            Assert.False(nan > one);
            Assert.False(one > nan);
            Assert.False(nan > nan);

            // NaN < anything should be false
            Assert.False(nan < one);
            Assert.False(one < nan);
            Assert.False(nan < nan);

            // NaN >= anything should be false
            Assert.False(nan >= one);
            Assert.False(one >= nan);
            Assert.False(nan >= nan);

            // NaN <= anything should be false
            Assert.False(nan <= one);
            Assert.False(one <= nan);
            Assert.False(nan <= nan);
        }

        [Fact]
        public void Normal_Equality()
        {
            var a = new Fixed32(1);
            var b = new Fixed32(1);
            var c = new Fixed32(2);

            Assert.True(a == b);
            Assert.False(a == c);
            Assert.True(a != c);
            Assert.False(a != b);
        }

        [Fact]
        public void Normal_Relational()
        {
            var one = new Fixed32(1);
            var two = new Fixed32(2);

            Assert.True(two > one);
            Assert.False(one > two);
            Assert.True(one < two);
            Assert.False(two < one);
            Assert.True(one >= one);
            Assert.True(two >= one);
            Assert.False(one >= two);
            Assert.True(one <= one);
            Assert.True(one <= two);
            Assert.False(two <= one);
        }

        [Fact]
        public void Infinity_Comparisons()
        {
            Assert.True(Fixed32.PositiveInfinity > Fixed32.MaxValue);
            Assert.True(Fixed32.NegativeInfinity < Fixed32.MinValue);
            Assert.True(Fixed32.PositiveInfinity == Fixed32.PositiveInfinity);
            Assert.True(Fixed32.NegativeInfinity == Fixed32.NegativeInfinity);
        }

        [Fact]
        public void CompareTo()
        {
            Assert.Equal(1, Fixed32.One.CompareTo(Fixed32.Zero));
            Assert.Equal(-1, Fixed32.Zero.CompareTo(Fixed32.One));
            Assert.Equal(0, Fixed32.One.CompareTo(Fixed32.One));
        }

        [Fact]
        public void CompareTo_Object()
        {
            Assert.Equal(1, Fixed32.One.CompareTo((object)Fixed32.Zero));
            Assert.Equal(-1, Fixed32.Zero.CompareTo((object)Fixed32.One));
            Assert.Equal(0, Fixed32.One.CompareTo((object)Fixed32.One));
        }
    }
}
