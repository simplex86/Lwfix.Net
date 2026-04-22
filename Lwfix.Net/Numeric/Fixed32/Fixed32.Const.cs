namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 常量
    /// <para>包含定点数的各种常量定义</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        #region values

        /// <summary>
        /// 最大原始值
        /// <para>定点数能表示的最大值的原始存储值</para>
        /// </summary>
        private const long S_MAX_RAW_VALUE = long.MaxValue - 1; // 0x7FFFFFFFFFFFFFFE
        /// <summary>
        /// 最小原始值
        /// <para>定点数能表示的最小值的原始存储值</para>
        /// </summary>
        private const long S_MIN_RAW_VALUE = long.MinValue + 2; // 0x8000000000000002

        /// <summary>
        /// 加法单位元
        /// <para>与任何数相加都保持不变的数（0）</para>
        /// </summary>
        private readonly static Fixed32 S_ADDITIVE_IDENTITY = new Fixed32(0);
        /// <summary>
        /// 乘法单位元
        /// <para>与任何数相乘都保持不变的数（1）</para>
        /// </summary>
        private readonly static Fixed32 S_MULTIPLICATIVE_IDENTITY = new Fixed32(1);
        /// <summary>
        /// 零
        /// <para>表示数值0的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_ZERO = new Fixed32(0);
        /// <summary>
        /// 二分之一
        /// <para>表示数值0.5的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_HALF = FromRaw(1L << 31);
        /// <summary>
        /// 一
        /// <para>表示数值1的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_ONE = new Fixed32(1);
        /// <summary>
        /// 负一
        /// <para>表示数值-1的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_NEGATIVE_ONE = new Fixed32(-1);
        /// <summary>
        /// 二
        /// <para>表示数值2的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_TWO = new Fixed32(2);
        /// <summary>
        /// 自然对数的底e的ln(2)值
        /// <para>表示ln(2)的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_LN2 = FromRaw(2977044472);
        /// <summary>
        /// 自然对数的底e的ln(10)值
        /// <para>表示ln(10)的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_LN10 = FromRaw(9889527671);
        /// <summary>
        /// 非数字
        /// <para>表示不是一个数字的特殊值</para>
        /// </summary>
        private readonly static Fixed32 S_NaN = FromRaw(long.MinValue); // 0x8000000000000000
        /// <summary>
        /// 精度
        /// <para>表示定点数的最小精度</para>
        /// </summary>
        private readonly static Fixed32 S_EPSILON = FromRaw(EPSILON_VALUE);

        /// <summary>
        /// 自然常数e
        /// <para>表示自然常数e的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_E = FromRaw(11674931554);
        /// <summary>
        /// 圆周率π
        /// <para>表示圆周率π的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_PI = FromRaw(13493037705);
        /// <summary>
        /// 圆周率π的一半
        /// <para>表示π/2的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_HALF_PI = FromRaw(6746518852);
        /// <summary>
        /// 圆周率π的四分之一
        /// <para>表示π/4的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_QUARTER_PI = FromRaw(3373259426);
        /// <summary>
        /// 圆周率π的两倍
        /// <para>表示2π的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_TWO_PI = FromRaw(26986075409);

        /// <summary>
        /// 10的-1次方
        /// <para>表示0.1的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_TPN1 = FromRaw(429496730);
        /// <summary>
        /// 10的-2次方
        /// <para>表示0.01的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_TPN2 = FromRaw(42949673);
        /// <summary>
        /// 10的-3次方
        /// <para>表示0.001的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_TPN3 = FromRaw(4294967);
        /// <summary>
        /// 10的-4次方
        /// <para>表示0.0001的定点数</para>
        /// </summary>
        private readonly static Fixed32 S_TPN4 = FromRaw(429497);

        /// <summary>
        /// 180
        /// <para>表示数值180的定点数，用于角度转换</para>
        /// </summary>
        private readonly static Fixed32 S_N180 = FromRaw(773094113280);
        /// <summary>
        /// 360
        /// <para>表示数值360的定点数，用于角度转换</para>
        /// </summary>
        private readonly static Fixed32 S_N360 = FromRaw(1546188226560);

        /// <summary>
        /// 正无穷
        /// <para>表示正无穷大的特殊值</para>
        /// </summary>
        private readonly static Fixed32 S_POSITIVE_INFINITY = FromRaw(long.MaxValue); // 0x7FFFFFFFFFFFFFFF;
        /// <summary>
        /// 负无穷
        /// <para>表示负无穷大的特殊值</para>
        /// </summary>
        private readonly static Fixed32 S_NEGATIVE_INFINITY = FromRaw(long.MinValue + 1); // 0x8000000000000001

        #endregion

        #region properties

        /// <summary>
        /// 最大值
        /// <para>定点数能表示的最大值</para>
        /// </summary>
        public static Fixed32 MaxValue => FromRaw(S_MAX_RAW_VALUE);
        /// <summary>
        /// 最小值
        /// <para>定点数能表示的最小值</para>
        /// </summary>
        public static Fixed32 MinValue => FromRaw(S_MIN_RAW_VALUE);
        /// <summary>
        /// 加法单位元
        /// <para>与任何数相加都保持不变的数（0）</para>
        /// </summary>
        public static Fixed32 AdditiveIdentity => S_ADDITIVE_IDENTITY;
        /// <summary>
        /// 乘法单位元
        /// <para>与任何数相乘都保持不变的数（1）</para>
        /// </summary>
        public static Fixed32 MultiplicativeIdentity => S_MULTIPLICATIVE_IDENTITY;
        /// <summary>
        /// 零
        /// <para>表示数值0的定点数</para>
        /// </summary>
        public static Fixed32 Zero => S_ZERO;
        /// <summary>
        /// 二分之一
        /// <para>表示数值0.5的定点数</para>
        /// </summary>
        public static Fixed32 Half => S_HALF;
        /// <summary>
        /// 一
        /// <para>表示数值1的定点数</para>
        /// </summary>
        public static Fixed32 One => S_ONE;
        /// <summary>
        /// 负一
        /// <para>表示数值-1的定点数</para>
        /// </summary>
        public static Fixed32 NegativeOne => S_NEGATIVE_ONE;
        /// <summary>
        /// 二
        /// <para>表示数值2的定点数</para>
        /// </summary>
        public static Fixed32 Two => S_TWO;
        /// <summary>
        /// 自然对数的底e的ln(2)值
        /// <para>表示ln(2)的定点数</para>
        /// </summary>
        public static Fixed32 Ln2 => S_LN2;
        /// <summary>
        /// 自然对数的底e的ln(10)值
        /// <para>表示ln(10)的定点数</para>
        /// </summary>
        public static Fixed32 Ln10 => S_LN10;
        /// <summary>
        /// 非数字
        /// <para>表示不是一个数字的特殊值</para>
        /// </summary>
        public static Fixed32 NaN => S_NaN;
        /// <summary>
        /// 精度
        /// <para>表示定点数的最小精度</para>
        /// </summary>
        public static Fixed32 Epsilon => S_EPSILON;

        /// <summary>
        /// 自然常数e
        /// <para>表示自然常数e的定点数</para>
        /// </summary>
        public static Fixed32 E => S_E;
        /// <summary>
        /// 圆周率π
        /// <para>表示圆周率π的定点数</para>
        /// </summary>
        public static Fixed32 PI => S_PI;
        /// <summary>
        /// 圆周率π的一半
        /// <para>表示π/2的定点数</para>
        /// </summary>
        public static Fixed32 Half_PI => S_HALF_PI;
        /// <summary>
        /// 圆周率π的四分之一
        /// <para>表示π/4的定点数</para>
        /// </summary>
        public static Fixed32 Quarter_PI => S_QUARTER_PI;
        /// <summary>
        /// 圆周率π的两倍
        /// <para>表示2π的定点数</para>
        /// </summary>
        public static Fixed32 Two_PI => S_TWO_PI;

        /// <summary>
        /// 10的-1次方
        /// <para>表示0.1的定点数</para>
        /// </summary>
        public static Fixed32 TPN1 => S_TPN1;
        /// <summary>
        /// 10的-2次方
        /// <para>表示0.01的定点数</para>
        /// </summary>
        public static Fixed32 TPN2 => S_TPN2;
        /// <summary>
        /// 10的-3次方
        /// <para>表示0.001的定点数</para>
        /// </summary>
        public static Fixed32 TPN3 => S_TPN3;
        /// <summary>
        /// 10的-4次方
        /// <para>表示0.0001的定点数</para>
        /// </summary>
        public static Fixed32 TPN4 => S_TPN4;

        /// <summary>
        /// 180
        /// <para>表示数值180的定点数，用于角度转换</para>
        /// </summary>
        public static Fixed32 N180 => S_N180;
        /// <summary>
        /// 360
        /// <para>表示数值360的定点数，用于角度转换</para>
        /// </summary>
        public static Fixed32 N360 => S_N360;

        /// <summary>
        /// 正无穷
        /// <para>表示正无穷大的特殊值</para>
        /// </summary>
        public static Fixed32 PositiveInfinity => S_POSITIVE_INFINITY;
        /// <summary>
        /// 负无穷
        /// <para>表示负无穷大的特殊值</para>
        /// </summary>
        public static Fixed32 NegativeInfinity => S_NEGATIVE_INFINITY;

        #endregion
    }
}
