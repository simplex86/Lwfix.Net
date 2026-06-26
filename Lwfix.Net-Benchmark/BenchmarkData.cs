using SimplexLab.Lwfix;

namespace SimplexLab.Lwfix.TBenchmark
{
    /// <summary>
    /// 基准测试数据生成器
    /// <para>使用固定种子的伪随机数生成器，确保基准测试的可重复性。
    /// 所有数据在 GlobalSetup 中一次性生成，不影响计时。</para>
    /// </summary>
    internal static class BenchmarkData
    {
        /// <summary>默认数据量</summary>
        public const int DefaultCount = 1024;

        // ── Fixed32 数据 ──────────────────────────────────────────

        /// <summary>
        /// 生成 [min, max) 范围内的 Fixed32 数组
        /// </summary>
        public static Fixed32[] GenerateFixed32(double min, double max, int count = DefaultCount, int seed = 42)
        {
            var rng = new Random(seed);
            var data = new Fixed32[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = (Fixed32)(rng.NextDouble() * (max - min) + min);
            }
            return data;
        }

        /// <summary>小数 [0, 1) — 三角函数、归一化常用区间</summary>
        public static Fixed32[] SmallFractional(int count = DefaultCount)
            => GenerateFixed32(0.0, 1.0, count, seed: 100);

        /// <summary>中等值 [1, 100) — 一般算术</summary>
        public static Fixed32[] Medium(int count = DefaultCount)
            => GenerateFixed32(1.0, 100.0, count, seed: 200);

        /// <summary>大值 [100, 10000) — 压力测试</summary>
        public static Fixed32[] Large(int count = DefaultCount)
            => GenerateFixed32(100.0, 10000.0, count, seed: 300);

        /// <summary>混合正负 [-100, 100)</summary>
        public static Fixed32[] MixedSign(int count = DefaultCount)
            => GenerateFixed32(-100.0, 100.0, count, seed: 400);

        /// <summary>角度 [0, 2π) — 三角函数输入</summary>
        public static Fixed32[] Angles(int count = DefaultCount)
            => GenerateFixed32(0.0, 6.283185307179586, count, seed: 500);

        /// <summary>角度 [-π, π) — 三角函数输入（含负角）</summary>
        public static Fixed32[] AnglesSigned(int count = DefaultCount)
            => GenerateFixed32(-3.141592653589793, 3.141592653589793, count, seed: 600);

        /// <summary>
        /// 非零除数 [1, 100)，排除 2 的幂以触发逐位长除法慢路径
        /// </summary>
        public static Fixed32[] NonPowerOfTwoDivisors(int count = DefaultCount)
        {
            var rng = new Random(700);
            var data = new Fixed32[count];
            for (int i = 0; i < count; i++)
            {
                // 生成 1..100 之间的整数，跳过 2 的幂
                int v;
                do { v = rng.Next(1, 101); }
                while ((v & (v - 1)) == 0); // 排除 2 的幂
                data[i] = (Fixed32)v;
            }
            return data;
        }

        /// <summary>
        /// 2 的幂除数 — 触发 Div 快速路径（右移）
        /// </summary>
        public static Fixed32[] PowerOfTwoDivisors(int count = DefaultCount)
        {
            var rng = new Random(800);
            var data = new Fixed32[count];
            for (int i = 0; i < count; i++)
            {
                int pow = rng.Next(0, 7); // 1, 2, 4, 8, 16, 32, 64
                data[i] = (Fixed32)(1 << pow);
            }
            return data;
        }

        /// <summary>[-1, 1] 范围 — Atan / Asin / Acos 输入</summary>
        public static Fixed32[] UnitRange(int count = DefaultCount)
            => GenerateFixed32(-1.0, 1.0, count, seed: 900);

        /// <summary>[-10, 10] 范围 — Atan 大值输入（触发 invert 路径）</summary>
        public static Fixed32[] AtanInput(int count = DefaultCount)
            => GenerateFixed32(-10.0, 10.0, count, seed: 1000);

        /// <summary>小指数 [-2, 2] — Exp 输入（避免溢出）</summary>
        public static Fixed32[] ExpInput(int count = DefaultCount)
            => GenerateFixed32(-2.0, 2.0, count, seed: 1100);

        /// <summary>正数 (0, 100] — Log / Sqrt / Cbrt 输入</summary>
        public static Fixed32[] Positive(int count = DefaultCount)
            => GenerateFixed32(0.001, 100.0, count, seed: 1200);

        // ── Fixed64 数据 ──────────────────────────────────────────

        /// <summary>生成 [min, max) 范围内的 Fixed64 数组</summary>
        public static Fixed64[] GenerateFixed64(double min, double max, int count = DefaultCount, int seed = 42)
        {
            var rng = new Random(seed);
            var data = new Fixed64[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = (Fixed64)(rng.NextDouble() * (max - min) + min);
            }
            return data;
        }

        /// <summary>Fixed64 中等值 [1, 100)</summary>
        public static Fixed64[] Fixed64Medium(int count = DefaultCount)
            => GenerateFixed64(1.0, 100.0, count, seed: 200);

        /// <summary>Fixed64 非零除数 [1, 100)，排除 2 的幂</summary>
        public static Fixed64[] Fixed64NonPowerOfTwoDivisors(int count = DefaultCount)
        {
            var rng = new Random(700);
            var data = new Fixed64[count];
            for (int i = 0; i < count; i++)
            {
                int v;
                do { v = rng.Next(1, 101); }
                while ((v & (v - 1)) == 0);
                data[i] = (Fixed64)v;
            }
            return data;
        }

        /// <summary>Fixed64 混合正负 [-100, 100)</summary>
        public static Fixed64[] Fixed64MixedSign(int count = DefaultCount)
            => GenerateFixed64(-100.0, 100.0, count, seed: 400);

        /// <summary>Fixed64 2 的幂除数 — 触发 Div 快速路径</summary>
        public static Fixed64[] Fixed64PowerOfTwoDivisors(int count = DefaultCount)
        {
            var rng = new Random(800);
            var data = new Fixed64[count];
            for (int i = 0; i < count; i++)
            {
                int pow = rng.Next(0, 7);
                data[i] = (Fixed64)(1 << pow);
            }
            return data;
        }

        // ── 向量数据 ──────────────────────────────────────────────

        /// <summary>生成 FVector3&lt;Fixed32&gt; 数组</summary>
        public static FVector3<Fixed32>[] GenerateVector3(double min, double max, int count = DefaultCount, int seed = 42)
        {
            var rng = new Random(seed);
            var data = new FVector3<Fixed32>[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = new FVector3<Fixed32>(
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min));
            }
            return data;
        }

        // ── 矩阵数据 ──────────────────────────────────────────────

        /// <summary>生成 FMatrix3x3&lt;Fixed32&gt; 数组（各分量在 [min, max) 范围内）</summary>
        public static FMatrix3x3<Fixed32>[] GenerateMatrix3x3(double min, double max, int count = DefaultCount, int seed = 42)
        {
            var rng = new Random(seed);
            var data = new FMatrix3x3<Fixed32>[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = new FMatrix3x3<Fixed32>(
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min));
            }
            return data;
        }

        // ── 四元数数据 ────────────────────────────────────────────

        /// <summary>生成 FQuaternion&lt;Fixed32&gt; 数组（各分量在 [min, max) 范围内）</summary>
        public static FQuaternion<Fixed32>[] GenerateQuaternion(double min, double max, int count = DefaultCount, int seed = 42)
        {
            var rng = new Random(seed);
            var data = new FQuaternion<Fixed32>[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = new FQuaternion<Fixed32>(
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min),
                    (Fixed32)(rng.NextDouble() * (max - min) + min));
            }
            return data;
        }

        /// <summary>生成单位四元数数组（通过 AngleAxis 构造，确保可逆）</summary>
        public static FQuaternion<Fixed32>[] GenerateUnitQuaternion(int count = DefaultCount, int seed = 42)
        {
            var rng = new Random(seed);
            var data = new FQuaternion<Fixed32>[count];
            var axes = GenerateVector3(-1.0, 1.0, count, seed + 1);
            for (int i = 0; i < count; i++)
            {
                var angle = (Fixed32)(rng.NextDouble() * 360.0);
                data[i] = FQuaternion<Fixed32>.AngleAxis(angle, axes[i]);
            }
            return data;
        }
    }
}
