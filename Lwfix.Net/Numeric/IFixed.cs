using System;
using System.Numerics;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数接口
    /// <para>定义了定点数的基本操作和常量</para>
    /// <typeparam name="T">实现该接口的定点数类型</typeparam>
    /// </summary>
    public interface IFixed<T> : IMinMaxValue<T>
                               , IComparable
                               , IComparable<T>
                               , IEquatable<T> 
        where T : IFixed<T>
    {
        #region property

        /// <summary>
        /// 加法单位元
        /// <para>对于任何值x，x + AdditiveIdentity = x</para>
        /// </summary>
        abstract static T AdditiveIdentity { get; }
        
        /// <summary>
        /// 乘法单位元
        /// <para>对于任何值x，x * MultiplicativeIdentity = x</para>
        /// </summary>
        abstract static T MultiplicativeIdentity { get; }
        
        /// <summary>
        /// 零值
        /// </summary>
        abstract static T Zero { get; }
        
        /// <summary>
        /// 二分之一
        /// <para>值为0.5</para>
        /// </summary>
        abstract static T Half { get; }
        
        /// <summary>
        /// 一值
        /// </summary>
        abstract static T One { get; }
        
        /// <summary>
        /// 负一值
        /// </summary>
        abstract static T NegativeOne { get; }
        
        /// <summary>
        /// 二值
        /// </summary>
        abstract static T Two { get; }
        
        /// <summary>
        /// 自然对数的底e的常用对数值
        /// <para>值为ln(2)</para>
        /// </summary>
        abstract static T Ln2 { get; }
        
        /// <summary>
        /// 自然对数的底e的常用对数值
        /// <para>值为ln(10)</para>
        /// </summary>
        abstract static T Ln10 { get; }
        
        /// <summary>
        /// 非数字
        /// <para>表示无效的数值</para>
        /// </summary>
        abstract static T NaN { get; }
        
        /// <summary>
        /// 精度
        /// <para>表示能够区分的最小差异值</para>
        /// </summary>
        abstract static T Epsilon { get; }

        /// <summary>
        /// 自然常数e
        /// <para>值约为2.71828</para>
        /// </summary>
        abstract static T E { get; }
        
        /// <summary>
        /// 圆周率π
        /// <para>值约为3.14159</para>
        /// </summary>
        abstract static T PI { get; }
        
        /// <summary>
        /// 二分之一圆周率
        /// <para>值为π/2，约为1.5708</para>
        /// </summary>
        abstract static T Half_PI { get; }
        
        /// <summary>
        /// 四分之一圆周率
        /// <para>值为π/4，约为0.7854</para>
        /// </summary>
        abstract static T Quarter_PI { get; }
        
        /// <summary>
        /// 两倍圆周率
        /// <para>值为2π，约为6.28319</para>
        /// </summary>
        abstract static T Two_PI { get; }

        /// <summary>
        /// 10的-1次方
        /// <para>值为0.1</para>
        /// </summary>
        abstract static T TPN1 { get; }
        
        /// <summary>
        /// 10的-2次方
        /// <para>值为0.01</para>
        /// </summary>
        abstract static T TPN2 { get; }
        
        /// <summary>
        /// 10的-3次方
        /// <para>值为0.001</para>
        /// </summary>
        abstract static T TPN3 { get; }
        
        /// <summary>
        /// 10的-4次方
        /// <para>值为0.0001</para>
        /// </summary>
        abstract static T TPN4 { get; }

        /// <summary>
        /// 180度
        /// <para>用于角度和弧度的转换</para>
        /// </summary>
        abstract static T N180 { get; }
        
        /// <summary>
        /// 360度
        /// <para>用于角度和弧度的转换</para>
        /// </summary>
        abstract static T N360 { get; }

        /// <summary>
        /// 正无穷
        /// <para>表示正方向的无穷大</para>
        /// </summary>
        abstract static T PositiveInfinity { get; }
        
        /// <summary>
        /// 负无穷
        /// <para>表示负方向的无穷大</para>
        /// </summary>
        abstract static T NegativeInfinity { get; }

        /// <summary>
        /// 角度转弧度的系数
        /// <para>值为π/180</para>
        /// </summary>
        abstract static T DegToRad { get; }

        /// <summary>
        /// 弧度转角度的系数
        /// <para>值为180/π</para>
        /// </summary>
        abstract static T RadToDeg { get; }

        #endregion

        #region cast

        /// <summary>
        /// 获取整数部分
        /// <para>返回当前定点数的整数部分，小数部分被截断</para>
        /// </summary>
        /// <returns>当前定点数的整数部分</returns>
        T Integral();

        /// <summary>
        /// 获取整数部分
        /// <para>返回指定定点数的整数部分，小数部分被截断</para>
        /// </summary>
        /// <param name="n">要获取整数部分的定点数</param>
        /// <returns>指定定点数的整数部分</returns>
        abstract static T Integral(T n);

        /// <summary>
        /// 获取小数部分
        /// <para>返回当前定点数的小数部分，整数部分被截断</para>
        /// </summary>
        /// <returns>当前定点数的小数部分</returns>
        T Fractional();

        /// <summary>
        /// 获取小数部分
        /// <para>返回指定定点数的小数部分，整数部分被截断</para>
        /// </summary>
        /// <param name="n">要获取小数部分的定点数</param>
        /// <returns>指定定点数的小数部分</returns>
        abstract static T Fractional(T n);

        /// <summary>
        /// 转换为byte类型
        /// <para>将定点数显式转换为byte类型，可能会丢失精度</para>
        /// </summary>
        /// <param name="n">要转换的定点数</param>
        /// <returns>转换后的byte值</returns>
        abstract static explicit operator byte(T n);

        /// <summary>
        /// 转换为short类型
        /// <para>将定点数显式转换为short类型，可能会丢失精度</para>
        /// </summary>
        /// <param name="n">要转换的定点数</param>
        /// <returns>转换后的short值</returns>
        abstract static explicit operator short(T n);

        /// <summary>
        /// 转换为int类型
        /// <para>将定点数显式转换为int类型，可能会丢失精度</para>
        /// </summary>
        /// <param name="n">要转换的定点数</param>
        /// <returns>转换后的int值</returns>
        abstract static explicit operator int(T n);

        /// <summary>
        /// 转换为long类型
        /// <para>将定点数显式转换为long类型，可能会丢失精度</para>
        /// </summary>
        /// <param name="n">要转换的定点数</param>
        /// <returns>转换后的long值</returns>
        abstract static explicit operator long(T n);

        /// <summary>
        /// 转换为float类型
        /// <para>将定点数显式转换为float类型，可能会丢失精度</para>
        /// </summary>
        /// <param name="n">要转换的定点数</param>
        /// <returns>转换后的float值</returns>
        abstract static explicit operator float(T n);

        /// <summary>
        /// 转换为double类型
        /// <para>将定点数显式转换为double类型，可能会丢失精度</para>
        /// </summary>
        /// <param name="n">要转换的定点数</param>
        /// <returns>转换后的double值</returns>
        abstract static explicit operator double(T n);

        /// <summary>
        /// 从byte类型转换
        /// <para>将byte类型隐式转换为定点数</para>
        /// </summary>
        /// <param name="n">要转换的byte值</param>
        /// <returns>转换后的定点数</returns>
        abstract static implicit operator T(byte n);

        /// <summary>
        /// 从short类型转换
        /// <para>将short类型隐式转换为定点数</para>
        /// </summary>
        /// <param name="n">要转换的short值</param>
        /// <returns>转换后的定点数</returns>
        abstract static implicit operator T(short n);

        /// <summary>
        /// 从int类型转换
        /// <para>将int类型隐式转换为定点数</para>
        /// </summary>
        /// <param name="n">要转换的int值</param>
        /// <returns>转换后的定点数</returns>
        abstract static implicit operator T(int n);

        /// <summary>
        /// 从long类型转换
        /// <para>将long类型显式转换为定点数</para>
        /// </summary>
        /// <param name="n">要转换的long值</param>
        /// <returns>转换后的定点数</returns>
        abstract static explicit operator T(long n);

        /// <summary>
        /// 从float类型转换
        /// <para>将float类型显式转换为定点数</para>
        /// </summary>
        /// <param name="n">要转换的float值</param>
        /// <returns>转换后的定点数</returns>
        abstract static explicit operator T(float n);

        /// <summary>
        /// 从double类型转换
        /// <para>将double类型显式转换为定点数</para>
        /// </summary>
        /// <param name="n">要转换的double值</param>
        /// <returns>转换后的定点数</returns>
        abstract static explicit operator T(double n);

        #endregion

        #region compare

        /// <summary>
        /// 等于运算符
        /// <para>比较两个定点数是否相等</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果两个定点数相等则返回true，否则返回false</returns>
        abstract static bool operator ==(T a, T b);

        /// <summary>
        /// 不等于运算符
        /// <para>比较两个定点数是否不相等</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果两个定点数不相等则返回true，否则返回false</returns>
        abstract static bool operator !=(T a, T b);

        /// <summary>
        /// 大于运算符
        /// <para>比较第一个定点数是否大于第二个定点数</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果第一个定点数大于第二个定点数则返回true，否则返回false</returns>
        abstract static bool operator >(T a, T b);

        /// <summary>
        /// 小于运算符
        /// <para>比较第一个定点数是否小于第二个定点数</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果第一个定点数小于第二个定点数则返回true，否则返回false</returns>
        abstract static bool operator <(T a, T b);

        /// <summary>
        /// 大于等于运算符
        /// <para>比较第一个定点数是否大于或等于第二个定点数</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果第一个定点数大于或等于第二个定点数则返回true，否则返回false</returns>
        abstract static bool operator >=(T a, T b);

        /// <summary>
        /// 小于等于运算符
        /// <para>比较第一个定点数是否小于或等于第二个定点数</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果第一个定点数小于或等于第二个定点数则返回true，否则返回false</returns>
        abstract static bool operator <=(T a, T b);

        #endregion

        #region add

        /// <summary>
        /// 加法运算符（定点数加整数）
        /// <para>将定点数与整数相加</para>
        /// </summary>
        /// <param name="a">定点数</param>
        /// <param name="b">整数</param>
        /// <returns>相加后的结果</returns>
        abstract static T operator +(T a, int b);

        /// <summary>
        /// 加法运算符（整数加定点数）
        /// <para>将整数与定点数相加</para>
        /// </summary>
        /// <param name="a">整数</param>
        /// <param name="b">定点数</param>
        /// <returns>相加后的结果</returns>
        abstract static T operator +(int a, T b);

        /// <summary>
        /// 加法运算符（定点数加定点数）
        /// <para>将两个定点数相加</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>相加后的结果</returns>
        abstract static T operator +(T a, T b);

        #endregion

        #region sub

        /// <summary>
        /// 减法运算符（定点数减整数）
        /// <para>将定点数减去整数</para>
        /// </summary>
        /// <param name="a">定点数</param>
        /// <param name="b">整数</param>
        /// <returns>相减后的结果</returns>
        abstract static T operator -(T a, int b);

        /// <summary>
        /// 减法运算符（整数减定点数）
        /// <para>将整数减去定点数</para>
        /// </summary>
        /// <param name="a">整数</param>
        /// <param name="b">定点数</param>
        /// <returns>相减后的结果</returns>
        abstract static T operator -(int a, T b);

        /// <summary>
        /// 减法运算符（定点数减定点数）
        /// <para>将第一个定点数减去第二个定点数</para>
        /// </summary>
        /// <param name="a">被减数</param>
        /// <param name="b">减数</param>
        /// <returns>相减后的结果</returns>
        abstract static T operator -(T a, T b);

        #endregion

        #region mul

        /// <summary>
        /// 乘法运算符（定点数乘整数）
        /// <para>将定点数与整数相乘</para>
        /// </summary>
        /// <param name="a">定点数</param>
        /// <param name="b">整数</param>
        /// <returns>相乘后的结果</returns>
        abstract static T operator *(T a, int b);

        /// <summary>
        /// 乘法运算符（整数乘定点数）
        /// <para>将整数与定点数相乘</para>
        /// </summary>
        /// <param name="a">整数</param>
        /// <param name="b">定点数</param>
        /// <returns>相乘后的结果</returns>
        abstract static T operator *(int a, T b);

        /// <summary>
        /// 乘法运算符（定点数乘定点数）
        /// <para>将两个定点数相乘</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>相乘后的结果</returns>
        abstract static T operator *(T a, T b);

        #endregion

        #region div

        /// <summary>
        /// 除法运算符（定点数除整数）
        /// <para>将定点数除以整数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>相除后的结果</returns>
        /// <exception cref="DivideByZeroException">当除数为0时抛出</exception>
        abstract static T operator /(T a, int b);

        /// <summary>
        /// 除法运算符（整数除定点数）
        /// <para>将整数除以定点数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>相除后的结果</returns>
        /// <exception cref="DivideByZeroException">当除数为0时抛出</exception>
        abstract static T operator /(int a, T b);

        /// <summary>
        /// 除法运算符（定点数除定点数）
        /// <para>将第一个定点数除以第二个定点数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>相除后的结果</returns>
        /// <exception cref="DivideByZeroException">当除数为0时抛出</exception>
        abstract static T operator /(T a, T b);

        #endregion

        #region mod

        /// <summary>
        /// 取模运算符（定点数取模整数）
        /// <para>计算定点数除以整数的余数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>取模后的结果</returns>
        /// <exception cref="DivideByZeroException">当除数为0时抛出</exception>
        abstract static T operator %(T a, int b);

        /// <summary>
        /// 取模运算符（整数取模定点数）
        /// <para>计算整数除以定点数的余数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>取模后的结果</returns>
        /// <exception cref="DivideByZeroException">当除数为0时抛出</exception>
        abstract static T operator %(int a, T b);

        /// <summary>
        /// 取模运算符（定点数取模定点数）
        /// <para>计算第一个定点数除以第二个定点数的余数</para>
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        /// <returns>取模后的结果</returns>
        /// <exception cref="DivideByZeroException">当除数为0时抛出</exception>
        abstract static T operator %(T a, T b);

        #endregion

        #region opposite

        /// <summary>
        /// 取反运算符
        /// <para>返回定点数的相反数</para>
        /// </summary>
        /// <param name="n">要取反的定点数</param>
        /// <returns>取反后的结果</returns>
        abstract static T operator -(T n);

        #endregion

        #region cast

        /// <summary>
        /// 转换为byte类型
        /// <para>将定点数转换为byte类型，可能会丢失精度</para>
        /// </summary>
        /// <returns>转换后的byte值</returns>
        byte ToByte();

        /// <summary>
        /// 转换为short类型
        /// <para>将定点数转换为short类型，可能会丢失精度</para>
        /// </summary>
        /// <returns>转换后的short值</returns>
        short ToShort();

        /// <summary>
        /// 转换为int类型
        /// <para>将定点数转换为int类型，可能会丢失精度</para>
        /// </summary>
        /// <returns>转换后的int值</returns>
        int ToInt();

        /// <summary>
        /// 转换为long类型
        /// <para>将定点数转换为long类型，可能会丢失精度</para>
        /// </summary>
        /// <returns>转换后的long值</returns>
        long ToLong();

        /// <summary>
        /// 转换为float类型
        /// <para>将定点数转换为float类型，可能会丢失精度</para>
        /// </summary>
        /// <returns>转换后的float值</returns>
        float ToFloat();

        /// <summary>
        /// 转换为double类型
        /// <para>将定点数转换为double类型，可能会丢失精度</para>
        /// </summary>
        /// <returns>转换后的double值</returns>
        double ToDouble();

        #endregion

        #region abs

        /// <summary>
        /// 绝对值
        /// <para>返回定点数的绝对值</para>
        /// </summary>
        /// <returns>定点数的绝对值</returns>
        T Abs();

        #endregion

        #region pow

        /// <summary>
        /// 幂运算（整数指数）
        /// <para>计算定点数的整数次幂</para>
        /// </summary>
        /// <param name="n">指数</param>
        /// <returns>计算结果</returns>
        T Pow(int n);

        /// <summary>
        /// 幂运算（定点数指数）
        /// <para>计算定点数的定点数次幂</para>
        /// </summary>
        /// <param name="n">指数</param>
        /// <returns>计算结果</returns>
        T Pow(T n);

        /// <summary>
        /// 判断是否为2的幂
        /// <para>检查定点数是否为2的幂</para>
        /// </summary>
        /// <returns>如果是2的幂则返回true，否则返回false</returns>
        bool IsPowerOfTwo();

        /// <summary>
        /// 最近的2的幂
        /// <para>返回最接近当前定点数的2的幂</para>
        /// </summary>
        /// <returns>最接近的2的幂</returns>
        T ClosestPowerOfTwo();

        /// <summary>
        /// 下一个2的幂
        /// <para>返回大于或等于当前定点数的最小2的幂</para>
        /// </summary>
        /// <returns>下一个2的幂</returns>
        T NextPowerOfTwo();

        #endregion

        #region exp

        /// <summary>
        /// 指数函数
        /// <para>计算自然常数e的定点数次幂</para>
        /// </summary>
        /// <returns>计算结果</returns>
        T Exp();

        #endregion

        #region sqrt

        /// <summary>
        /// 平方根
        /// <para>计算当前定点数的平方根</para>
        /// </summary>
        /// <returns>计算结果</returns>
        /// <exception cref="ArgumentException">当定点数为负数时抛出</exception>
        T Sqrt();

        /// <summary>
        /// 平方根
        /// <para>计算指定定点数的平方根</para>
        /// </summary>
        /// <param name="n">要计算平方根的定点数</param>
        /// <returns>计算结果</returns>
        /// <exception cref="ArgumentException">当定点数为负数时抛出</exception>
        abstract static T Sqrt(T n);

        /// <summary>
        /// 立方根
        /// <para>计算当前定点数的立方根</para>
        /// </summary>
        /// <returns>计算结果</returns>
        T Cbrt();

        /// <summary>
        /// 立方根
        /// <para>计算指定定点数的立方根</para>
        /// </summary>
        /// <param name="n">要计算立方根的定点数</param>
        /// <returns>计算结果</returns>
        abstract static T Cbrt(T n);

        #endregion

        #region log

        /// <summary>
        /// 自然对数
        /// <para>计算当前定点数的自然对数（以e为底）</para>
        /// </summary>
        /// <returns>计算结果</returns>
        /// <exception cref="ArgumentException">当定点数为负数或零时抛出</exception>
        T Log();

        /// <summary>
        /// 以2为底的对数
        /// <para>计算当前定点数的以2为底的对数</para>
        /// </summary>
        /// <returns>计算结果</returns>
        /// <exception cref="ArgumentException">当定点数为负数或零时抛出</exception>
        T Log2();

        /// <summary>
        /// 以10为底的对数
        /// <para>计算当前定点数的以10为底的对数</para>
        /// </summary>
        /// <returns>计算结果</returns>
        /// <exception cref="ArgumentException">当定点数为负数或零时抛出</exception>
        T Log10();

        #endregion

        #region round

        /// <summary>
        /// 四舍五入
        /// <para>将当前定点数四舍五入到最近的整数</para>
        /// </summary>
        /// <returns>四舍五入后的结果</returns>
        T Round();

        /// <summary>
        /// 四舍五入，返回整数类型
        /// <para>将当前定点数四舍五入到最近的整数，并返回int类型</para>
        /// </summary>
        /// <returns>四舍五入后的整数结果</returns>
        int RoundToInt();

        #endregion

        #region reciprocal

        /// <summary>
        /// 倒数
        /// <para>计算当前定点数的倒数</para>
        /// </summary>
        /// <returns>计算结果</returns>
        /// <exception cref="DivideByZeroException">当定点数为0时抛出</exception>
        T Reciprocal();

        #endregion

        #region floor

        /// <summary>
        /// 向下取整
        /// <para>返回小于或等于当前定点数的最大整数</para>
        /// </summary>
        /// <returns>向下取整后的结果</returns>
        T Floor();

        /// <summary>
        /// 向下取整，返回整数类型
        /// <para>返回小于或等于当前定点数的最大整数，并返回int类型</para>
        /// </summary>
        /// <returns>向下取整后的整数结果</returns>
        int FloorToInt();

        #endregion

        #region ceil

        /// <summary>
        /// 向上取整
        /// <para>返回大于或等于当前定点数的最小整数</para>
        /// </summary>
        /// <returns>向上取整后的结果</returns>
        T Ceil();

        /// <summary>
        /// 向上取整，返回整数类型
        /// <para>返回大于或等于当前定点数的最小整数，并返回int类型</para>
        /// </summary>
        /// <returns>向上取整后的整数结果</returns>
        int CeilToInt();

        #endregion

        #region min

        /// <summary>
        /// 最小值
        /// <para>返回两个定点数中的较小值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>两个定点数中的较小值</returns>
        abstract static T Min(T a, T b);

        /// <summary>
        /// 最小值
        /// <para>返回三个定点数中的较小值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <param name="c">第三个定点数</param>
        /// <returns>三个定点数中的较小值</returns>
        abstract static T Min(T a, T b, T c);

        /// <summary>
        /// 最小值
        /// <para>返回四个定点数中的较小值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <param name="c">第三个定点数</param>
        /// <param name="d">第四个定点数</param>
        /// <returns>四个定点数中的较小值</returns>
        abstract static T Min(T a, T b, T c, T d);

        /// <summary>
        /// 最小值
        /// <para>返回多个定点数中的较小值</para>
        /// </summary>
        /// <param name="fixeds">定点数数组</param>
        /// <returns>多个定点数中的较小值</returns>
        /// <exception cref="ArgumentException">当数组为空时抛出</exception>
        abstract static T Min(params T[] fixeds);

        #endregion

        #region max

        /// <summary>
        /// 最大值
        /// <para>返回两个定点数中的较大值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>两个定点数中的较大值</returns>
        abstract static T Max(T a, T b);

        /// <summary>
        /// 最大值
        /// <para>返回三个定点数中的较大值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <param name="c">第三个定点数</param>
        /// <returns>三个定点数中的较大值</returns>
        abstract static T Max(T a, T b, T c);

        /// <summary>
        /// 最大值
        /// <para>返回四个定点数中的较大值</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <param name="c">第三个定点数</param>
        /// <param name="d">第四个定点数</param>
        /// <returns>四个定点数中的较大值</returns>
        abstract static T Max(T a, T b, T c, T d);

        /// <summary>
        /// 最大值
        /// <para>返回多个定点数中的较大值</para>
        /// </summary>
        /// <param name="fixeds">定点数数组</param>
        /// <returns>多个定点数中的较大值</returns>
        /// <exception cref="ArgumentException">当数组为空时抛出</exception>
        abstract static T Max(params T[] fixeds);

        #endregion

        #region clamp

        /// <summary>
        /// 限制范围
        /// <para>将定点数限制在指定的最小值和最大值之间</para>
        /// </summary>
        /// <param name="value">要限制的定点数</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>限制在范围内的结果</returns>
        abstract static T Clamp(T value, T min, T max);

        /// <summary>
        /// 限制在0到1之间
        /// <para>将定点数限制在0到1的范围内</para>
        /// </summary>
        /// <param name="value">要限制的定点数</param>
        /// <returns>限制在0到1之间的结果</returns>
        abstract static T Clamp01(T value);

        #endregion

        #region is

        /// <summary>
        /// 是否为非数字
        /// <para>检查当前定点数是否为NaN</para>
        /// </summary>
        /// <returns>如果是NaN则返回true，否则返回false</returns>
        bool IsNaN();

        /// <summary>
        /// 是否为非数字
        /// <para>检查指定定点数是否为NaN</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是NaN则返回true，否则返回false</returns>
        abstract static bool IsNaN(T n);

        /// <summary>
        /// 是否为零
        /// <para>检查当前定点数是否为零</para>
        /// </summary>
        /// <returns>如果是零则返回true，否则返回false</returns>
        bool IsZero();

        /// <summary>
        /// 是否为零
        /// <para>检查指定定点数是否为零</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是零则返回true，否则返回false</returns>
        abstract static bool IsZero(T n);

        /// <summary>
        /// 是否为最小值
        /// <para>检查当前定点数是否为类型的最小值</para>
        /// </summary>
        /// <returns>如果是最小值则返回true，否则返回false</returns>
        bool IsMin();

        /// <summary>
        /// 是否为最小值
        /// <para>检查指定定点数是否为类型的最小值</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是最小值则返回true，否则返回false</returns>
        abstract static bool IsMin(T n);

        /// <summary>
        /// 是否为最大值
        /// <para>检查当前定点数是否为类型的最大值</para>
        /// </summary>
        /// <returns>如果是最大值则返回true，否则返回false</returns>
        bool IsMax();

        /// <summary>
        /// 是否为最大值
        /// <para>检查指定定点数是否为类型的最大值</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是最大值则返回true，否则返回false</returns>
        abstract static bool IsMax(T n);

        /// <summary>
        /// 是否为无穷大
        /// <para>检查当前定点数是否为无穷大</para>
        /// </summary>
        /// <returns>如果是无穷大则返回true，否则返回false</returns>
        bool IsInfinity();

        /// <summary>
        /// 是否为无穷大
        /// <para>检查指定定点数是否为无穷大</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是无穷大则返回true，否则返回false</returns>
        abstract static bool IsInfinity(T n);

        /// <summary>
        /// 是否为正无穷大
        /// <para>检查当前定点数是否为正无穷大</para>
        /// </summary>
        /// <returns>如果是正无穷大则返回true，否则返回false</returns>
        bool IsPositiveInfinity();

        /// <summary>
        /// 是否为正无穷大
        /// <para>检查指定定点数是否为正无穷大</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是正无穷大则返回true，否则返回false</returns>
        abstract static bool IsPositiveInfinity(T n);

        /// <summary>
        /// 是否为负无穷大
        /// <para>检查当前定点数是否为负无穷大</para>
        /// </summary>
        /// <returns>如果是负无穷大则返回true，否则返回false</returns>
        bool IsNegativeInfinity();

        /// <summary>
        /// 是否为负无穷大
        /// <para>检查指定定点数是否为负无穷大</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是负无穷大则返回true，否则返回false</returns>
        abstract static bool IsNegativeInfinity(T n);

        /// <summary>
        /// 是否为正数
        /// <para>检查当前定点数是否为正数</para>
        /// </summary>
        /// <returns>如果是正数则返回true，否则返回false</returns>
        bool IsPositive();

        /// <summary>
        /// 是否为正数
        /// <para>检查指定定点数是否为正数</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是正数则返回true，否则返回false</returns>
        abstract static bool IsPositive(T n);

        /// <summary>
        /// 是否为负数
        /// <para>检查当前定点数是否为负数</para>
        /// </summary>
        /// <returns>如果是负数则返回true，否则返回false</returns>
        bool IsNegative();

        /// <summary>
        /// 是否为负数
        /// <para>检查指定定点数是否为负数</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是负数则返回true，否则返回false</returns>
        abstract static bool IsNegative(T n);

        /// <summary>
        /// 是否为小数
        /// <para>检查当前定点数是否有小数部分</para>
        /// </summary>
        /// <returns>如果有小数部分则返回true，否则返回false</returns>
        bool IsFractional();

        /// <summary>
        /// 是否为小数
        /// <para>检查指定定点数是否有小数部分</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果有小数部分则返回true，否则返回false</returns>
        abstract static bool IsFractional(T n);

        /// <summary>
        /// 是否为正常数（非零、有限、非NaN）
        /// <para>检查当前定点数是否为正常数</para>
        /// </summary>
        /// <returns>如果是正常数则返回true；否则返回false</returns>
        bool IsNormal();

        /// <summary>
        /// 是否为正常数（非零、有限、非NaN）
        /// <para>检查指定定点数是否为正常数</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是正常数则返回true；否则返回false</returns>
        abstract static bool IsNormal(T n);

        /// <summary>
        /// 是否为有限数（非NaN、非无穷大）
        /// <para>检查当前定点数是否为有限数</para>
        /// </summary>
        /// <returns>如果是有限数则返回true；否则返回false</returns>
        bool IsFinite();

        /// <summary>
        /// 是否为有限数（非NaN、非无穷大）
        /// <para>检查指定定点数是否为有限数</para>
        /// </summary>
        /// <param name="n">要检查的定点数</param>
        /// <returns>如果是有限数则返回true；否则返回false</returns>
        abstract static bool IsFinite(T n);

        #endregion

        #region sign

        /// <summary>
        /// 获取符号
        /// <para>返回当前定点数的符号：正数返回1，负数返回-1，零返回0</para>
        /// </summary>
        /// <returns>符号值</returns>
        int Sign();

        /// <summary>
        /// 获取符号
        /// <para>返回指定定点数的符号：正数返回1，负数返回-1，零返回0</para>
        /// </summary>
        /// <param name="n">要获取符号的定点数</param>
        /// <returns>符号值</returns>
        abstract static int Sign(T n);

        /// <summary>
        /// 符号是否相同
        /// <para>检查两个定点数的符号是否相同</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>如果符号相同则返回true，否则返回false</returns>
        abstract static bool IsSigns(T a, T b);

        #endregion

        #region sin

        /// <summary>
        /// 正弦
        /// <para>计算指定弧度的正弦值</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>正弦值，范围在[-1, 1]之间</returns>
        abstract static T Sin(T radian);

        /// <summary>
        /// 快速正弦
        /// <para>使用查表法计算指定弧度的正弦值，速度更快但精度较低</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>正弦值，范围在[-1, 1]之间</returns>
        abstract static T FastSin(T radian);

        /// <summary>
        /// 反正弦
        /// <para>计算指定值的反正弦值</para>
        /// </summary>
        /// <param name="radian">输入值，范围在[-1, 1]之间</param>
        /// <returns>反正弦值，范围在[-π/2, π/2]之间</returns>
        /// <exception cref="ArgumentException">当输入值不在[-1, 1]范围内时抛出</exception>
        abstract static T Asin(T radian);

        #endregion

        #region cos

        /// <summary>
        /// 余弦
        /// <para>计算指定弧度的余弦值</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>余弦值，范围在[-1, 1]之间</returns>
        abstract static T Cos(T radian);

        /// <summary>
        /// 快速余弦
        /// <para>使用查表法计算指定弧度的余弦值，速度更快但精度较低</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>余弦值，范围在[-1, 1]之间</returns>
        abstract static T FastCos(T radian);

        /// <summary>
        /// 反余弦
        /// <para>计算指定值的反余弦值</para>
        /// </summary>
        /// <param name="radian">输入值，范围在[-1, 1]之间</param>
        /// <returns>反余弦值，范围在[0, π]之间</returns>
        /// <exception cref="ArgumentException">当输入值不在[-1, 1]范围内时抛出</exception>
        abstract static T Acos(T radian);

        #endregion

        #region tan

        /// <summary>
        /// 正切
        /// <para>计算指定弧度的正切值</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>正切值</returns>
        abstract static T Tan(T radian);

        /// <summary>
        /// 快速正切
        /// <para>使用查表法计算指定弧度的正切值，速度更快但精度较低</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>正切值</returns>
        abstract static T FastTan(T radian);

        /// <summary>
        /// 反正切
        /// <para>计算指定值的反正切值</para>
        /// </summary>
        /// <param name="radian">输入值</param>
        /// <returns>反正切值，范围在[-π/2, π/2]之间</returns>
        abstract static T Atan(T radian);

        /// <summary>
        /// 两参数反正切
        /// <para>计算指定y/x的反正切值，考虑象限</para>
        /// </summary>
        /// <param name="y">y坐标</param>
        /// <param name="x">x坐标</param>
        /// <returns>反正切值，范围在[-π, π]之间</returns>
        abstract static T Atan2(T y, T x);

        #endregion

        #region utils

        /// <summary>
        /// 角度规范化到[0, 2π]
        /// <para>将弧度值规范化到[0, 2π]范围内</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>规范化后的弧度值</returns>
        abstract static T NormalizeRadian(T radian);

        /// <summary>
        /// 角度转弧度
        /// <para>将角度值转换为弧度值</para>
        /// </summary>
        /// <param name="degree">角度值</param>
        /// <returns>对应的弧度值</returns>
        abstract static T DegreeToRadian(T degree);

        /// <summary>
        /// 弧度转角度
        /// <para>将弧度值转换为角度值</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>对应的角度值</returns>
        abstract static T RadianToDegree(T radian);

        #endregion

        #region unity

        /// <summary>
        /// 移向目标值
        /// <para>将当前值向目标值移动，每次移动的最大距离为maxDelta</para>
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="maxDelta">最大移动距离</param>
        /// <returns>移动后的结果</returns>
        abstract static T MoveTowards(T current, T target, T maxDelta);

        /// <summary>
        /// 移向目标角度
        /// <para>将当前角度向目标角度移动，每次移动的最大角度为maxDelta</para>
        /// </summary>
        /// <param name="current">当前角度（弧度）</param>
        /// <param name="target">目标角度（弧度）</param>
        /// <param name="maxDelta">最大移动角度</param>
        /// <returns>移动后的角度</returns>
        abstract static T MoveTowardsAngle(T current, T target, T maxDelta);

        /// <summary>
        /// 重复值
        /// <para>将值限制在[0, length)范围内，超过则循环</para>
        /// </summary>
        /// <param name="t">输入值</param>
        /// <param name="length">范围长度</param>
        /// <returns>重复后的值</returns>
        abstract static T Repeat(T t, T length);

        /// <summary>
        /// 角度差
        /// <para>计算两个角度之间的最小差值</para>
        /// </summary>
        /// <param name="current">当前角度（弧度）</param>
        /// <param name="target">目标角度（弧度）</param>
        /// <returns>角度差，范围在[-π, π]之间</returns>
        abstract static T DeltaAngle(T current, T target);

        #endregion

        #region damp

        /// <summary>
        /// 平滑阻尼
        /// <para>使用阻尼算法平滑地移向目标值</para>
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="currentVelocity">当前速度（引用）</param>
        /// <param name="smoothTime">平滑时间</param>
        /// <param name="maxSpeed">最大速度</param>
        /// <param name="deltaTime">时间增量</param>
        /// <returns>平滑后的结果</returns>
        abstract static T SmoothDamp(T current, T target, ref T currentVelocity, T smoothTime, T maxSpeed, T deltaTime);

        /// <summary>
        /// 平滑阻尼
        /// <para>使用阻尼算法平滑地移向目标值，使用默认时间增量</para>
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="currentVelocity">当前速度（引用）</param>
        /// <param name="smoothTime">平滑时间</param>
        /// <param name="maxSpeed">最大速度</param>
        /// <returns>平滑后的结果</returns>
        abstract static T SmoothDamp(T current, T target, ref T currentVelocity, T smoothTime, T maxSpeed);

        /// <summary>
        /// 平滑阻尼
        /// <para>使用阻尼算法平滑地移向目标值，使用默认时间增量和最大速度</para>
        /// </summary>
        /// <param name="current">当前值</param>
        /// <param name="target">目标值</param>
        /// <param name="currentVelocity">当前速度（引用）</param>
        /// <param name="smoothTime">平滑时间</param>
        /// <returns>平滑后的结果</returns>
        abstract static T SmoothDamp(T current, T target, ref T currentVelocity, T smoothTime);

        #endregion
    }
}
