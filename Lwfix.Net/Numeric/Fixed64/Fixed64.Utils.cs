using System;

namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 数学工具
    /// <para>包含定点数的数学工具方法，如角度规范化、角度转弧度等</para>
    /// </summary>
    public partial struct Fixed64 : IFixed<Fixed64>
    {
        /// <summary>
        /// 两个数符号是否相同
        /// </summary>
        /// <param name="a">第一个数的原始存储值</param>
        /// <param name="b">第二个数的原始存储值</param>
        /// <returns>符号是否相同</returns>
        private static bool IsSigns(Int128 a, Int128 b)
        {
            return ((a ^ b) & SIGN_BIT_MASK) == 0;
        }

        /// <summary>
        /// 角度规范化到[0, 2π]
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>规范化后的角度，范围在[0, 2π]之间</returns>
        public static Fixed64 NormalizeRadian(Fixed64 radian)
        {
            return NormalizeRadian(radian, Two_PI);
        }

        /// <summary>
        /// 角度规范化
        /// <para>将角度规范化到[0, unit]区间</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <param name="unit">规范化的单位，如2π</param>
        /// <returns>规范化后的角度，范围在[0, unit]之间</returns>
        private static Fixed64 NormalizeRadian(Fixed64 radian, Fixed64 unit)
        {
            // 使用除法和乘法来计算余数，提高精度
            var quotient = radian / unit;
            var integerPart = quotient.Floor();
            var remainder = radian - integerPart * unit;

            if (remainder < Zero)
            {
                remainder += unit;
            }

            return remainder;
        }

        /// <summary>
        /// 角度转弧度的系数
        /// <para>值为π/180</para>
        /// </summary>
        public static Fixed64 DegToRad { get; } = FromRaw((Int128)(Math.PI / 180 * FRACTIONAL_MULTIPLIER + 0.5));
        /// <summary>
        /// 弧度转角度的系数
        /// <para>值为180/π</para>
        /// </summary>
        public static Fixed64 RadToDeg { get; } = FromRaw((Int128)(180 / Math.PI * FRACTIONAL_MULTIPLIER + 0.5));

        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="degree">角度值</param>
        /// <returns>对应的弧度值</returns>
        public static Fixed64 DegreeToRadian(Fixed64 degree)
        {
            return degree * DegToRad;
        }

        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>对应的角度值</returns>
        public static Fixed64 RadianToDegree(Fixed64 radian)
        {
            return radian * RadToDeg;
        }
    }
}
