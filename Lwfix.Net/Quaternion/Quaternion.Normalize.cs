namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 四元数 - 归一化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 归一化
        /// </summary>
        public void Normalize()
        {
            this = Normalize(this);
        }

        /// <summary>
        /// 归一化
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static FQuaternion<T> Normalize(FQuaternion<T> q)
        {
            var d = Dot(q, q).Sqrt();
            return d.IsZero() ? FQuaternion<T>.Identity 
                              : new FQuaternion<T>(q.X / d, q.Y / d, q.Z / d, q.W / d);
        }
    }
}
