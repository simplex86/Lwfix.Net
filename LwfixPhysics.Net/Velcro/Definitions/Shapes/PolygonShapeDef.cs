using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;
using SimplexLab.LwfixPhysics.Velcro.Shared;

namespace SimplexLab.LwfixPhysics.Velcro.Definitions.Shapes
{
    public sealed class PolygonShapeDef : ShapeDef
    {
        public PolygonShapeDef() : base(ShapeType.Polygon)
        {
            SetDefaults();
        }

        public Vertices Vertices { get; set; }

        public override void SetDefaults()
        {
            Vertices = null;
            base.SetDefaults();
        }
    }
}
