using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Quaternion
{
    public partial class TQuaternion
    {
        [Fact]
        public void Operator_Add_TwoQuaternions()
        {
            var q1 = new FQuaternion<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3), new Fixed32(4));
            var q2 = new FQuaternion<Fixed32>(new Fixed32(5), new Fixed32(6), new Fixed32(7), new Fixed32(8));

            var result = q1 + q2;

            Assert.Equal(6.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(8.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(10.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(12.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Subtract_TwoQuaternions()
        {
            var q1 = new FQuaternion<Fixed32>(new Fixed32(5), new Fixed32(6), new Fixed32(7), new Fixed32(8));
            var q2 = new FQuaternion<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3), new Fixed32(4));

            var result = q1 - q2;

            Assert.Equal(4.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Multiply_QuaternionByScalar()
        {
            var q = new FQuaternion<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3), new Fixed32(4));
            var scalar = new Fixed32(2);

            var result = q * scalar;

            Assert.Equal(2.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(6.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(8.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Multiply_ScalarByQuaternion()
        {
            var q = new FQuaternion<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3), new Fixed32(4));
            var scalar = new Fixed32(2);

            var result = scalar * q;

            Assert.Equal(2.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(4.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(6.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(8.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Multiply_QuaternionByQuaternion_HamiltonProduct()
        {
            // Identity * Identity = Identity
            var result = FQuaternion<Fixed32>.Identity * FQuaternion<Fixed32>.Identity;

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Multiply_QuaternionByQuaternion_KnownValues()
        {
            // q1 = (1, 0, 0, 0), q2 = (0, 1, 0, 0)
            // Hamilton: w = 0*0 - 1*0 - 0*0 - 0*0 = 0
            //          x = 0*0 + 1*0 + 0*0 - 0*1 = 0  ... wait let me compute properly
            // Using the formula from the source:
            // w1=0,x1=1,y1=0,z1=0, w2=0,x2=0,y2=1,z2=0
            // x = w1*x2 + x1*w2 + y1*z2 - z1*y2 = 0+0+0-0 = 0
            // y = w1*y2 + y1*w2 + z1*x2 - x1*z2 = 0+0+0-0 = 0
            // z = w1*z2 + z1*w2 + x1*y2 - y1*x2 = 0+0+1-0 = 1
            // w = w1*w2 - x1*x2 - y1*y2 - z1*z2 = 0-0-0-0 = 0
            var q1 = new FQuaternion<Fixed32>(Fixed32.One, Fixed32.Zero, Fixed32.Zero, Fixed32.Zero);
            var q2 = new FQuaternion<Fixed32>(Fixed32.Zero, Fixed32.One, Fixed32.Zero, Fixed32.Zero);

            var result = q1 * q2;

            Assert.Equal(0.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(1.0, result.Z.ToDouble(), TOLERANCE);
            Assert.Equal(0.0, result.W.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Multiply_QuaternionByVector3()
        {
            // Identity rotation should not change the vector
            var q = FQuaternion<Fixed32>.Identity;
            var v = new FVector3<Fixed32>(new Fixed32(1), new Fixed32(2), new Fixed32(3));

            var result = q * v;

            Assert.Equal(1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Z.ToDouble(), TOLERANCE);
        }
    }
}
