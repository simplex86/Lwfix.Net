
using System.Numerics;

namespace SimplexLab.Lwfix
{
    /// <summary>
    /// 四元数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial struct FQuaternion<T> where T : struct, IFixed<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static FQuaternion<T> operator +(FQuaternion<T> lhs, FQuaternion<T> rhs)
        {
            var quaternion = new FQuaternion<T>(lhs.X + rhs.X,
                                            lhs.Y + rhs.Y,
                                            lhs.Z + rhs.Z,
                                            lhs.W + rhs.W);
            return quaternion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static FQuaternion<T> operator -(FQuaternion<T> lhs, FQuaternion<T> rhs)
        {
            var quaternion = new FQuaternion<T>(lhs.X - rhs.X,
                                            lhs.Y - rhs.Y,
                                            lhs.Z - rhs.Z,
                                            lhs.W - rhs.W);
            return quaternion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quaternion"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static FQuaternion<T> operator *(FQuaternion<T> quaternion, T t)
        {
            var result = new FQuaternion<T>(quaternion.X * t,
                                            quaternion.Y * t,
                                            quaternion.Z * t,
                                            quaternion.W * t);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static FQuaternion<T> operator *(T t, FQuaternion<T> quaternion)
        {
            return quaternion * t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static FQuaternion<T> operator *(FQuaternion<T> lhs, FQuaternion<T> rhs)
        {
            var x1 = lhs.X;
            var y1 = lhs.Y;
            var z1 = lhs.Z;
            var w1 = lhs.W;

            var x2 = rhs.X;
            var y2 = rhs.Y;
            var z2 = rhs.Z;
            var w2 = rhs.W;

            var x = w1 * x2 + x1 * w2 + y1 * z2 - z1 * y2;
            var y = w1 * y2 + y1 * w2 + z1 * x2 - x1 * z2;
            var z = w1 * z2 + z1 * w2 + x1 * y2 - y1 * x2;
            var w = w1 * w2 - x1 * x2 - y1 * y2 - z1 * z2;

            return new FQuaternion<T>(x, y, z, w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static FVector3<T> operator *(FQuaternion<T> rotation, FVector3<T> point)
        {
            var n1  = rotation.X * 2;
            var n2  = rotation.Y * 2;
            var n3  = rotation.Z * 2;
            var n4  = rotation.X * n1;
            var n5  = rotation.Y * n2;
            var n6  = rotation.Z * n3;
            var n7  = rotation.X * n2;
            var n8  = rotation.X * n3;
            var n9  = rotation.Y * n3;
            var n10 = rotation.W * n1;
            var n11 = rotation.W * n2;
            var n12 = rotation.W * n3;

            var x = (1 - (n5 + n6)) * point.X + (n7 - n12) * point.Y + (n8 + n11) * point.Z;
            var y = (n7 + n12) * point.X + (1 - (n4 + n6)) * point.Y + (n9 - n10) * point.Z;
            var z = (n8 - n11) * point.X + (n9 + n10) * point.Y + (1 - (n4 + n5)) * point.Z;

            return new FVector3<T>(x, y, z);
        }
    }
}
