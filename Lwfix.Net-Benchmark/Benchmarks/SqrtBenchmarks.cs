using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// Fixed32 平方根与立方根基准
    /// <para>对应优化项：
    /// - 优化5：Sqrt 牛顿迭代（已完成，8.33x 加速）
    /// - 优化7：Cbrt 牛顿迭代（已完成，14.41x 加速）</para>
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class SqrtBenchmarks
    {
        private Fixed32[] _positive = null!;
        private Fixed32[] _small = null!;
        private Fixed32[] _large = null!;

        [GlobalSetup]
        public void Setup()
        {
            _positive = BenchmarkData.Positive();
            _small = BenchmarkData.GenerateFixed32(0.001, 1.0, seed: 1201);
            _large = BenchmarkData.Large();
        }

        // ── 平方根：优化5 已完成（牛顿迭代，8.33x 加速）──────────────

        [Benchmark(Baseline = true), BenchmarkCategory("Sqrt")]
        public Fixed32 Sqrt_Mixed()
        {
            var acc = Fixed32.Zero;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Sqrt();
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Sqrt")]
        public Fixed32 Sqrt_Small()
        {
            var acc = Fixed32.Zero;
            var d = _small;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Sqrt();
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Sqrt")]
        public Fixed32 Sqrt_Large()
        {
            var acc = Fixed32.Zero;
            var d = _large;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Sqrt();
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Sqrt")]
        public Fixed32 Sqrt_Static()
        {
            var acc = Fixed32.Zero;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Sqrt(d[i]);
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Sqrt")]
        public Fixed32 Sqrt_FMath()
        {
            var acc = Fixed32.Zero;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += FMath.Sqrt(d[i]);
            }
            return acc;
        }

        // ── 立方根：优化7 已完成（牛顿迭代，14.41x 加速）──────────

        [Benchmark(Baseline = true), BenchmarkCategory("Cbrt")]
        public Fixed32 Cbrt_Mixed()
        {
            var acc = Fixed32.Zero;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Cbrt();
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Cbrt")]
        public Fixed32 Cbrt_Small()
        {
            var acc = Fixed32.Zero;
            var d = _small;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Cbrt();
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Cbrt")]
        public Fixed32 Cbrt_FMath()
        {
            var acc = Fixed32.Zero;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += FMath.Cbrt(d[i]);
            }
            return acc;
        }

        // ── 对比：Sqrt vs Cbrt vs 1/Sqrt（RSqrt 隐含模式）─────────

        /// <summary>1/Sqrt — 物理引擎 Normalize 的核心模式</summary>
        [Benchmark, BenchmarkCategory("RSqrt")]
        public Fixed32 InverseSqrt()
        {
            var acc = Fixed32.Zero;
            var one = Fixed32.One;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += one / d[i].Sqrt();
            }
            return acc;
        }
    }
}
