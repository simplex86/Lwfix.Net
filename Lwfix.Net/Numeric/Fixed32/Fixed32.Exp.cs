namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - e的幂
    /// <para>包含定点数的指数函数实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 计算e的幂
        /// <para>计算e的当前定点数次方</para>
        /// </summary>
        /// <returns>e的幂值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是0，返回1</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// <item>如果输入是负无穷，返回0</item>
        /// </list>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>分解 x = k * ln(2) + r，其中 |r| ≤ 0.5 * ln(2)</item>
        /// <item>计算 e^r 的泰勒级数展开</item>
        /// <item>计算 2^k * e^r 得到最终结果</item>
        /// </list>
        /// </remarks>
        public Fixed32 Exp()
        {
            if (PreprocessExp(rawvalue, out var r))
            {
                return r;
            }

            // 分解 x = k * ln(2) + r，其中 |r| ≤ 0.5 * ln(2)
            var k = (this / Ln2).Round();
            var residual = this - k * Ln2;

            // 计算 e^r 的泰勒级数展开
            var ter = One;
            var sum = One;
            var idx = 0;
            while (ter != Zero) // 迭代多次确保精度
            {
                idx++;
                ter = ter * residual / idx;
                sum += ter;
            }

            var s = k.IsNegative();
            var t = k.Abs().ToInt();

            var pow = new Fixed32(1 << t);
            if (s) pow = pow.Reciprocal();

            // e^x = e^(k * ln(2) + r) = 2^k * e^r
            return pow * sum;
        }

        /// <summary>
        /// 计算e的幂
        /// <para>静态方法，计算e的定点数次方</para>
        /// </summary>
        /// <param name="m">指数</param>
        /// <returns>e的幂值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是0，返回1</item>
        /// <item>如果输入是正无穷，返回正无穷</item>
        /// <item>如果输入是负无穷，返回0</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Exp(Fixed32 m)
        {
            return m.Exp();
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理指数函数的特殊输入情况</para>
        /// </summary>
        /// <param name="n">原始存储值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private bool PreprocessExp(long n, out Fixed32 r)
        {
            if (n.IsNaN()) { r = NaN; return true; }
            if (n.IsZero()) { r = One; return true; }
            if (n.IsPositiveInfinity()) { r = PositiveInfinity; return true; }
            if (n.IsNegativeInfinity()) { r = Zero; return true; }

            r = Zero;
            return false;
        }
    }
}
