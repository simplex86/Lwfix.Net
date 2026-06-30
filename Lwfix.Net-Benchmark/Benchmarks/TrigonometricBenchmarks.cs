using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark.Benchmarks
{
    /// <summary>
    /// Fixed32 三角函数基准
    /// <para>已完成优化（P0→P1 顺序实施，Short job 实测，AMD Ryzen 7 5700G）：</para>
    /// <para>■ P0（核心路径重写，大幅加速）</para>
    /// <list type="bullet">
    /// <item>P0-1 NormalizeRadian：硬件 % 替代逐位长除法 + Mul。NormalizeRadian 53.06→10.86 μs (4.89x)</item>
    /// <item>P0-2 TaylorEvaluate4Sin：UInt128 原始乘法替代 11 次 Mul（4 项分解+溢出检查）。Sin 270.66→41.4 μs (6.5x)</item>
    /// <item>P0-3 ReduceRadian4Sin：比较链替代硬件 idiv（边界用 PI.raw 倍数保证比特级等价）</item>
    /// </list>
    /// <para>■ P1（架构清理 + 融合，边际收益）</para>
    /// <list type="bullet">
    /// <item>P1-1 Cos/FastCos：提取 SinFromNormalized/FastSinFromNormalized，跳过 Sin(radian+Half_PI) 内冗余 PreprocessSin。Cos 51.1 μs（中性，Preprocess≈1ns 可忽略）</item>
    /// <item>P1-2 SinCos 真融合：Fixed32.SinCos 一次 Preprocess + 一次 NormalizeRadian，cos 复用归一化结果（+Half_PI 单次条件减法）。FMath.SinCos(Fixed32) 非泛型重载优先匹配。
    /// SinCos_FMath 81.4 μs vs SinCos_Separate 79.2 μs —— 节省 1 次 NormalizeRadian(~10ns) 被 16 字节元组返回开销(~10ns) 抵消，净中性。
    /// 瓶颈为 2× TaylorEvaluate4Sin（两路径相同）。精度不衰减（19 测试通过）。</item>
    /// </list>
    /// <para>■ 其他已完成</para>
    /// <list type="bullet">
    /// <item>Atan：UInt128 MulDiv 替代循环内逐位长除法（2.34x）</item>
    /// <item>优化14 SinLut 瘦身+线性插值：步长 2^15→2^19（205887→12868 条目，瘦身 16x），
    /// 最近邻→线性插值（long 运算）。FastSin 精度 ~3.8e-6→~1.9e-9（提升 2000x）</item>
    /// <item>优化15 FastTan 位移索引：索引 referenced×(Len-1)/Half_PI（Fixed32 Mul+Div ~70ns）→raw>>19（单次位移）；
    /// 插值 Fixed32 Mul→UInt128 (frac×delta)>>19。TanLut 均匀角度映射→步长 2^19（瘦身 16x），
    /// 消除原索引映射舍入误差。精度不衰减、平台一致（纯整数运算）</item>
    /// </list>
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

        /// <summary>FMath.SinCos（P1-2 真融合：FMath.SinCos(Fixed32) 非泛型重载 → Fixed32.SinCos，1 次 Preprocess + 1 次 NormalizeRadian）</summary>
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

        /// <summary>手动分别调 Sin 和 Cos（P1-2 对比基线：2 次 Preprocess + 2 次 NormalizeRadian）</summary>
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

        /// <summary>FastSin — 优化14 已完成（步长 2^15→2^19，瘦身 16x；最近邻→线性插值，精度 ~3.8e-6→~1.9e-9）</summary>
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

        /// <summary>FastTan — 优化15 已完成（位移索引 raw>>19 替代 Mul+Div；UInt128 线性插值；TanLut 瘦身 16x）</summary>
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
