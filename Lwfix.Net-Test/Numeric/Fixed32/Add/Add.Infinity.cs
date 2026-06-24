using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 加法 - 极值
    /// </summary>
    public partial class TAdd
    {
        [Fact]
        public void Infinity()
        {
            var k = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue);
            var w = new Fixed32(k);

            // 正无穷加上任何数，都等于正无穷
            Assert.True(double.IsPositiveInfinity(double.PositiveInfinity + k));
            Assert.True(Fixed32.IsPositiveInfinity(Fixed32.PositiveInfinity + w));
            // 负无穷加上任何数，都等于负无穷
            Assert.True(double.IsNegativeInfinity(double.NegativeInfinity + k));
            Assert.True(Fixed32.IsNegativeInfinity(Fixed32.NegativeInfinity + w));
        }
    }
}
