using SimplexLab.LwfixPhysics.Velcro.Shared.Optimization;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Code;

/// <summary>
/// Minimal poolable object used by the Pool tests. Mirrors the helper shipped
/// with the upstream VelcroPhysics test project.
/// </summary>
internal sealed class PoolObject : IPoolable<PoolObject>
{
    public PoolObject()
    {
        IsNew = true;
    }

    /// <summary>True when freshly created by the factory, false when returned to the pool.</summary>
    public bool IsNew { get; private set; }

    public void Dispose() { }

    public void Reset()
    {
        IsNew = false;
    }
}
