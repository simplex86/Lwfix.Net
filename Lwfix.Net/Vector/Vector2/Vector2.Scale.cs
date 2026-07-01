using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 缩放
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scale"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(FVector2<T> scale)
        {
            X *= scale.X;
            Y *= scale.Y;
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector2<T> Scale(FVector2<T> a, FVector2<T> b)
        {
            return new FVector2<T>(a.X * b.X, a.Y * b.Y);
        }
    }
}
