namespace SimplexLab.Fixed
{
    /// <summary>
    /// 3x3矩阵
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix3x3<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="upwards"></param>
        /// <returns></returns>
        public static FMatrix3x3<T> LookAt(FVector3<T> forward, FVector3<T> upwards)
        {
            var zaxis = forward;
            zaxis.Normalize();
            var xaxis = FVector3<T>.Cross(upwards, zaxis);
            xaxis.Normalize();
            var yaxis = FVector3<T>.Cross(zaxis, xaxis);

            var matrix = new FMatrix3x3<T>()
            {
                M11 = xaxis.X,
                M21 = yaxis.X,
                M31 = zaxis.X,
                M12 = xaxis.Y,
                M22 = yaxis.Y,
                M32 = zaxis.Y,
                M13 = xaxis.Z,
                M23 = yaxis.Z,
                M33 = zaxis.Z,
            };

            return matrix;
        }
    }
}
