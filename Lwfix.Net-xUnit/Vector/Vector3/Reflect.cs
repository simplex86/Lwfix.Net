using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Reflect_OffYUp_ReturnsMirrored()
        {
            var direction = new FVector3<Fixed32>(Fixed32.One, new Fixed32(-1), Fixed32.Zero);
            var normal = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var result = FVector3<Fixed32>.Reflect(direction, normal);

            // Reflect((1,-1,0), (0,1,0)) = (1,-1,0) - 2*Dot((0,1,0),(1,-1,0))*(0,1,0)
            // = (1,-1,0) - 2*(-1)*(0,1,0) = (1,-1,0) + (0,2,0) = (1,1,0)
            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
