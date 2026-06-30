using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;

namespace SimplexLab.LwfixPhysics.Velcro.Tools.Triangulation.Earclip
{
    public class Triangle : Vertices
    {
        //Constructor automatically fixes orientation to ccw
        public Triangle(Fixed32 x1, Fixed32 y1, Fixed32 x2, Fixed32 y2, Fixed32 x3, Fixed32 y3)
        {
            Fixed32 cross = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);
            if (cross > 0)
            {
                Add(new Vector2(x1, y1));
                Add(new Vector2(x2, y2));
                Add(new Vector2(x3, y3));
            }
            else
            {
                Add(new Vector2(x1, y1));
                Add(new Vector2(x3, y3));
                Add(new Vector2(x2, y2));
            }
        }

        public bool IsInside(Fixed32 x, Fixed32 y)
        {
            Vector2 a = this[0];
            Vector2 b = this[1];
            Vector2 c = this[2];

            if (x < a.X && x < b.X && x < c.X) return false;
            if (x > a.X && x > b.X && x > c.X) return false;
            if (y < a.Y && y < b.Y && y < c.Y) return false;
            if (y > a.Y && y > b.Y && y > c.Y) return false;

            Fixed32 vx2 = x - a.X;
            Fixed32 vy2 = y - a.Y;
            Fixed32 vx1 = b.X - a.X;
            Fixed32 vy1 = b.Y - a.Y;
            Fixed32 vx0 = c.X - a.X;
            Fixed32 vy0 = c.Y - a.Y;

            Fixed32 dot00 = vx0 * vx0 + vy0 * vy0;
            Fixed32 dot01 = vx0 * vx1 + vy0 * vy1;
            Fixed32 dot02 = vx0 * vx2 + vy0 * vy2;
            Fixed32 dot11 = vx1 * vx1 + vy1 * vy1;
            Fixed32 dot12 = vx1 * vx2 + vy1 * vy2;
            Fixed32 invDenom = Fixed32.One / (dot00 * dot11 - dot01 * dot01);
            Fixed32 u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            Fixed32 v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            return u > 0 && v > 0 && u + v < Fixed32.One;
        }
    }
}
