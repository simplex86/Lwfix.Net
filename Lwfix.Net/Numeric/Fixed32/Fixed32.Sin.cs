namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 正弦
    /// <para>包含定点数的正弦函数实现，包括标准实现和快速实现</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>1/(3!) 常量</summary>
        private static readonly Fixed32 C3  = FromRaw(715827882); // 1/(3!) 
        /// <summary>1/(5!) 常量</summary>
        private static readonly Fixed32 C5  = FromRaw(35791394);  // 1/(5!) 
        /// <summary>1/(7!) 常量</summary>
        private static readonly Fixed32 C7  = FromRaw(852176);    // 1/(7!) 
        /// <summary>1/(9!) 常量</summary>
        private static readonly Fixed32 C9  = FromRaw(11836);     // 1/(9!) 
        /// <summary>1/(11!) 常量</summary>
        private static readonly Fixed32 C11 = FromRaw(108);       // 1/(11!)

        /// <summary>
        /// 计算正弦值
        /// <para>使用泰勒展开计算定点数的正弦值</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正弦值，范围在[-1, 1]之间</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷或负无穷，返回NaN</item>
        /// </list>
        /// </remarks>
        public static Fixed32 Sin(Fixed32 radian)
        {
            if (PreprocessSin(radian,out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian);
            var referenced = ReduceRadian4Sin(normalized, out var sign);
            var result = TaylorEvaluate4Sin(referenced);

            return sign ? -result : result;
        }

        /// <summary>
        /// 将角度缩减到[0, π/2]区间
        /// <para>利用正弦函数的对称性，将角度缩减到[0, π/2]区间以减少计算误差</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="sign">正弦值的符号</param>
        /// <returns>缩减到[0, π/2]区间的角度</returns>
        internal static Fixed32 ReduceRadian4Sin(Fixed32 radian, out bool sign)
        {
            sign = false;

            var reference = radian;
            var quadrant = (radian.rawvalue << 1) / PI.rawvalue;

            switch (quadrant)
            {
                case 0: // 第一象限 [0, π/2)
                    break;
                case 1: // 第二象限 [π/2, π)
                    reference = PI - radian;
                    break;
                case 2: // 第三象限 [π, 3π/2)
                    reference = radian - PI;
                    sign = true;
                    break;
                case 3: // 第四象限 [3π/2, 2π)
                    reference = Two_PI - radian;
                    sign = true;
                    break;
                default:
                    reference = Zero;
                    break;
            }

            return reference;
        }

        /// <summary>
        /// 使用泰勒展开计算正弦值
        /// <para>在[0, π/2]区间内使用泰勒展开计算正弦值</para>
        /// </summary>
        /// <param name="x">角度（弧度），范围在[0, π/2]之间</param>
        /// <returns>正弦值</returns>
        private static Fixed32 TaylorEvaluate4Sin(Fixed32 x)
        {
            var x1  = x;
            var x2  = x1 * x1;
            var x3  = x1 * x2;
            var x5  = x3 * x2;
            var x7  = x5 * x2;
            var x9  = x7 * x2;
            var x11 = x9 * x2;

            return x1 - x3 * C3 + x5 * C5 - x7 * C7 + x9 * C9 - x11 * C11;
        }

        /// <summary>
        /// 快速计算正弦值
        /// <para>使用查表法快速计算正弦值，速度比Sin函数快，但精度较低</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>正弦值，范围在[-1, 1]之间</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，返回NaN</item>
        /// <item>如果输入是正无穷或负无穷，返回NaN</item>
        /// </list>
        /// 注意：该方法的误差大于Sin函数
        /// </remarks>
        public static Fixed32 FastSin(Fixed32 radian)
        {
            if (PreprocessSin(radian, out var r))
            {
                return r;
            }

            var normalized = NormalizeRadian(radian);
            var referenced = ReduceRadian4Sin(normalized, out var sign);

            var index = referenced.rawvalue >> 15;
            if (index >= SinLut.Length) index = SinLut.Length - 1;

            var nearest = SinLut[index];
            if (sign) nearest = -nearest;

            return FromRaw(nearest);
        }

        /// <summary>
        /// 预处理特殊边界值
        /// <para>处理正弦函数的特殊输入情况</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="r">处理结果</param>
        /// <returns>是否处理了特殊情况</returns>
        private static bool PreprocessSin(Fixed32 radian, out Fixed32 r)
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
        /// 计算反正弦值
        /// <para>使用余弦函数计算反正弦值</para>
        /// </summary>
        /// <param name="value">正弦值，范围在[-1, 1]之间</param>
        /// <returns>反正弦值，范围在[-π/2, π/2]之间</returns>
        public static Fixed32 Asin(Fixed32 value)
        {
            return Half_PI - Acos(value);
        }
    }
}
