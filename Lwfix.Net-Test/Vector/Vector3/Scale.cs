using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Scale_ComponentWise()
        {
            var a = new FVector3<Fixed32>(new Fixed32(2), new Fixed32(3), new Fixed32(4));
            var b = new FVector3<Fixed32>(new Fixed32(5), new Fixed32(6), new Fixed32(7));
            var result = FVector3<Fixed32>.Scale(a, b);

            Assert.Equal(10.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(18.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(28.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
