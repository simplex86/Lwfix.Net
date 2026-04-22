namespace SimplexLab.Fixed
{
    /// <summary>
    /// 4x4矩阵
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> LookAt(FVector3<T> from, FVector3<T> to, FVector3<T> up)
        {
            return TRS(from, FQuaternion<T>.LookRotation(to - from, up), FVector3<T>.One);
        }
    }
}
