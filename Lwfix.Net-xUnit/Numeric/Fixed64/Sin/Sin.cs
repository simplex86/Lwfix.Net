using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TSin64
    {
        private readonly static List<double> normal_numbers =
        [
            Fixed64.Quarter_PI.ToDouble(),
            Fixed64.Half_PI.ToDouble(),
            Fixed64.PI.ToDouble(),
            Fixed64.Two_PI.ToDouble(),
            -13.784,
            26.358,
            -906.786,
            979.358,
        ];
        private const double TOLERANCE = 10e-6;
        private const double FAST_TOLERANCE = 10e-4;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f = new Fixed64(n);

                var s1 = Math.Sin(n);
                var s2 = Fixed64.Sin(f);
                var s3 = Fixed64.FastSin(f);

                Assert.Equal(s1, s2.ToDouble(), TOLERANCE);
                Assert.Equal(s1, s3.ToDouble(), FAST_TOLERANCE);
            }
        }
    }
}
