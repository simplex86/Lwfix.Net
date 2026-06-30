using Xunit;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.Test.Numerics
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

        // Atan 精度容差：Q32.32 分辨率 ~2.3e-10，28 项级数 + 累积截断误差 < 1e-7
        private const double ATAN_TOLERANCE = 1e-5;

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

        [Fact]
        public void Atan_Normal()
        {
            // 覆盖各量级：零、单位圆内、单位圆外（触发 invert 路径）、正负值
            double[] testValues =
            [
                0.0, 1.0, -1.0,
                0.5, -0.5, 0.1, -0.1, 0.9, -0.9,
                0.01, -0.01, 0.001,
                2.0, -2.0, 10.0, -10.0, 100.0,
            ];

            foreach (var n in testValues)
            {
                var expected = Math.Atan(n);
                var actual = Fixed32.Atan(new Fixed32(n)).ToDouble();
                Assert.Equal(expected, actual, ATAN_TOLERANCE);
            }
        }

        [Fact]
        public void Atan_SpecialValues()
        {
            Assert.Equal(0.0, Fixed32.Atan(Fixed32.Zero).ToDouble(), ATAN_TOLERANCE);

            // Atan(1) = π/4, Atan(-1) = -π/4
            Assert.Equal(Math.PI / 4, Fixed32.Atan(Fixed32.One).ToDouble(), ATAN_TOLERANCE);
            Assert.Equal(-Math.PI / 4, Fixed32.Atan(Fixed32.NegativeOne).ToDouble(), ATAN_TOLERANCE);
        }

        [Fact]
        public void Atan_Random()
        {
            // 随机测试 1000 次，覆盖 [-100, 100] 范围
            for (int i = 0; i < 1000; i++)
            {
                var n = (System.Random.Shared.NextDouble() * 2 - 1) * 100;
                var expected = Math.Atan(n);
                var actual = Fixed32.Atan(new Fixed32(n)).ToDouble();
                Assert.Equal(expected, actual, ATAN_TOLERANCE);
            }
        }
    }
}
