using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// Fixed32 算术运算基准
    /// <para>对应优化项：
    /// - 优化2：Div 用硬件 long 除法替换逐位长除法
    /// - 优化16：Reciprocal 重新评估牛顿迭代</para>
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class ArithmeticBenchmarks
    {
        private Fixed32[] _a = null!;
        private Fixed32[] _b = null!;
        private Fixed32[] _divisors = null!;
        private Fixed32[] _pow2Divisors = null!;

        [GlobalSetup]
        public void Setup()
        {
            _a = BenchmarkData.MixedSign();
            _b = BenchmarkData.GenerateFixed32(-100.0, 100.0, seed: 401);
            _divisors = BenchmarkData.NonPowerOfTwoDivisors();
            _pow2Divisors = BenchmarkData.PowerOfTwoDivisors();
        }

        // ── 基线：加/减/乘（用于对比 Div 的相对开销）──────────────

        [Benchmark(Baseline = true), BenchmarkCategory("Add")]
        public Fixed32 Add()
        {
            var acc = Fixed32.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] + b[i];
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Sub")]
        public Fixed32 Sub()
        {
            var acc = Fixed32.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] - b[i];
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Mul")]
        public Fixed32 Mul()
        {
            var acc = Fixed32.Zero;
            var a = _a; var b = _b;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] * b[i];
            }
            return acc;
        }

        // ── 除法：优化2 的核心目标 ────────────────────────────────

        /// <summary>除以非 2 的幂（触发逐位长除法慢路径 — 优化2 的主目标）</summary>
        [Benchmark, BenchmarkCategory("Div")]
        public Fixed32 Div_NonPowerOfTwo()
        {
            var acc = Fixed32.Zero;
            var a = _a; var d = _divisors;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] / d[i];
            }
            return acc;
        }

        /// <summary>除以 2 的幂（已有快速路径 — 右移）</summary>
        [Benchmark, BenchmarkCategory("Div")]
        public Fixed32 Div_PowerOfTwo()
        {
            var acc = Fixed32.Zero;
            var a = _a; var d = _pow2Divisors;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] / d[i];
            }
            return acc;
        }

        /// <summary>除以常数 7（JIT 可能无法消除的通用路径）</summary>
        [Benchmark, BenchmarkCategory("Div")]
        public Fixed32 Div_ByConstant7()
        {
            var seven = (Fixed32)7;
            var acc = Fixed32.Zero;
            var a = _a;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] / seven;
            }
            return acc;
        }

        /// <summary>除以整数（走 operator /(Fixed32, int) 路径）</summary>
        [Benchmark, BenchmarkCategory("Div")]
        public Fixed32 Div_ByInt()
        {
            var acc = Fixed32.Zero;
            var a = _a;
            var d = _divisors;
            for (int i = 0; i < a.Length; i++)
            {
                acc += a[i] / (int)d[i];
            }
            return acc;
        }

        // ── 倒数：优化16 的目标 ───────────────────────────────────

        /// <summary>Reciprocal 当前实现 = 牛顿迭代（UInt128 中间运算 + 1 次硬件除法初始估计）</summary>
        [Benchmark, BenchmarkCategory("Reciprocal")]
        public Fixed32 Reciprocal()
        {
            var acc = Fixed32.Zero;
            var d = _divisors;
            for (int i = 0; i < d.Length; i++)
            {
                acc += d[i].Reciprocal();
            }
            return acc;
        }

        /// <summary>FMath.Reciprocal 泛型入口</summary>
        [Benchmark, BenchmarkCategory("Reciprocal")]
        public Fixed32 Reciprocal_FMath()
        {
            var acc = Fixed32.Zero;
            var d = _divisors;
            for (int i = 0; i < d.Length; i++)
            {
                acc += FMath.Reciprocal(d[i]);
            }
            return acc;
        }

        /// <summary>手动 1/x（走完整 Div — 优化前 Reciprocal 的基线实现）</summary>
        [Benchmark, BenchmarkCategory("Reciprocal")]
        public Fixed32 Reciprocal_Manual()
        {
            var acc = Fixed32.Zero;
            var d = _divisors;
            var one = Fixed32.One;
            for (int i = 0; i < d.Length; i++)
            {
                acc += one / d[i];
            }
            return acc;
        }
    }
}
