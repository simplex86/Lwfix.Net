using System.Collections.Generic;
using System.Diagnostics;
using SimplexLab.Lwfix;
using SimplexLab.LwfixPhysics.Velcro.Primitives;
using SimplexLab.LwfixPhysics.Velcro.Shared;
using SimplexLab.LwfixPhysics.Velcro.Utilities;

namespace SimplexLab.LwfixPhysics.Velcro.Tools.Triangulation.Bayazit
{
    //From phed rev 36: http://code.google.com/p/phed/source/browse/trunk/Polygon.cpp

    /// <summary>
    /// Convex decomposition algorithm created by Mark Bayazit (http://mnbayazit.com/)
    /// 
    /// Properties:
    /// - Tries to decompose using polygons instead of triangles.
    /// - Tends to produce optimal results with low processing time.
    /// - Running time is O(nr), n = number of vertices, r = reflex vertices.
    /// - Does not support holes.
    /// 
    /// For more information about this algorithm, see http://mnbayazit.com/406/bayazit
    /// </summary>
    internal static class BayazitDecomposer
    {
        /// <summary>
        /// Decompose the polygon into several smaller non-concave polygon. If the polygon is already convex, it will
        /// return the original polygon, unless it is over Settings.MaxPolygonVertices.
        /// </summary>
        public static List<Vertices> ConvexPartition(Vertices vertices)
        {
            Debug.Assert(vertices.Count > 3);
            Debug.Assert(vertices.IsCounterClockWise());

            return TriangulatePolygon(vertices);
        }

        private static List<Vertices> TriangulatePolygon(Vertices vertices)
        {
            List<Vertices> list = new List<Vertices>();
            Vector2 lowerInt = new Vector2();
            Vector2 upperInt = new Vector2(); // intersection points
            int lowerIndex = 0, upperIndex = 0;
            Vertices lowerPoly, upperPoly;

            for (int i = 0; i < vertices.Count; ++i)
            {
                if (Reflex(i, vertices))
                {
                    Fixed32 upperDist;
                    Fixed32 lowerDist = upperDist = Fixed32.MaxValue;
                    for (int j = 0; j < vertices.Count; ++j)
                    {
                        // if line intersects with an edge
                        Fixed32 d;
                        Vector2 p;
                        if (Left(At(i - 1, vertices), At(i, vertices), At(j, vertices)) && RightOn(At(i - 1, vertices), At(i, vertices), At(j - 1, vertices)))
                        {
                            // find the point of intersection
                            p = LineUtils.LineIntersect(At(i - 1, vertices), At(i, vertices), At(j, vertices), At(j - 1, vertices));

                            if (Right(At(i + 1, vertices), At(i, vertices), p))
                            {
                                // make sure it's inside the poly
                                d = SquareDist(At(i, vertices), p);
                                if (d < lowerDist)
                                {
                                    // keep only the closest intersection
                                    lowerDist = d;
                                    lowerInt = p;
                                    lowerIndex = j;
                                }
                            }
                        }

                        if (Left(At(i + 1, vertices), At(i, vertices), At(j + 1, vertices)) && RightOn(At(i + 1, vertices), At(i, vertices), At(j, vertices)))
                        {
                            p = LineUtils.LineIntersect(At(i + 1, vertices), At(i, vertices), At(j, vertices), At(j + 1, vertices));

                            if (Left(At(i - 1, vertices), At(i, vertices), p))
                            {
                                d = SquareDist(At(i, vertices), p);
                                if (d < upperDist)
                                {
                                    upperDist = d;
                                    upperIndex = j;
                                    upperInt = p;
                                }
                            }
                        }
                    }

                    // if there are no vertices to connect to, choose a point in the middle
                    if (lowerIndex == (upperIndex + 1) % vertices.Count)
                    {
                        Vector2 p = (lowerInt + upperInt) / Fixed32.Two;

                        lowerPoly = Copy(i, upperIndex, vertices);
                        lowerPoly.Add(p);
                        upperPoly = Copy(lowerIndex, i, vertices);
                        upperPoly.Add(p);
                    }
                    else
                    {
                        Fixed32 highestScore = 0;
                        int bestIndex = lowerIndex;
                        while (upperIndex < lowerIndex)
                        {
                            upperIndex += vertices.Count;
                        }

                        for (int j = lowerIndex; j <= upperIndex; ++j)
                        {
                            if (CanSee(i, j, vertices))
                            {
                                Fixed32 score = Fixed32.One / (SquareDist(At(i, vertices), At(j, vertices)) + Fixed32.One);
                                if (Reflex(j, vertices))
                                {
                                    if (RightOn(At(j - 1, vertices), At(j, vertices), At(i, vertices)) && LeftOn(At(j + 1, vertices), At(j, vertices), At(i, vertices)))
                                        score += 3;
                                    else
                                        score += 2;
                                }
                                else
                                    score += 1;
                                if (score > highestScore)
                                {
                                    bestIndex = j;
                                    highestScore = score;
                                }
                            }
                        }
                        lowerPoly = Copy(i, bestIndex, vertices);
                        upperPoly = Copy(bestIndex, i, vertices);
                    }
                    list.AddRange(TriangulatePolygon(lowerPoly));
                    list.AddRange(TriangulatePolygon(upperPoly));
                    return list;
                }
            }

            // polygon is already convex
            if (vertices.Count > Settings.MaxPolygonVertices)
            {
                lowerPoly = Copy(0, vertices.Count / 2, vertices);
                upperPoly = Copy(vertices.Count / 2, 0, vertices);
                list.AddRange(TriangulatePolygon(lowerPoly));
                list.AddRange(TriangulatePolygon(upperPoly));
            }
            else
                list.Add(vertices);

            return list;
        }

        private static Vector2 At(int i, Vertices vertices)
        {
            int s = vertices.Count;
            return vertices[i < 0 ? s - 1 - ((-i - 1) % s) : i % s];
        }

        private static Vertices Copy(int i, int j, Vertices vertices)
        {
            while (j < i)
            {
                j += vertices.Count;
            }

            Vertices p = new Vertices(j);

            for (; i <= j; ++i)
            {
                p.Add(At(i, vertices));
            }
            return p;
        }

        private static bool CanSee(int i, int j, Vertices vertices)
        {
            if (Reflex(i, vertices))
            {
                if (LeftOn(At(i, vertices), At(i - 1, vertices), At(j, vertices)) && RightOn(At(i, vertices), At(i + 1, vertices), At(j, vertices)))
                    return false;
            }
            else
            {
                if (RightOn(At(i, vertices), At(i + 1, vertices), At(j, vertices)) || LeftOn(At(i, vertices), At(i - 1, vertices), At(j, vertices)))
                    return false;
            }
            if (Reflex(j, vertices))
            {
                if (LeftOn(At(j, vertices), At(j - 1, vertices), At(i, vertices)) && RightOn(At(j, vertices), At(j + 1, vertices), At(i, vertices)))
                    return false;
            }
            else
            {
                if (RightOn(At(j, vertices), At(j + 1, vertices), At(i, vertices)) || LeftOn(At(j, vertices), At(j - 1, vertices), At(i, vertices)))
                    return false;
            }
            for (int k = 0; k < vertices.Count; ++k)
            {
                if ((k + 1) % vertices.Count == i || k == i || (k + 1) % vertices.Count == j || k == j)
                    continue; // ignore incident edges

                if (LineUtils.LineIntersect(At(i, vertices), At(j, vertices), At(k, vertices), At(k + 1, vertices), out _))
                    return false;
            }
            return true;
        }

        private static bool Reflex(int i, Vertices vertices)
        {
            return Right(i, vertices);
        }

        private static bool Right(int i, Vertices vertices)
        {
            return Right(At(i - 1, vertices), At(i, vertices), At(i + 1, vertices));
        }

        private static bool Left(Vector2 a, Vector2 b, Vector2 c)
        {
            return MathUtils.Area(ref a, ref b, ref c) > 0;
        }

        private static bool LeftOn(Vector2 a, Vector2 b, Vector2 c)
        {
            return MathUtils.Area(ref a, ref b, ref c) >= 0;
        }

        private static bool Right(Vector2 a, Vector2 b, Vector2 c)
        {
            return MathUtils.Area(ref a, ref b, ref c) < 0;
        }

        private static bool RightOn(Vector2 a, Vector2 b, Vector2 c)
        {
            return MathUtils.Area(ref a, ref b, ref c) <= 0;
        }

        private static Fixed32 SquareDist(Vector2 a, Vector2 b)
        {
            Fixed32 dx = b.X - a.X;
            Fixed32 dy = b.Y - a.Y;
            return dx * dx + dy * dy;
        }
    }
}
