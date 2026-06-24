using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 加法 - 常规，检验准确性
    /// </summary>
    public partial class TAdd
    {
        private readonly static List<double[]> normal_numbers =
        [
            [65.506, 24.1307],
            [-16.3083, -28.2577],
            [15.5667775, -11.09],
            [44.92, -98.7889],
            [-631299.896, 909539.5],
            [-660730.7668, -788217.62],
            [395260.552, 610316.5062],
            [230618.374, -840202.24]
        ];
        private const double TOLERANCE = 10e-7;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var u = n[0];
                var v = n[1];

                var t = new Fixed32(u);
                var w = new Fixed32(v);

                Assert.Equal(u + v, (t + w).ToDouble(), TOLERANCE);
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

            // 0加任何数都应该是其本身
            Assert.Equal(zero, zero + zero);
            Assert.Equal(one, zero + one);
            Assert.Equal(negativeOne, zero + negativeOne);

            // 交换律测试
            Assert.Equal(one + zero, zero + one);
            Assert.Equal(negativeOne + one, one + negativeOne);

            // NaN测试
            Assert.True((nan + zero).IsNaN());
            Assert.True((nan + one).IsNaN());
            Assert.True((zero + nan).IsNaN());
            Assert.True((one + nan).IsNaN());

            // 无穷大测试
            Assert.True((positiveInfinity + zero).IsPositiveInfinity());
            Assert.True((positiveInfinity + one).IsPositiveInfinity());
            Assert.True((negativeInfinity + zero).IsNegativeInfinity());
            Assert.True((negativeInfinity + negativeOne).IsNegativeInfinity());
            Assert.True((positiveInfinity + negativeInfinity).IsNaN());
        }
    }
}
