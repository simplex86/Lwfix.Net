using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Operator_Add_ReturnsSum()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));
            var b = new FVector3<Fixed32>(new Fixed32(4), new Fixed32(5), new Fixed32(6));
            var result = a + b;

            Assert.Equal(5.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(7.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(9.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Subtract_ReturnsDifference()
        {
            var a = new FVector3<Fixed32>(new Fixed32(5), new Fixed32(5), new Fixed32(5));
            var b = new FVector3<Fixed32>(new Fixed32(2), new Fixed32(3), new Fixed32(4));
            var result = a - b;

            Assert.Equal(3.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_MultiplyByScalar_ReturnsScaled()
        {
            var a = new FVector3<Fixed32>(new Fixed32(2), new Fixed32(3), new Fixed32(4));
            var result = a * new Fixed32(2);

            Assert.Equal(4.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(6.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(8.0, result.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Negate_ReturnsNegated()
        {
            var a = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(-2), new Fixed32(3));
            var result = -a;

            Assert.Equal(-1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(-3.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
