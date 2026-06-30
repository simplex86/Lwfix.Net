using System;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Shared
{
    /// <summary>Rotation</summary>
    public struct Rot
    {
        /// Sine and cosine
        public Fixed32 s,
                     c;

        /// <summary>Initialize from an angle in radians</summary>
        /// <param name="angle">Angle in radians</param>
        public Rot(Fixed32 angle)
        {
            // TODO_ERIN optimize
            s = Fixed32.Sin(angle);
            c = Fixed32.Cos(angle);
        }

        /// <summary>Set using an angle in radians.</summary>
        /// <param name="angle"></param>
        public void Set(Fixed32 angle)
        {
            //Velcro: Optimization
            if (angle == 0)
            {
                s = 0;
                c = 1;
            }
            else
            {
                // TODO_ERIN optimize
                s = Fixed32.Sin(angle);
                c = Fixed32.Cos(angle);
            }
        }

        /// <summary>Set to the identity rotation</summary>
        public void SetIdentity()
        {
            s = Fixed32.Zero;
            c = Fixed32.One;
        }

        /// <summary>Get the angle in radians</summary>
        public Fixed32 GetAngle()
        {
            return Fixed32.Atan2(s, c);
        }

        /// <summary>Get the x-axis</summary>
        public Vector2 GetXAxis()
        {
            return new Vector2(c, s);
        }

        /// <summary>Get the y-axis</summary>
        public Vector2 GetYAxis()
        {
            return new Vector2(-s, c);
        }
    }
}
