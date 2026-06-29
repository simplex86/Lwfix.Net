namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 幂
    /// <para>包含定点数的幂运算相关方法，包括整数次幂、任意次幂、2的幂等操作</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 计算整数次幂
        /// <para>使用快速幂算法计算定点数的整数次幂</para>
        /// </summary>
        /// <param name="n">指数，整数</param>
        /// <returns>幂值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>任何数的0次幂，返回1</item>
        /// <item>0的负数次幂，返回正无穷</item>
        /// <item>-1的无穷次幂，返回1</item>
        /// <item>纯小数的正无穷次幂，返回0</item>
        /// <item>纯小数的负无穷次幂，返回正无穷</item>
        /// <item>非纯小数的正无穷次幂，返回正无穷</item>
        /// <item>非纯小数的负无穷次幂，返回0</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>对于负指数，先计算倒数</item>
        /// <item>使用快速幂算法（二进制分解）计算幂值</item>
        /// </list>
        /// </remarks>
        public Fixed32 Pow(int n)
        {
            var n_rawvalue = Int32ToRaw(n);
            if (PreprocessLog(rawvalue, n_rawvalue, out var r))
            {
                return r;
            }
        
            var m = this;
            if (n < 0)
            {
                m = Reciprocal();
                n = -n;
            }
        
            var c = One;
            while (n > 0)
            {
                if (n % 2 == 1)
                {
                    c *= m;
                }
                m *= m;
                n /= 2;
            }
        
            return c;
        }

        /// <summary>
        /// 计算整数次幂
        /// <para>静态方法，计算定点数的整数次幂</para>
        /// </summary>
        /// <param name="m">底数</param>
        /// <param name="n">指数，整数</param>
        /// <returns>幂值</returns>
        public static Fixed32 Pow(Fixed32 m, int n)
        {
            return FMath.Pow(m, n);
        }

        /// <summary>
        /// 计算任意次幂
        /// <para>计算定点数的任意次幂</para>
        /// </summary>
        /// <param name="n">指数</param>
        /// <returns>幂值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>任何数的0次幂，返回1</item>
        /// <item>负数的小数次幂，返回NaN</item>
        /// <item>0的负数次幂，返回正无穷</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>对于小数指数，使用公式 m^n = e^(n * ln(m))</item>
        /// <item>对于整数指数，调用Pow(int n)方法</item>
        /// </list>
        /// </remarks>
        public Fixed32 Pow(Fixed32 n)
        {
            if (PreprocessLog(rawvalue, n.rawvalue, out var r))
            {
                return r;
            }

            if (n.IsFractional())
            {
                if (IsNegative()) return NaN;
                return (n * Log()).Exp(); // m^n = e^(n * ln(m))
            }

            return Pow(n.ToInt());
        }

        /// <summary>
        /// 计算任意次幂
        /// <para>静态方法，计算定点数的任意次幂</para>
        /// </summary>
        /// <param name="m">底数</param>
        /// <param name="n">指数</param>
        /// <returns>幂值</returns>
        public static Fixed32 Pow(Fixed32 m, Fixed32 n)
        {
            return FMath.Pow(m, n);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理幂函数的特殊输入情况</para>
        /// </summary>
        /// <param name="m">底数的原始存储值</param>
        /// <param name="n">指数的原始存储值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessLog(long m, long n, out Fixed32 r)
        {
            // 有NaN参与的运算，都等于NaN
            if (m.IsNaN() || n.IsNaN()) { r = NaN; return true; }
            // 任何数（非NaN）的0次幂，都等于1
            if (n.IsZero()) { r = One; return true; }
            // 负数的小数次幂，等于NaN
            if (m < 0 && n.IsFractional()) { r = NaN; return true; }
            if (m.IsZero()) { r = (n < 0) ? PositiveInfinity : Zero; return true; }
            // 1的任何次幂都等于1
            if (m.IsOne()) { r = One; return true; }
            if (m.IsNegativeOne() && (n.IsInfinity())) { r = One; return true; }
            if (m.IsPureFractional())
            {
                if (n.IsPositiveInfinity()) { r = Zero; return true; }
                if (n.IsNegativeInfinity()) { r = PositiveInfinity; return true; }
            }
            else // if (!m.IsPureFractional())
            {
                if (n.IsPositiveInfinity()) { r = PositiveInfinity; return true; }
                if (n.IsNegativeInfinity()) { r = Zero; return true; }
            }

            r = Zero;
            return false;
        }

        /// <summary>
        /// 计算2的幂
        /// <para>计算2的定点数次幂</para>
        /// </summary>
        /// <param name="x">指数</param>
        /// <returns>2的幂值</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>利用恒等式 2^x = e^(x·ln2)，直接复用已优化的 Exp()</item>
        /// <item>Exp 内部已做范围归约：x·ln2 分解为 k·ln2 + r，|r| ≤ 0.5·ln2</item>
        /// <item>NaN/Infinity/Zero 等边界值由 Exp 的预处理逻辑自动处理</item>
        /// </list>
        /// </remarks>
        public Fixed32 Pow2(Fixed32 x)
        {
            // 2^x = e^(x * ln2)
            // Exp 的范围归约保证 |residual| ≤ 0.5·ln2，对任意 x 均成立
            return (x * Ln2).Exp();
        }

        /// <summary>
        /// 是否为2的幂
        /// <para>判断定点数是否为2的幂</para>
        /// </summary>
        /// <returns>是否为2的幂</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>非正数不可能是2的幂</item>
        /// <item>使用位运算判断：(n & (n-1)) == 0</item>
        /// </list>
        /// </remarks>
        public bool IsPowerOfTwo()
        {
            if (rawvalue <= 0) return false;
            return (rawvalue & (rawvalue - 1)) == 0;
        }

        /// <summary>
        /// 是否为2的幂
        /// <para>静态方法，判断定点数是否为2的幂</para>
        /// </summary>
        /// <param name="value">要判断的定点数</param>
        /// <returns>是否为2的幂</returns>
        public static bool IsPowerOfTwo(Fixed32 value)
        {
            return FMath.IsPowerOfTwo(value);
        }

        /// <summary>
        /// 最接近的2的幂
        /// <para>计算最接近当前定点数的2的幂</para>
        /// </summary>
        /// <returns>最接近的2的幂</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>非正数返回1</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>找到最高有效位的位置</item>
        /// <item>计算上下两个2的幂</item>
        /// <item>返回距离更近的那个</item>
        /// </list>
        /// </remarks>
        public Fixed32 ClosestPowerOfTwo()
        {
            // 非正数的情况，返回最小的2^0
            if (rawvalue <= 0)
            {
                return One;
            }

            var raw = (ulong)rawvalue;
            var pos = TOTAL_BITS - 1;

            // 找到最高有效位的位置
            while (pos >= 0 && (raw & (1UL << pos)) == 0)
            {
                pos--;
            }

            var k = pos - FRACTIONAL_BITS; // 计算指数k=最高位-定点数偏移
            var lower = (long)(1UL << (k + FRACTIONAL_BITS)); // 下界2^k的Q32.32表示

            // 检查上界2^(k+1)是否可表示
            var valid = (k + FRACTIONAL_BITS + 1) < 64;
            var upper = valid ? (long)(1UL << (k + FRACTIONAL_BITS + 1)) : -1;

            // 比较距离选择最近值
            if (valid)
            {
                var diffLower = rawvalue - lower;
                var diffUpper = upper - rawvalue;

                return (diffLower < diffUpper) ? FromRaw(lower)
                                               : FromRaw(upper);
            }

            return FromRaw(lower); // 上界溢出时返回下界
        }

        /// <summary>
        /// 最接近的2的幂
        /// <para>静态方法，计算最接近定点数的2的幂</para>
        /// </summary>
        /// <param name="value">要计算的定点数</param>
        /// <returns>最接近的2的幂</returns>
        public static Fixed32 ClosestPowerOfTwo(Fixed32 value)
        {
            return FMath.ClosestPowerOfTwo(value);
        }

        /// <summary>
        /// 下一个2的幂
        /// <para>计算大于当前定点数的最小2的幂</para>
        /// </summary>
        /// <returns>下一个2的幂</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>找到最高有效位的位置</item>
        /// <item>返回该位置+1的2的幂</item>
        /// </list>
        /// </remarks>
        public Fixed32 NextPowerOfTwo()
        {
            var raw = (ulong)rawvalue;
            var pos = TOTAL_BITS - 1;

            // 找到最高有效位的位置
            while (pos >= 0 && (raw & (1UL << pos)) == 0)
            {
                pos--;
            }

            return FromRaw(1 << (pos + 1));
        }

        /// <summary>
        /// 下一个2的幂
        /// <para>静态方法，计算大于定点数的最小2的幂</para>
        /// </summary>
        /// <param name="value">要计算的定点数</param>
        /// <returns>下一个2的幂</returns>
        public static Fixed32 NextPowerOfTwo(Fixed32 value)
        {
            return FMath.NextPowerOfTwo(value);
        }
    }
}
