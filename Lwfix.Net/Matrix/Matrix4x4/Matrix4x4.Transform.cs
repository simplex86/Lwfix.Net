namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 4x4矩阵 - 变换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FMatrix4x4<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 获取平移矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> Translate(T x, T y, T z)
        {
            var matrix = new FMatrix4x4<T>(1, 0, 0, 0,
                                           0, 1, 0, 0,
                                           0, 0, 1, 0,
                                           x, y, z, 1);
            return matrix;
        }

        /// <summary>
        /// 获取平移矩阵
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> Translate(FVector3<T> translation)
        {
            return Translate(translation.X, translation.Y, translation.Z);
        }

        /// <summary>
        /// 获取旋转矩阵
        /// </summary>
        /// <param name="radians">与 X 轴的夹角（单位：弧度）</param>
        /// <returns></returns>
        public static FMatrix4x4<T> Rotate(FQuaternion<T> rotation)
        {
            // Precalculate coordinate products
            var x  = rotation.X * 2;
            var y  = rotation.Y * 2;
            var z  = rotation.Z * 2;
            var xx = rotation.X * x;
            var yy = rotation.Y * y;
            var zz = rotation.Z * z;
            var xy = rotation.X * y;
            var xz = rotation.X * z;
            var yz = rotation.Y * z;
            var wx = rotation.W * x;
            var wy = rotation.W * y;
            var wz = rotation.W * z;

            var result = new FMatrix4x4<T>()
            {
                M11 = T.One - (yy + zz),
                M21 = xy + wz,
                M31 = xz - wy,
                M41 = T.Zero,
                M12 = xy - wz,
                M22 = T.One - (xx + zz),
                M32 = yz + wx,
                M42 = T.Zero,
                M13 = xz + wy,
                M23 = yz - wx,
                M33 = T.One - (xx + yy),
                M43 = T.Zero,
                M14 = T.Zero,
                M24 = T.Zero,
                M34 = T.Zero,
                M44 = T.One,
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> RotateX(T radians)
        {
            var c = T.Cos(radians);
            var s = T.Sin(radians);

            // [  1  0  0  0 ]
            // [  0  c  s  0 ]
            // [  0 -s  c  0 ]
            // [  0  0  0  1 ]
            var result = new FMatrix4x4<T>()
            {
                M11 = T.One,
                M12 = T.Zero,
                M13 = T.Zero,
                M14 = T.Zero,
                M21 = T.Zero,
                M22 = c,
                M23 = s,
                M24 = T.Zero,
                M31 = T.Zero,
                M32 = -s,
                M33 = c,
                M34 = T.Zero,
                M41 = T.Zero,
                M42 = T.Zero,
                M43 = T.Zero,
                M44 = T.One,
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        /// <param name="centerPoint"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> RotateX(T radians, FVector3<T> centerPoint)
        {
            var c = T.Cos(radians);
            var s = T.Sin(radians);

            var y = centerPoint.Y * (T.One - c) + centerPoint.Z * s;
            var z = centerPoint.Z * (T.One - c) - centerPoint.Y * s;

            // [  1  0  0  0 ]
            // [  0  c  s  0 ]
            // [  0 -s  c  0 ]
            // [  0  y  z  1 ]
            var result = new FMatrix4x4<T>()
            {
                M11 = T.One,
                M12 = T.Zero,
                M13 = T.Zero,
                M14 = T.Zero,
                M21 = T.Zero,
                M22 = c,
                M23 = s,
                M24 = T.Zero,
                M31 = T.Zero,
                M32 = -s,
                M33 = c,
                M34 = T.Zero,
                M41 = T.Zero,
                M42 = y,
                M43 = z,
                M44 = T.One,
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> RotateY(T radians)
        {
            var c = T.Cos(radians);
            var s = T.Sin(radians);

            // [  c  0 -s  0 ]
            // [  0  1  0  0 ]
            // [  s  0  c  0 ]
            // [  0  0  0  1 ]
            var result = new FMatrix4x4<T>()
            {
                M11 = c,
                M12 = T.Zero,
                M13 = -s,
                M14 = T.Zero,
                M21 = T.Zero,
                M22 = T.One,
                M23 = T.Zero,
                M24 = T.Zero,
                M31 = s,
                M32 = T.Zero,
                M33 = c,
                M34 = T.Zero,
                M41 = T.Zero,
                M42 = T.Zero,
                M43 = T.Zero,
                M44 = T.One,
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        /// <param name="centerPoint"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> RotateY(T radians, FVector3<T> centerPoint)
        {
            var c = T.Cos(radians);
            var s = T.Sin(radians);

            var x = centerPoint.X * (T.One - c) - centerPoint.Z * s;
            var z = centerPoint.Z * (T.One - c) + centerPoint.X * s;

            // [  c  0 -s  0 ]
            // [  0  1  0  0 ]
            // [  s  0  c  0 ]
            // [  x  0  z  1 ]
            var result = new FMatrix4x4<T>()
            {
                M11 = c,
                M12 = T.Zero,
                M13 = -s,
                M14 = T.Zero,
                M21 = T.Zero,
                M22 = T.One,
                M23 = T.Zero,
                M24 = T.Zero,
                M31 = s,
                M32 = T.Zero,
                M33 = c,
                M34 = T.Zero,
                M41 = x,
                M42 = T.Zero,
                M43 = z,
                M44 = T.One,
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> RotateZ(T radians)
        {
            var c = T.Cos(radians);
            var s = T.Sin(radians);

            // [  c  s  0  0 ]
            // [ -s  c  0  0 ]
            // [  0  0  1  0 ]
            // [  0  0  0  1 ]
            var result = new FMatrix4x4<T>()
            {
                M11 = c,
                M12 = s,
                M13 = T.Zero,
                M14 = T.Zero,
                M21 = -s,
                M22 = c,
                M23 = T.Zero,
                M24 = T.Zero,
                M31 = T.Zero,
                M32 = T.Zero,
                M33 = T.One,
                M34 = T.Zero,
                M41 = T.Zero,
                M42 = T.Zero,
                M43 = T.Zero,
                M44 = T.One,
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        /// <param name="centerPoint"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> RotateZ(T radians, FVector3<T> centerPoint)
        {
            var c = T.Cos(radians);
            var s = T.Sin(radians);

            var x = centerPoint.X * (1 - c) + centerPoint.Y * s;
            var y = centerPoint.Y * (1 - c) - centerPoint.X * s;

            // [  c  s  0  0 ]
            // [ -s  c  0  0 ]
            // [  0  0  1  0 ]
            // [  x  y  0  1 ]
            var result = new FMatrix4x4<T>()
            {
                M11 = c,
                M12 = s,
                M13 = T.Zero,
                M14 = T.Zero,
                M21 = -s,
                M22 = c,
                M23 = T.Zero,
                M24 = T.Zero,
                M31 = T.Zero,
                M32 = T.Zero,
                M33 = T.One,
                M34 = T.Zero,
                M41 = T.Zero,
                M42 = T.Zero,
                M43 = T.Zero,
                M44 = T.One,
            };

            return result;
        }

        /// <summary>
        /// 获取缩放矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> Scale(T x, T y, T z)
        {
            var matrix = new FMatrix4x4<T>(x, 0, 0, 0,
                                           0, y, 0, 0,
                                           0, 0, z, 0,
                                           0, 0, 0, 1);
            return matrix;
        }

        /// <summary>
        /// 获取缩放矩阵
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> Scale(FVector3<T> scale)
        {
            return Scale(scale.X, scale.Y, scale.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="centerPoint"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> Scale(T x, T y, T z, FVector3<T> centerPoint)
        {
            var tx = centerPoint.X * (T.One - x);
            var ty = centerPoint.Y * (T.One - y);
            var tz = centerPoint.Z * (T.One - z);

            var result = new FMatrix4x4<T>()
            {
                M11 = x,
                M12 = T.Zero,
                M13 = T.Zero,
                M14 = T.Zero,
                M21 = T.Zero,
                M22 = y,
                M23 = T.Zero,
                M24 = T.Zero,
                M31 = T.Zero,
                M32 = T.Zero,
                M33 = z,
                M34 = T.Zero,
                M41 = tx,
                M42 = ty,
                M43 = tz,
                M44 = T.One,
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        /// <param name="centerPoint"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> Scale(FVector3<T> scale, FVector3<T> centerPoint)
        {
            var tx = centerPoint.X * (T.One - scale.X);
            var ty = centerPoint.Y * (T.One - scale.Y);
            var tz = centerPoint.Z * (T.One - scale.Z);

            var result = new FMatrix4x4<T>()
            {
                M11 = scale.X,
                M12 = T.Zero,
                M13 = T.Zero,
                M14 = T.Zero,
                M21 = T.Zero,
                M22 = scale.Y,
                M23 = T.Zero,
                M24 = T.Zero,
                M31 = T.Zero,
                M32 = T.Zero,
                M33 = scale.Z,
                M34 = T.Zero,
                M41 = tx,
                M42 = ty,
                M43 = tz,
                M44 = T.One,
            };

            return result;
        }

        /// <summary>
        /// 获取平移旋转缩放矩阵
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="radians"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public static FMatrix4x4<T> TRS(FVector3<T> translation, FQuaternion<T> rotation, FVector3<T> scale)
        {
            return Scale(scale) * Rotate(rotation) * Translate(translation);
        }

        public static FMatrix4x4<T> AngleAxis(T angle, FVector3<T> axis)
        {
            // a: angle
            // x, y, z: unit vector for axis.
            //
            // Rotation matrix M can compute by using below equation.
            //
            //        T               T
            //  M = uu + (cos a)( I-uu ) + (sin a)S
            //
            // Where:
            //
            //  u = ( x, y, z )
            //
            //      [  0 -z  y ]
            //  S = [  z  0 -x ]
            //      [ -y  x  0 ]
            //
            //      [ 1 0 0 ]
            //  I = [ 0 1 0 ]
            //      [ 0 0 1 ]
            //
            //
            //     [ xx+cosa*(1-xx)     yx-cosa*yx-sina*z  zx-cosa*xz+sina*y ]
            // M = [ xy-cosa*yx+sina*z  yy+cosa(1-yy)      yz-cosa*yz-sina*x ]
            //     [ zx-cosa*zx-sina*y  zy-cosa*zy+sina*x  zz+cosa*(1-zz)    ]
            //
            var x = axis.X;
            var y = axis.Y;
            var z = axis.Z;
            var sa = T.Sin(angle);
            var ca = T.Cos(angle);
            var xx = x * x;
            var yy = y * y;
            var zz = z * z;
            var xy = x * y;
            var xz = x * z;
            var yz = y * z;

            var result = new FMatrix4x4<T>()
            {
                M11 = xx + ca * (T.One - xx),
                M12 = xy - ca * xy + sa * z,
                M13 = xz - ca * xz - sa * y,
                M14 = T.Zero,
                M21 = xy - ca * xy - sa * z,
                M22 = yy + ca * (T.One - yy),
                M23 = yz - ca * yz + sa * x,
                M24 = T.Zero,
                M31 = xz - ca * xz + sa * y,
                M32 = yz - ca * yz - sa * x,
                M33 = zz + ca * (T.One - zz),
                M34 = T.Zero,
                M41 = T.Zero,
                M42 = T.Zero,
                M43 = T.Zero,
                M44 = T.One,
            };

            return result;
        }
    }
}
