using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Shared
{
    /// <summary>A 2-by-2 matrix. Stored in column-major order.</summary>
    public struct Mat22
    {
        public Vector2 ex, ey;

        /// <summary>Construct this matrix using columns.</summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        public Mat22(Vector2 c1, Vector2 c2)
        {
            ex = c1;
            ey = c2;
        }

        /// <summary>Construct this matrix using scalars.</summary>
        /// <param name="a11">The a11.</param>
        /// <param name="a12">The a12.</param>
        /// <param name="a21">The a21.</param>
        /// <param name="a22">The a22.</param>
        public Mat22(Fixed32 a11, Fixed32 a12, Fixed32 a21, Fixed32 a22)
        {
            ex = new Vector2(a11, a21);
            ey = new Vector2(a12, a22);
        }

        public Mat22 Inverse
        {
            get
            {
                Fixed32 a = ex.X, b = ey.X, c = ex.Y, d = ey.Y;
                Fixed32 det = a * d - b * c;
                if (det != Fixed32.Zero)
                    det = Fixed32.One / det;

                Mat22 result = new Mat22();
                result.ex.X = det * d;
                result.ex.Y = -det * c;

                result.ey.X = -det * b;
                result.ey.Y = det * a;

                return result;
            }
        }

        /// <summary>Initialize this matrix using columns.</summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        public void Set(Vector2 c1, Vector2 c2)
        {
            ex = c1;
            ey = c2;
        }

        /// <summary>Set this to the identity matrix.</summary>
        public void SetIdentity()
        {
            ex.X = Fixed32.One;
            ey.X = Fixed32.Zero;
            ex.Y = Fixed32.Zero;
            ey.Y = Fixed32.One;
        }

        /// <summary>Set this matrix to all zeros.</summary>
        public void SetZero()
        {
            ex.X = Fixed32.Zero;
            ey.X = Fixed32.Zero;
            ex.Y = Fixed32.Zero;
            ey.Y = Fixed32.Zero;
        }

        /// <summary>
        /// Solve A * x = b, where b is a column vector. This is more efficient than computing the inverse in one-shot
        /// cases.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public Vector2 Solve(Vector2 b)
        {
            Fixed32 a11 = ex.X, a12 = ey.X, a21 = ex.Y, a22 = ey.Y;
            Fixed32 det = a11 * a22 - a12 * a21;
            if (det != Fixed32.Zero)
                det = Fixed32.One / det;

            return new Vector2(det * (a22 * b.X - a12 * b.Y), det * (a11 * b.Y - a21 * b.X));
        }

        public static void Add(ref Mat22 A, ref Mat22 B, out Mat22 R)
        {
            R.ex = A.ex + B.ex;
            R.ey = A.ey + B.ey;
        }
    }
}
