using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 乘法 - NaN
    /// </summary>
    public partial class TMul
    {
        [Fact]
        public void NaN()
        {
            var k = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue); // 任意整数
            var w = new Fixed32(k); // 任意

            // NaN乘以任何书，等于NaN
            Assert.True(Fixed32.IsNaN(Fixed32.NaN * w));

            // 正无穷乘以零，等于NaN
            Assert.True(Fixed32.IsNaN(Fixed32.PositiveInfinity * 0));
            Assert.True(Fixed32.IsNaN(Fixed32.PositiveInfinity * Fixed32.Zero));

            // 负无穷乘以零，等于NaN
            Assert.True(Fixed32.IsNaN(Fixed32.NegativeInfinity * 0));
            Assert.True(Fixed32.IsNaN(Fixed32.NegativeInfinity * Fixed32.Zero));
        }
    }
}
