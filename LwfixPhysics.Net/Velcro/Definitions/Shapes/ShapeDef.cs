using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Shapes
{
    public abstract class ShapeDef : IDef
    {
        protected ShapeDef(ShapeType type)
        {
            ShapeType = type;
        }

        /// <summary>Gets or sets the density.</summary>
        public Fixed32 Density { get; set; }

        /// <summary>Radius of the Shape</summary>
        public Fixed32 Radius { get; set; }

        /// <summary>Get the type of this shape.</summary>
        public ShapeType ShapeType { get; }

        public virtual void SetDefaults()
        {
            Density = 0;
            Radius = 0;
        }
    }
}
