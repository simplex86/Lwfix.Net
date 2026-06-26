using SimplexLab.Lwfix;
using Xunit;
using System;

namespace SimplexLab.Lwfix.Test.Vectors
{
    public partial class TVector3
    {
        [Fact]
        public void Zero_IsAllZeros()
        {
            var v = FVector3<Fixed32>.Zero;

            Assert.Equal(0.0, v.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void One_IsAllOnes()
        {
            var v = FVector3<Fixed32>.One;

            Assert.Equal(1.0, v.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, v.Y.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, v.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Up_IsYPositive()
        {
            var v = FVector3<Fixed32>.Up;

            Assert.Equal(0.0, v.X.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, v.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Down_IsYNegative()
        {
            var v = FVector3<Fixed32>.Down;

            Assert.Equal(0.0, v.X.ToDouble(), TOLERANCE);
            Assert.Equal(-1.0, v.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Left_IsXNegative()
        {
            var v = FVector3<Fixed32>.Left;

            Assert.Equal(-1.0, v.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Right_IsXPositive()
        {
            var v = FVector3<Fixed32>.Right;

            Assert.Equal(1.0, v.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Forward_IsZPositive()
        {
            var v = FVector3<Fixed32>.Forward;

            Assert.Equal(0.0, v.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Y.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, v.Z.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Back_IsZNegative()
        {
            var v = FVector3<Fixed32>.Back;

            Assert.Equal(0.0, v.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, v.Y.ToDouble(), TOLERANCE);
            Assert.Equal(-1.0, v.Z.ToDouble(), TOLERANCE);
        }
    }
}
