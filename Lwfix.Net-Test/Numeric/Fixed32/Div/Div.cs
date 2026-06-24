using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 除法 - 常规
    /// </summary>
    public partial class TDiv
    {
        private readonly static List<double[]> normal_numbers =
        [
            [1, 2],
            [16.3083, 28.2577],
            [-15.5667775, 11.09],
            [44.92, -98.7889],
            [-631299.896, 909539.5],
            [-660730.7668, -788217.62],
            [395260.552, 610316.5062],
            [230618.374, -840202.24]
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

                Assert.Equal(u / v, (t / w).ToDouble(), TOLERANCE);
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

            // 任何数除以1都应该是其本身
            Assert.Equal(zero, zero / one);
            Assert.Equal(one, one / one);
            Assert.Equal(negativeOne, negativeOne / one);
            Assert.Equal(maxValue, maxValue / one);
            Assert.Equal(minValue, minValue / one);
            Assert.Equal(positiveInfinity, positiveInfinity / one);
            Assert.Equal(negativeInfinity, negativeInfinity / one);

            // 任何数除以-1都应该是其相反数
            Assert.Equal(zero, zero / negativeOne);
            Assert.Equal(negativeOne, one / negativeOne);
            Assert.Equal(one, negativeOne / negativeOne);
            Assert.Equal(-maxValue, maxValue / negativeOne);
            Assert.Equal(-minValue, minValue / negativeOne);
            Assert.Equal(negativeInfinity, positiveInfinity / negativeOne);
            Assert.Equal(positiveInfinity, negativeInfinity / negativeOne);

            // 0除以任何非零数都应该是0
            Assert.Equal(zero, zero / one);
            Assert.Equal(zero, zero / negativeOne);
            Assert.Equal(zero, zero / maxValue);
            Assert.Equal(zero, zero / minValue);
            Assert.Equal(zero, zero / positiveInfinity);
            Assert.Equal(zero, zero / negativeInfinity);

            // 除以0的情况
            Assert.True((one / zero).IsPositiveInfinity());
            Assert.True((negativeOne / zero).IsNegativeInfinity());
            Assert.True((zero / zero).IsNaN());
            Assert.True((maxValue / zero).IsPositiveInfinity());
            Assert.True((minValue / zero).IsNegativeInfinity());
            Assert.True((positiveInfinity / zero).IsPositiveInfinity());
            Assert.True((negativeInfinity / zero).IsNegativeInfinity());

            // NaN测试
            Assert.True((nan / one).IsNaN());
            Assert.True((one / nan).IsNaN());
            Assert.True((nan / nan).IsNaN());
            Assert.True((nan / zero).IsNaN());
            Assert.True((nan / positiveInfinity).IsNaN());

            // 无穷大测试
            Assert.True((positiveInfinity / one).IsPositiveInfinity());
            Assert.True((positiveInfinity / negativeOne).IsNegativeInfinity());
            Assert.True((negativeInfinity / one).IsNegativeInfinity());
            Assert.True((negativeInfinity / negativeOne).IsPositiveInfinity());
            Assert.True((one / positiveInfinity).IsZero());
            Assert.True((one / negativeInfinity).IsZero());
            Assert.True((positiveInfinity / positiveInfinity).IsNaN());
            Assert.True((negativeInfinity / negativeInfinity).IsNaN());
            Assert.True((positiveInfinity / negativeInfinity).IsNaN());
            Assert.True((negativeInfinity / positiveInfinity).IsNaN());
        }
    }
}
