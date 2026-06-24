using SimplexLab.Fixed;
using Xunit;
using System;

namespace Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Project_OnXAxis_ReturnsXComponent()
        {
            var vector = new FVector3<Fixed32>(new Fixed32(5), new Fixed32(5), Fixed32.Zero);
            var normal = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.Project(vector, normal);

            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void ProjectOnPlane_XPlane_RemovesXComponent()
        {
            var vector = new FVector3<Fixed32>(new Fixed32(5), new Fixed32(5), Fixed32.Zero);
            var planeNormal = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var result = FVector3<Fixed32>.ProjectOnPlane(vector, planeNormal);

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(5.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
