using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 乘法 - 常规
    /// </summary>
    public partial class TMul
    {
        private readonly static List<double[]> normal_numbers =
        [
            [0.506, 0.13],
            [-16.3083, -28.2577],
            [15.5667775, -11.09],
            [44.92, -98.7889],
            [-63129.896, 9095.5],
            [-66073.7668, -7882.62],
            [9526.552, 61031.5062],
            [2306.374, -84020.24]
        ];
        private const double TOLERANCE = 10e-5;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var u = n[0];
                var v = n[1];

                var t = new Fixed32(u);
                var w = new Fixed32(v);

                Assert.Equal(u * v, (t * w).ToDouble(), TOLERANCE);
            }
        }

        [Fact]
        public void BoundaryCases()
        {
            // 测试特殊值
            var zero = Fixed32.Zero;
            var one = Fixed32.One;
            var negativeOne = Fixed32.NegativeOne;
            var maxValue = Fixed32.MaxValue;
            var minValue = Fixed32.MinValue;
            var nan = Fixed32.NaN;
            var positiveInfinity = Fixed32.PositiveInfinity;
            var negativeInfinity = Fixed32.NegativeInfinity;

            // 0乘以任何数都应该是0，但0乘以无穷大应该是NaN
            Assert.Equal(zero, zero * zero);
            Assert.Equal(zero, zero * one);
            Assert.Equal(zero, zero * negativeOne);
            Assert.Equal(zero, zero * maxValue);
            Assert.Equal(zero, zero * minValue);
            Assert.True((zero * positiveInfinity).IsNaN());
            Assert.True((zero * negativeInfinity).IsNaN());

            // 1乘以任何数都应该是其本身
            Assert.Equal(zero, one * zero);
            Assert.Equal(one, one * one);
            Assert.Equal(negativeOne, one * negativeOne);
            Assert.Equal(maxValue, one * maxValue);
            Assert.Equal(minValue, one * minValue);
            Assert.Equal(positiveInfinity, one * positiveInfinity);
            Assert.Equal(negativeInfinity, one * negativeInfinity);

            // -1乘以任何数都应该是其相反数
            Assert.Equal(zero, negativeOne * zero);
            Assert.Equal(negativeOne, negativeOne * one);
            Assert.Equal(one, negativeOne * negativeOne);
            Assert.Equal(-maxValue, negativeOne * maxValue);
            Assert.Equal(-minValue, negativeOne * minValue);
            Assert.Equal(negativeInfinity, negativeOne * positiveInfinity);
            Assert.Equal(positiveInfinity, negativeOne * negativeInfinity);

            // NaN测试
            Assert.True((nan * zero).IsNaN());
            Assert.True((nan * one).IsNaN());
            Assert.True((nan * maxValue).IsNaN());
            Assert.True((zero * nan).IsNaN());
            Assert.True((one * nan).IsNaN());
            Assert.True((maxValue * nan).IsNaN());

            // 无穷大测试
            Assert.True((positiveInfinity * one).IsPositiveInfinity());
            Assert.True((positiveInfinity * negativeOne).IsNegativeInfinity());
            Assert.True((negativeInfinity * one).IsNegativeInfinity());
            Assert.True((negativeInfinity * negativeOne).IsPositiveInfinity());
            Assert.True((positiveInfinity * zero).IsNaN());
            Assert.True((negativeInfinity * zero).IsNaN());
            Assert.True((positiveInfinity * positiveInfinity).IsPositiveInfinity());
            Assert.True((positiveInfinity * negativeInfinity).IsNegativeInfinity());
            Assert.True((negativeInfinity * negativeInfinity).IsPositiveInfinity());
        }
    }
}
