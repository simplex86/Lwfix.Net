using SimplexLab.Fixed;
using Xunit;
using System;

namespace LwfixTest.Fixed.Vectors
{
    public partial class TVector2
    {
        [Fact]
        public void Operator_Add()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(2));
            var b = new FVector2<Fixed32>(new Fixed32(3), new Fixed32(4));
            var result = a + b;
            Assert.Equal(4.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(6.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Subtract()
        {
            var a = new FVector2<Fixed32>(new Fixed32(5), new Fixed32(5));
            var b = new FVector2<Fixed32>(new Fixed32(2), new Fixed32(3));
            var result = a - b;
            Assert.Equal(3.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_MultiplyByScalar()
        {
            var a = new FVector2<Fixed32>(new Fixed32(2), new Fixed32(3));
            var result = a * new Fixed32(2);
            Assert.Equal(4.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(6.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_DivideByScalar()
        {
            var a = new FVector2<Fixed32>(new Fixed32(4), new Fixed32(6));
            var result = a / new Fixed32(2);
            Assert.Equal(2.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(3.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_Negate()
        {
            var a = new FVector2<Fixed32>(new Fixed32(1), new Fixed32(-2));
            var result = -a;
            Assert.Equal(-1.0, result.X.ToDouble(), TOLERANCE);
            Assert.Equal(2.0, result.Y.ToDouble(), TOLERANCE);
        }

        [Fact]
        public void Operator_RandomCompareWithSystemMath()
        {
            var rand = new System.Random(42);
            for (int i = 0; i < 100; i++)
            {
                double ax = rand.NextDouble() * 200 - 100;
                double ay = rand.NextDouble() * 200 - 100;
                double bx = rand.NextDouble() * 200 - 100;
                double by = rand.NextDouble() * 200 - 100;

                var fa = new FVector2<Fixed32>(new Fixed32(ax), new Fixed32(ay));
                var fb = new FVector2<Fixed32>(new Fixed32(bx), new Fixed32(by));

                var add = fa + fb;
                Assert.Equal(ax + bx, add.X.ToDouble(), TOLERANCE);
                Assert.Equal(ay + by, add.Y.ToDouble(), TOLERANCE);

                var sub = fa - fb;
                Assert.Equal(ax - bx, sub.X.ToDouble(), TOLERANCE);
                Assert.Equal(ay - by, sub.Y.ToDouble(), TOLERANCE);

                var neg = -fa;
                Assert.Equal(-ax, neg.X.ToDouble(), TOLERANCE);
                Assert.Equal(-ay, neg.Y.ToDouble(), TOLERANCE);
            }
        }
    }
}
