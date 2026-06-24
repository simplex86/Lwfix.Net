using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TMul
    {
        [Fact]
        public void MinMax()
        {
            var p = System.Random.Shared.NextDouble() * System.Random.Shared.Next(1, int.MaxValue); // 正整数
            var n = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, 0); // 负整数

            var u = new Fixed32(p); // 正
            var v = new Fixed32(n); // 负

            // 最大值乘以任何正整数，等于正无穷
            Assert.True(double.IsPositiveInfinity(double.MaxValue * p));
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.MaxValue * u));
            // 最大值乘以任何负整数，等于负无穷
            Assert.True(double.IsNegativeInfinity(double.MaxValue * n));
            Assert.True(Fixed32.IsNegativeInfinity(Fixed32.MaxValue * v));
            // 最大值乘以零，等于零
            Assert.True(double.MaxValue * 0 == 0);
            Assert.True(Fixed32.IsZero(Fixed32.MaxValue * 0));
            Assert.True(Fixed32.IsZero(Fixed32.MaxValue * Fixed32.Zero));

            // 最小值乘以任何正整数，等于负无穷
            Assert.True(double.IsNegativeInfinity(double.MinValue * p));
            Assert.True(Fixed32.IsNegativeInfinity(Fixed32.MinValue * u));
            // 最小值乘以任何负整数，等于正无穷
            Assert.True(double.IsPositiveInfinity(double.MinValue * n));
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.MinValue * v));
            // 最小值乘以零，等于零
            Assert.True(double.MinValue * 0 == 0);
            Assert.True(Fixed32.IsZero(Fixed32.MinValue * 0));
            Assert.True(Fixed32.IsZero(Fixed32.MinValue * Fixed32.Zero));
        }
    }
}
