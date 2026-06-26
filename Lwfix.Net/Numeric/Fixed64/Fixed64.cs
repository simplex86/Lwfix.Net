using System;
using System.Numerics;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 64位定点数结构体
    /// <para>使用128位整型存储，其中64位用于整数部分，64位用于小数部分</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 原始存储值
        /// </summary>
        internal Int128 rawvalue = 0;

        /// <summary>
        /// 总位宽
        /// </summary>
        internal const byte TOTAL_BITS = 128;
        /// <summary>
        /// 整数部分占用的位宽
        /// </summary>
        internal const byte INTEGRAL_BITS = TOTAL_BITS / 2;
        /// <summary>
        /// 小数部分占用的位宽
        /// </summary>
        internal const byte FRACTIONAL_BITS = TOTAL_BITS - INTEGRAL_BITS;
        /// <summary>
        /// 小数精度（2^64），用const避免静态初始化顺序问题
        /// </summary>
        internal const double FRACTIONAL_MULTIPLIER = 18446744073709551616.0;

        /// <summary>
        /// 符号位的掩码
        /// </summary>
        internal static readonly Int128 SIGN_BIT_MASK = Int128.MinValue;
        /// <summary>
        /// 小数部分的掩码
        /// </summary>
        internal static readonly Int128 FRACTIONAL_MASK = (Int128.One << FRACTIONAL_BITS) - 1;
        /// <summary>
        /// 整数部分的掩码
        /// </summary>
        internal static readonly Int128 INTEGRAL_MASK = ~FRACTIONAL_MASK;
        /// <summary>
        /// 所有位的掩码
        /// </summary>
        internal static readonly UInt128 FULL_BIT_MASK = (UInt128)Int128.MaxValue;

        /// <summary>
        /// 精度阈值
        /// </summary>
        internal static readonly Int128 EPSILON_VALUE = (Int128)8 << 32;

        // 用属性替代static readonly字段，避免跨partial文件初始化顺序不确定导致取到0
        /// <summary>
        /// 最大原始值
        /// </summary>
        internal static Int128 S_MAX_RAW_VALUE => Int128.MaxValue - 1;
        /// <summary>
        /// 最小原始值
        /// </summary>
        internal static Int128 S_MIN_RAW_VALUE => Int128.MinValue + 2;

        /// <summary>
        /// 小数部分缩放因子（1 &lt;&lt; 64），供三角函数泰勒系数等使用
        /// </summary>
        internal static Int128 FRAC_SCALE => Int128.One << FRACTIONAL_BITS;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Fixed64()
        {
            rawvalue = 0;
        }

        /// <summary>
        /// 从整数创建定点数
        /// </summary>
        public Fixed64(int value)
        {
            rawvalue = Int32ToRaw(value);
        }

        /// <summary>
        /// 从浮点数创建定点数
        /// </summary>
        public Fixed64(float value)
            : this((double)value)
        {
        }

        /// <summary>
        /// 从双精度浮点数创建定点数
        /// </summary>
        public Fixed64(double value)
        {
            rawvalue = DoubleToRaw(value);
        }

        /// <summary>
        /// 从另一个定点数创建定点数
        /// </summary>
        public Fixed64(Fixed64 other)
        {
            rawvalue = other.rawvalue;
        }

        /// <summary>
        /// 获取哈希码
        /// </summary>
        public override int GetHashCode()
        {
            return rawvalue.GetHashCode();
        }
    }
}
