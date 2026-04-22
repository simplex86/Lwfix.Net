namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 检测
    /// <para>包含定点数的状态检测方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 检测当前定点数是否为0
        /// <para>检查定点数的原始值是否为0</para>
        /// </summary>
        /// <returns>如果当前定点数为0，返回true；否则返回false</returns>
        public bool IsZero()
        {
            return rawvalue.IsZero();
        }

        /// <summary>
        /// 检测给定定点数是否为0
        /// <para>检查定点数的原始值是否为0</para>
        /// </summary>
        /// <param name="n">要检测的定点数</param>
        /// <returns>如果给定定点数为0，返回true；否则返回false</returns>
        public static bool IsZero(Fixed32 n)
        {
            return n.IsZero();
        }

        /// <summary>
        /// 检测当前定点数是否为最大值
        /// <para>检查定点数的原始值是否为最大值</para>
        /// </summary>
        /// <returns>如果当前定点数为最大值，返回true；否则返回false</returns>
        public bool IsMax()
        {
            return rawvalue.IsMax();
        }

        /// <summary>
        /// 检测给定定点数是否为最大值
        /// <para>检查定点数的原始值是否为最大值</para>
        /// </summary>
        /// <param name="n">要检测的定点数</param>
        /// <returns>如果给定定点数为最大值，返回true；否则返回false</returns>
        public static bool IsMax(Fixed32 n)
        {
            return n.IsMax();
        }

        /// <summary>
        /// 检测当前定点数是否为最小值
        /// <para>检查定点数的原始值是否为最小值</para>
        /// </summary>
        /// <returns>如果当前定点数为最小值，返回true；否则返回false</returns>
        public bool IsMin()
        {
            return rawvalue.IsMin();
        }

        /// <summary>
        /// 检测给定定点数是否为最小值
        /// <para>检查定点数的原始值是否为最小值</para>
        /// </summary>
        /// <param name="n">要检测的定点数</param>
        /// <returns>如果给定定点数为最小值，返回true；否则返回false</returns>
        public static bool IsMin(Fixed32 n)
        {
            return n.IsMin();
        }

        /// <summary>
        /// 检测当前定点数是否为无穷大（正无穷或负无穷）
        /// <para>检查定点数是否为正无穷或负无穷</para>
        /// </summary>
        /// <returns>如果当前定点数为无穷大，返回true；否则返回false</returns>
        public bool IsInfinity()
        {
            return IsPositiveInfinity() || IsNegativeInfinity();
        }

        /// <summary>
        /// 检测给定定点数是否为无穷大（正无穷或负无穷）
        /// <para>检查定点数是否为正无穷或负无穷</para>
        /// </summary>
        /// <param name="value">要检测的定点数</param>
        /// <returns>如果给定定点数为无穷大，返回true；否则返回false</returns>
        public static bool IsInfinity(Fixed32 value)
        {
            return value.IsInfinity();
        }

        /// <summary>
        /// 检测当前定点数是否为正无穷
        /// <para>检查定点数的原始值是否为正无穷</para>
        /// </summary>
        /// <returns>如果当前定点数为正无穷，返回true；否则返回false</returns>
        public bool IsPositiveInfinity()
        {
            return rawvalue.IsPositiveInfinity();
        }

        /// <summary>
        /// 检测给定定点数是否为正无穷
        /// <para>检查定点数的原始值是否为正无穷</para>
        /// </summary>
        /// <param name="value">要检测的定点数</param>
        /// <returns>如果给定定点数为正无穷，返回true；否则返回false</returns>
        public static bool IsPositiveInfinity(Fixed32 value)
        {
            return value.IsPositiveInfinity();
        }

        /// <summary>
        /// 检测当前定点数是否为负无穷
        /// <para>检查定点数的原始值是否为负无穷</para>
        /// </summary>
        /// <returns>如果当前定点数为负无穷，返回true；否则返回false</returns>
        public bool IsNegativeInfinity()
        {
            return rawvalue.IsNegativeInfinity();
        }

        /// <summary>
        /// 检测给定定点数是否为负无穷
        /// <para>检查定点数的原始值是否为负无穷</para>
        /// </summary>
        /// <param name="value">要检测的定点数</param>
        /// <returns>如果给定定点数为负无穷，返回true；否则返回false</returns>
        public static bool IsNegativeInfinity(Fixed32 value)
        {
            return value.IsNegativeInfinity();
        }

        /// <summary>
        /// 检测当前定点数是否为NaN（非数字）
        /// <para>检查定点数的原始值是否为NaN</para>
        /// </summary>
        /// <returns>如果当前定点数为NaN，返回true；否则返回false</returns>
        public bool IsNaN()
        {
            return rawvalue.IsNaN();
        }

        /// <summary>
        /// 检测给定定点数是否为NaN（非数字）
        /// <para>检查定点数的原始值是否为NaN</para>
        /// </summary>
        /// <param name="value">要检测的定点数</param>
        /// <returns>如果给定定点数为NaN，返回true；否则返回false</returns>
        public static bool IsNaN(Fixed32 value)
        {
            return value.IsNaN();
        }

        /// <summary>
        /// 检测当前定点数是否为正数（包括0）
        /// <para>检查定点数的原始值是否大于或等于0</para>
        /// </summary>
        /// <returns>如果当前定点数为正数（包括0），返回true；否则返回false</returns>
        public bool IsPositive()
        {
            return rawvalue >= 0;
        }

        /// <summary>
        /// 检测给定定点数是否为正数（包括0）
        /// <para>检查定点数的原始值是否大于或等于0</para>
        /// </summary>
        /// <param name="value">要检测的定点数</param>
        /// <returns>如果给定定点数为正数（包括0），返回true；否则返回false</returns>
        public static bool IsPositive(Fixed32 value)
        {
            return value.IsPositive();
        }

        /// <summary>
        /// 检测当前定点数是否为负数
        /// <para>检查定点数的原始值是否小于0</para>
        /// </summary>
        /// <returns>如果当前定点数为负数，返回true；否则返回false</returns>
        public bool IsNegative()
        {
            return rawvalue < 0;
        }

        /// <summary>
        /// 检测给定定点数是否为负数
        /// <para>检查定点数的原始值是否小于0</para>
        /// </summary>
        /// <param name="value">要检测的定点数</param>
        /// <returns>如果给定定点数为负数，返回true；否则返回false</returns>
        public static bool IsNegative(Fixed32 value)
        {
            return value.IsNegative();
        }

        /// <summary>
        /// 检测当前定点数是否为小数（包含小数部分）
        /// <para>检查定点数的原始值是否包含小数部分</para>
        /// </summary>
        /// <returns>如果当前定点数为小数，返回true；否则返回false</returns>
        public bool IsFractional()
        {
            return rawvalue.IsFractional();
        }

        /// <summary>
        /// 检测给定定点数是否为小数（包含小数部分）
        /// <para>检查定点数的原始值是否包含小数部分</para>
        /// </summary>
        /// <param name="value">要检测的定点数</param>
        /// <returns>如果给定定点数为小数，返回true；否则返回false</returns>
        public static bool IsFractional(Fixed32 value)
        {
            return value.IsFractional();
        }
    }
}
