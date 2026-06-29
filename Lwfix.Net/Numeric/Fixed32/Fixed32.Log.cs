using System;
using System.Numerics;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 对数
    /// <para>包含定点数的对数函数实现，包括自然对数、以2为底的对数和以10为底的对数</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /************************************************************************
         * 精度较低，弃用
         * 
        /// <summary>
        /// 自然对数（e为底）
        /// </summary>
        /// <returns></returns>
        public Fixed32 Log()
        {
            if (ProprocessLog(rawvalue, out var r))
            {
                return r;
            }

            // 1. 归一化到 [1, 2) 并记录指数
            var exponent = 0;
            var mantissa = this;

            while (mantissa >= Two)
            {
                mantissa = mantissa / Two;
                exponent++;
            }
            while (mantissa < One)
            {
                mantissa = mantissa * Two;
                exponent--;
            }
            var e = Ln2 * exponent;

            // 2. 计算 ln(mantissa) 的泰勒级数展开
            var x = mantissa - One;
            var p = x;
            var c = x;

            for (int i = 2; i < 300; i++)
            {
                p = p * x;
                c = (i % 2 == 0) ? c - p / i
                                 : c + p / i;
            }

            // 3. ln(n) = ln(mantissa) + exponent * ln(2)
            return c + e;
        }
        * 
        ************************************************************************/

        /// <summary>
        /// 自然对数（e为底）
        /// <para>计算定点数的自然对数</para>
        /// </summary>
        /// <returns>自然对数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN或负数，返回NaN</item>
        /// <item>如果输入是0，返回负无穷</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用Log2()计算以2为底的对数</item>
        /// <item>乘以ln(2)得到自然对数</item>
        /// </list>
        /// </remarks>
        public Fixed32 Log()
        {
            if (ProprocessLog(rawvalue, out var r))
            {
                return r;
            }

            return Log2() * Ln2;
        }

        /// <summary>
        /// 自然对数（e为底）
        /// <para>静态方法，计算定点数的自然对数</para>
        /// </summary>
        /// <param name="n">要计算自然对数的定点数</param>
        /// <returns>自然对数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN或负数，返回NaN</item>
        /// <item>如果输入是0，返回负无穷</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Log(Fixed32 n)
        {
            return n.Log();
        }

        /************************************************************************
         * 精度较低，弃用
         * 
        /// <summary>
        /// 以2为底的对数
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Fixed32 Log2()
        {
            if (ProprocessLog(rawvalue, out var r))
            {
                return r;
            }

            // 1. 归一化到 [1, 2) 并记录指数
            var exponent = 0;
            var mantissa = this;

            // 2.计算 ln(mantissa) 的泰勒级数展开
            while (mantissa >= Two)
            {
                mantissa = mantissa / Two;
                exponent++;
            }
            while (mantissa < One)
            {
                mantissa = mantissa * Two;
                exponent--;
            }

            // Now mantissa is in [1, 2)
            var x = mantissa - One;
            var p = x;
            var c = x;

            for (int i = 2; i < 300; i++)
            {
                p = p * x;
                c = (i % 2 == 0) ? c - p / i
                                 : c + p / i;
            }
            c = c / Ln2;

            return c + exponent;
        }
        * 
        ************************************************************************/

        /// <summary>
        /// 以2为底的对数
        /// <para>计算定点数的以2为底的对数</para>
        /// </summary>
        /// <returns>以2为底的对数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN或负数，返回NaN</item>
        /// <item>如果输入是0，返回负无穷</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>归一化输入值到[1, 2)区间</item>
        /// <item>使用二进制搜索算法计算对数的小数部分</item>
        /// <item>返回整数部分和小数部分的和</item>
        /// </list>
        /// </remarks>
        public Fixed32 Log2()
        {
            if (ProprocessLog(rawvalue, out var r))
            {
                return r;
            }

            var b = 1L << (FRACTIONAL_BITS - 1);
            var y = 0L;

            // 归一化到 [1, 2)，即 z_raw ∈ [2^32, 2^33)
            // 使用硬件 lzcnt 指令替换逐位移位 while 循环
            // 目标：最高有效位落在 bit 32，对应 lz = 31
            var z_raw = rawvalue;
            var lz = BitOperations.LeadingZeroCount((ulong)z_raw);
            var shift = lz - 31;     // 正：需左移（v<1）；负：需右移（v>=2）
            if (shift > 0)
            {
                z_raw <<= shift;
                y -= shift * One.rawvalue;
            }
            else if (shift < 0)
            {
                z_raw >>= -shift;
                y += (-shift) * One.rawvalue;
            }

            // 逐位算法：32 次迭代，每次平方 z 并检查是否 >= 2
            // z_raw ∈ [2^32, 2^33)，z_raw² ∈ [2^64, 2^66) — 用 UInt128 避免溢出
            // UInt128 在 .NET 8+ 被 JIT 编译为 64×64→128 硬件乘法指令（mulx）
            // 数值等价于 Fixed32.Mul(z, z)，但绕开了 4 项分解 + 溢出检查开销
            for (int i = 0; i < FRACTIONAL_BITS; i++)
            {
                z_raw = (long)((UInt128)(ulong)z_raw * (ulong)z_raw >> FRACTIONAL_BITS);
                if (z_raw >= Two.rawvalue)
                {
                    z_raw >>= 1;
                    y += b;
                }
                b >>= 1;
            }

            return FromRaw(y);
        }

        /// <summary>
        /// 以2为底的对数
        /// <para>静态方法，计算定点数的以2为底的对数</para>
        /// </summary>
        /// <param name="n">要计算以2为底的对数的定点数</param>
        /// <returns>以2为底的对数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN或负数，返回NaN</item>
        /// <item>如果输入是0，返回负无穷</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Log2(Fixed32 n)
        {
            return n.Log2();
        }

        /// <summary>
        /// 以10为底的对数
        /// <para>计算定点数的以10为底的对数</para>
        /// </summary>
        /// <returns>以10为底的对数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN或负数，返回NaN</item>
        /// <item>如果输入是0，返回负无穷</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用Log()计算自然对数</item>
        /// <item>除以 ln(10) 得到以10为底的对数</item>
        /// </list>
        /// </remarks>
        public Fixed32 Log10()
        {
            return Log() / Ln10;
        }

        /// <summary>
        /// 以10为底的对数
        /// <para>静态方法，计算定点数的以10为底的对数</para>
        /// </summary>
        /// <param name="n">要计算以10为底的对数的定点数</param>
        /// <returns>以10为底的对数值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN或负数，返回NaN</item>
        /// <item>如果输入是0，返回负无穷</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Log10(Fixed32 n)
        {
            return n.Log10();
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理对数函数的特殊输入情况</para>
        /// </summary>
        /// <param name="n">原始存储值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private bool ProprocessLog(long n, out Fixed32 r)
        {
            if (n.IsNaN() || n < 0) { r = NaN; return true; }
            if (n.IsZero()) { r = NegativeInfinity; return true; }
            if (n.IsPositiveInfinity()) { r = PositiveInfinity; return true; }

            r = Zero;
            return false;
        }
    }
}
