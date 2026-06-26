using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Distance_345Triangle_Returns5()
        {
            var a = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var b = new FVector3<Fixed32>(new Fixed32(3), new Fixed32(4), Fixed32.Zero);
            var result = FVector3<Fixed32>.Distance(a, b);

            // sqrt(3^2 + 4^2 + 0^2) = sqrt(25) = 5
            Assert.Equal(5.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Distance_122_Returns3()
        {
            var a = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var b = new FVector3<Fixed32>(Fixed32.One, new Fixed32(2), new Fixed32(2));
            var result = FVector3<Fixed32>.Distance(a, b);

            // sqrt(1 + 4 + 4) = sqrt(9) = 3
            Assert.Equal(3.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Distance_SamePoint_ReturnsZero()
        {
            var a = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.Distance(a, a);

            Assert.Equal(0.0, result.ToDouble(), TOLERANCE);
        }
    }
}
