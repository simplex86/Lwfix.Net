using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量 - 三重积
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector2<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 三重积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FVector2<T> Multiply3(FVector2<T> a, FVector2<T> b, FVector2<T> c)
        {
            var z = a.X * b.Y - a.Y * b.X;
            return new FVector2<T>(-z * c.Y, z * c.X);
        }
    }
}
