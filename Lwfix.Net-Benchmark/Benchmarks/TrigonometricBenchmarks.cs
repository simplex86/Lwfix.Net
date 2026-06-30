using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// Fixed32 三角函数基准
    /// <para>对应优化项：
    /// - 优化3：FMath.SinCos 真正融合（当前为假融合，调 Sin 两次）
    /// - 优化4：Atan 去除循环内除法（已完成，UInt128 MulDiv 替代逐位长除法，2.34x 加速）
    /// - 优化14：SinLut 瘦身+线性插值（当前 20 万条无插值）
    /// - 优化15：FastTan 索引计算改用位移（当前含乘+除）</para>
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class TrigonometricBenchmarks
    {
        private Fixed32[] _angles = null!;       // [0, 2π)
        private Fixed32[] _anglesSigned = null!;  // [-π, π)
        private Fixed32[] _unitRange = null!;     // [-1, 1] for Atan/Asin/Acos
        private Fixed32[] _atanInput = null!;     // [-10, 10] for Atan invert path

        [GlobalSetup]
        public void Setup()
        {
            _angles = BenchmarkData.Angles();
            _anglesSigned = BenchmarkData.AnglesSigned();
            _unitRange = BenchmarkData.UnitRange();
            _atanInput = BenchmarkData.AtanInput();
        }

        // ── Sin / Cos / Tan 标准版（泰勒展开）────────────────────

        [Benchmark(Baseline = true), BenchmarkCategory("Sin")]
        public Fixed32 Sin()
        {
            var acc = Fixed32.Zero;
            var d = _angles;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Sin(d[i]);
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Cos")]
        public Fixed32 Cos()
        {
            var acc = Fixed32.Zero;
            var d = _angles;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Cos(d[i]);
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Tan")]
        public Fixed32 Tan()
        {
            var acc = Fixed32.Zero;
            var d = _anglesSigned;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Tan(d[i]);
            }
            return acc;
        }

        // ── SinCos 融合：优化3 的目标 ─────────────────────────────

        /// <summary>当前 FMath.SinCos（假融合 — 内部调 Sin 两次）</summary>
        [Benchmark, BenchmarkCategory("SinCos")]
        public Fixed32 SinCos_FMath()
        {
            var acc = Fixed32.Zero;
            var d = _angles;
            for (int i = 0; i < d.Length; i++)
            {
                var (sin, cos) = FMath.SinCos(d[i]);
                acc += sin + cos;
            }
            return acc;
        }

        /// <summary>手动分别调 Sin 和 Cos（等价于当前 SinCos，用于对比调用开销）</summary>
        [Benchmark, BenchmarkCategory("SinCos")]
        public Fixed32 SinCos_Separate()
        {
            var acc = Fixed32.Zero;
            var d = _angles;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Sin(d[i]) + Fixed32.Cos(d[i]);
            }
            return acc;
        }

        /// <summary>仅 Sin（用于估算 SinCos 融合后应达到的理论下限）</summary>
        [Benchmark, BenchmarkCategory("SinCos")]
        public Fixed32 SinCos_OnlySin()
        {
            var acc = Fixed32.Zero;
            var d = _angles;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Sin(d[i]);
            }
            return acc;
        }

        // ── FastSin / FastCos / FastTan：LUT 查表 ────────────────

        /// <summary>FastSin — 优化14 的目标（当前 20 万条 LUT 无插值）</summary>
        [Benchmark, BenchmarkCategory("FastSin")]
        public Fixed32 FastSin()
        {
            var acc = Fixed32.Zero;
            var d = _angles;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.FastSin(d[i]);
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("FastSin")]
        public Fixed32 FastCos()
        {
            var acc = Fixed32.Zero;
            var d = _angles;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.FastCos(d[i]);
            }
            return acc;
        }

        /// <summary>FastTan — 优化15 的目标（当前索引含乘+除）</summary>
        [Benchmark, BenchmarkCategory("FastTan")]
        public Fixed32 FastTan()
        {
            var acc = Fixed32.Zero;
            var d = _anglesSigned;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.FastTan(d[i]);
            }
            return acc;
        }

        // ── 反三角函数：优化4 已完成（UInt128 MulDiv 替代循环内逐位长除法）──

        /// <summary>Atan — 优化4 已完成（UInt128 MulDiv，2.34x 加速）</summary>
        [Benchmark, BenchmarkCategory("Atan")]
        public Fixed32 Atan()
        {
            var acc = Fixed32.Zero;
            var d = _unitRange;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Atan(d[i]);
            }
            return acc;
        }

        /// <summary>Atan 大值输入（触发 invert: z > 1 → z = 1/z，2.00x 加速）</summary>
        [Benchmark, BenchmarkCategory("Atan")]
        public Fixed32 Atan_LargeInput()
        {
            var acc = Fixed32.Zero;
            var d = _atanInput;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Atan(d[i]);
            }
            return acc;
        }

        /// <summary>Atan2（含 y/x 除法 + Atan）</summary>
        [Benchmark, BenchmarkCategory("Atan")]
        public Fixed32 Atan2()
        {
            var acc = Fixed32.Zero;
            var y = _unitRange;
            var x = _atanInput;
            for (int i = 0; i < y.Length; i++)
            {
                acc += Fixed32.Atan2(y[i], x[i]);
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Atan")]
        public Fixed32 Asin()
        {
            var acc = Fixed32.Zero;
            var d = _unitRange;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Asin(d[i]);
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("Atan")]
        public Fixed32 Acos()
        {
            var acc = Fixed32.Zero;
            var d = _unitRange;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.Acos(d[i]);
            }
            return acc;
        }

        // ── 角度归一化（三角函数内部调用）────────────────────────

        [Benchmark, BenchmarkCategory("NormalizeRadian")]
        public Fixed32 NormalizeRadian()
        {
            var acc = Fixed32.Zero;
            var d = _atanInput;
            for (int i = 0; i < d.Length; i++)
            {
                acc += Fixed32.NormalizeRadian(d[i]);
            }
            return acc;
        }

        [Benchmark, BenchmarkCategory("NormalizeRadian")]
        public Fixed32 NormalizeRadian_FMath()
        {
            var acc = Fixed32.Zero;
            var d = _atanInput;
            for (int i = 0; i < d.Length; i++)
            {
                acc += FMath.NormalizeRadian(d[i]);
            }
            return acc;
        }
    }
}
