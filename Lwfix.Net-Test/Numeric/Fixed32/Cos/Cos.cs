using Xunit;
using SimplexLab.Fixed;

namespace Test.Numerics
{
    public partial class TCos
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

        private const double TOLERANCE = 10e-5;
        private const double FAST_TOLERANCE = 10e-5;

        [Fact]
        public void Normal()
        {
            foreach (var n in normal_numbers)
            {
                var f = new Fixed32(n);

                var c1 = Math.Cos(n);
                var c2 = Fixed32.Cos(f);
                var c3 = Fixed32.FastCos(f);

                Assert.Equal(c1, c2.ToDouble(), TOLERANCE);
                Assert.Equal(c1, c3.ToDouble(), FAST_TOLERANCE);
            }
        }
    }
}
