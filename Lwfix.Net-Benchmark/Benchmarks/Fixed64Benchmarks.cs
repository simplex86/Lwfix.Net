using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// Fixed64 算术运算基准
    /// <para>Fixed64 使用 Int128 存储（Q64.64），与 Fixed32（Q32.32）形成对比。
    /// 对应优化项：
    /// - 优化2：Div 用硬件除法替换逐位长除法（Fixed64.Div 同样使用逐位长除法）
    /// - 优化16：Reciprocal 重新评估牛顿迭代</para>
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class Fixed64Benchmarks
    {
        private Fixed64[] _a = null!;
        private Fixed64[] _b = null!;
        private Fixed64[] _divisors = null!;
        private Fixed64[] _pow2Divisors = null!;
        private Fixed64[] _positive = null!;

        [GlobalSetup]
        public void Setup()
        {
            _a = BenchmarkData.Fixed64MixedSign();
            _b = BenchmarkData.GenerateFixed64(-100.0, 100.0, seed: 401);
            _divisors = BenchmarkData.Fixed64NonPowerOfTwoDivisors();
            _pow2Divisors = BenchmarkData.Fixed64PowerOfTwoDivisors();
            _positive = BenchmarkData.GenerateFixed64(0.001, 100.0, seed: 1200);
        }

        // ── 基线：加/减/乘 ───────────────────────────────────────

        [Benchmark(Baseline = true), BenchmarkCategory("Add")]
        public Fixed64 Add()
        {
            var acc = Fixed64.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] + b[i];
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Sub")]
        public Fixed64 Sub()
        {
            var acc = Fixed64.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] - b[i];
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Mul")]
        public Fixed64 Mul()
        {
            var acc = Fixed64.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] * b[i];
            }
            return acc;
        }

        // ── 除法：优化2 的 Fixed64 对照 ──────────────────────────

        /// <summary>Fixed64 除以非 2 的幂（逐位长除法慢路径）</summary>
        [Benchmark, BenchmarkCategory("Div")]
        public Fixed64 Div_NonPowerOfTwo()
        {
            var acc = Fixed64.Zero;
            var a = _a; var d = _divisors;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] / d[i];
            }
            return acc;
        }

        /// <summary>Fixed64 除以 2 的幂（快速路径 — 右移）</summary>
        [Benchmark, BenchmarkCategory("Div")]
        public Fixed64 Div_PowerOfTwo()
        {
            var acc = Fixed64.Zero;
            var a = _a; var d = _pow2Divisors;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] / d[i];
            }
            return acc;
        }

        // ── 倒数：优化16 的 Fixed64 对照 ─────────────────────────

        [Benchmark, BenchmarkCategory("Reciprocal")]
        public Fixed64 Reciprocal()
        {
            var acc = Fixed64.Zero;
            var d = _divisors;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Reciprocal();
            }
            return acc;
        }

        // ── Sqrt：优化5 的 Fixed64 对照 ──────────────────────────

        [Benchmark, BenchmarkCategory("Sqrt")]
        public Fixed64 Sqrt()
        {
            var acc = Fixed64.Zero;
            var d = _positive;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Sqrt();
            }
            return acc;
        }
    }
}
