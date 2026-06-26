namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 二维向量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 归一化后的向量
        /// </summary>
        public readonly FVector3<T> Normalized => Normalize(this);

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
        /// <param name="v"></param>
        /// <returns></returns>
        public static FVector3<T> Normalize(FVector3<T> v)
        {
            var m = v.Magnitude;
            return (m.IsZero()) ? Zero : v / m;
        }

        /// <summary>
        /// 正交并归一化
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="tangent"></param>
        public static void OrthoNormalize(ref FVector3<T> normal, ref FVector3<T> tangent)
        {
            var ortho = Cross(normal, tangent);
            tangent = Cross(normal, ortho);

            normal.Normalize();
            tangent.Normalize();
        }

        /// <summary>
        /// 正交并归一化
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="tangent"></param>
        /// <param name="binormal"></param>
        public static void OrthoNormalize(ref FVector3<T> normal, ref FVector3<T> tangent, ref FVector3<T> binormal)
        {
            binormal = Cross(normal, tangent);
            tangent = Cross(normal, binormal);

            normal.Normalize();
            tangent.Normalize();
            binormal.Normalize();
        }
    }
}
