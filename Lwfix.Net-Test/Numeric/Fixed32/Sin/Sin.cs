using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TSin
    {
        private readonly static List<double> normal_numbers =
        [
            Fixed32.Quarter_PI.ToDouble(),
            Fixed32.Half_PI.ToDouble(),
            Fixed32.PI.ToDouble(),
            Fixed32.Two_PI.ToDouble(),
            -13.784,
            26.358,
            -906.786,
            979.358,
        ];
        private const double TOLERANCE = 10e-7;
        private const double FAST_TOLERANCE = 10e-5;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f = new Fixed32(n);

                var s1 = Math.Sin(n);
                var s2 = Fixed32.Sin(f);
                var s3 = Fixed32.FastSin(f);

                Assert.Equal(s1, s2.ToDouble(), TOLERANCE);
                Assert.Equal(s1, s3.ToDouble(), FAST_TOLERANCE);
            }
        }
    }
}
