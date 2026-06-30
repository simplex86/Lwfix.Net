using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Test.Code;
using Xunit;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Shared;

/// <summary>
/// Port of <c>VelcroPhysics.Tests/Tests/Shared/PoolTests.cs</c> adapted to the Fixed32 type,
/// plus additional coverage for <see cref="Pool{T}"/> (capacity, reset toggling, LeftInPool).
/// </summary>
public class PoolTests
{
    // ---------------------------------------------------------------------
    // Original VelcroPhysics tests (ported)
    // ---------------------------------------------------------------------

    [Fact]
    public void GetWhileAdding()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), 1);

        bool first = true;

        //We use the fact that it is lazy loaded to re-add the same item
        IEnumerable<PoolObject> many = pool.GetManyFromPool(10);
        foreach (PoolObject obj in many)
        {
            if (first)
            {
                Assert.True(obj.IsNew);
                first = false;
            }
            else
                Assert.False(obj.IsNew);

            pool.ReturnToPool(obj);
        }
    }

    [Fact]
    public void GetManyAcrossBoundary()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), null, 6);

        //We get twice as many as in pool
        List<PoolObject> many = pool.GetManyFromPool(12).ToList();
        foreach (PoolObject obj in many)
        {
            Assert.True(obj.IsNew);
        }

        Assert.Equal(12, many.Count);
    }

    [Fact]
    public void GetManyNewAndPooled()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), 10);

        //Empty whole pool
        List<PoolObject> many = pool.GetManyFromPool(10).ToList();

        foreach (PoolObject obj in many)
        {
            Assert.True(obj.IsNew);
        }

        Assert.Equal(10, many.Count);

        many = pool.GetManyFromPool(10).ToList();
        foreach (PoolObject obj in many)
        {
            Assert.True(obj.IsNew);
            pool.ReturnToPool(obj);
        }

        Assert.Equal(10, many.Count);

        many = pool.GetManyFromPool(10).ToList();
        foreach (PoolObject obj in many)
        {
            Assert.False(obj.IsNew);
        }

        Assert.Equal(10, many.Count);
    }

    [Fact]
    public void GetOnePooled()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), 1);
        PoolObject obj = pool.GetFromPool();

        Assert.True(obj.IsNew);

        pool.ReturnToPool(obj);
        obj = pool.GetFromPool();

        Assert.False(obj.IsNew);
    }

    [Fact]
    public void GetOneNew()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), 0);
        PoolObject obj = pool.GetFromPool();

        Assert.True(obj.IsNew);

        obj = pool.GetFromPool();
        Assert.True(obj.IsNew);
    }

    // ---------------------------------------------------------------------
    // Additional coverage
    // ---------------------------------------------------------------------

    [Fact]
    public void LeftInPool_TracksAvailableCount()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), capacity: 5);

        Assert.Equal(5, pool.LeftInPool);

        var a = pool.GetFromPool();
        Assert.Equal(4, pool.LeftInPool);

        var b = pool.GetFromPool();
        Assert.Equal(3, pool.LeftInPool);

        pool.ReturnToPool(a);
        Assert.Equal(4, pool.LeftInPool);

        pool.ReturnToPool(b);
        Assert.Equal(5, pool.LeftInPool);
    }

    [Fact]
    public void PreCreateInstances_False_StartsEmpty()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), capacity: 3, preCreateInstances: false);
        Assert.Equal(0, pool.LeftInPool);

        // First pull is freshly created.
        var obj = pool.GetFromPool();
        Assert.True(obj.IsNew);
    }

    [Fact]
    public void ReturnToPool_DoesNotReset_WhenResetFalse()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), capacity: 1);
        var obj = pool.GetFromPool();
        Assert.True(obj.IsNew); // pre-created is "new" until returned

        // Return without reset: object keeps IsNew=true.
        pool.ReturnToPool(obj, reset: false);
        var obj2 = pool.GetFromPool();
        Assert.Same(obj, obj2);
        Assert.True(obj2.IsNew);
    }

    [Fact]
    public void ReturnToPool_Many_ResetsEach()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), capacity: 0);
        var a = pool.GetFromPool();
        var b = pool.GetFromPool();
        var c = pool.GetFromPool();

        Assert.True(a.IsNew);
        Assert.True(b.IsNew);
        Assert.True(c.IsNew);

        pool.ReturnToPool(new[] { a, b, c });

        Assert.False(a.IsNew);
        Assert.False(b.IsNew);
        Assert.False(c.IsNew);
    }

    [Fact]
    public void GetFromPool_WithReset_ReturnsFreshlyResetObject()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), capacity: 1);

        var obj = pool.GetFromPool();
        // Mark "used" by toggling back to "new" via a manual reset.
        obj.Reset(); // IsNew -> false
        pool.ReturnToPool(obj, reset: false); // do not change IsNew
        Assert.False(obj.IsNew);

        // Now pull it again with reset=true: should reset on the way out.
        var obj2 = pool.GetFromPool(reset: true);
        Assert.Same(obj, obj2);
        Assert.False(obj2.IsNew); // reset does not flip it back; it just runs the reset delegate.
    }

    [Fact]
    public void GetManyFromPool_EmptyPool_StillCreatesAll()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), x => x.Reset(), capacity: 0);
        var items = pool.GetManyFromPool(5).ToList();

        Assert.Equal(5, items.Count);
        foreach (var item in items)
            Assert.True(item.IsNew);
    }

    [Fact]
    public void NoResetDelegate_DoesNotThrowOnReturn()
    {
        Pool<PoolObject> pool = new Pool<PoolObject>(() => new PoolObject(), null, capacity: 2);
        var a = pool.GetFromPool();
        var b = pool.GetFromPool();

        // Returning with no reset delegate should be a no-op for reset.
        var ex = Record.Exception(() => pool.ReturnToPool(a));
        Assert.Null(ex);

        var ex2 = Record.Exception(() => pool.ReturnToPool(new[] { b }));
        Assert.Null(ex2);

        Assert.Equal(2, pool.LeftInPool);
    }
}
