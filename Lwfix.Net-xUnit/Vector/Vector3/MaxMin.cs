using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Max_ComponentWise()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(5), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(3), new Fixed32(2), new Fixed32(4));
            var result = FVector3<Fixed32>.Max(a, b);

            Assert.Equal(3.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Min_ComponentWise()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(5), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(3), new Fixed32(2), new Fixed32(4));
            var result = FVector3<Fixed32>.Min(a, b);

            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
