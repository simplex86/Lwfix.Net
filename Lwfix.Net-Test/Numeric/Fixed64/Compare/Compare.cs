using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TCompare64
    {
        [Fact]
        public void NaN_Equality()
        {
            var nan = Fixed64.NaN;
            var one = Fixed64.One;

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
            var nan = Fixed64.NaN;
            var one = Fixed64.One;

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
            var a = new Fixed64(1);
            var b = new Fixed64(1);
            var c = new Fixed64(2);

            Assert.True(a == b);
            Assert.False(a == c);
            Assert.True(a != c);
            Assert.False(a != b);
        }

        [Fact]
        public void Normal_Relational()
        {
            var one = new Fixed64(1);
            var two = new Fixed64(2);

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
            Assert.True(Fixed64.PositiveInfinity > Fixed64.MaxValue);
            Assert.True(Fixed64.NegativeInfinity < Fixed64.MinValue);
            Assert.True(Fixed64.PositiveInfinity == Fixed64.PositiveInfinity);
            Assert.True(Fixed64.NegativeInfinity == Fixed64.NegativeInfinity);
        }

        [Fact]
        public void CompareTo()
        {
            Assert.Equal(1, Fixed64.One.CompareTo(Fixed64.Zero));
            Assert.Equal(-1, Fixed64.Zero.CompareTo(Fixed64.One));
            Assert.Equal(0, Fixed64.One.CompareTo(Fixed64.One));
        }

        [Fact]
        public void CompareTo_Object()
        {
            Assert.Equal(1, Fixed64.One.CompareTo((object)Fixed64.Zero));
            Assert.Equal(-1, Fixed64.Zero.CompareTo((object)Fixed64.One));
            Assert.Equal(0, Fixed64.One.CompareTo((object)Fixed64.One));
        }
    }
}
