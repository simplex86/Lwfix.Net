using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TTan
    {
        private readonly static List<double> normal_numbers =
        [
            Fixed32.Quarter_PI.ToDouble(),
            0.1,
            26.358,
            -13.784,
            89.0,
            -906.786,
            979.358,
        ];

        private const double TOLERANCE = 4e-3;
        private const double FAST_TOLERANCE = 4e-3;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f = new Fixed32(n);

                var s1 = Math.Tan(n);
                var s2 = Fixed32.Tan(f);
                var s3 = Fixed32.FastTan(f);

                Assert.Equal(s1, s2.ToDouble(), TOLERANCE);
                Assert.Equal(s1, s3.ToDouble(), FAST_TOLERANCE);
            }
        }
    }
}
