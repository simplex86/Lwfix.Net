namespace SimplexLab.Fixed
{
    /// <summary>
    /// 四元数 - 矩阵
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 从4x4矩阵构造四元数
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static FQuaternion<T> FromMatrix(FMatrix3x3<T> matrix)
        {
            FQuaternion<T> result;

            var num8 = matrix.M11 + matrix.M22 + matrix.M33;
            if (num8 > 0)
            {
                var num = T.Sqrt(num8 + 1);
                result.W = num * T.Half;
                num = T.Half / num;
                result.X = (matrix.M23 - matrix.M32) * num;
                result.Y = (matrix.M31 - matrix.M13) * num;
                result.Z = (matrix.M12 - matrix.M21) * num;
            }
            else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                var num7 = T.Sqrt(1 + matrix.M11 - matrix.M22 - matrix.M33);
                var num4 = T.Half / num7;
                result.X = T.Half * num7;
                result.Y = (matrix.M12 + matrix.M21) * num4;
                result.Z = (matrix.M13 + matrix.M31) * num4;
                result.W = (matrix.M23 - matrix.M32) * num4;
            }
            else if (matrix.M22 > matrix.M33)
            {
                var num6 = T.Sqrt(1 + matrix.M22 - matrix.M11 - matrix.M33);
                var num3 = T.Half / num6;
                result.X = (matrix.M21 + matrix.M12) * num3;
                result.Y = T.Half * num6;
                result.Z = (matrix.M32 + matrix.M23) * num3;
                result.W = (matrix.M31 - matrix.M13) * num3;
            }
            else
            {
                var num5 = T.Sqrt(1 + matrix.M33 - matrix.M11 - matrix.M22);
                var num2 = T.Half / num5;
                result.X = (matrix.M31 + matrix.M13) * num2;
                result.Y = (matrix.M32 + matrix.M23) * num2;
                result.Z = T.Half * num5;
                result.W = (matrix.M12 - matrix.M21) * num2;
            }

            return result;
        }

        /// <summary>
        /// 转换成4x4矩阵
        /// </summary>
        /// <param name="quat"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> ToMatrix(FQuaternion<T> quat)
        {
            var matrix = new FMatrix4x4<T>();

            var x = quat.X * 2;
            var y = quat.Y * 2;
            var z = quat.Z * 2;
            var xx = quat.X * x;
            var yy = quat.Y * y;
            var zz = quat.Z * z;
            var xy = quat.X * y;
            var xz = quat.X * z;
            var yz = quat.Y * z;
            var wx = quat.W * x;
            var wy = quat.W * y;
            var wz = quat.W * z;

            matrix.M11 = 1 - (yy + zz);
            matrix.M21 = xy + wz;
            matrix.M31 = xz - wy;
            matrix.M41 = 0;

            matrix.M12 = xy - wz;
            matrix.M22 = 1 - (xx + zz);
            matrix.M32 = yz + wx;
            matrix.M42 = 0;

            matrix.M13 = xz + wy;
            matrix.M23 = yz - wx;
            matrix.M33 = 1 - (xx + yy);
            matrix.M43 = 0;

            matrix.M14 = 0;
            matrix.M24 = 0;
            matrix.M34 = 0;
            matrix.M44 = 1;

            return matrix;
        }
    }
}
