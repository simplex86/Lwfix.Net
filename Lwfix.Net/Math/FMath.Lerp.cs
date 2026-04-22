namespace SimplexLab.Fixed
{
    /// <summary>
    /// 函数库 - 插值
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// Hermite插值
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="tangent1"></param>
        /// <param name="value2"></param>
        /// <param name="tangent2"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static T Hermite<T>(T value1, T tangent1, T value2, T tangent2, T amount) where T : struct, IFixed<T>
        {
            // All transformed to Fixed32 not to lose precission
            // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
            if (amount == T.Zero) return value1;
            if (amount == T.One)  return value2;

            var s1 = amount;
            var s2 = s1 * s1;
            var s3 = s1 * s2;

            var result = (2 * value1 - 2 * value2 + tangent2 + tangent1) * s3 +
                         (3 * value2 - 3 * value1 - 2 * tangent1 - tangent2) * s2 +
                         tangent1 * s1 + value1;

            return result;
        }

        /// <summary>
        /// 线性插值
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static T Lerp<T>(T value1, T value2, T amount) where T : struct, IFixed<T>
        {
            if (PreprocessLerp(value1, value2, amount, out var r))
            {
                return r;
            }

            if (value1 == value2 ||
                amount == T.Zero)
            {
                return value1;
            }

            return value1 + (value2 - value1) * amount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static T ClampLerp<T>(T value1, T value2, T amount) where T : struct, IFixed<T>
        {
            amount = Clamp01(amount);
            return Lerp(value1, value2, amount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static T InverseLerp<T>(T value1, T value2, T amount) where T : struct, IFixed<T>
        {
            if (PreprocessLerp(value1, value2, amount, out var r))
            {
                return r;
            }

            if (value1 == value2) return T.Zero;
            return Clamp01((amount - value1) / (value2 - value1));
        }

        /// <summary>
        /// 平滑插值
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static T SmoothStep<T>(T value1, T value2, T amount) where T : struct, IFixed<T>
        {
            if (PreprocessLerp(value1, value2, amount, out var r))
            {
                return r;
            }

            amount = Clamp01(amount);
            return Hermite(value1, T.Zero, value2, T.Zero, amount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="amount"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private static bool PreprocessLerp<T>(T value1, T value2, T amount, out T r) where T : struct, IFixed<T>
        {
            if (amount.IsNaN() || amount.IsInfinity())
            {
                r = T.NaN;
                return true;
            }

            if (value1.IsNaN() || value2.IsNaN())
            {
                r = T.NaN;
                return true;
            }

            if (value1.IsInfinity() && value2.IsInfinity())
            {
                r = T.IsSigns(value1, value2) ? value1 : T.NaN;
                return true;
            }

            r = T.Zero;
            return false;
        }
    }
}
