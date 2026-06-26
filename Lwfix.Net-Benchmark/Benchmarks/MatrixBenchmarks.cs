using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// FMatrix3x3&lt;Fixed32&gt; 矩阵运算基准
    /// <para>矩阵 Inverse 间接依赖 Div（优化2），AngleAxis 依赖 Sin/Cos（优化3）。
    /// 矩阵乘法是纯 Mul+Add，作为基线对照。</para>
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class MatrixBenchmarks
    {
        private FMatrix3x3<Fixed32>[] _a = null!;
        private FMatrix3x3<Fixed32>[] _b = null!;
        private Fixed32[] _angles = null!;
        private FVector3<Fixed32>[] _axes = null!;

        [GlobalSetup]
        public void Setup()
        {
            // 使用 [-1, 1] 范围避免乘法累加溢出
            _a = BenchmarkData.GenerateMatrix3x3(-1.0, 1.0, seed: 300);
            _b = BenchmarkData.GenerateMatrix3x3(-1.0, 1.0, seed: 301);
            _angles = BenchmarkData.Angles();
            _axes = BenchmarkData.GenerateVector3(-1.0, 1.0, seed: 302);
        }

        // ── 基线：加法 ───────────────────────────────────────────

        [Benchmark(Baseline = true), BenchmarkCategory("Add")]
        public FMatrix3x3<Fixed32> Add()
        {
            var acc = new FMatrix3x3<Fixed32>();
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] + b[i];
            }
            return acc;
        }

        // ── 矩阵乘法：27 次 Mul + 18 次 Add ──────────────────────

        [Benchmark, BenchmarkCategory("Multiply")]
        public FMatrix3x3<Fixed32> Multiply()
        {
            var acc = new FMatrix3x3<Fixed32>();
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] * b[i];
            }
            return acc;
        }

        // ── 转置 ─────────────────────────────────────────────────

        [Benchmark, BenchmarkCategory("Transpose")]
        public FMatrix3x3<Fixed32> Transpose()
        {
            var acc = new FMatrix3x3<Fixed32>();
            var a = _a;
            for (int i = 0; i < a.Length; i++)
            {
                acc += FMatrix3x3<Fixed32>.Transpose(a[i]);
            }
            return acc;
        }

        // ── 行列式：优化2（Div）的间接目标 ──────────────────────

        [Benchmark, BenchmarkCategory("Determinant")]
        public Fixed32 Determinant()
        {
            var acc = Fixed32.Zero;
            var a = _a;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i].Determinant;
            }
            return acc;
        }

        // ── 逆矩阵：含 Div（优化2） ──────────────────────────────

        /// <summary>Inverse — 内部计算行列式后做除法（1/Det × 余子式）</summary>
        [Benchmark, BenchmarkCategory("Inverse")]
        public FMatrix3x3<Fixed32> Inverse()
        {
            var acc = new FMatrix3x3<Fixed32>();
            var a = _a;
            for (int i = 0; i < a.Length; i++)
            {
                acc += FMatrix3x3<Fixed32>.Inverse(a[i]);
            }
            return acc;
        }

        // ── 旋转变换：依赖 Sin/Cos（优化3） ──────────────────────

        /// <summary>Rotate — 2D 旋转矩阵（内部调用 Sin/Cos）</summary>
        [Benchmark, BenchmarkCategory("Rotate")]
        public FMatrix3x3<Fixed32> Rotate()
        {
            var acc = new FMatrix3x3<Fixed32>();
            var a = _angles;
            for (int i = 0; i < a.Length; i++)
            {
                acc += FMatrix3x3<Fixed32>.Rotate(a[i]);
            }
            return acc;
        }

        /// <summary>AngleAxis — 3D 轴角旋转矩阵（内部调用 Sin/Cos）</summary>
        [Benchmark, BenchmarkCategory("AngleAxis")]
        public FMatrix3x3<Fixed32> AngleAxis()
        {
            var acc = new FMatrix3x3<Fixed32>();
            var ang = _angles; var ax = _axes;
            for (int i = 0; i < ang.Length; i++)
            {
                acc += FMatrix3x3<Fixed32>.AngleAxis(ang[i], ax[i]);
            }
            return acc;
        }
    }
}
