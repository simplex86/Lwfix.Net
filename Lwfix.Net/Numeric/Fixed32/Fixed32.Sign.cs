using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 符号
    /// <para>包含定点数的符号相关操作方法</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 获取符号
        /// <para>获取当前定点数的符号</para>
        /// </summary>
        /// <returns>符号值：-1表示负数，0表示零，1表示正数</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，抛出ArithmeticException异常</item>
        /// <item>如果输入是零，返回0</item>
        /// <item>如果输入是负数，返回-1</item>
        /// <item>如果输入是正数，返回1</item>
        /// </list>
        /// </remarks>
        public int Sign()
        {
            if (IsNaN()) throw new ArithmeticException("Function does not accept Not-a-Number values.");
            if (IsZero()) return 0;

            return IsNegative() ? -1 : 1;
        }

        /// <summary>
        /// 获取符号
        /// <para>静态方法，获取定点数的符号</para>
        /// </summary>
        /// <param name="n">要获取符号的值</param>
        /// <returns>符号值：-1表示负数，0表示零，1表示正数</returns>
        /// <remarks>
        /// 特殊情况处理：
        /// <list type="bullet">
        /// <item>如果输入是NaN，抛出ArithmeticException异常</item>
        /// <item>如果输入是零，返回0</item>
        /// <item>如果输入是负数，返回-1</item>
        /// <item>如果输入是正数，返回1</item>
        /// </list>
        /// </remarks>
        public static int Sign(Fixed32 n)
        {
            return FMath.Sign(n);
        }

        /// <summary>
        /// 符号是否相同
        /// <para>判断两个定点数的符号是否相同</para>
        /// </summary>
        /// <param name="a">第一个定点数</param>
        /// <param name="b">第二个定点数</param>
        /// <returns>符号是否相同</returns>
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>调用内部的IsSigns方法，比较两个数的原始存储值的符号</item>
        /// </list>
        /// </remarks>
        public static bool IsSigns(Fixed32 a, Fixed32 b)
        {
            return IsSigns(a.rawvalue, b.rawvalue);
        }
    }
}
