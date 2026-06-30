using System;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 正切
    /// <para>包含定点数的正切函数实现，包括标准实现和快速实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>1/3 常量</summary>
        private static readonly Fixed32 T3  = FromRaw(1431655765); // 1/3 
        /// <summary>2/15 常量</summary>
        private static readonly Fixed32 T5  = FromRaw(572662306);  // 2/15
        /// <summary>17/315 常量</summary>
        private static readonly Fixed32 T7  = FromRaw(231791886);  // 17/315
        /// <summary>62/2835 常量</summary>
        private static readonly Fixed32 T9  = FromRaw(93928738);   // 62/2835
        /// <summary>1382/155925 常量</summary>
        private static readonly Fixed32 T11 = FromRaw(38067306);   // 1382/155925
        /// <summary>21844/6081075 常量</summary>
        private static readonly Fixed32 T13 = FromRaw(15428072);   // 21844/6081075
        /// <summary>929569/638512875 常量</summary>
        private static readonly Fixed32 T15 = FromRaw(6252761);    // 929569/638512875
        /// <summary>6404582/10854718875 常量</summary>
        private static readonly Fixed32 T17 = FromRaw(2534149);    // 6404582/10854718875
        /// <summary>44361662/1338557227875 常量</summary>
        private static readonly Fixed32 T19 = FromRaw(1027052);    // 44361662/1338557227875
        /// <summary>188684306/11097486871875 常量</summary>
        private static readonly Fixed32 T21 = FromRaw(415920);     // 188684306/11097486871875

        /// <summary>
        /// 计算正切值
        /// <para>使用泰勒展开计算定点数的正切值</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正切值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷或负无穷，返回NaN</item>
        /// <item>如果输入是0，返回0</item>
        /// <item>如果输入是π/2，返回正无穷大</item>
        /// <item>如果输入是-π/2，返回负无穷大</item>
        /// <item>如果输入是π/4，返回1</item>
        /// </list>
        /// 注意：
        /// <list type="bullet">
        /// <item>将radian规范化到[-π/2, π/2]范围内</item>
        /// <item>其值越接近(±π/2)误差越大</item>
        /// <item>经测试，与(±π/2)差值小于0.0017时，误差将大于0.1</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Tan(Fixed32 radian)
        {
            if (PreprocessTan(radian, out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian, PI);
            var referenced = ReduceRadian4Tan(normalized, out var sign);

            if (referenced == Zero)       return Zero;
            if (referenced == Half_PI)    return sign ? MinValue : MaxValue;
            if (referenced == Quarter_PI) return sign ? NegativeOne : One;

            var result = Zero;
            if (referenced < Quarter_PI)
            {
                result = TaylorEvaluate4Tan(referenced);
            }
            else
            {
                // 使用cotangent来计算接近π/2的角度，提高精度
                var temp = Half_PI - referenced;
                var cot = TaylorEvaluate4Tan(temp);
                result = cot.Reciprocal();
            }

            return sign ? -result : result;
        }

        /// <summary>
        /// 将角度缩减到[0, π/2]区间
        /// <para>利用正切函数的对称性，将角度缩减到[0, π/2]区间</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="sign">正切值的符号</param>
        /// <returns>缩减到[0, π/2]区间的角度</returns>
        internal static Fixed32 ReduceRadian4Tan(Fixed32 radian, out bool sign)
        {
            sign = false;

            var referenced = radian;
            if (referenced > Half_PI)
            {
                sign = true;
                referenced = PI - referenced;
            }

            return referenced;
        }

        /// <summary>
        /// 使用泰勒展开计算正切值
        /// <para>在[0, π/4]区间内使用泰勒展开计算正切值</para>
        /// </summary>
        /// <param name="x">角度（弧度），范围在[0, π/4]之间</param>
        /// <returns>正切值</returns>
        private static Fixed32 TaylorEvaluate4Tan(Fixed32 x)
        {
            var x1  = x;
            var x2  = x1 * x1;
            var x3  = x1 * x2;
            var x5  = x3 * x2;
            var x7  = x5 * x2;
            var x9  = x7 * x2;
            var x11 = x9 * x2;
            var x13 = x11 * x2;
            var x15 = x13 * x2;
            var x17 = x15 * x2;
            var x19 = x17 * x2;
            var x21 = x19 * x2;

            return x1        + 
                   x3  * T3  + 
                   x5  * T5  + 
                   x7  * T7  + 
                   x9  * T9  + 
                   x11 * T11 + 
                   x13 * T13 + 
                   x15 * T15 + 
                   x17 * T17 + 
                   x19 * T19 +
                   x21 * T21;
        }

        /// <summary>
        /// 快速计算正切值
        /// <para>使用查表法快速计算正切值，速度比Tan函数快，但精度较低</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正切值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷或负无穷，返回NaN</item>
        /// <item>如果输入是0，返回0</item>
        /// <item>如果输入是π/2，返回正无穷大</item>
        /// <item>如果输入是-π/2，返回负无穷大</item>
        /// <item>如果输入是π/4，返回1</item>
        /// </list>
        /// 注意：
        /// <list type="bullet">
        /// <item>将radian规范化到[-π/2, π/2]范围内</item>
        /// <item>其值越接近(±π/2)误差越大</item>
        /// <item>误差大于Tan函数，但计算速度比Tan函数更快</item>
        /// </list>
        /// 优化15：位移索引（原 Mul+Div）+ UInt128 线性插值（原 Fixed32 Mul）。
        /// <list type="bullet">
        /// <item>索引：raw &gt;&gt; 19（原 referenced×(Len-1)/Half_PI，Fixed32 Mul+Div ~70ns）</item>
        /// <item>插值：UInt128 (frac×delta)&gt;&gt;19（原 Fixed32 Mul，4 项分解+溢出检查）
        /// —— tan 在 π/2 附近 delta 可接近 MaxValue，frac×delta 可达 2^82，long 溢出，
        /// 故用 UInt128（JIT 编译为 mulx 硬件指令）</item>
        /// <item>LUT 步长 2^19（原均匀角度映射），瘦身 16x（205887→12868）；
        /// 索引计算从 Fixed32 Div（逐位长除法~33 次迭代）变为单次位移，且消除了原映射的舍入误差</item>
        /// <item>末项 MaxValue 哨兵：π/2 处 tan→∞，前向插值在末桶自动趋向 MaxValue</item>
        /// </list>
        /// 精度不衰减（索引计算更精确、插值精度等价）、平台一致（纯整数运算，UInt128 结果确定）。
        /// </remarks>
        public static Fixed32 FastTan(Fixed32 radian)
        {
            if (PreprocessTan(radian, out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian, PI);
            var referenced = ReduceRadian4Tan(normalized, out var sign);

            if (referenced == Zero)       return Zero;
            if (referenced == Half_PI)    return sign ? MinValue : MaxValue;
            if (referenced == Quarter_PI) return sign ? NegativeOne : One;

            // 优化15：位移索引 + UInt128 线性插值
            const int SHIFT = 19;
            const long MASK = (1L << SHIFT) - 1;

            var raw = referenced.rawvalue;
            int index = (int)(raw >> SHIFT);
            if (index >= TanLut.Length - 1)
            {
                // 越界（理论上不会到达，referenced < Half_PI 已由上面 == Half_PI 守卫）
                return sign ? MinValue : MaxValue;
            }

            var v0 = (ulong)TanLut[index];
            var v1 = (ulong)TanLut[index + 1];
            var frac = (ulong)(raw & MASK);
            // 线性插值：v0 + frac/2^19 × (v1 - v0)
            // tan 单调递增，v1 > v0，delta > 0；末桶 v1 = MaxValue 哨兵
            // frac < 2^19，delta 可达 ~2^63（末桶），frac×delta 可达 ~2^82 → 需 UInt128
            var interp128 = (UInt128)v0 + (((UInt128)frac * (v1 - v0)) >> SHIFT);
            // 钳位到 MaxValue（末桶插值可能超出）
            long interp = interp128 > (UInt128)long.MaxValue ? long.MaxValue : (long)interp128;
            return sign ? -FromRaw(interp) : FromRaw(interp);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理正切函数的特殊输入情况</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessTan(Fixed32 radian, out Fixed32 r)
        {
            if (radian.IsNaN() ||
                radian.IsPositiveInfinity() ||
                radian.IsNegativeInfinity())
            {
                r = NaN;
                return true;
            }

            r = Zero;
            return false;
        }

        /// <summary>
        /// 计算反正切值
        /// <para>计算定点数的反正切值</para>
        /// </summary>
        /// <param name="z">正切值</param>
        /// <returns>反正切值，范围在[-π/2, π/2]之间</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是0，返回0</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Atan(Fixed32 z)
        {
            if (z.IsZero()) return Zero;

            // Force positive values for argument: Atan(-z) = -Atan(z).
            var neg = z.IsNegative();
            if (neg) z = -z;

            var invert = z > One;
            if (invert) z = z.Reciprocal();

            var result = One;
            var term = One;
            var two = Two;
            var three = new Fixed32(3);

            var sq1 = z * z;
            var sq2 = sq1 * two;
            var sqp1 = sq1 + One;
            var sqp2 = sqp1 * two;
            var dividend = sq2;
            var divisor = sqp1 * three;

            // 优化4：使用 UInt128 MulDiv 替代循环内逐位长除法
            // 原始：term *= dividend / divisor（Div 逐位长除法 + Mul 截断，两次误差源）
            // 优化：term.raw = (term.raw * dividend.raw) / divisor.raw（硬件除法，单次截断）
            // 精度更优（单次截断 vs. 原始的舍入+截断），平台一致（纯整数运算）
            for (var i = 2; i < 30; ++i)
            {
                // 所有值均为正（z 已取正且 ≤1），divisor ≥ 3*(1+z²) > 0，乘积不溢出 UInt128
                term = FromRaw((long)((UInt128)(ulong)term.rawvalue * (ulong)dividend.rawvalue / (ulong)divisor.rawvalue));
                result += term;

                dividend += sq2;
                divisor += sqp2;

                if (term.IsZero()) break;
            }

            result = result * z / sqp1;
            if (invert) result = Half_PI - result;

            return neg ? -result : result;
        }

        /// <summary>
        /// 计算 y/x 的反正切值
        /// <para>计算两个定点数的商的反正切值</para>
        /// </summary>
        /// <param name="y">分子</param>
        /// <param name="x">分母</param>
        /// <returns>反正切值，范围在 [-π, π]</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果x为0且y>0，返回π/2</item>
        /// <item>如果x为0且y=0，返回0</item>
        /// <item>如果x为0且y<0，返回-π/2</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Atan2(Fixed32 y, Fixed32 x)
        {
            var yl = y.rawvalue;
            var xl = x.rawvalue;

            if (xl == 0)
            {
                if (yl > 0)  return Half_PI;
                if (yl == 0) return Zero;
                return -Half_PI;
            }
            
            var z = y / x;
            var sm = new Fixed32(0.28125); // 7/24，用于提高精度的常数
            
            // 处理溢出情况
            var temp = sm * z * z;
            if (One + temp == MaxValue)
            {
                return y < Zero ? -Half_PI : Half_PI;
            }

            var atan = Zero;
            if (Abs(z) < One)
            {
                atan = z / (One + sm * z * z);
                if (xl < 0) return (yl < 0) ? atan - PI : atan + PI;
            }
            else
            {
                atan = Half_PI - z / (z * z + sm);
                if (yl < 0) return atan - PI;
            }

            return atan;
        }
    }
}
