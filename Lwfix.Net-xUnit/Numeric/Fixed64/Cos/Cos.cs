using Xunit;
using SimplexLab.Fixed;

namespace LwfixTest.Fixed.Numerics
{
    public partial class TCos64
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

                var c1 = Math.Cos(n);
                var c2 = Fixed64.Cos(f);
                var c3 = Fixed64.FastCos(f);

                Assert.Equal(c1, c2.ToDouble(), TOLERANCE);
                Assert.Equal(c1, c3.ToDouble(), FAST_TOLERANCE);
            }
        }
    }
}
