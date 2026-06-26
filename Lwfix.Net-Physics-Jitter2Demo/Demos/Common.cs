using System;
using System.Collections.Generic;
using SimplexLab.Fixed.Physics.Collision;
using SimplexLab.Fixed.Physics.Collision.Shapes;
using SimplexLab.Fixed.Physics.Dynamics;
using SimplexLab.Fixed.Physics.Dynamics.Constraints;
using SimplexLab.Fixed.Physics.JDemo.Renderer;
using SimplexLab.Fixed.Physics.LinearMath;

namespace SimplexLab.Fixed.Physics.JDemo;

public static class Common
{
    public class IgnoreCollisionBetweenFilter : IBroadPhaseFilter
    {
        private readonly struct Pair : IEquatable<Pair>
        {
            private readonly RigidBodyShape shapeA, shapeB;

            public Pair(RigidBodyShape shapeA, RigidBodyShape shapeB)
            {
                this.shapeA = shapeA;
                this.shapeB = shapeB;
            }

            public bool Equals(Pair other)
            {
                return shapeA.Equals(other.shapeA) && shapeB.Equals(other.shapeB);
            }

            public override bool Equals(object? obj)
            {
                return obj is Pair other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(shapeA, shapeB);
            }
        }

        private readonly HashSet<Pair> ignore = new();

        public bool Filter(IDynamicTreeProxy proxyA, IDynamicTreeProxy proxyB)
        {
            if (proxyA is not RigidBodyShape shapeA || proxyB is not RigidBodyShape shapeB) return false;

            if (shapeB.ShapeId < shapeA.ShapeId) (shapeA, shapeB) = (shapeB, shapeA);
            return !ignore.Contains(new Pair(shapeA, shapeB));
        }

        public void IgnoreCollisionBetween(RigidBodyShape shapeA, RigidBodyShape shapeB)
        {
            if (shapeB.ShapeId < shapeA.ShapeId) (shapeA, shapeB) = (shapeB, shapeA);
            ignore.Add(new Pair(shapeA, shapeB));
        }
    }

    public enum RagdollParts
    {
        Head,
        UpperLegLeft,
        UpperLegRight,
        LowerLegLeft,
        LowerLegRight,
        UpperArmLeft,
        UpperArmRight,
        LowerArmLeft,
        LowerArmRight,
        Torso
    }

    private const int RagdollPartCount = (int)RagdollParts.Torso + 1;

    public static void BuildRagdoll(JVector position, Action<RigidBody>? action = null)
    {
        World world = ((Playground)RenderWindow.Instance).World;

        RigidBody[] parts = new RigidBody[RagdollPartCount];

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i] = world.CreateRigidBody();
        }

        var head = parts[(int)RagdollParts.Head];
        var torso = parts[(int)RagdollParts.Torso];
        var upperLegLeft = parts[(int)RagdollParts.UpperLegLeft];
        var upperLegRight = parts[(int)RagdollParts.UpperLegRight];
        var lowerLegLeft = parts[(int)RagdollParts.LowerLegLeft];
        var lowerLegRight = parts[(int)RagdollParts.LowerLegRight];
        var upperArmLeft = parts[(int)RagdollParts.UpperArmLeft];
        var upperArmRight = parts[(int)RagdollParts.UpperArmRight];
        var lowerArmLeft = parts[(int)RagdollParts.LowerArmLeft];
        var lowerArmRight = parts[(int)RagdollParts.LowerArmRight];

        head.AddShape(new SphereShape((Real)0.15));
        upperLegLeft.AddShape(new CapsuleShape((Real)0.08, (Real)0.3));
        upperLegRight.AddShape(new CapsuleShape((Real)0.08, (Real)0.3));
        lowerLegLeft.AddShape(new CapsuleShape((Real)0.08, (Real)0.3));
        lowerLegRight.AddShape(new CapsuleShape((Real)0.08, (Real)0.3));
        upperArmLeft.AddShape(new CapsuleShape((Real)0.07, (Real)0.2));
        upperArmRight.AddShape(new CapsuleShape((Real)0.07, (Real)0.2));
        lowerArmLeft.AddShape(new CapsuleShape((Real)0.06, (Real)0.2));
        lowerArmRight.AddShape(new CapsuleShape((Real)0.06, (Real)0.2));
        torso.AddShape(new BoxShape((Real)0.35, (Real)0.6, (Real)0.2));

        head.Position = new JVector(0, 0, 0);
        torso.Position = new JVector(0, (Real)(-0.46), 0);
        upperLegLeft.Position = new JVector((Real)0.11, (Real)(-0.85), 0);
        upperLegRight.Position = new JVector((Real)(-0.11), (Real)(-0.85), 0);
        lowerLegLeft.Position = new JVector((Real)0.11, (Real)(-1.2), 0);
        lowerLegRight.Position = new JVector((Real)(-0.11), (Real)(-1.2), 0);

        upperArmLeft.Orientation = JQuaternion.CreateRotationZ(Real.Half_PI);
        upperArmRight.Orientation = JQuaternion.CreateRotationZ(Real.Half_PI);
        lowerArmLeft.Orientation = JQuaternion.CreateRotationZ(Real.Half_PI);
        lowerArmRight.Orientation = JQuaternion.CreateRotationZ(Real.Half_PI);

        upperArmLeft.Position = new JVector((Real)0.30, (Real)(-0.2), 0);
        upperArmRight.Position = new JVector((Real)(-0.30), (Real)(-0.2), 0);

        lowerArmLeft.Position = new JVector((Real)0.55, (Real)(-0.2), 0);
        lowerArmRight.Position = new JVector((Real)(-0.55), (Real)(-0.2), 0);

        var spine0 = world.CreateConstraint<BallSocket>(head, torso);
        spine0.Initialize(new JVector(0, (Real)(-0.15), 0));

        var spine1 = world.CreateConstraint<ConeLimit>(head, torso);
        spine1.Initialize(-JVector.UnitZ, AngularLimit.FromDegree(0, 45));

        var hipLeft0 = world.CreateConstraint<BallSocket>(torso, upperLegLeft);
        hipLeft0.Initialize(new JVector((Real)0.11, (Real)(-0.7), 0));

        var hipLeft1 = world.CreateConstraint<TwistAngle>(torso, upperLegLeft);
        hipLeft1.Initialize(JVector.UnitY, JVector.UnitY, AngularLimit.FromDegree(-80, +80));

        var hipLeft2 = world.CreateConstraint<ConeLimit>(torso, upperLegLeft);
        hipLeft2.Initialize(-JVector.UnitY, AngularLimit.FromDegree(0, 60));

        var hipRight0 = world.CreateConstraint<BallSocket>(torso, upperLegRight);
        hipRight0.Initialize(new JVector((Real)(-0.11), (Real)(-0.7), 0));

        var hipRight1 = world.CreateConstraint<TwistAngle>(torso, upperLegRight);
        hipRight1.Initialize(JVector.UnitY, JVector.UnitY, AngularLimit.FromDegree(-80, +80));

        var hipRight2 = world.CreateConstraint<ConeLimit>(torso, upperLegRight);
        hipRight2.Initialize(-JVector.UnitY, AngularLimit.FromDegree(0, 60));

        var kneeLeft = new HingeJoint(world, upperLegLeft, lowerLegLeft,
            new JVector((Real)0.11, (Real)(-1.05), 0), JVector.UnitX, AngularLimit.FromDegree(-120, 0));

        var kneeRight = new HingeJoint(world, upperLegRight, lowerLegRight,
            new JVector((Real)(-0.11), (Real)(-1.05), 0), JVector.UnitX, AngularLimit.FromDegree(-120, 0));

        var armLeft = new HingeJoint(world, lowerArmLeft, upperArmLeft,
            new JVector((Real)0.42, (Real)(-0.2), 0), JVector.UnitY, AngularLimit.FromDegree(-160, 0));

        var armRight = new HingeJoint(world, lowerArmRight, upperArmRight,
            new JVector((Real)(-0.42), (Real)(-0.2), 0), JVector.UnitY, AngularLimit.FromDegree(0, 160));

        // Soften the limits for the hinge joints
        kneeLeft.HingeAngle.LimitSoftness = kneeLeft.HingeAngle.Softness = 1;
        kneeRight.HingeAngle.LimitSoftness = kneeRight.HingeAngle.Softness = 1;
        armLeft.HingeAngle.LimitSoftness = armLeft.HingeAngle.Softness = 1;
        armRight.HingeAngle.LimitSoftness = armRight.HingeAngle.Softness = 1;

        var shoulderLeft0 = world.CreateConstraint<BallSocket>(upperArmLeft, torso);
        shoulderLeft0.Initialize(new JVector((Real)0.20, (Real)(-0.2), 0));

        var shoulderLeft1 = world.CreateConstraint<TwistAngle>(torso, upperArmLeft);
        shoulderLeft1.Initialize(JVector.UnitX, JVector.UnitX, AngularLimit.FromDegree(-20, 60));

        var shoulderRight0 = world.CreateConstraint<BallSocket>(upperArmRight, torso);
        shoulderRight0.Initialize(new JVector((Real)(-0.20), (Real)(-0.2), 0));

        var shoulderRight1 = world.CreateConstraint<TwistAngle>(torso, upperArmRight);
        shoulderRight1.Initialize(JVector.UnitX, JVector.UnitX, AngularLimit.FromDegree(-20, 60));

        shoulderLeft1.Bias = (Real)0.01;
        shoulderRight1.Bias = (Real)0.01;
        hipLeft1.Bias = (Real)0.01;
        hipRight1.Bias = (Real)0.01;

        if (world.BroadPhaseFilter is not IgnoreCollisionBetweenFilter filter)
        {
            filter = new IgnoreCollisionBetweenFilter();
            world.BroadPhaseFilter = filter;
        }

        filter.IgnoreCollisionBetween(upperLegLeft.Shapes[0], torso.Shapes[0]);
        filter.IgnoreCollisionBetween(upperLegRight.Shapes[0], torso.Shapes[0]);
        filter.IgnoreCollisionBetween(upperLegLeft.Shapes[0], lowerLegLeft.Shapes[0]);
        filter.IgnoreCollisionBetween(upperLegRight.Shapes[0], lowerLegRight.Shapes[0]);
        filter.IgnoreCollisionBetween(upperArmLeft.Shapes[0], torso.Shapes[0]);
        filter.IgnoreCollisionBetween(upperArmRight.Shapes[0], torso.Shapes[0]);
        filter.IgnoreCollisionBetween(upperArmLeft.Shapes[0], lowerArmLeft.Shapes[0]);
        filter.IgnoreCollisionBetween(upperArmRight.Shapes[0], lowerArmRight.Shapes[0]);

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].Position += position;
            action?.Invoke(parts[i]);
        }
    }

    public static void BuildPyramid(JVector position, int size = 20, Action<RigidBody>? action = null)
    {
        World world = ((Playground)RenderWindow.Instance).World;

        for (int i = 0; i < size; i++)
        {
            for (int e = i; e < size; e++)
            {
                RigidBody body = world.CreateRigidBody();
                body.Position = position + new JVector((Real)((e - i * 0.5) * 1.01), (Real)(0.5 + i * 1.0), 0);
                var shape = new BoxShape((Real)1);
                body.AddShape(shape);
                action?.Invoke(body);
            }
        }
    }

    public static void BuildTower(JVector pos, int height = 40, Action<RigidBody>? action = null)
    {
        World world = ((Playground)RenderWindow.Instance).World;

        JQuaternion halfRotationStep = JQuaternion.CreateRotationY(Real.Two_PI / 64);
        JQuaternion fullRotationStep = halfRotationStep * halfRotationStep;
        JQuaternion orientation = JQuaternion.Identity;

        for (int e = 0; e < height; e++)
        {
            orientation *= halfRotationStep;

            for (int i = 0; i < 32; i++)
            {
                JVector position = pos + JVector.Transform(
                    new JVector(0, (Real)(0.5 + e), (Real)19.5), orientation);

                var shape = new BoxShape((Real)3, (Real)1, (Real)0.2);
                var body = world.CreateRigidBody();

                body.Orientation = orientation;
                body.Position = new JVector(position.X, position.Y, position.Z);

                body.AddShape(shape);

                orientation *= fullRotationStep;

                action?.Invoke(body);
            }
        }
    }

    public static void BuildJenga(JVector position, int size = 20, Action<RigidBody>? action = null)
    {
        World world = ((Playground)RenderWindow.Instance).World;

        position += new JVector(0, (Real)0.5, 0);

        for (int i = 0; i < size; i++)
        {
            for (int e = 0; e < 3; e++)
            {
                var body = world.CreateRigidBody();

                if (i % 2 == 0)
                {
                    body.AddShape(new BoxShape((Real)3, (Real)1, (Real)1));
                    body.Position = position + new JVector(0, (Real)i, (Real)(-1 + e));
                }
                else
                {
                    body.AddShape(new BoxShape((Real)1, (Real)1, (Real)3));
                    body.Position = position + new JVector((Real)(-1 + e), (Real)i, 0);
                }

                action?.Invoke(body);
            }
        }
    }

    public static void BuildWall(JVector position, int sizex = 20, int sizey = 14, Action<RigidBody>? action = null)
    {
        World world = ((Playground)RenderWindow.Instance).World;

        for (int i = 0; i < sizex; i++)
        {
            for (int e = 0; e < sizey; e++)
            {
                RigidBody body = world.CreateRigidBody();
                body.Position = position + new JVector((Real)((i % 2 == 0 ? 0.5 : 0.0) + e * 2.01), (Real)(0.5 + i * 1.0), (Real)0.0);
                var shape = new BoxShape((Real)2, (Real)1, (Real)1);
                body.AddShape(shape);
                action?.Invoke(body);
            }
        }
    }

    public static void BuildPyramidCylinder(JVector position, int size = 20, Action<RigidBody>? action = null)
    {
        World world = ((Playground)RenderWindow.Instance).World;

        for (int i = 0; i < size; i++)
        {
            for (int e = i; e < size; e++)
            {
                RigidBody body = world.CreateRigidBody();
                body.Position = position + new JVector((Real)((e - i * 0.5) * 1.01), (Real)(0.5 + i * 1.0), (Real)0.0);
                var shape = new CylinderShape((Real)1, (Real)0.5);
                body.AddShape(shape);
                action?.Invoke(body);
            }
        }
    }
}
