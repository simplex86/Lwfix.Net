using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// Fixed32 超越函数基准
    /// <para>对应优化项：
    /// - 优化6：Exp 固定迭代 + raw 算术（已完成，3.60x 加速）
    /// - 优化P0-1：Log2 UInt128 平方 + lzcnt 归一化（已完成，3.00x 加速）
    /// - 优化P0-2：Pow2 简化为 (x*Ln2).Exp()（已完成，3.73x 加速）
    /// - 优化P1：Log10 保持 / Ln10（乘法替代有 1 ULP 精度损失，测试不通过）</para>
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

        // ── Log：P0-1 Log2 优化已完成（UInt128 平方 + lzcnt，3.00x 加速）──
        //    Log = Log2 * Ln2（2.72x）  Log10 = Log / Ln10（2.34x）

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

        // ── Pow：P0-2 Pow2 优化已完成（(x*Ln2).Exp()，3.73x 加速）──────
        //    Pow_Fixed 走 Log+Exp 链，间接受益（1.89x）

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

        /// <summary>2 的幂（(x*Ln2).Exp()，复用已优化的 Exp）</summary>
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
