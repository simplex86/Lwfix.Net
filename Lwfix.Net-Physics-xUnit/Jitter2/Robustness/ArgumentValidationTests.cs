using System;
using SimplexLab.Fixed.Physics.Unmanaged;

namespace LwfixTest.Physics.Jitter2.Robustness;

public class ArgumentValidationTests
{
    [Fact]
    public void ConstraintInitialize_WithZeroAxis_Throws()
    {
        using World world = new();
        RigidBody body1 = world.CreateRigidBody();
        RigidBody body2 = world.CreateRigidBody();
        PointOnLine constraint = world.CreateConstraint<PointOnLine>(body1, body2);

        Assert.Throws<ArgumentException>(() => constraint.Initialize(JVector.Zero, JVector.Zero, JVector.Zero));
    }

    [Fact]
    public void ShapeCreation_WithNaNDimension_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = new SphereShape(Real.NaN));
    }

    [Fact]
    public void PointCloudShape_WithDegenerateVertices_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => _ = new PointCloudShape([JVector.Zero]));

        Assert.Throws<InvalidOperationException>(() => _ = new PointCloudShape([
            JVector.Zero,
            JVector.UnitX,
            JVector.UnitY,
            JVector.UnitX + JVector.UnitY
        ]));

        Assert.Throws<InvalidOperationException>(() => _ = new PointCloudShape([
            new JVector(-1, -1, 1),
            new JVector(+1, -1, 1),
            new JVector(+1, +1, 1),
            new JVector(-1, +1, 1)
        ]));
    }

    [Fact]
    public void ConvexHullShape_WithZeroMassTriangles_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => _ = new ConvexHullShape([
            new JTriangle(JVector.Zero, JVector.UnitX, JVector.UnitY)
        ]));
    }

    [Fact]
    public void SupportPrimitiveCreation_WithInfiniteDimension_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = SupportPrimitives.CreateSphere(Real.PositiveInfinity));
    }

    [Fact]
    public void DynamicTreeAddProxy_WithDuplicateOrOversizedProxy_ThrowsDistinctExceptions()
    {
        using World world = new();
        var proxy = new SphereShape((Real)1.0);
        world.DynamicTree.AddProxy(proxy);

        var duplicate = Assert.Throws<ArgumentException>(() => world.DynamicTree.AddProxy(proxy));
        Assert.Equal("proxy", duplicate.ParamName);

        var oversized = new BoxShape((Real)1e8);
        var tooLarge = Assert.Throws<ArgumentOutOfRangeException>(() => world.DynamicTree.AddProxy(oversized));
        Assert.Equal("proxy", tooLarge.ParamName);
    }

    [Fact]
    public void TriangleMeshCreate_WithInvalidVertexTypeOrIndexCount_ReportsParameter()
    {
        Real[] rawVertices = [(Real)0.0, (Real)1.0, (Real)2.0];
        int[] triangle = [0, 0, 0];

        var vertexType = Assert.Throws<ArgumentException>(() =>
            TriangleMesh.Create<Real>(rawVertices, triangle, true));
        Assert.Equal("vertices", vertexType.ParamName);

        JVector[] vertices = [JVector.Zero];
        int[] incompleteTriangle = [0, 0];

        var indexCount = Assert.Throws<ArgumentException>(() =>
            TriangleMesh.Create<JVector>(vertices, incompleteTriangle, true));
        Assert.Equal("indices", indexCount.ParamName);
    }

    [Fact]
    public void SolverIterations_WithInvalidTupleComponent_ReportsComponent()
    {
        using World world = new();

        var solver = Assert.Throws<ArgumentOutOfRangeException>(() => world.SolverIterations = (0, 0));
        Assert.Equal("solver", solver.ParamName);

        var relaxation = Assert.Throws<ArgumentOutOfRangeException>(() => world.SolverIterations = (1, -1));
        Assert.Equal("relaxation", relaxation.ParamName);
    }

    [Fact]
    public void CustomExceptions_HaveStandardConstructors()
    {
        AssertStandardExceptionConstructors(typeof(SameBodyException));
        AssertStandardExceptionConstructors(typeof(World.InvalidCollisionTypeException));
        AssertStandardExceptionConstructors(typeof(TriangleMesh.DegenerateTriangleException));
        AssertStandardExceptionConstructors(typeof(PartitionedBuffer<int>.MaximumSizeException));
    }

    private static void AssertStandardExceptionConstructors(Type exceptionType)
    {
        Assert.NotNull(exceptionType.GetConstructor(Type.EmptyTypes));
        Assert.NotNull(exceptionType.GetConstructor(new[] { typeof(string) }));
        Assert.NotNull(exceptionType.GetConstructor(new[] { typeof(string), typeof(Exception) }));
    }
}
