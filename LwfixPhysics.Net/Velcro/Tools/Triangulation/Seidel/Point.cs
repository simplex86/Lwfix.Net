using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Tools.Triangulation.Seidel
{
    internal class Point
    {
        // Pointers to next and previous points in Monontone Mountain
        public Point Next, Prev;

        public Fixed32 X, Y;

        public Point(Fixed32 x, Fixed32 y)
        {
            X = x;
            Y = y;
            Next = null;
            Prev = null;
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, Fixed32 f)
        {
            return new Point(p1.X - f, p1.Y - f);
        }

        public static Point operator +(Point p1, Fixed32 f)
        {
            return new Point(p1.X + f, p1.Y + f);
        }

        public Fixed32 Cross(Point p)
        {
            return X * p.Y - Y * p.X;
        }

        public Fixed32 Dot(Point p)
        {
            return X * p.X + Y * p.Y;
        }

        public bool Neq(Point p)
        {
            return p.X != X || p.Y != Y;
        }

        public Fixed32 Orient2D(Point pb, Point pc)
        {
            Fixed32 acx = X - pc.X;
            Fixed32 bcx = pb.X - pc.X;
            Fixed32 acy = Y - pc.Y;
            Fixed32 bcy = pb.Y - pc.Y;
            return acx * bcy - acy * bcx;
        }
    }
}
