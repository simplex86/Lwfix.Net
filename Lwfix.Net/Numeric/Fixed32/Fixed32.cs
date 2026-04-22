namespace SimplexLab.Fixed
{
    /// <summary>
    /// 32位定点数结构体
    /// <para>使用64位长整型存储，其中32位用于整数部分，32位用于小数部分</para>
    /// <para>提供高精度的小数计算，避免浮点数的精度误差</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 原始存储值
        /// </summary>
        internal long rawvalue = 0;

        /// <summary>
        /// 总位宽
        /// </summary>
        internal const byte TOTAL_BITS = sizeof(long) * 8;
        /// <summary>
        /// 整数部分占用的位宽
        /// </summary>
        internal const byte INTEGRAL_BITS = TOTAL_BITS / 2;
        /// <summary>
        /// 小数部分占用的位宽
        /// </summary>
        internal const byte FRACTIONAL_BITS = TOTAL_BITS - INTEGRAL_BITS;
        /// <summary>
        /// 小数精度
        /// </summary>
        internal const double FRACTIONAL_MULTIPLIER = uint.MaxValue + 1.0;

        /// <summary>
        /// 所有位的掩码
        /// </summary>
        internal const ulong FULL_BIT_MASK = 0xFFFFFFFFFFFFFFFF;
        /// <summary>
        /// 符号位的掩码
        /// </summary>
        internal const long SIGN_BIT_MASK = unchecked((long)0x8000000000000000L);
        /// <summary>
        /// 整数部分的掩码
        /// </summary>
        internal const long INTEGRAL_MASK = FRACTIONAL_MASK << INTEGRAL_BITS;
        /// <summary>
        /// 小数部分的掩码
        /// </summary>
        internal const long FRACTIONAL_MASK = (1L << FRACTIONAL_BITS) - 1L;

        /// <summary>
        /// 精度阈值
        /// <para>用于比较两个定点数是否相等的最小差异值</para>
        /// </summary>
        internal const long EPSILON_VALUE = 8L;

        /// <summary>
        /// 默认构造函数
        /// <para>创建值为0的定点数</para>
        /// </summary>
        public Fixed32()
        {
            rawvalue = 0;
        }

        /// <summary>
        /// 从整数创建定点数
        /// </summary>
        /// <param name="value">整数值</param>
        /// <returns>对应的定点数</returns>
        public Fixed32(int value)
        {
            rawvalue = Int32ToRaw(value);
        }

        /// <summary>
        /// 从浮点数创建定点数
        /// </summary>
        /// <param name="value">浮点数值</param>
        /// <returns>对应的定点数</returns>
        public Fixed32(float value)
            : this((double)value)
        {
            
        }

        /// <summary>
        /// 从双精度浮点数创建定点数
        /// </summary>
        /// <param name="value">双精度浮点数值</param>
        /// <returns>对应的定点数</returns>
        public Fixed32(double value)
        {
            rawvalue = DoubleToRaw(value);
        }

        /// <summary>
        /// 从另一个定点数创建定点数
        /// </summary>
        /// <param name="other">另一个定点数</param>
        /// <returns>与另一个定点数相等的新定点数</returns>
        public Fixed32(Fixed32 other)
        {
            rawvalue = other.rawvalue;
        }

        /// <summary>
        /// 获取哈希码
        /// </summary>
        /// <returns>当前定点数的哈希码</returns>
        public override int GetHashCode()
        {
            return rawvalue.GetHashCode();
        }
    }
}
