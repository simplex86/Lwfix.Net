namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 定点数 - 取余
    /// <para>包含定点数的取余操作实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 取余运算符
        /// <para>计算定点数与整数的余数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>余数</returns>
        public static Fixed32 operator %(Fixed32 a, int b)
        {
            var b_rawvalue = Int32ToRaw(b);
            return Mod(a.rawvalue, b_rawvalue);
        }

        /// <summary>
        /// 取余运算符
        /// <para>计算整数与定点数的余数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>余数</returns>
        public static Fixed32 operator %(int a, Fixed32 b)
        {
            var a_rawvalue = Int32ToRaw(a);
            return Mod(a_rawvalue, b.rawvalue);
        }

        /// <summary>
        /// 取余运算符
        /// <para>计算两个定点数的余数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>余数</returns>
        public static Fixed32 operator %(Fixed32 a, Fixed32 b)
        {
            return Mod(a.rawvalue, b.rawvalue);
        }

        /// <summary>
        /// 取余
        /// <para>计算两个原始存储值的余数</para>
        /// </summary>
        /// <param name="a">被除数的原始存储值</param>
        /// <param name="b">除数的原始存储值</param>
        /// <returns>余数</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果有任何输入是NaN，返回NaN</item>
        /// <item>如果被除数是无穷大，返回NaN</item>
        /// <item>如果被除数是最小值，返回0</item>
        /// <item>如果除数是-1，返回0</item>
        /// <item>如果除数是最小值、最大值或无穷大，返回被除数</item>
        /// </list>
        /// </remarks>
        private static Fixed32 Mod(long a, long b)
        {
            if (PreprocessMod(a, b, out var r))
            {
                return r;
            }

            return FromRaw(a % b);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理取余操作的特殊输入情况</para>
        /// </summary>
        /// <param name="a">被除数的原始存储值</param>
        /// <param name="b">除数的原始存储值</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessMod(long a, long b, out Fixed32 r)
        {
            if (a.IsNaN() || b.IsNaN()) { r = NaN; return true; }
            if (a.IsInfinity()) { r = NaN; return true; }
            if (b.IsZero()) { r = NaN; return true; }
            if (a.IsMin()) { r = Zero; return true; }
            if (b.IsNegativeOne()) { r = Zero; return true; }
            if (b.IsMinMax() || b.IsInfinity()) { r = FromRaw(a); return true; }

            r = Zero;
            return false;
        }
    }
}
