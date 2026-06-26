using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// FQuaternion&lt;Fixed32&gt; 四元数运算基准
    /// <para>四元数 Normalize 间接依赖 Sqrt+Reciprocal（优化5/16），
    /// Slerp 依赖 Sin/Acos（优化3/4），AngleAxis 依赖 Sin/Cos（优化3）。</para>
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class QuaternionBenchmarks
    {
        private FQuaternion<Fixed32>[] _qA = null!;      // 单位四元数
        private FQuaternion<Fixed32>[] _qB = null!;      // 单位四元数
        private FQuaternion<Fixed32>[] _rawQuats = null!; // 非单位四元数（用于 Normalize）
        private Fixed32[] _t = null!;                     // 插值参数 [0, 1]
        private Fixed32[] _angles = null!;                // 角度 [0, 360)
        private FVector3<Fixed32>[] _axes = null!;        // 旋转轴

        [GlobalSetup]
        public void Setup()
        {
            _qA = BenchmarkData.GenerateUnitQuaternion(seed: 500);
            _qB = BenchmarkData.GenerateUnitQuaternion(seed: 501);
            _rawQuats = BenchmarkData.GenerateQuaternion(-1.0, 1.0, seed: 502);
            _t = BenchmarkData.GenerateFixed32(0.0, 1.0, seed: 503);
            _angles = BenchmarkData.GenerateFixed32(0.0, 360.0, seed: 504);
            _axes = BenchmarkData.GenerateVector3(-1.0, 1.0, seed: 505);
        }

        // ── 基线：加法 ───────────────────────────────────────────

        [Benchmark(Baseline = true), BenchmarkCategory("Add")]
        public FQuaternion<Fixed32> Add()
        {
            var acc = new FQuaternion<Fixed32>();
            var a = _qA; var b = _qB;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] + b[i];
            }
            return acc;
        }

        // ── 四元数乘法（Hamilton 积）：16 次 Mul + 12 次 Add/Sub ─

        [Benchmark, BenchmarkCategory("Multiply")]
        public FQuaternion<Fixed32> Multiply()
        {
            var acc = new FQuaternion<Fixed32>();
            var a = _qA; var b = _qB;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] * b[i];
            }
            return acc;
        }

        // ── Normalize：优化5+16（Sqrt+Reciprocal）的间接目标 ───

        /// <summary>Normalize — 内部计算模长后取倒数（1 次 Sqrt + 1 次 Reciprocal）</summary>
        [Benchmark, BenchmarkCategory("Normalize")]
        public FQuaternion<Fixed32> Normalize()
        {
            var acc = new FQuaternion<Fixed32>();
            var q = _rawQuats;
            for (int i = 0; i < q.Length; i++)
            {
                acc += FQuaternion<Fixed32>.Normalize(q[i]);
            }
            return acc;
        }

        // ── Slerp：优化3+4（Sin/Acos）的间接目标 ────────────────

        /// <summary>Slerp — 球面线性插值（内部含 Acos + Sin）</summary>
        [Benchmark, BenchmarkCategory("Slerp")]
        public FQuaternion<Fixed32> Slerp()
        {
            var acc = new FQuaternion<Fixed32>();
            var a = _qA; var b = _qB; var t = _t;
            for (int i = 0; i < a.Length; i++)
            {
                acc += FQuaternion<Fixed32>.Slerp(a[i], b[i], t[i]);
            }
            return acc;
        }

        /// <summary>Lerp — 线性插值（无三角函数，作为 Slerp 对照）</summary>
        [Benchmark, BenchmarkCategory("Slerp")]
        public FQuaternion<Fixed32> Lerp()
        {
            var acc = new FQuaternion<Fixed32>();
            var a = _qA; var b = _qB; var t = _t;
            for (int i = 0; i < a.Length; i++)
            {
                acc += FQuaternion<Fixed32>.Lerp(a[i], b[i], t[i]);
            }
            return acc;
        }

        // ── AngleAxis：优化3（Sin/Cos）的间接目标 ───────────────

        /// <summary>AngleAxis — 从轴角构造四元数（内部含 Sin/Cos）</summary>
        [Benchmark, BenchmarkCategory("AngleAxis")]
        public FQuaternion<Fixed32> AngleAxis()
        {
            var acc = new FQuaternion<Fixed32>();
            var ang = _angles; var ax = _axes;
            for (int i = 0; i < ang.Length; i++)
            {
                acc += FQuaternion<Fixed32>.AngleAxis(ang[i], ax[i]);
            }
            return acc;
        }

        // ── Inverse / Dot ────────────────────────────────────────

        [Benchmark, BenchmarkCategory("Inverse")]
        public FQuaternion<Fixed32> Inverse()
        {
            var acc = new FQuaternion<Fixed32>();
            var q = _qA;
            for (int i = 0; i < q.Length; i++)
            {
                acc += FQuaternion<Fixed32>.Inverse(q[i]);
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Dot")]
        public Fixed32 Dot()
        {
            var acc = Fixed32.Zero;
            var a = _qA; var b = _qB;
            for (int i = 0; i < a.Length; i++)
            {
                acc += FQuaternion<Fixed32>.Dot(a[i], b[i]);
            }
            return acc;
        }
    }
}
