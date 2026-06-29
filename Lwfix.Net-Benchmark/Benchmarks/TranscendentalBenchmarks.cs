using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// Fixed32 超越函数基准
    /// <para>对应优化项：
    /// - 优化6：Exp 固定迭代 + raw 算术（已完成，3.60x 加速）</para>
    /// Cbrt 见 SqrtBenchmarks（优化7）
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class TranscendentalBenchmarks
    {
        private Fixed32[] _expInput = null!;    // [-2, 2]
        private Fixed32[] _positive = null!;    // (0, 100]
        private Fixed32[] _base = null!;        // [1, 10)
        private Fixed32[] _exponent = null!;    // [0, 5)

        [GlobalSetup]
        public void Setup()
        {
            _expInput = BenchmarkData.ExpInput();
            _positive = BenchmarkData.Positive();
            _base = BenchmarkData.GenerateFixed32(1.0, 10.0, seed: 1300);
            _exponent = BenchmarkData.GenerateFixed32(0.0, 5.0, seed: 1400);
        }

        // ── Exp：优化6 已完成（固定迭代 + raw 算术，3.60x 加速）────

        [Benchmark(Baseline = true), BenchmarkCategory("Exp")]
        public Fixed32 Exp()
        {
            var acc = Fixed32.Zero;
            var d = _expInput;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Exp();
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Exp")]
        public Fixed32 Exp_Static()
        {
            var acc = Fixed32.Zero;
            var d = _expInput;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Exp(d[i]);
            }
            return acc;
        }

        // ── Log（Cbrt 内部调用，优化7 的间接目标）────────────────

        [Benchmark, BenchmarkCategory("Log")]
        public Fixed32 Log()
        {
            var acc = Fixed32.Zero;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Log();
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Log")]
        public Fixed32 Log2()
        {
            var acc = Fixed32.Zero;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Log2();
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Log")]
        public Fixed32 Log10()
        {
            var acc = Fixed32.Zero;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Log10();
            }
            return acc;
        }

        // ── Pow ───────────────────────────────────────────────────

        /// <summary>整数幂（快速幂，O(log n) 次乘法）</summary>
        [Benchmark, BenchmarkCategory("Pow")]
        public Fixed32 Pow_Int()
        {
            var acc = Fixed32.Zero;
            var d = _base;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Pow(5);
            }
            return acc;
        }

        /// <summary>定点数幂（走 Log + Exp 链）</summary>
        [Benchmark, BenchmarkCategory("Pow")]
        public Fixed32 Pow_Fixed()
        {
            var acc = Fixed32.Zero;
            var b = _base; var e = _exponent;
            for (int i = 0; i < b.Length; i++)
            {
                acc += b[i].Pow(e[i]);
            }
            return acc;
        }

        /// <summary>2 的幂（泰勒级数 + 位移）</summary>
        [Benchmark, BenchmarkCategory("Pow")]
        public Fixed32 Pow2()
        {
            var acc = Fixed32.Zero;
            var d = _exponent;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.One.Pow2(d[i]);
            }
            return acc;
        }
    }
}
