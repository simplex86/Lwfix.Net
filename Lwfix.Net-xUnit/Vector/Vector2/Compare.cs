using SimplexLab.Fixed;
using Xunit;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Equality_EqualVectors()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(2));
            var b = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(2));
            Assert.True(a == b);
        }

        [Fact]
        public void Equality_DifferentVectors()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(2));
            var b = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(3));
            Assert.False(a == b);
        }

        [Fact]
        public void Inequality_DifferentVectors()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(2));
            var b = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(3));
            Assert.True(a != b);
        }

        [Fact]
        public void Inequality_SameVectors()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(2));
            var b = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(2));
            Assert.False(a != b);
        }
    }
}
