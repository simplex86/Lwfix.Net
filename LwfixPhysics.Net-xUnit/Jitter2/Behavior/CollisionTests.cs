namespace SimplexLab.LwfixPhysics.Jitter2.Test.Behavior;

public class CollisionTests
{
    [Fact]
    public void NoBodyWorldBoundingBox()
    {
        Real boxSize = (Real)10.0;
        BoxShape shape = new BoxShape(boxSize);
        Assert.True(MathHelper.CloseToZero(shape.WorldBoundingBox.Max - shape.Size * (Real)0.5));
        Assert.True(MathHelper.CloseToZero(shape.WorldBoundingBox.Min + shape.Size * (Real)0.5));
    }

    [Fact]
    public void OverlapDistanceTest()
    {
        BoxShape bs = new BoxShape(1);
        SphereShape ss = new SphereShape(1);

        var overlap = NarrowPhase.Overlap(bs, ss,
            JQuaternion.CreateRotationX((Real)0.2), JVector.UnitY * (Real)3.0);

        var separated = NarrowPhase.Distance(bs, ss,
            JQuaternion.CreateRotationX((Real)0.2), JVector.UnitY * (Real)3.0,
            out JVector pA, out JVector pB, out JVector normal, out Real dist);

        Assert.False(overlap);
        Assert.True(separated);

        Assert.True(MathR.Abs(dist - (Real)1.5) < (Real)1e-4);
        Assert.True(MathHelper.CloseToZero(pA - new JVector(0, (Real)0.5, 0), (Real)1e-4));
        Assert.True(MathHelper.CloseToZero(pB - new JVector(0, (Real)2.0, 0), (Real)1e-4));

        overlap = NarrowPhase.Overlap(bs, ss,
            JQuaternion.CreateRotationX((Real)0.2), JVector.UnitY * (Real)0.5);

        separated = NarrowPhase.Distance(bs, ss,
            JQuaternion.CreateRotationX((Real)0.2), JVector.UnitY * (Real)0.5,
            out pA, out pB, out normal, out dist);

        Assert.True(overlap);
        Assert.False(separated);

        JVector delta = new JVector(10, 13, -22);

        overlap = NarrowPhase.Overlap(ss, ss,
            JQuaternion.CreateRotationX((Real)0.2), delta);

        separated = NarrowPhase.Distance(ss, ss,
            JQuaternion.CreateRotationX((Real)0.2), delta,
            out pA, out pB, out normal, out dist);

        Assert.False(overlap);
        Assert.True(separated);

        Assert.True(MathR.Abs(dist - delta.Length() + 2) < (Real)1e-4);
        Assert.True(MathHelper.CloseToZero(pA - JVector.Normalize(delta), (Real)1e-4));
        Assert.True(MathHelper.CloseToZero(pB - delta + JVector.Normalize(delta), (Real)1e-4));
    }

    [Fact]
    public void SphereRayCast()
    {
        SphereShape ss = new SphereShape((Real)1.2);

        // Fixed32 precision (~2.3e-10) prevents the 1e-12 epsilon used by the float-based reference.
        Real epsilon = (Real)1e-6;

        bool hit = ss.LocalRayCast(new JVector(0, (Real)1.2 + (Real)0.25, 0), -JVector.UnitY, out JVector normal, out Real lambda);
        Assert.True(hit);
        Assert.True(MathR.Abs(lambda - (Real)0.25) < epsilon);
        Assert.True(MathHelper.CloseToZero(normal - JVector.UnitY));

        hit = ss.LocalRayCast(new JVector(0, (Real)1.2 + (Real)0.25, 0), -(Real)2.0 * JVector.UnitY, out normal, out lambda);
        Assert.True(hit);
        Assert.True(MathR.Abs(lambda - (Real)0.125) < epsilon);
        Assert.True(MathHelper.CloseToZero(normal - JVector.UnitY));

        hit = ss.LocalRayCast(new JVector(0, (Real)1.2 - (Real)0.25, 0), -JVector.UnitY, out normal, out lambda);
        Assert.True(hit);
        Assert.True(MathR.Abs(lambda) < epsilon);
        Assert.True(MathHelper.CloseToZero(normal));

        hit = ss.LocalRayCast(new JVector(0, -(Real)1.2 - (Real)0.25, 0), -JVector.UnitY * (Real)1.1, out normal, out lambda);
        Assert.False(hit);
        Assert.True(MathR.Abs(lambda) < epsilon);
        Assert.True(MathHelper.CloseToZero(normal));
    }

    [Fact]
    public void BoxRayCast()
    {
        BoxShape bs = new BoxShape((Real)1.2 * (Real)2.0);

        // Fixed32 precision (~2.3e-10) prevents the 1e-12 epsilon used by the float-based reference.
        Real epsilon = (Real)1e-6;

        bool hit = bs.LocalRayCast(new JVector(0, (Real)1.2 + (Real)0.25, 0), -JVector.UnitY, out JVector normal, out Real lambda);
        Assert.True(hit);
        Assert.True(MathR.Abs(lambda - (Real)0.25) < epsilon);
        Assert.True(MathHelper.CloseToZero(normal - JVector.UnitY));

        hit = bs.LocalRayCast(new JVector(0, (Real)1.2 + (Real)0.25, 0), -(Real)2.0 * JVector.UnitY, out normal, out lambda);
        Assert.True(hit);
        Assert.True(MathR.Abs(lambda - (Real)0.125) < epsilon);
        Assert.True(MathHelper.CloseToZero(normal - JVector.UnitY));

        hit = bs.LocalRayCast(new JVector(0, (Real)1.2 - (Real)0.25, 0), -JVector.UnitY, out normal, out lambda);
        Assert.True(hit);
        Assert.True(MathR.Abs(lambda) < epsilon);
        Assert.True(MathHelper.CloseToZero(normal));

        hit = bs.LocalRayCast(new JVector(0, -(Real)1.2 - (Real)0.25, 0), -JVector.UnitY * (Real)1.1, out normal, out lambda);
        Assert.False(hit);
        Assert.True(MathR.Abs(lambda) < epsilon);
        Assert.True(MathHelper.CloseToZero(normal));
    }

    [Fact]
    public void RayCast()
    {
        Real radius = (Real)4;

        JVector sp = new JVector(10, 11, 12);
        JVector op = new JVector(1, 2, 3);

        SphereShape s1 = new(radius);

        bool hit = NarrowPhase.RayCast(s1, JQuaternion.CreateRotationX((Real)0.32), sp,
            op, sp - op, out Real lambda, out JVector normal);

        JVector cn = JVector.Normalize(op - sp); // analytical normal
        JVector hp = op + (sp - op) * lambda; // hit point

        Assert.True(hit);
        Assert.True(MathHelper.CloseToZero(normal - cn, (Real)1e-6));

        Real distance = (hp - sp).Length();
        Assert.True(MathR.Abs(distance - radius) < (Real)1e-4);
    }

    [Fact]
    public void TransformedShapeOverlapDistance()
    {
        var box = new BoxShape(1);
        var rotation = JMatrix.CreateRotationZ(Real.PI / (Real)4.0);
        var ts = new TransformedShape(box, new JVector(0, 5, 0), rotation);

        var sphere = new SphereShape(1);

        var overlap = NarrowPhase.Overlap(ts, sphere,
            JQuaternion.Identity, JVector.Zero);
        Assert.False(overlap);

        var separated = NarrowPhase.Distance(ts, sphere,
            JQuaternion.Identity, JVector.Zero,
            out JVector pA, out JVector pB, out JVector normal, out Real dist);
        Assert.True(separated);
        Assert.True(dist > (Real)0.0);

        var tsClose = new TransformedShape(box, new JVector(0, (Real)0.5, 0), rotation);

        overlap = NarrowPhase.Overlap(tsClose, sphere,
            JQuaternion.Identity, JVector.Zero);
        Assert.True(overlap);
    }

    [Fact]
    public void TransformedShapeScaledOverlap()
    {
        var sphere = new SphereShape((Real)0.5);
        var ts = new TransformedShape(sphere, JMatrix.CreateScale((Real)3.0, (Real)1.0, (Real)1.0));

        var probe = new SphereShape((Real)0.1);

        var overlap = NarrowPhase.Overlap(ts, probe,
            JQuaternion.Identity, new JVector((Real)1.2, 0, 0));
        Assert.True(overlap);

        overlap = NarrowPhase.Overlap(ts, probe,
            JQuaternion.Identity, new JVector((Real)1.7, 0, 0));
        Assert.False(overlap);

        overlap = NarrowPhase.Overlap(ts, probe,
            JQuaternion.Identity, new JVector(0, (Real)0.7, 0));
        Assert.False(overlap);
    }

    [Fact]
    public void NormalDirection()
    {
        SphereShape s1 = new((Real)0.5);
        SphereShape s2 = new((Real)0.5);

        // -----------------------------------------------

        Assert.True(NarrowPhase.MprEpa(s1, s2, JQuaternion.Identity, JQuaternion.Identity, new JVector(-(Real)0.25, 0, 0), new JVector((Real)0.25, 0, 0),
            out JVector pointA, out JVector pointB, out JVector normal, out Real penetration));

        // pointA is on s1 and pointB is on s2
        Assert.True(pointA.X > (Real)0.0);
        Assert.True(pointB.X < (Real)0.0);

        // the collision normal points from s2 to s1
        Assert.True(normal.X > 0);

        // the separation is negative
        Assert.True(penetration > 0);

        // -----------------------------------------------

        Assert.True(NarrowPhase.Collision(s1, s2, JQuaternion.Identity, JQuaternion.Identity, new JVector(-(Real)0.25, 0, 0), new JVector((Real)0.25, 0, 0),
            out pointA, out pointB, out normal, out penetration));

        // pointA is on s1 and pointB is on s2
        Assert.True(pointA.X > 0);
        Assert.True(pointB.X < 0);

        // the collision normal points from s2 to s1
        Assert.True(normal.X > 0);

        // the separation is negative
        Assert.True(penetration > 0);

        // -----------------------------------------------

        BoxShape b1 = new(1);
        BoxShape b2 = new(1);

        Assert.True(NarrowPhase.MprEpa(b1, b2, JQuaternion.Identity, JQuaternion.Identity, new JVector(-(Real)0.25, (Real)0.1, 0), new JVector((Real)0.25, -(Real)0.1, 0),
            out pointA, out pointB, out normal, out penetration));

        // pointA is on s1 and pointB is on s2
        Assert.True(pointA.X > 0);
        Assert.True(pointB.X < 0);

        // the collision normal points from s2 to s1
        Assert.True(normal.X > 0);

        // the penetration is positive
        Assert.True(penetration > 0);

        // -----------------------------------------------

        Assert.True(NarrowPhase.Collision(b1, b2, JQuaternion.Identity, JQuaternion.Identity, new JVector(-(Real)2.25, 0, 0), new JVector((Real)2.25, 0, 0),
            out pointA, out pointB, out normal, out penetration));

        // the collision normal points from s2 to s1
        Assert.True(normal.X > 0);

        // the penetration is negative
        Assert.True(penetration < 0);
    }
}
