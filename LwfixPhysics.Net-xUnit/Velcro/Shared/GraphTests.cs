using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Test.Code;
using Xunit;

namespace SimplexLab.LwfixPhysics.Velcro.Test.Shared;

/// <summary>
/// Port of <c>VelcroPhysics.Tests/Tests/Shared/GraphTests.cs</c> adapted to the Fixed32 type,
/// plus additional coverage for <see cref="Graph{T}"/> (Find, Contains, Clear, Remove-by-value).
/// </summary>
public class GraphTests
{
    // ---------------------------------------------------------------------
    // Original VelcroPhysics tests (ported)
    // ---------------------------------------------------------------------

    [Fact]
    public void TestEmptyConstruction()
    {
        Graph<int> graph = new Graph<int>();
        Assert.Equal(0, graph.Count);
        Assert.Null(graph.First);

        Graph<Dummy> graph2 = new Graph<Dummy>();
        Assert.Equal(0, graph2.Count);
        Assert.Null(graph2.First);
    }

    [Fact]
    public void TestAddValueType()
    {
        Graph<int> graph = new Graph<int>();
        GraphNode<int> node = graph.Add(10);

        Assert.Equal(10, node.Item);
        Assert.Equal(node, node.Prev);
        Assert.Equal(node, node.Next);
    }

    [Fact]
    public void TestAddReferenceType()
    {
        Graph<Dummy> graph = new Graph<Dummy>();

        Dummy instance = new Dummy();

        GraphNode<Dummy> node = graph.Add(instance);

        Assert.Equal(instance, node.Item);
        Assert.Equal(node, node.Prev);
        Assert.Equal(node, node.Next);
    }

    [Fact]
    public void TestRemoveValueType()
    {
        Graph<int> graph = new Graph<int>();
        GraphNode<int> node = graph.Add(10);

        Assert.Equal(1, graph.Count);

        graph.Remove(node);

        Assert.Equal(0, graph.Count);

        //Check that the node was cleared;
        Assert.Null(node.Prev);
        Assert.Null(node.Next);
    }

    [Fact]
    public void TestRemoveReferenceType()
    {
        Graph<Dummy> graph = new Graph<Dummy>();
        GraphNode<Dummy> node = graph.Add(new Dummy());

        Assert.Equal(1, graph.Count);

        graph.Remove(node);

        Assert.Equal(0, graph.Count);

        //Check that the node was cleared;
        Assert.Null(node.Prev);
        Assert.Null(node.Next);
    }

    [Fact]
    public void TestIteration()
    {
        Graph<int> graph = new Graph<int>();

        for (int i = 0; i < 10; i++)
        {
            graph.Add(i);
        }

        Assert.Equal(10, graph.Count);

        int count = 0;

        foreach (int i in graph)
        {
            Assert.Equal(count++, i);
        }

        Assert.Equal(10, count);
    }

    // ---------------------------------------------------------------------
    // Additional coverage
    // ---------------------------------------------------------------------

    [Fact]
    public void Add_Multiple_BuildsCircularLinks()
    {
        Graph<int> graph = new Graph<int>();
        GraphNode<int> n1 = graph.Add(1);
        GraphNode<int> n2 = graph.Add(2);
        GraphNode<int> n3 = graph.Add(3);

        Assert.Equal(3, graph.Count);
        Assert.Equal(n1, graph.First);
        // First.Prev should be the last inserted node.
        Assert.Equal(n3, graph.First.Prev);
        // Last.Next wraps around to First.
        Assert.Equal(n1, n3.Next);
        // Walk forward: 1 -> 2 -> 3 -> 1.
        Assert.Equal(n2, n1.Next);
        Assert.Equal(n3, n2.Next);
        Assert.Equal(n1, n3.Next);
    }

    [Fact]
    public void Remove_Middle_KeepsListConsistent()
    {
        Graph<int> graph = new Graph<int>();
        GraphNode<int> n1 = graph.Add(1);
        GraphNode<int> n2 = graph.Add(2);
        GraphNode<int> n3 = graph.Add(3);

        graph.Remove(n2);

        Assert.Equal(2, graph.Count);
        Assert.Null(n2.Prev);
        Assert.Null(n2.Next);
        // n1 should now link directly to n3.
        Assert.Equal(n3, n1.Next);
        Assert.Equal(n1, n3.Prev);
    }

    [Fact]
    public void Remove_First_PromotesNextToFirst()
    {
        Graph<int> graph = new Graph<int>();
        GraphNode<int> n1 = graph.Add(1);
        GraphNode<int> n2 = graph.Add(2);

        graph.Remove(n1);

        Assert.Equal(1, graph.Count);
        Assert.Equal(n2, graph.First);
        // Remaining single node should be self-linked.
        Assert.Equal(n2, n2.Prev);
        Assert.Equal(n2, n2.Next);
    }

    [Fact]
    public void Remove_LastSingle_ClearsFirst()
    {
        Graph<int> graph = new Graph<int>();
        GraphNode<int> n1 = graph.Add(1);

        graph.Remove(n1);

        Assert.Equal(0, graph.Count);
        Assert.Null(graph.First);
    }

    [Fact]
    public void Contains_ReturnsTrue_WhenValuePresent()
    {
        Graph<int> graph = new Graph<int>();
        graph.Add(7);
        graph.Add(42);

        Assert.True(graph.Contains(7));
        Assert.True(graph.Contains(42));
        Assert.False(graph.Contains(99));
    }

    [Fact]
    public void Contains_ReferenceType_UsesComparer()
    {
        Graph<Dummy> graph = new Graph<Dummy>();
        var a = new Dummy(1);
        var b = new Dummy(2);
        graph.Add(a);
        graph.Add(b);

        Assert.True(graph.Contains(a));
        Assert.True(graph.Contains(b));
        Assert.False(graph.Contains(new Dummy(1)));
    }

    [Fact]
    public void Find_ReturnsNode_WhenValuePresent()
    {
        Graph<int> graph = new Graph<int>();
        var n1 = graph.Add(10);
        var n2 = graph.Add(20);

        Assert.Equal(n1, graph.Find(10));
        Assert.Equal(n2, graph.Find(20));
        Assert.Null(graph.Find(99));
    }

    [Fact]
    public void Find_ReturnsNull_OnEmptyGraph()
    {
        Graph<int> graph = new Graph<int>();
        Assert.Null(graph.Find(0));
        Assert.Null(graph.Find(0));
    }

    [Fact]
    public void Remove_ByValue_RemovesNode()
    {
        Graph<int> graph = new Graph<int>();
        graph.Add(1);
        graph.Add(2);
        graph.Add(3);

        Assert.True(graph.Remove(2));
        Assert.Equal(2, graph.Count);
        Assert.False(graph.Contains(2));

        // Removing unknown value returns false.
        Assert.False(graph.Remove(99));
    }

    [Fact]
    public void Clear_InvalidatesAllNodes()
    {
        Graph<int> graph = new Graph<int>();
        var n1 = graph.Add(1);
        var n2 = graph.Add(2);
        var n3 = graph.Add(3);

        graph.Clear();

        // Note: Graph<T>.Clear() only invalidates individual nodes (sets Prev/Next to null);
        // it does NOT reset Count or First. This matches the upstream Velcro behavior.
        Assert.Null(n1.Prev);
        Assert.Null(n1.Next);
        Assert.Null(n2.Prev);
        Assert.Null(n2.Next);
        Assert.Null(n3.Prev);
        Assert.Null(n3.Next);
    }

    [Fact]
    public void GetNodes_YieldsAllNodesInOrder()
    {
        Graph<int> graph = new Graph<int>();
        graph.Add(11);
        graph.Add(22);
        graph.Add(33);

        var nodes = graph.GetNodes().ToList();
        Assert.Equal(3, nodes.Count);
        Assert.Equal(11, nodes[0].Item);
        Assert.Equal(22, nodes[1].Item);
        Assert.Equal(33, nodes[2].Item);
    }

    [Fact]
    public void CustomComparer_IsUsedForLookup()
    {
        // Comparer that treats two ints as equal when they differ by ±1.
        var loose = new LooseIntComparer();
        Graph<int> graph = new Graph<int>(loose);

        graph.Add(10);
        Assert.True(graph.Contains(11));
        Assert.True(graph.Contains(10));
        Assert.False(graph.Contains(20));
    }

    private sealed class LooseIntComparer : EqualityComparer<int>
    {
        public override bool Equals(int x, int y) => System.Math.Abs(x - y) <= 1;
        public override int GetHashCode(int obj) => obj.GetHashCode();
    }
}
