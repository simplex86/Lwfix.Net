using System;
using System.Collections.Generic;
using System.Linq;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Collision.RayCast;
using SimplexLab.LwfixPhysics.Velcro.Collision.Shapes;
using SimplexLab.LwfixPhysics.Velcro.Dynamics;
using SimplexLab.LwfixPhysics.Velcro.Extensions.PhysicsLogics.PhysicsLogicBase;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;
using SimplexLab.LwfixPhysics.Velcro.Primitives;

namespace SimplexLab.LwfixPhysics.Velcro.Extensions.PhysicsLogics.Explosion
{
    // Original Code by Steven Lu - see http://www.box2d.org/forum/viewtopic.php?f=3&t=1688
    // Ported by Nicol�s Hormaz�bal

    /* Methodology:
     * Force applied at a ray is inversely proportional to the square of distance from source
     * AABB is used to query for shapes that may be affected
     * For each RIGID BODY (not shape -- this is an optimization) that is matched, loop through its vertices to determine
     *		the extreme points -- if there is structure that contains outlining polygon, use that as an additional optimization
     * Evenly cast a number of rays against the shape - number roughly proportional to the arc coverage
     *		- Something like every 3 degrees should do the trick although this can be altered depending on the distance (if really close don't need such a high density of rays)
     *		- There should be a minimum number of rays (3-5?) applied to each body so that small bodies far away are still accurately modeled
     *		- Be sure to have the forces of each ray be proportional to the average arc length covered by each.
     * For each ray that actually intersects with the shape (non intersections indicate something blocking the path of explosion):
     *		- Apply the appropriate force dotted with the negative of the collision normal at the collision point
     *		- Optionally apply linear interpolation between aforementioned Normal force and the original explosion force in the direction of ray to simulate "surface friction" of sorts
     */

    /// <summary>
    /// Creates a realistic explosion based on raycasting. Objects in the open will be affected, but objects behind
    /// static bodies will not. A body that is half in cover, half in the open will get half the force applied to the end in
    /// the open.
    /// </summary>
    public sealed class RealExplosion : PhysicsLogic
    {
        /// <summary>Two degrees: maximum angle from edges to first ray tested</summary>
        private static readonly Fixed32 MaxEdgeOffset = MathConstants.Pi / 90;

        private List<ShapeData> _data = new List<ShapeData>();
        private RayDataComparer _rdc;

        /// <summary>Ratio of arc length to angle from edges to first ray tested. Defaults to 1/40.</summary>
        public Fixed32 EdgeRatio = (Fixed32)(1.0 / 40.0);

        /// <summary>Ignore Explosion if it happens inside a shape. Default value is false.</summary>
        public bool IgnoreWhenInsideShape = false;

        /// <summary>Max angle between rays (used when segment is large). Defaults to 15 degrees</summary>
        public Fixed32 MaxAngle = MathConstants.Pi / 15;

        /// <summary>Maximum number of shapes involved in the explosion. Defaults to 100</summary>
        public int MaxShapes = 100;

        /// <summary>How many rays per shape/body/segment. Defaults to 5</summary>
        public int MinRays = 5;

        public RealExplosion(World world)
            : base(world, PhysicsLogicType.Explosion)
        {
            _rdc = new RayDataComparer();
            _data = new List<ShapeData>();
        }

        /// <summary>Activate the explosion at the specified position.</summary>
        /// <param name="pos">The position where the explosion happens </param>
        /// <param name="radius">The explosion radius </param>
        /// <param name="maxForce">
        /// The explosion force at the explosion point (then is inversely proportional to the square of the
        /// distance)
        /// </param>
        /// <returns>A list of bodies and the amount of force that was applied to them.</returns>
        public Dictionary<Fixture, Vector2> Activate(Vector2 pos, Fixed32 radius, Fixed32 maxForce)
        {
            AABB aabb;
            aabb.LowerBound = pos + new Vector2(-radius, -radius);
            aabb.UpperBound = pos + new Vector2(radius, radius);
            Fixture[] shapes = new Fixture[MaxShapes];

            // More than 5 shapes in an explosion could be possible, but still strange.
            Fixture[] containedShapes = new Fixture[5];
            bool exit = false;

            int shapeCount = 0;
            int containedShapeCount = 0;

            // Query the world for overlapping shapes.
            World.QueryAABB(
                fixture =>
                {
                    if (fixture.TestPoint(ref pos))
                    {
                        if (IgnoreWhenInsideShape)
                        {
                            exit = true;
                            return false;
                        }

                        containedShapes[containedShapeCount++] = fixture;
                    }
                    else
                        shapes[shapeCount++] = fixture;

                    // Continue the query.
                    return true;
                }, ref aabb);

            if (exit)
                return new Dictionary<Fixture, Vector2>();

            Dictionary<Fixture, Vector2> exploded = new Dictionary<Fixture, Vector2>(shapeCount + containedShapeCount);

            // Per shape max/min angles for now.
            Fixed32[] vals = new Fixed32[shapeCount * 2];
            int valIndex = 0;
            for (int i = 0; i < shapeCount; ++i)
            {
                PolygonShape ps;
                if (shapes[i].Shape is CircleShape cs)
                {
                    // We create a "diamond" approximation of the circle
                    Vertices v = new Vertices();
                    Vector2 vec = Vector2.Zero + new Vector2(cs._radius, 0);
                    v.Add(vec);
                    vec = Vector2.Zero + new Vector2(0, cs._radius);
                    v.Add(vec);
                    vec = Vector2.Zero + new Vector2(-cs._radius, cs._radius);
                    v.Add(vec);
                    vec = Vector2.Zero + new Vector2(0, -cs._radius);
                    v.Add(vec);
                    ps = new PolygonShape(v, 0);
                }
                else
                    ps = shapes[i].Shape as PolygonShape;

                if (shapes[i].Body.BodyType == BodyType.Dynamic && ps != null)
                {
                    Vector2 toCentroid = shapes[i].Body.GetWorldPoint(ps._massData._centroid) - pos;
                    Fixed32 angleToCentroid = Fixed32.Atan2(toCentroid.Y, toCentroid.X);
                    Fixed32 min = Fixed32.MaxValue;
                    Fixed32 max = Fixed32.MinValue;
                    Fixed32 minAbsolute = Fixed32.Zero;
                    Fixed32 maxAbsolute = Fixed32.Zero;

                    for (int j = 0; j < ps._vertices.Count; ++j)
                    {
                        Vector2 toVertex = shapes[i].Body.GetWorldPoint(ps._vertices[j]) - pos;
                        Fixed32 newAngle = Fixed32.Atan2(toVertex.Y, toVertex.X);
                        Fixed32 diff = newAngle - angleToCentroid;

                        diff = (diff - MathConstants.Pi) % (2 * MathConstants.Pi);

                        // the minus pi is important. It means cutoff for going other direction is at 180 deg where it needs to be

                        if (diff < Fixed32.Zero)
                            diff += 2 * MathConstants.Pi; // correction for not handling negs

                        diff -= MathConstants.Pi;

                        if (FMath.Abs(diff) > MathConstants.Pi)
                            continue; // Something's wrong, point not in shape but exists angle diff > 180

                        if (diff > max)
                        {
                            max = diff;
                            maxAbsolute = newAngle;
                        }
                        if (diff < min)
                        {
                            min = diff;
                            minAbsolute = newAngle;
                        }
                    }

                    vals[valIndex] = minAbsolute;
                    ++valIndex;
                    vals[valIndex] = maxAbsolute;
                    ++valIndex;
                }
            }

            Array.Sort(vals, 0, valIndex, _rdc);
            _data.Clear();
            bool rayMissed = true;

            for (int i = 0; i < valIndex; ++i)
            {
                Fixture fixture = null;
                Fixed32 midpt;

                int iplus = i == valIndex - 1 ? 0 : i + 1;
                if (vals[i] == vals[iplus])
                    continue;

                if (i == valIndex - 1)
                {
                    // the single edgecase
                    midpt = vals[0] + MathConstants.Pi * 2 + vals[i];
                }
                else
                    midpt = vals[i + 1] + vals[i];

                midpt /= 2;

                Vector2 p1 = pos;
                Vector2 p2 = radius * new Vector2(Fixed32.Cos(midpt), Fixed32.Sin(midpt)) + pos;

                // RaycastOne
                bool hitClosest = false;
                World.RayCast((f, p, n, fr) =>
                {
                    Body body = f.Body;

                    if (!IsActiveOn(body))
                        return 0;

                    hitClosest = true;
                    fixture = f;
                    return fr;
                }, p1, p2);

                //draws radius points
                if (hitClosest && fixture.Body.BodyType == BodyType.Dynamic)
                {
                    if (_data.Any() && _data.Last().Body == fixture.Body && !rayMissed)
                    {
                        int laPos = _data.Count - 1;
                        ShapeData la = _data[laPos];
                        la.Max = vals[iplus];
                        _data[laPos] = la;
                    }
                    else
                    {
                        // make new
                        ShapeData d;
                        d.Body = fixture.Body;
                        d.Min = vals[i];
                        d.Max = vals[iplus];
                        _data.Add(d);
                    }

                    if (_data.Count > 1
                        && i == valIndex - 1
                        && _data.Last().Body == _data.First().Body
                        && _data.Last().Max == _data.First().Min)
                    {
                        ShapeData fi = _data[0];
                        fi.Min = _data.Last().Min;
                        _data.RemoveAt(_data.Count - 1);
                        _data[0] = fi;
                        while (_data.First().Min >= _data.First().Max)
                        {
                            fi.Min -= MathConstants.Pi * 2;
                            _data[0] = fi;
                        }
                    }

                    int lastPos = _data.Count - 1;
                    ShapeData last = _data[lastPos];
                    while (_data.Count > 0
                           && _data.Last().Min >= _data.Last().Max) // just making sure min<max
                    {
                        last.Min = _data.Last().Min - 2 * MathConstants.Pi;
                        _data[lastPos] = last;
                    }
                    rayMissed = false;
                }
                else
                    rayMissed = true; // raycast did not find a shape
            }

            for (int i = 0; i < _data.Count; ++i)
            {
                if (!IsActiveOn(_data[i].Body))
                    continue;

                Fixed32 arclen = _data[i].Max - _data[i].Min;

                Fixed32 first = MathHelper.Min(MaxEdgeOffset, EdgeRatio * arclen);
                int insertedRays = (int)FMath.Ceil(((arclen - Fixed32.Two * first) - (MinRays - 1) * MaxAngle) / MaxAngle);

                if (insertedRays < 0)
                    insertedRays = 0;

                Fixed32 offset = (arclen - first * Fixed32.Two) / ((Fixed32)MinRays + insertedRays - 1);

                //Note: This loop can go into infinite as it operates on floats.
                //Added FloatEquals with a large epsilon.
                for (Fixed32 j = _data[i].Min + first;
                     j < _data[i].Max || MathUtils.FloatEquals(j, _data[i].Max, (Fixed32)0.0001);
                     j += offset)
                {
                    Vector2 p1 = pos;
                    Vector2 p2 = pos + radius * new Vector2(Fixed32.Cos(j), Fixed32.Sin(j));
                    Vector2 hitpoint = Vector2.Zero;
                    Fixed32 minlambda = Fixed32.MaxValue;

                    List<Fixture> fl = _data[i].Body.FixtureList;
                    for (int x = 0; x < fl.Count; x++)
                    {
                        Fixture f = fl[x];
                        RayCastInput ri;
                        ri.Point1 = p1;
                        ri.Point2 = p2;
                        ri.MaxFraction = (Fixed32)50.0;

                        if (f.RayCast(out RayCastOutput ro, ref ri, 0))
                        {
                            if (minlambda > ro.Fraction)
                            {
                                minlambda = ro.Fraction;
                                hitpoint = ro.Fraction * p2 + (1 - ro.Fraction) * p1;
                            }
                        }

                        // the force that is to be applied for this particular ray.
                        // offset is angular coverage. lambda*length of segment is distance.
                        Fixed32 impulse = arclen / (MinRays + insertedRays) * maxForce * (Fixed32)180.0 / MathConstants.Pi * (Fixed32.One - Fixed32.Min(Fixed32.One, minlambda));

                        // We Apply the impulse!!!
                        Vector2 vectImp = Vector2.Dot(impulse * new Vector2(Fixed32.Cos(j), Fixed32.Sin(j)), -ro.Normal) * new Vector2(Fixed32.Cos(j), Fixed32.Sin(j));
                        _data[i].Body.ApplyLinearImpulse(ref vectImp, ref hitpoint);

                        // We gather the fixtures for returning them
                        if (exploded.ContainsKey(f))
                            exploded[f] += vectImp;
                        else
                            exploded.Add(f, vectImp);

                        if (minlambda > Fixed32.One)
                            hitpoint = p2;
                    }
                }
            }

            // We check contained shapes
            for (int i = 0; i < containedShapeCount; ++i)
            {
                Fixture fix = containedShapes[i];

                if (!IsActiveOn(fix.Body))
                    continue;

                Fixed32 impulse = MinRays * maxForce * (Fixed32)180.0 / MathConstants.Pi;
                Vector2 hitPoint;

                if (fix.Shape is CircleShape circShape)
                    hitPoint = fix.Body.GetWorldPoint(circShape.Position);
                else
                {
                    PolygonShape shape = fix.Shape as PolygonShape;
                    hitPoint = fix.Body.GetWorldPoint(shape._massData._centroid);
                }

                Vector2 vectImp = impulse * (hitPoint - pos);

                fix.Body.ApplyLinearImpulse(ref vectImp, ref hitPoint);

                if (!exploded.ContainsKey(fix))
                    exploded.Add(fix, vectImp);
            }

            return exploded;
        }
    }
}
