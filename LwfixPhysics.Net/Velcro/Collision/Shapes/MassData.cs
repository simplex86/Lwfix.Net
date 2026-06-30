using System;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Shapes
{
    /// <summary>This holds the mass data computed for a shape.</summary>
    public struct MassData : IEquatable<MassData>
    {
        internal Fixed32 _area;
        internal Vector2 _centroid;
        internal Fixed32 _inertia;
        internal Fixed32 _mass;

        //Velcro: We store the area as well
        /// <summary>The area of the shape</summary>
        public Fixed32 Area
        {
            get => _area;
            set => _area = value;
        }

        /// <summary>The position of the shape's centroid relative to the shape's origin.</summary>
        public Vector2 Centroid
        {
            get => _centroid;
            set => _centroid = value;
        }

        /// <summary>The rotational inertia of the shape about the local origin.</summary>
        public Fixed32 Inertia
        {
            get => _inertia;
            set => _inertia = value;
        }

        /// <summary>The mass of the shape, usually in kilograms.</summary>
        public Fixed32 Mass
        {
            get => _mass;
            set => _mass = value;
        }

        /// <summary>The equal operator</summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static bool operator ==(MassData left, MassData right)
        {
            return left._area == right._area && left._mass == right._mass && left._centroid == right._centroid && left._inertia == right._inertia;
        }

        /// <summary>The not equal operator</summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static bool operator !=(MassData left, MassData right)
        {
            return !(left == right);
        }

        public bool Equals(MassData other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (obj.GetType() != typeof(MassData))
                return false;

            return Equals((MassData)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _area.GetHashCode();
                result = (result * 397) ^ _centroid.GetHashCode();
                result = (result * 397) ^ _inertia.GetHashCode();
                result = (result * 397) ^ _mass.GetHashCode();
                return result;
            }
        }
    }
}
