namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 函数库 - 正弦/余弦融合计算
    /// </summary>
    public static partial class FMath
    {
        /// <summary>
        /// 同时计算正弦和余弦（Fixed32 专用真融合重载）
        /// <para>调用 <see cref="Fixed32.SinCos(Fixed32)"/>：一次 Preprocess + 一次 NormalizeRadian，
        /// sin/cos 共享象限缩减 + 泰勒展开路径。</para>
        /// </summary>
        /// <param name="radian">弧度值</param>
        /// <returns>(正弦值, 余弦值)</returns>
        /// <remarks>
        /// 优化（P1-2）：C# 重载解析优先选择非泛型重载，故 FMath.SinCos(fixed32Value) 走此路径，
        /// 而非泛型 <see cref="SinCos{T}"/>（假融合，分别调 Sin/Cos）。
        /// </remarks>
        public static (Fixed32 sin, Fixed32 cos) SinCos(Fixed32 radian)
        {
            return Fixed32.SinCos(radian);
        }

        /// <summary>
        /// 同时计算正弦和余弦（泛型回退实现）
        /// <para>一次调用返回 (sin, cos) 元组，避免对同一角度分别调用 <see cref="Sin{T}"/> 和 <see cref="Cos{T}"/></para>
        /// <para>注意：此为假融合，内部仍分别调用 Sin/Cos。Fixed32 实例会优先匹配上面的非泛型真融合重载。</para>
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
