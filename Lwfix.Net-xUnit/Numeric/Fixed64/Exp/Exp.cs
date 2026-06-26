using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TExp64
    {
        private readonly static List<double> normal_numbers =
        [
            0.23,
            1.05,
            3.7,
            -1.4,
            -7.35,
        ];
        private const double TOLERANCE = 10e-5;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f = new Fixed64(n);
                Assert.Equal(Math.Exp(n), Fixed64.Exp(f).ToDouble(), TOLERANCE);
            }
        }
    }
}
