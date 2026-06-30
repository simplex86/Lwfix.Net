using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.ContactSystem
{
    public enum ContactType : byte
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
