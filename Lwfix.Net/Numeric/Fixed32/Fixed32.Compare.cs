using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 比较
    /// <para>包含定点数的比较操作符和方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 相等运算符
        /// <para>比较两个定点数是否相等</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果两个定点数相等，返回true；否则返回false</returns>
        /// <remarks>如果任何一个操作数是NaN，返回false</remarks>
        public static bool operator ==(Fixed32 a, Fixed32 b)
        {
            if (a.IsNaN() || b.IsNaN()) return false;
            return a.rawvalue == b.rawvalue;
        }

        /// <summary>
        /// 不相等运算符
        /// <para>比较两个定点数是否不相等</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果两个定点数不相等，返回true；否则返回false</returns>
        /// <remarks>如果任何一个操作数是NaN，返回true</remarks>
        public static bool operator !=(Fixed32 a, Fixed32 b)
        {
            if (a.IsNaN() || b.IsNaN()) return true;
            return a.rawvalue != b.rawvalue;
        }

        /// <summary>
        /// 大于运算符
        /// <para>比较第一个定点数是否大于第二个定点数</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果第一个定点数大于第二个定点数，返回true；否则返回false</returns>
        /// <remarks>如果任何一个操作数是NaN，返回false</remarks>
        public static bool operator >(Fixed32 a, Fixed32 b)
        {
            if (a.IsNaN() || b.IsNaN()) return false;
            return a.rawvalue > b.rawvalue;
        }

        /// <summary>
        /// 小于运算符
        /// <para>比较第一个定点数是否小于第二个定点数</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果第一个定点数小于第二个定点数，返回true；否则返回false</returns>
        /// <remarks>如果任何一个操作数是NaN，返回false</remarks>
        public static bool operator <(Fixed32 a, Fixed32 b)
        {
            if (a.IsNaN() || b.IsNaN()) return false;
            return a.rawvalue < b.rawvalue;
        }

        /// <summary>
        /// 大于等于运算符
        /// <para>比较第一个定点数是否大于等于第二个定点数</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果第一个定点数大于等于第二个定点数，返回true；否则返回false</returns>
        /// <remarks>如果任何一个操作数是NaN，返回false</remarks>
        public static bool operator >=(Fixed32 a, Fixed32 b)
        {
            if (a.IsNaN() || b.IsNaN()) return false;
            return a.rawvalue >= b.rawvalue;
        }

        /// <summary>
        /// 小于等于运算符
        /// <para>比较第一个定点数是否小于等于第二个定点数</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果第一个定点数小于等于第二个定点数，返回true；否则返回false</returns>
        /// <remarks>如果任何一个操作数是NaN，返回false</remarks>
        public static bool operator <=(Fixed32 a, Fixed32 b)
        {
            if (a.IsNaN() || b.IsNaN()) return false;
            return a.rawvalue <= b.rawvalue;
        }

        /// <summary>
        /// 确定当前对象是否等于另一个对象
        /// <para>检查当前定点数是否等于指定的对象</para>
        /// </summary>
        /// <param name="other">要比较的对象</param>
        /// <returns>如果当前对象等于另一个对象，返回true；否则返回false</returns>
        public override bool Equals(object? other)
        {
            return base.Equals(other);
        }

        /// <summary>
        /// 确定当前定点数是否等于另一个定点数
        /// <para>检查当前定点数是否等于指定的定点数</para>
        /// </summary>
        /// <param name="other">要比较的定点数</param>
        /// <returns>如果当前定点数等于另一个定点数，返回true；否则返回false</returns>
        public bool Equals(Fixed32 other)
        {
            return rawvalue == other.rawvalue;
        }

        /// <summary>
        /// 比较当前对象与另一个对象
        /// <para>比较当前定点数与指定对象的大小关系</para>
        /// </summary>
        /// <param name="value">要比较的对象</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>如果当前对象小于value，返回-1</item>
        /// <item>如果当前对象大于value，返回1</item>
        /// <item>如果当前对象等于value，返回0</item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentException">当value不是Fixed32类型时抛出</exception>
        public int CompareTo(object? value)
        {
            if (value == null)
            {
                return 1;
            }

            if (value is Fixed32 d)
            {
                if (this <  d) return -1;
                if (this >  d) return  1;
                if (this == d) return  0;
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// 比较当前定点数与另一个定点数
        /// <para>比较当前定点数与指定定点数的大小关系</para>
        /// </summary>
        /// <param name="value">要比较的定点数</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>如果当前定点数小于value，返回-1</item>
        /// <item>如果当前定点数大于value，返回1</item>
        /// <item>如果当前定点数等于value，返回0</item>
        /// </list>
        /// </returns>
        public int CompareTo(Fixed32 value)
        {
            if (this < value) return -1;
            if (this > value) return 1;
            return 0;
        }
    }
}
