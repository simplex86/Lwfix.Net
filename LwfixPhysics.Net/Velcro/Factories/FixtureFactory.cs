using System;
using System.Collections.Generic;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;
using SimplexLab.LwfixPhysics.Velcro.Definitions;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Tools.Triangulation.TriangulationBase;
using SimplexLab.LwfixPhysics.Velcro.Utilities;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Factories
{
    /// <summary>An easy to use factory for creating bodies</summary>
    public static class FixtureFactory
    {
        public static Fixture AttachEdge(Vector2 start, Vector2 end, Body body, object? userData = null)
        {
            EdgeShape edgeShape = new EdgeShape(start, end);
            Fixture f = body.AddFixture(edgeShape);
            f.UserData = userData;
            return f;
        }

        public static Fixture AttachChainShape(Vertices vertices, Body body, object? userData = null)
        {
            ChainShape shape = new ChainShape(vertices);
            Fixture f = body.AddFixture(shape);
            f.UserData = userData;
            return f;
        }

        public static Fixture AttachLoopShape(Vertices vertices, Body body, object? userData = null)
        {
            ChainShape shape = new ChainShape(vertices, true);
            Fixture f = body.AddFixture(shape);
            f.UserData = userData;
            return f;
        }

        public static Fixture AttachRectangle(Fixed32 width, Fixed32 height, Fixed32 density, Vector2 offset, Body body, object? userData = null)
        {
            Vertices rectangleVertices = PolygonUtils.CreateRectangle(width / 2, height / 2);
            rectangleVertices.Translate(ref offset);
            PolygonShape rectangleShape = new PolygonShape(rectangleVertices, density);
            Fixture f = body.AddFixture(rectangleShape);
            f.UserData = userData;
            return f;
        }

        public static Fixture AttachCircle(Fixed32 radius, Fixed32 density, Body body, object? userData = null)
        {
            if (radius <= 0)
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be more than 0 meters");

            CircleShape circleShape = new CircleShape(radius, density);
            Fixture f = body.AddFixture(circleShape);
            f.UserData = userData;
            return f;
        }

        public static Fixture AttachCircle(Fixed32 radius, Fixed32 density, Body body, Vector2 offset, object? userData = null)
        {
            if (radius <= 0)
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be more than 0 meters");

            CircleShape circleShape = new CircleShape(radius, density);
            circleShape.Position = offset;
            Fixture f = body.AddFixture(circleShape);
            f.UserData = userData;
            return f;
        }

        public static Fixture AttachPolygon(Vertices vertices, Fixed32 density, Body body, object? userData = null)
        {
            if (vertices.Count <= 1)
                throw new ArgumentOutOfRangeException(nameof(vertices), "Too few points to be a polygon");

            PolygonShape polygon = new PolygonShape(vertices, density);
            Fixture f = body.AddFixture(polygon);
            f.UserData = userData;
            return f;
        }

        public static Fixture AttachEllipse(Fixed32 xRadius, Fixed32 yRadius, int edges, Fixed32 density, Body body, object? userData = null)
        {
            if (xRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(xRadius), "X-radius must be more than 0");

            if (yRadius <= 0)
                throw new ArgumentOutOfRangeException(nameof(yRadius), "Y-radius must be more than 0");

            Vertices ellipseVertices = PolygonUtils.CreateEllipse(xRadius, yRadius, edges);
            PolygonShape polygonShape = new PolygonShape(ellipseVertices, density);
            Fixture f = body.AddFixture(polygonShape);
            f.UserData = userData;
            return f;
        }

        public static List<Fixture> AttachCompoundPolygon(List<Vertices> list, Fixed32 density, Body body)
        {
            List<Fixture> res = new List<Fixture>(list.Count);

            //Then we create several fixtures using the body
            foreach (Vertices vertices in list)
            {
                if (vertices.Count == 2)
                {
                    EdgeShape shape = new EdgeShape(vertices[0], vertices[1]);
                    res.Add(body.AddFixture(shape));
                }
                else
                {
                    PolygonShape shape = new PolygonShape(vertices, density);
                    res.Add(body.AddFixture(shape));
                }
            }

            return res;
        }

        public static Fixture AttachLineArc(Fixed32 radians, int sides, Fixed32 radius, bool closed, Body body)
        {
            Vertices arc = PolygonUtils.CreateArc(radians, sides, radius);
            arc.Rotate((MathConstants.Pi - radians) / 2);
            return closed ? AttachLoopShape(arc, body) : AttachChainShape(arc, body);
        }

        public static List<Fixture> AttachSolidArc(Fixed32 density, Fixed32 radians, int sides, Fixed32 radius, Body body)
        {
            Vertices arc = PolygonUtils.CreateArc(radians, sides, radius);
            arc.Rotate((MathConstants.Pi - radians) / 2);

            //Close the arc
            arc.Add(arc[0]);

            List<Vertices> triangles = Triangulate.ConvexPartition(arc, TriangulationAlgorithm.Earclip);

            return AttachCompoundPolygon(triangles, density, body);
        }

        public static Fixture CreateFromDef(Body body, FixtureDef fixtureDef)
        {
            return body.AddFixture(fixtureDef);
        }
    }
}
