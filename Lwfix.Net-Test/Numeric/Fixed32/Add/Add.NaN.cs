using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 加法 - NaN
    /// </summary>
    public partial class TAdd
    {
        [Fact]
        public void NaN()
        {
            var k = System.Random.Shared.NextDouble() * System.Random.Shared.Next(int.MinValue, int.MaxValue);
            var w = new Fixed32(k);

            // NaN加上任何数，都等于NaN
            Assert.True(double.IsNaN(double.NaN + k));
            Assert.True(Fixed32.IsNaN(Fixed32.NaN + w));

            // 正无穷加上负无穷，等于NaN
            Assert.True(double.IsNaN(double.PositiveInfinity + double.NegativeInfinity));
            Assert.True(Fixed32.IsNaN(Fixed32.PositiveInfinity + Fixed32.NegativeInfinity));
        }
    }
}
