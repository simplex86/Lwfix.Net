using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// FVector3&lt;Fixed32&gt; 向量运算基准
    /// <para>向量的 Magnitude/Normalize 间接依赖 Sqrt+Div（优化2/5/16），
    /// 是物理引擎高频调用的复合操作。</para>
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class VectorBenchmarks
    {
        private FVector3<Fixed32>[] _a = null!;
        private FVector3<Fixed32>[] _b = null!;
        private Fixed32[] _scalars = null!;

        [GlobalSetup]
        public void Setup()
        {
            _a = BenchmarkData.GenerateVector3(-100.0, 100.0, seed: 200);
            _b = BenchmarkData.GenerateVector3(-100.0, 100.0, seed: 201);
            _scalars = BenchmarkData.Medium();
        }

        // ── 基线：加减乘 ─────────────────────────────────────────

        [Benchmark(Baseline = true), BenchmarkCategory("Add")]
        public FVector3<Fixed32> Add()
        {
            var acc = FVector3<Fixed32>.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] + b[i];
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Sub")]
        public FVector3<Fixed32> Sub()
        {
            var acc = FVector3<Fixed32>.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] - b[i];
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Mul")]
        public FVector3<Fixed32> ScalarMul()
        {
            var acc = FVector3<Fixed32>.Zero;
            var a = _a; var s = _scalars;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] * s[i];
            }
            return acc;
        }

        // ── Magnitude：优化5（Sqrt）的间接目标 ───────────────────

        [Benchmark, BenchmarkCategory("Magnitude")]
        public Fixed32 Magnitude()
        {
            var acc = Fixed32.Zero;
            var a = _a;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i].Magnitude;
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Magnitude")]
        public Fixed32 SqrMagnitude()
        {
            var acc = Fixed32.Zero;
            var a = _a;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i].SqrMagnitude;
            }
            return acc;
        }

        // ── Normalize：优化2+5+16 的复合目标 ────────────────────

        /// <summary>Normalize — 内部 = v / v.Magnitude（1 次 Sqrt + 3 次 Div）</summary>
        [Benchmark, BenchmarkCategory("Normalize")]
        public FVector3<Fixed32> Normalize()
        {
            var acc = FVector3<Fixed32>.Zero;
            var a = _a;
            for (int i = 0; i < a.Length; i++)
            {
                acc += FVector3<Fixed32>.Normalize(a[i]);
            }
            return acc;
        }

        /// <summary>Normalized 属性（等价于 Normalize，用于对比调用开销）</summary>
        [Benchmark, BenchmarkCategory("Normalize")]
        public FVector3<Fixed32> Normalized()
        {
            var acc = FVector3<Fixed32>.Zero;
            var a = _a;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i].Normalized;
            }
            return acc;
        }

        // ── Dot / Cross ──────────────────────────────────────────

        [Benchmark, BenchmarkCategory("Dot")]
        public Fixed32 Dot()
        {
            var acc = Fixed32.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += FVector3<Fixed32>.Dot(a[i], b[i]);
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Cross")]
        public FVector3<Fixed32> Cross()
        {
            var acc = FVector3<Fixed32>.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += FVector3<Fixed32>.Cross(a[i], b[i]);
            }
            return acc;
        }
    }
}
