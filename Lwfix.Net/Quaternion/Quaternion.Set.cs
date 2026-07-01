using System.Runtime.CompilerServices;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 四元数 - 设置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(T x, T y, T z, T w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFromToRotation(FVector3<T> from, FVector3<T> to)
        {
            this = FQuaternion<T>.FromToRotation(from, to);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLookRotation(FVector3<T> view)
        {
            this.SetLookRotation(view, FVector3<T>.Up);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="upwards"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLookRotation(FVector3<T> view, FVector3<T> upwards)
        {
            //this = FQuaternion<T>.LookRotation(view, upwards);
        }
    }
}
