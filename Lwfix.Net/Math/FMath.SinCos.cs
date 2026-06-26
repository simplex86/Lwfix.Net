namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 函数库 - 正弦/余弦融合计算
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 同时计算正弦和余弦
        /// <para>一次调用返回 (sin, cos) 元组，避免对同一角度分别调用 <see cref="Sin{T}"/> 和 <see cref="Cos{T}"/></para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="radian">弧度值</param>
        /// <returns>(正弦值, 余弦值)</returns>
        public static (T sin, T cos) SinCos<T>(T radian) where T : struct, IFixed<T>
        {
            return (T.Sin(radian), T.Cos(radian));
        }
    }
}
