using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Equality_SameVectors_ReturnsTrue()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));

            Assert.True(a == b);
        }

        [Fact]
        public void Equality_DifferentVectors_ReturnsFalse()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(4));

            Assert.False(a == b);
        }

        [Fact]
        public void Inequality_DifferentVectors_ReturnsTrue()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(4));

            Assert.True(a != b);
        }

        [Fact]
        public void Inequality_SameVectors_ReturnsFalse()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));

            Assert.False(a != b);
        }
    }
}
