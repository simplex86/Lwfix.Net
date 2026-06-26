using SimplexLab.Lwfix;
using Xunit;

namespace SimplexLab.Lwfix.Test.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void ZeroConstant()
        {
            var zero = FVector2<Fixed32>.Zero;
            Assert.Equal(0.0, zero.X.ToDouble(), 10e-5);
            Assert.Equal(0.0, zero.Y.ToDouble(), 10e-5);
        }

        [Fact]
        public void OneConstant()
        {
            var one = FVector2<Fixed32>.One;
            Assert.Equal(1.0, one.X.ToDouble(), 10e-5);
            Assert.Equal(1.0, one.Y.ToDouble(), 10e-5);
        }

        [Fact]
        public void UpConstant()
        {
            var up = FVector2<Fixed32>.Up;
            Assert.Equal(0.0, up.X.ToDouble(), 10e-5);
            Assert.Equal(1.0, up.Y.ToDouble(), 10e-5);
        }

        [Fact]
        public void DownConstant()
        {
            var down = FVector2<Fixed32>.Down;
            Assert.Equal(0.0, down.X.ToDouble(), 10e-5);
            Assert.Equal(-1.0, down.Y.ToDouble(), 10e-5);
        }

        [Fact]
        public void LeftConstant()
        {
            var left = FVector2<Fixed32>.Left;
            Assert.Equal(-1.0, left.X.ToDouble(), 10e-5);
            Assert.Equal(0.0, left.Y.ToDouble(), 10e-5);
        }

        [Fact]
        public void RightConstant()
        {
            var right = FVector2<Fixed32>.Right;
            Assert.Equal(1.0, right.X.ToDouble(), 10e-5);
            Assert.Equal(0.0, right.Y.ToDouble(), 10e-5);
        }
    }
}
