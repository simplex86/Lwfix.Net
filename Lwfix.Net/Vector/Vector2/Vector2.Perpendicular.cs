using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 垂直
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 垂直向量（逆时针旋转90度）
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector2<T> Perpendicular(FVector2<T> v)
        {
            return new FVector2<T>(-v.Y, v.X);
        }
    }
}
