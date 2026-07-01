using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 四元数 - 点乘
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 点乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Dot(FQuaternion<T> a, FQuaternion<T> b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
    }
}
