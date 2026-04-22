namespace SimplexLab.Fixed
{
    /// <summary>
    /// 定点数 - 数学工具
    /// <para>包含定点数的数学工具方法，如角度规范化、角度转弧度等</para>
    /// </summary>
    public partial struct Fixed32 : IFixed<Fixed32>
    {
        /// <summary>
        /// 两个数符号是否相同
        /// <para>判断两个数的符号是否相同</para>
        /// </summary>
        /// <param name="a">第一个数的原始存储值</param>
        /// <param name="b">第二个数的原始存储值</param>
        /// <returns>符号是否相同</returns>
        private static bool IsSigns(long a, long b)
        {
            return ((a ^ b) & SIGN_BIT_MASK) == 0;
        }

        /// <summary>
        /// 角度规范化到[0, 2π]
        /// <para>将角度规范化到[0, 2π]区间</para>
        /// </summary>
        /// <param name="radian">角度（弧度）</param>
        /// <returns>规范化后的角度，范围在[0, 2π]之间</returns>
        public static Fixed32 NormalizeRadian(Fixed32 radian)
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
        /// <remarks>
        /// 实现原理：
        /// <list type="bullet">
        /// <item>使用除法和乘法来计算余数，提高精度</item>
        /// <item>对于负数余数，加上unit使其变为正数</item>
        /// </list>
        /// </remarks>
        private static Fixed32 NormalizeRadian(Fixed32 radian, Fixed32 unit)
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
        /// <para>用于将角度转换为弧度的系数，值为π/180</para>
        /// </summary>
        public static Fixed32 DegToRad { get; } = FromRaw(74961321);
        /// <summary>
        /// 弧度转角度的系数
        /// <para>用于将弧度转换为角度的系数，值为180/π</para>
        /// </summary>
        public static Fixed32 RadToDeg { get; } = FromRaw(246083499208);

        /// <summary>
        /// 角度转弧度
        /// <para>将角度值转换为弧度值</para>
        /// </summary>
        /// <param name="degree">角度值</param>
        /// <returns>对应的弧度值</returns>
        public static Fixed32 DegreeToRadian(Fixed32 degree)
        {
            return degree * DegToRad;
        }

        /// <summary>
        /// 弧度转角度
        /// <para>将弧度值转换为角度值</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>对应的角度值</returns>
        public static Fixed32 RadianToDegree(Fixed32 radian)
        {
            return radian * RadToDeg;
        }
    }
}
