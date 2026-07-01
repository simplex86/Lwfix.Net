using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Narrowphase
{
    /// <summary>This is used for determining the state of contact points.</summary>
    internal enum PointState
    {
        /// <summary>Point does not exist</summary>
        Null,

        /// <summary>Point was added in the update</summary>
        Add,

        /// <summary>Point persisted across the update</summary>
        Persist,

        /// <summary>Point was removed in the update</summary>
        Remove
    }
}
