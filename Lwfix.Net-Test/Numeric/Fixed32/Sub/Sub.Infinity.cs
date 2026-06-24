using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 减法 - 极值
    /// </summary>
    public partial class TSub
    {
        [Fact]
        public void Infinity()
        {
            var k = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue); // 任意整数
            var w = new Fixed32(k);

            // 负无穷减去任何数，等于负无穷
            Assert.True(double.IsNegativeInfinity(double.NegativeInfinity - k));
            Assert.True(Fixed32.IsNegativeInfinity(Fixed32.NegativeInfinity - w));
            // 任何数减去负无穷，等于正无穷
            Assert.True(double.IsPositiveInfinity(k - double.NegativeInfinity));
            Assert.True(Fixed32.IsPositiveInfinity(w - Fixed32.NegativeInfinity));
            // 正无穷减去任何数，等于正无穷
            Assert.True(double.IsPositiveInfinity(double.PositiveInfinity - k));
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.PositiveInfinity - w));
            // 任何数减去正无穷，等于负无穷
            Assert.True(double.IsNegativeInfinity(k - double.PositiveInfinity));
            Assert.True(Fixed32.IsNegativeInfinity(w - Fixed32.PositiveInfinity));
        }
    }
}
