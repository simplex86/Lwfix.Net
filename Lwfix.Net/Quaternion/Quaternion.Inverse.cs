using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 四元数 - 逆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 逆
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FQuaternion<T> Inverse(FQuaternion<T> rotation)
        {
            var dx = rotation.X;
            var dy = rotation.Y;
            var dz = rotation.Z;
            var dw = rotation.W;

            var invn = (dx * dx + dy * dy + dz * dz + dw * dw).Reciprocal();
            return Conjugate(rotation) * invn;
        }
    }
}
