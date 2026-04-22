namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 倒数
    /// <para>包含定点数的倒数计算实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /************************************************************************
         * 用牛顿迭代求解，但不知为啥误差比较大，弃用
         * 
        /// <summary>
        /// 倒数
        /// </summary>
        /// <returns></returns>
        public Fixed32 Reciprocal()
        {
            if (PreprocessReciprocal(rawvalue, out var r))
            {
                return r;
            }

            // 处理符号和绝对值
            var negative = rawvalue < 0;
            var value = (ulong)(negative ? -rawvalue : rawvalue);

            // 计算初始近似值（基于整数部分的倒数）
            var integer = value >> 32; // 提取整数部分的高32位
            if (integer == 0) integer = 1; // 防止除零

            // initial = (2^64 / integer) >> 32
            ulong quotient = (0x1000000000000000UL / integer) >> 32;
            var x = FromRaw((long)(quotient >> 32));

            // 牛顿迭代（3次迭代达到Q32.32精度）
            x = x * (Two - (this * x)); // 第1次迭代
            x = x * (Two - (this * x)); // 第2次迭代
            x = x * (Two - (this * x)); // 第3次迭代

            return negative ? -x : x;
        }
        * 
        ************************************************************************/

        /// <summary>
        /// 计算倒数
        /// <para>计算定点数的倒数</para>
        /// </summary>
        /// <returns>倒数</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是0，返回正无穷</item>
        /// <item>如果输入是无穷大，返回0</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用1除以当前值来计算倒数</item>
        /// </list>
        /// </remarks>
        public Fixed32 Reciprocal()
        {
            if (PreprocessReciprocal(rawvalue, out var r))
            {
                return r;
            }

            return One / this;
        }

        /// <summary>
        /// 计算倒数
        /// <para>静态方法，计算定点数的倒数</para>
        /// </summary>
        /// <param name="n">要计算倒数的定点数</param>
        /// <returns>倒数</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是0，返回正无穷</item>
        /// <item>如果输入是无穷大，返回0</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Reciprocal(Fixed32 n)
        {
            return FMath.Reciprocal(n);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理倒数函数的特殊输入情况</para>
        /// </summary>
        /// <param name="n">原始存储值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessReciprocal(long n, out Fixed32 r)
        {
            if (n.IsNaN()) { r = NaN; return true; }
            if (n.IsZero()) { r = PositiveInfinity; return true; }
            if (n.IsInfinity()) { r = Zero; return true; }

            r = Zero;
            return false;
        }
    }
}
