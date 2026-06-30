using System;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Handlers;
using SimplexLab.LwfixPhysics.Velcro.Collision.RayCast;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;

namespace SimplexLab.LwfixPhysics.Velcro.Collision.Broadphase
{
    public interface IBroadPhase
    {
        int ProxyCount { get; }

        void UpdatePairs(BroadphaseHandler callback);

        bool TestOverlap(int proxyIdA, int proxyIdB);

        int AddProxy(ref FixtureProxy proxy);

        void RemoveProxy(int proxyId);

        void MoveProxy(int proxyId, ref AABB aabb, Vector2 displacement);

        FixtureProxy GetProxy(int proxyId);

        void TouchProxy(int proxyId);

        void GetFatAABB(int proxyId, out AABB aabb);

        void Query(Func<int, bool> callback, ref AABB aabb);

        void RayCast(Func<RayCastInput, int, Fixed32> callback, ref RayCastInput input);

        void ShiftOrigin(ref Vector2 newOrigin);
    }
}
