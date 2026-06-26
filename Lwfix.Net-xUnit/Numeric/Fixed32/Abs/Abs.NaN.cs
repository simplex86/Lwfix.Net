using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
{
    public partial class TAbs
    {
        [Fact]
        public void NaN()
        {
            Assert.True(double.IsNaN(Math.Abs(double.NaN)));
            Assert.True(Fixed32.IsNaN(Fixed32.Abs(Fixed32.NaN)));
        }
    }
}
