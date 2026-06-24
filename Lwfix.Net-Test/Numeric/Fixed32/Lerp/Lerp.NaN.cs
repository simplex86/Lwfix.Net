using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TLerp
    {
        [Fact]
        public void NaN()
        {
            var n1 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(MIN_NUMBER, MAX_NUMBER);
            var n2 = System.Random.Shared.NextDouble() * System.Random.Shared.Next(MIN_NUMBER, MAX_NUMBER);
            var n3 = System.Random.Shared.NextDouble();

            var f1 = new Fixed32(n1);
            var f2 = new Fixed32(n2);
            var f3 = new Fixed32(n3);

            Assert.True(double.IsNaN(double.Lerp(double.NaN, n2, n3)));
            Assert.True(Fixed32.IsNaN(Fixed32.Lerp(Fixed32.NaN, f2, f3)));
            
            Assert.True(double.IsNaN(double.Lerp(n1, double.NaN, n3)));
            Assert.True(Fixed32.IsNaN(Fixed32.Lerp(f1, Fixed32.NaN, f3)));
            
            Assert.True(double.IsNaN(double.Lerp(n1, n2, double.NaN)));
            Assert.True(Fixed32.IsNaN(Fixed32.Lerp(f1, f2, Fixed32.NaN)));
        }
    }
}
