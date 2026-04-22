namespace SimplexLab.Fixed
{
    /// <summary>
    /// 三维向量 - 投影
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FVector3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static FVector3<T> Project(FVector3<T> vector, FVector3<T> normal)
        {
            var magnitude = normal.SqrMagnitude; // Dot(normal, normal);
            return magnitude.IsZero() ? Zero : normal * Dot(vector, normal) / magnitude;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="planeNormal"></param>
        /// <returns></returns>
        public static FVector3<T> ProjectOnPlane(FVector3<T> vector, FVector3<T> planeNormal)
        {
            return vector - Project(vector, planeNormal);
        }
    }
}
