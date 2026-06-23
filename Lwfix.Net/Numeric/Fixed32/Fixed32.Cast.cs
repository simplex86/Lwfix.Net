using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 类型转换
    /// <para>包含定点数的类型转换相关方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 从原始值创建定点数
        /// <para>直接使用给定的原始值创建定点数</para>
        /// </summary>
        /// <param name="value">原始值</param>
        /// <returns>创建的定点数</returns>
        public static Fixed32 FromRaw(long value)
        {
            return new Fixed32() { rawvalue = value };
        }

        /// <summary>
        /// 获取定点数的原始值
        /// <para>返回定点数的内部原始存储值</para>
        /// </summary>
        /// <param name="value">定点数</param>
        /// <returns>原始值</returns>
        public static long ToRaw(Fixed32 value)
        {
            return value.rawvalue;
        }

        /// <summary>
        /// 将32位整数转换为原始值
        /// <para>将整数左移小数位数并限制在有效范围内</para>
        /// </summary>
        /// <param name="value">32位整数</param>
        /// <returns>转换后的原始值</returns>
        private static long Int32ToRaw(int value)
        {
            return Math.Clamp((long)value << FRACTIONAL_BITS, S_MIN_RAW_VALUE, S_MAX_RAW_VALUE);
        }

        /// <summary>
        /// 将双精度浮点数转换为原始值
        /// <para>将浮点数乘以小数部分乘数并四舍五入，限制在有效范围内</para>
        /// </summary>
        /// <param name="value">双精度浮点数</param>
        /// <returns>转换后的原始值</returns>
        private static long DoubleToRaw(double value)
        {
            return Math.Clamp((long)(value * FRACTIONAL_MULTIPLIER + 0.5), S_MIN_RAW_VALUE, S_MAX_RAW_VALUE);
        }

        /// <summary>
        /// 显式转换为byte类型
        /// <para>将定点数转换为byte类型</para>
        /// </summary>
        /// <param name="n">定点数</param>
        /// <returns>转换后的byte值</returns>
        public static explicit operator byte(Fixed32 n) => n.ToByte();

        /// <summary>
        /// 显式转换为short类型
        /// <para>将定点数转换为short类型</para>
        /// </summary>
        /// <param name="n">定点数</param>
        /// <returns>转换后的short值</returns>
        public static explicit operator short(Fixed32 n) => n.ToShort();

        /// <summary>
        /// 显式转换为int类型
        /// <para>将定点数转换为int类型</para>
        /// </summary>
        /// <param name="n">定点数</param>
        /// <returns>转换后的int值</returns>
        public static explicit operator int(Fixed32 n) => n.ToInt();

        /// <summary>
        /// 显式转换为long类型
        /// <para>将定点数转换为long类型</para>
        /// </summary>
        /// <param name="n">定点数</param>
        /// <returns>转换后的long值</returns>
        public static explicit operator long(Fixed32 n) => n.ToLong();

        /// <summary>
        /// 显式转换为float类型
        /// <para>将定点数转换为float类型</para>
        /// </summary>
        /// <param name="n">定点数</param>
        /// <returns>转换后的float值</returns>
        public static explicit operator float(Fixed32 n) => n.ToFloat();

        /// <summary>
        /// 显式转换为double类型
        /// <para>将定点数转换为double类型</para>
        /// </summary>
        /// <param name="n">定点数</param>
        /// <returns>转换后的double值</returns>
        public static explicit operator double(Fixed32 n) => n.ToDouble();

        /// <summary>
        /// 隐式转换为Fixed32类型
        /// <para>将byte转换为定点数</para>
        /// </summary>
        /// <param name="n">byte值</param>
        /// <returns>转换后的定点数</returns>
        public static implicit operator Fixed32(byte n) => new Fixed32(n);

        /// <summary>
        /// 隐式转换为Fixed32类型
        /// <para>将short转换为定点数</para>
        /// </summary>
        /// <param name="n">short值</param>
        /// <returns>转换后的定点数</returns>
        public static implicit operator Fixed32(short n) => new Fixed32(n);

        /// <summary>
        /// 隐式转换为Fixed32类型
        /// <para>将int转换为定点数</para>
        /// </summary>
        /// <param name="n">int值</param>
        /// <returns>转换后的定点数</returns>
        public static implicit operator Fixed32(int n) => new Fixed32(n);

        /// <summary>
        /// 显式转换为Fixed32类型
        /// <para>将long转换为定点数</para>
        /// </summary>
        /// <param name="n">long值</param>
        /// <returns>转换后的定点数</returns>
        public static explicit operator Fixed32(long n) => new Fixed32(n);

        /// <summary>
        /// 显式转换为Fixed32类型
        /// <para>将float转换为定点数</para>
        /// </summary>
        /// <param name="n">float值</param>
        /// <returns>转换后的定点数</returns>
        public static explicit operator Fixed32(float n) => new Fixed32(n);

        /// <summary>
        /// 显式转换为Fixed32类型
        /// <para>将double转换为定点数</para>
        /// </summary>
        /// <param name="n">double值</param>
        /// <returns>转换后的定点数</returns>
        public static explicit operator Fixed32(double n) => new Fixed32(n);

        /// <summary>
        /// 转换为byte类型
        /// <para>将定点数转换为byte类型</para>
        /// </summary>
        /// <returns>转换后的byte值</returns>
        /// <remarks>如果当前值是NaN，返回0</remarks>
        public byte ToByte()
        {
            return IsNaN() ? (byte)0 : (byte)ToLong();
        }

        /// <summary>
        /// 转换为short类型
        /// <para>将定点数转换为short类型</para>
        /// </summary>
        /// <returns>转换后的short值</returns>
        /// <remarks>如果当前值是NaN，返回0</remarks>
        public short ToShort()
        {
            return IsNaN() ? (short)0 : (short)ToLong();
        }

        /// <summary>
        /// 转换为int类型
        /// <para>将定点数转换为int类型</para>
        /// </summary>
        /// <returns>转换后的int值</returns>
        /// <remarks>如果当前值是NaN，返回0</remarks>
        public int ToInt()
        {
            return IsNaN() ? 0 : (int)ToLong();
        }

        /// <summary>
        /// 转换为long类型
        /// <para>将定点数转换为long类型</para>
        /// </summary>
        /// <returns>转换后的long值</returns>
        /// <remarks>如果当前值是NaN，返回0</remarks>
        public long ToLong()
        {
            if (IsNaN()) return 0L;
            return rawvalue >> FRACTIONAL_BITS;
        }

        /// <summary>
        /// 转换为float类型
        /// <para>将定点数转换为float类型</para>
        /// </summary>
        /// <returns>转换后的float值</returns>
        /// <remarks>如果当前值是NaN，返回float.NaN</remarks>
        public float ToFloat()
        {
            if (IsNaN()) return float.NaN;
            return (float)ToDouble();
        }

        /// <summary>
        /// 转换为double类型
        /// <para>将定点数转换为double类型</para>
        /// </summary>
        /// <returns>转换后的double值</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果当前值是NaN，返回double.NaN</item>
        /// <item>如果当前值是正无穷大，返回double.PositiveInfinity</item>
        /// <item>如果当前值是负无穷大，返回double.NegativeInfinity</item>
        /// </list>
        /// </remarks>
        public double ToDouble()
        {
            if (IsNaN()) return double.NaN;
            if (IsPositiveInfinity()) return double.PositiveInfinity;
            if (IsNegativeInfinity()) return double.NegativeInfinity;

            return rawvalue / FRACTIONAL_MULTIPLIER;
        }

        /// <summary>
        /// 转换为字符串
        /// <para>将定点数转换为字符串表示</para>
        /// </summary>
        /// <returns>字符串表示</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果当前值是NaN，返回"NaN"</item>
        /// <item>如果当前值是正无穷大，返回"+∞"</item>
        /// <item>如果当前值是负无穷大，返回"-∞"</item>
        /// <item>如果当前值是整数，返回整数字符串</item>
        /// <item>如果当前值有小数部分，返回浮点数字符串</item>
        /// </list>
        /// </remarks>
        public override string ToString()
        {
            if (IsNaN()) return "NaN";
            if (IsPositiveInfinity()) return "+∞";
            if (IsNegativeInfinity()) return "-∞";

            return IsFractional() ? ToDouble().ToString()
                                  : ToLong().ToString();
        }

        /// <summary>
        /// 获取整数部分
        /// <para>返回定点数的整数部分</para>
        /// </summary>
        /// <returns>整数部分</returns>
        /// <remarks>如果当前值是NaN，返回NaN</remarks>
        public Fixed32 Integral()
        {
            if (IsNaN()) return NaN;

            var result = rawvalue & INTEGRAL_MASK;
            // MinValue的rawvalue & INTEGRAL_MASK会得到NaN的rawvalue，需要特殊处理
            if (result == NaN.rawvalue) return MinValue;

            return FromRaw(result);
        }

        /// <summary>
        /// 获取整数部分
        /// <para>返回给定定点数的整数部分</para>
        /// </summary>
        /// <param name="n">定点数</param>
        /// <returns>整数部分</returns>
        /// <remarks>如果输入值是NaN，返回NaN</remarks>
        public static Fixed32 Integral(Fixed32 n)
        {
            return n.Integral();
        }

        /// <summary>
        /// 获取小数部分
        /// <para>返回定点数的小数部分</para>
        /// </summary>
        /// <returns>小数部分</returns>
        /// <remarks>如果当前值是NaN，返回NaN</remarks>
        public Fixed32 Fractional()
        {
            if (IsNaN()) return NaN;
            return FromRaw(rawvalue & FRACTIONAL_MASK);
        }

        /// <summary>
        /// 获取小数部分
        /// <para>返回给定定点数的小数部分</para>
        /// </summary>
        /// <param name="n">定点数</param>
        /// <returns>小数部分</returns>
        /// <remarks>如果输入值是NaN，返回NaN</remarks>
        public static Fixed32 Fractional(Fixed32 n)
        {
            return n.Fractional();
        }
    }
}
