using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 插值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 在向量a和b之间用t线性插值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector2<T> Lerp(FVector2<T> a, FVector2<T> b, T t)
        {

            return new FVector2<T>(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        /// <summary>
        /// 在向量a和b之间用t线性插值，t会被限制在[0, 1]之间
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector2<T> ClampLerp(FVector2<T> a, FVector2<T> b, T t)
        {
            t = T.Clamp01(t);
            return Lerp(a, b, t);
        }
    }
}
