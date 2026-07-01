using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.ContactSystem
{
    internal enum ContactType : byte
    {
        NotSupported,
        Polygon,
        PolygonAndCircle,
        Circle,
        EdgeAndPolygon,
        EdgeAndCircle,
        ChainAndPolygon,
        ChainAndCircle
    }
}
