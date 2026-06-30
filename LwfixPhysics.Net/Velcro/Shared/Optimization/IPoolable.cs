using System;
using SimplexLab.Lwfix;

namespace SimplexLab.LwfixPhysics.Velcro.Shared.Optimization
{
    public interface IPoolable<T> : IDisposable where T : IPoolable<T>
    {
        void Reset();
    }
}
