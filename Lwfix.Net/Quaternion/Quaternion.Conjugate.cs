using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 四元数 - 共轭
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 共轭
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> Conjugate(FQuaternion<T> value)
        {
            return new FQuaternion<T>(-value.X, -value.Y, -value.Z, value.W);
        }
    }
}
