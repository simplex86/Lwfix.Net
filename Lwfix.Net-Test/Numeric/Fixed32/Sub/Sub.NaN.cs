using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 减法 - NaN
    /// </summary>
    public partial class TSub
    {
        [Fact]
        public void NaN()
        {
            var d = System.Random.Shared.NextDouble();
            var k = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue); // 任意整数
            var w = new Fixed32(k);

            // NaN减去任何数，等于NaN
            Assert.True(double.IsNaN(double.NaN - k));
            Assert.True(Fixed32.IsNaN(Fixed32.NaN - w));
            // 任何数减去NaN，等于NaN
            Assert.True(double.IsNaN(k - double.NaN));
            Assert.True(Fixed32.IsNaN(w - Fixed32.NaN));
            // 正无穷减去正无穷，等于NaN
            Assert.True(double.IsNaN(double.PositiveInfinity - double.PositiveInfinity));
            Assert.True(Fixed32.IsNaN(Fixed32.PositiveInfinity - Fixed32.PositiveInfinity));
            // 负无穷减去负无穷，等于NaN
            Assert.True(double.IsNaN(double.NegativeInfinity - double.NegativeInfinity));
            Assert.True(Fixed32.IsNaN(Fixed32.NegativeInfinity - Fixed32.NegativeInfinity));
        }
    }
}
