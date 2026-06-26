using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Vectors
{
    public partial class TVector3
    {
        private const double TOLERANCE = 10e-5;

        [Fact]
        public void Dot_OrthogonalVectors_ReturnsZero()
        {
            var a = new FVector3<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero);
            var b = new FVector3<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero);
            var result = FVector3<Fixed32>.Dot(a, b);

            Assert.Equal(0.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Dot_KnownVectors_ReturnsExpected()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(4), new Fixed32(5), new Fixed32(6));
            var result = FVector3<Fixed32>.Dot(a, b);

            // 1*4 + 2*5 + 3*6 = 4 + 10 + 18 = 32
            Assert.Equal(32.0, result.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Dot_ParallelVectors_ReturnsSqrMagnitude()
        {
            var a = new FVector3<Fixed32>(new Fixed32(3), new Fixed32(4), Fixed32.Zero);
            var result = FVector3<Fixed32>.Dot(a, a);

            // Dot with self = SqrMagnitude = 3*3 + 4*4 + 0*0 = 25
            Assert.Equal(25.0, result.ToDouble(), TOLERANCE);
        }
    }
}
