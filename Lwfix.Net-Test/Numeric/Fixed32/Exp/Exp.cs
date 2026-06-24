using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TExp
    {
        private readonly static List<double> normal_numbers =
        [
            0.23,
            1.05,
            3.7,
            -1.4,
            -7.35,
        ];
        private const double TOLERANCE = 10e-4;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f = new Fixed32(n);
                Assert.Equal(Math.Exp(n), Fixed32.Exp(f).ToDouble(), TOLERANCE);
            }
        }
    }
}
