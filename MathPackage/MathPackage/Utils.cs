using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MathPackage
{
    public enum Unit { MM, CM, M, INCH, FT, DEG, RAD }
    public enum FaceSide { FRONT, BACK, BOTH }

    public class UnorderedTupleComparer<T> : IEqualityComparer<Tuple<T, T>>
    {
        private IEqualityComparer<T> comparer;
        public UnorderedTupleComparer(IEqualityComparer<T> comparer = null)
        {
            this.comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public bool Equals(Tuple<T, T> x, Tuple<T, T> y)
        {
            return comparer.Equals(x.Item1, y.Item1) && comparer.Equals(x.Item2, y.Item2) ||
                    comparer.Equals(x.Item1, y.Item2) && comparer.Equals(x.Item1, y.Item2);
        }

        public int GetHashCode(Tuple<T, T> obj)
        {
            return comparer.GetHashCode(obj.Item1) ^ comparer.GetHashCode(obj.Item2);
        }
    }

    public class Debug
    {
        public static List<string> logStrings = new List<string>();

        public static void Log(string log)
        {
            logStrings.Add(log);
        }

        public static string GetLog()
        {
            return string.Join("\n", logStrings);
        }

        public static string GetLogCondenced()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (string s in logStrings)
            {
                dict[s] = dict.ContainsKey(s) ? dict[s] + 1 : 1;
            }
            return string.Join("\n", dict.Select(kvp => kvp.Value.ToString() + ": \t" + kvp.Key));
        }

        public static void ResetLog()
        {
            logStrings = new List<string>();
        }
    }

    public class Utils
    {
        public static double INCH_TO_METER = 0.0254f;
        public static double FT_TO_METER = 0.3048f;
        public static double CM_TO_METER = 0.01f;
        public static double MM_TO_METER = 0.001f;

        public static void GetXYZDimentions(List<Vector3D> localVectors, out Vector3D center, out Vector3D dimentions)
        {
            double minX = localVectors.Min(v => v.x);
            double minY = localVectors.Min(v => v.y);
            double minZ = localVectors.Min(v => v.z);
            double maxX = localVectors.Max(v => v.x);
            double maxY = localVectors.Max(v => v.y);
            double maxZ = localVectors.Max(v => v.z);
            center = new Vector3D((maxX + minX) / 2.0, (maxY + minY) / 2.0, (maxZ + minZ) / 2.0);
            dimentions = new Vector3D(maxX - minX, maxY - minY, maxZ - minZ);
        }

        public static double ChangeUnitToMeterOrDeg(double value, Unit unit)
        {
            switch (unit)
            {
                case (Unit.MM):
                    return value * MM_TO_METER;
                case (Unit.CM):
                    return value * MM_TO_METER;
                case (Unit.M):
                    return value * 1.0;
                case (Unit.INCH):
                    return value * INCH_TO_METER;
                case (Unit.FT):
                    return value * FT_TO_METER;
                case (Unit.DEG):
                    return value * Math.PI / 180;
                case (Unit.RAD):
                    return value;
            }
            return value;
        }

        public static List<Vector3D> ShrinkShape(List<Vector3D> vects, double ratio)
        {
            List<Vector3D> returnList = new List<Vector3D>();
            if (vects.Count == 0)
            {
                return returnList;
            }

            double minVx = vects.Min(v => v.x);
            double maxVx = vects.Max(v => v.x);
            double minVy = vects.Min(v => v.y);
            double maxVy = vects.Max(v => v.y);
            double minVz = vects.Min(v => v.z);
            double maxVz = vects.Max(v => v.z);
            Vector4D centerV = new Vector4D((maxVx + minVx) / 2, (maxVy + minVy) / 2, (maxVz + minVz) / 2, 1);
            foreach (Vector3D v in vects)
            {
                Vector3D diff = Vector3D.Subract(v, centerV);
                diff.Scale(ratio);
                returnList.Add(Vector3D.Add(diff, centerV));
            }
            return returnList;
        }

        public static Mesh ShrinkMesh(Mesh m, double ratio)
        {
            List<Vector3D> newVects = ShrinkShape(m.VertexList, ratio);
            return new Mesh(newVects, m.TriangleList);
        }

        public static List<int> CreateTrianglesForExtrudedSolid(List<Vector3D> polyLinePoints)
        {
            List<int> triangleNums = new List<int>();
            if (polyLinePoints.Count == 0)
            {
                return triangleNums;
            }

            int mainPointCount = polyLinePoints.Count;
            int mainPointCountDouble = mainPointCount * 2;
            int mainPointCountTriple = mainPointCount * 3;

            // Base triangles:
            triangleNums.AddRange(Utils.EarClippingVariant(polyLinePoints, FaceSide.BACK));        // TODO: CHECK IF BACK IS CORRECT

            // Side triangles:
            for (int i = 0; i < mainPointCountDouble; i++)
            {
                int index1 = (i + 1) % mainPointCountDouble;
                int index2 = (i + 2) % mainPointCountDouble;

                if (i % 2 == 0)
                {
                    triangleNums.Add(i + mainPointCount);
                    triangleNums.Add(index1 + mainPointCount);
                    triangleNums.Add(index2 + mainPointCount);
                }
                else
                {
                    triangleNums.Add(i + mainPointCount);
                    triangleNums.Add(index2 + mainPointCount);
                    triangleNums.Add(index1 + mainPointCount);
                }
            }
            //Top trianlges:
            List<int> topTriangles = Utils.EarClippingVariant(polyLinePoints, FaceSide.FRONT);       // TODO: CHECK IF FRONT IS CORRECT
            foreach (int tri in topTriangles)
            {
                triangleNums.Add(tri + mainPointCountTriple);
            }

            return triangleNums;
        }

        public static List<Vector3D> TranslateVerticies(Matrix4 translationMatrix, List<Vector3D> verticies)
        {
            List<Vector3D> newVertices = new List<Vector3D>();
            foreach (Vector3D vect in verticies)
            {
                Vector4D vect4 = Matrix4.Multiply(translationMatrix, vect.Get4D(1));
                newVertices.Add(vect4.Get3D());
            }

            return newVertices;
        }

        public static Matrix4 GetTranslationMatrixFromLocationOrientation(Vector3D location, Vector4D orientation)
        {
            Vector4D o = orientation;
            Vector3D c1 = new Vector3D(1 - 2 * Math.Pow(o.y, 2) - 2 * Math.Pow(o.z, 2), 2 * (o.x * o.y + o.z * o.w), 2 * (o.x * o.z - o.y * o.w));
            Vector3D c2 = new Vector3D(2 * (o.x * o.y - o.z * o.w), 1 - 2 * Math.Pow(o.x, 2) - 2 * Math.Pow(o.z, 2), 2 * (o.x * o.w + o.y * o.z));
            Vector3D c3 = new Vector3D(2 * (o.x * o.z + o.y * o.w), 2 * (o.y * o.z - o.x * o.w), 1 - 2 * Math.Pow(o.x, 2) - 2 * Math.Pow(o.y, 2));

            // https://en.wikipedia.org/wiki/Quaternions_and_spatial_rotation (note that this matrix is custructed by columns...)
            return new Matrix4(c1.Get4D(location.x), c2.Get4D(location.y), c3.Get4D(location.z), new Vector4D(0, 0, 0, 1)).GetTranspose();
        }

        public static Vector4D GetQuaterion(Vector3D axis, double rotationRad)
        {
            // https://en.wikipedia.org/wiki/Quaternions_and_spatial_rotation#Using_quaternion_as_rotations
            Vector3D axisNorm = axis.Norm();
            double C = Math.Cos(rotationRad / 2.0);
            double S = Math.Sin(rotationRad / 2.0);
            return new Vector4D(axisNorm.x * S, axisNorm.y * S, axisNorm.z * S, C);
        }

        public static List<int> EarClippingVariant(List<Vector3D> points, FaceSide faceSide)
        {
            List<int> triangleList = new List<int>();

            List<Tuple<int, Vector3D>> remainingPoints = new List<Tuple<int, Vector3D>>();
            for (int i = 0; i < points.Count; i++)
            {
                Vector3D p = points[i];
                remainingPoints.Add(new Tuple<int, Vector3D>(i, p));
            }

            // Remove points that are in a straight line
            int pCount = remainingPoints.Count;
            for (int i = 0; i < pCount && pCount > 3; i++)
            {
                int index1 = (i + 0) % pCount;
                int index2 = (i + 1) % pCount;
                int index3 = (i + 2) % pCount;
                Vector3D p1 = remainingPoints[index1].Item2;
                Vector3D p2 = remainingPoints[index2].Item2;
                Vector3D p3 = remainingPoints[index3].Item2;

                Vector3D v1 = Vector3D.Subract(p1, p2);
                Vector3D v2 = Vector3D.Subract(p3, p2);
                Vector3D pointsFacingDir = Vector3D.Cross(v1, v2);
                if (pointsFacingDir.Length() < 0.01 * v1.Length() * v2.Length())
                {
                    double l1 = Vector3D.Distance(p1, p2);
                    double l2 = Vector3D.Distance(p1, p3);
                    double l3 = Vector3D.Distance(p2, p3);
                    if (Math.Abs(l1 - (l2 + l3)) < 0.001)
                    {
                        remainingPoints.RemoveAt(index3);
                        pCount = remainingPoints.Count;
                        i = -1;
                        continue;
                    }
                    if (Math.Abs(l2 - (l1 + l3)) < 0.001)
                    {
                        remainingPoints.RemoveAt(index2);
                        pCount = remainingPoints.Count;
                        i = -1;
                        continue;
                    }
                    if (Math.Abs(l3 - (l2 + l1)) < 0.001)
                    {
                        remainingPoints.RemoveAt(index1);
                        pCount = remainingPoints.Count;
                        i = -1;
                        continue;
                    }
                }
            }

            Vector3D facingDir = null;
            while (remainingPoints.Count > 3)
            {
                pCount = remainingPoints.Count;
                bool removedEar = false;
                for (int i = 0; i < pCount; i++)
                {
                    int index1 = (i + 0) % pCount;
                    int index2 = (i + 1) % pCount;
                    int index3 = (i + 2) % pCount;
                    Vector3D p1 = remainingPoints[index1].Item2;
                    Vector3D p2 = remainingPoints[index2].Item2;
                    Vector3D p3 = remainingPoints[index3].Item2;

                    // Check that the direction is parallel to the first triangle direction (the first three points will determine the direction)
                    Vector3D v1 = Vector3D.Subract(p1, p2);
                    Vector3D v2 = Vector3D.Subract(p3, p2);
                    Vector3D pointsFacingDir = Vector3D.Cross(v1, v2);
                    if (facingDir == null)
                    {
                        facingDir = pointsFacingDir.Copy();
                        if (facingDir.x != 0 || facingDir.y != 0)
                        {
                            int asda = 0;
                        }
                        else
                        {
                            if (facingDir.z > 0)
                            {
                                facingDir = facingDir.Neg();
                            }
                        }
                    }
                    double angle = Vector3D.AngleRad(facingDir, pointsFacingDir);
                    if (angle == double.NaN)
                    {
                        throw new Exception();
                    }
                    if (angle > Math.PI / 2.0)
                    {
                        continue;
                    }

                    bool inTriangle = false;
                    for (int j = 0; j < remainingPoints.Count; j++)
                    {
                        if ((j == index1) || (j == index2) || (j == index3))
                        {
                            continue;
                        }

                        Vector3D p4 = remainingPoints[j].Item2;
                        if (PointInTriangle(p4, p1, p2, p3))
                        {
                            inTriangle = true;
                            break;
                        }
                    }
                    if (!inTriangle)
                    {
                        if (faceSide == FaceSide.FRONT)
                        {
                            triangleList.Add(remainingPoints[index1].Item1);
                            triangleList.Add(remainingPoints[index3].Item1);
                            triangleList.Add(remainingPoints[index2].Item1);
                        }
                        if (faceSide == FaceSide.BACK)
                        {
                            triangleList.Add(remainingPoints[index1].Item1);
                            triangleList.Add(remainingPoints[index2].Item1);
                            triangleList.Add(remainingPoints[index3].Item1);
                        }
                        remainingPoints.RemoveAt(index2);
                        removedEar = true;
                        break;
                    }
                }

                if (!removedEar)
                {
                    break;
                }
            }

            if (faceSide == FaceSide.FRONT || faceSide == FaceSide.BOTH)
            {
                triangleList.Add(remainingPoints[0].Item1);
                triangleList.Add(remainingPoints[2].Item1);
                triangleList.Add(remainingPoints[1].Item1);
            }
            if (faceSide == FaceSide.BACK || faceSide == FaceSide.BOTH)
            {
                triangleList.Add(remainingPoints[0].Item1);
                triangleList.Add(remainingPoints[1].Item1);
                triangleList.Add(remainingPoints[2].Item1);
            }

            return triangleList;
        }
        // None of these work but are necessary for EarClippingVariant to work for ALL cases: ----------------
        public static bool IsConvexVectex(Vector3D a, Vector3D b, Vector3D c)
        {
            //https://dai.fmph.uniba.sk/upload/8/89/Gm17_lesson05.pdf
            //b-(b-a)x(c-b)
            Vector3D v1 = Vector3D.Subract(b, a);
            Vector3D v2 = Vector3D.Subract(c, b);
            Vector3D v3 = Vector3D.Cross(v1, v2);
            Vector3D v4 = Vector3D.Subract(b, v3);

            bool convex = v4.z >= 0;
            return convex;
        }
        public static bool PointsBendLeft(Vector3D a, Vector3D b, Vector3D c)
        {
            Vector3D v1 = Vector3D.Subract(b, a);
            double thetaDeg = Math.Abs(Math.Atan(v1.y / v1.x) * 180.0f / Math.PI);
            if (v1.x < 0 && v1.y >= 0)
            {
                thetaDeg = 180 - thetaDeg;
            }
            if (v1.x >= 0 && v1.y < 0)
            {
                thetaDeg = 180 + thetaDeg;
            }
            if (v1.x < 0 && v1.y < 0)
            {
                thetaDeg = 360 - thetaDeg;
            }
            Vector3D v2 = Vector3D.Subract(c, a);
            Vector3D v3 = RotatePointXY(v2, -thetaDeg);

            bool bendLeft = v3.y >= 0;

            return bendLeft;
        }
        private static Vector3D CalcNormal(List<Vector3D> points)
        {
            // calculating normal using Newell's method
            double x = 0;
            double y = 0;
            double z = 0;
            for (var i = 0; i < points.Count; i++)
            {
                var j = (i + 1) % (points.Count);
                x += (points[i].y - points[j].y) * (points[i].z + points[j].z);
                y += (points[i].z - points[j].z) * (points[i].x + points[j].x);
                z += (points[i].x - points[j].x) * (points[i].y + points[j].y);
            }
            return new Vector3D(x, y, z);
        }
        public static bool IsConvexVectex(Vector3D a, Vector3D b, Vector3D c, Vector3D n)
        {
            //https://github.com/NMO13/earclipper
            int orientation = GetOrientation(a, b, c, n);
            return orientation == 1;
        }
        public static int GetOrientation(Vector3D a, Vector3D b, Vector3D c, Vector3D n)
        {
            Vector3D v1 = Vector3D.Subract(a, b);
            Vector3D v2 = Vector3D.Subract(c, b);
            Vector3D v3 = Vector3D.Cross(v1, v2);
            var res = v3;
            if (Vector3D.Dot(res, res) == 0)
                return 0;
            if (Math.Sign(res.x) != Math.Sign(n.x) || Math.Sign(res.y) != Math.Sign(n.y) || Math.Sign(res.z) != Math.Sign(n.z))
                return 1;
            return -1;
        }
        // ---------------------------------------------------------------------------------------------------

        public static Vector3D RotatePointXY(Vector3D point, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180.0f);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            Vector3D newPoint = new Vector3D(
                                    point.x * cosTheta - point.y * sinTheta,
                                    point.x * sinTheta + point.y * cosTheta,
                                    point.z);
            return newPoint;
        }

        public static double TriangleArea(Vector3D a, Vector3D b, Vector3D c)
        {
            return Math.Abs((a.x * (b.y - c.y) +
                             b.x * (c.y - a.y) +
                             c.x * (a.y - b.y)) / 2.0);
        }

        public static bool PointInTriangle2(Vector3D p, Vector3D a, Vector3D b, Vector3D c)
        {
            double A = TriangleArea(a, b, c);
            double A1 = TriangleArea(p, b, c);
            double A2 = TriangleArea(a, p, c);
            double A3 = TriangleArea(a, b, p);
            return Math.Abs(A - (A1 + A2 + A3)) < 0.00000000001;
        }

        public static bool PointInTriangle(Vector3D p, Vector3D a, Vector3D b, Vector3D c)
        {
            if (SameSide(p, a, b, c) && SameSide(p, b, a, c) && SameSide(p, c, a, b))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SameSide(Vector3D p1, Vector3D p2, Vector3D a, Vector3D b)
        {
            Vector3D cp1 = Vector3D.Cross(Vector3D.Subract(b, a), Vector3D.Subract(p1, a));
            Vector3D cp2 = Vector3D.Cross(Vector3D.Subract(b, a), Vector3D.Subract(p2, a));
            if (Vector3D.Dot(cp1, cp2) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Mesh CreateTriangleMesh(Vector3D v1, Vector3D v2, Vector3D v3, double top, double bottom)
        {
            double mean = (bottom + top) / 2.0;
            List<Vector3D> verticies = new List<Vector3D>();
            verticies.Add(new Vector3D(v1.x, v1.y, mean));
            verticies.Add(new Vector3D(v2.x, v2.y, mean));
            verticies.Add(new Vector3D(v3.x, v3.y, mean));

            List<int> triangles = new List<int>();
            triangles.AddRange(new int[] { 0, 1, 2 });

            return new Mesh(verticies, triangles);
        }

        public static Mesh CreateExtrudedMesh(List<Vector3D> vects, double top, double bottom)
        {
            List<Vector3D> verticies = new List<Vector3D>();
            // add bottom
            foreach (Vector3D point in vects)
            {
                verticies.Add(new Vector3D(point.x, point.y, bottom));
            }
            // add sides
            foreach (Vector3D point in vects)
            {
                verticies.Add(new Vector3D(point.x, point.y, bottom));
                verticies.Add(new Vector3D(point.x, point.y, top));
            }
            // add top
            foreach (Vector3D point in vects)
            {
                verticies.Add(new Vector3D(point.x, point.y, top));
            }

            List<int> triangles = CreateTrianglesForExtrudedSolid(vects);

            return new Mesh(verticies, triangles);
        }

        public static Mesh CreateBoundingBox(Vector3D dimentions, FaceSide faceSide)
        {
            List<Vector3D> cornerList = new List<Vector3D>
            {
                new Vector3D(-dimentions.x / 2.0, -dimentions.y / 2.0, -dimentions.z / 2.0),
                new Vector3D(dimentions.x / 2.0, -dimentions.y / 2.0, -dimentions.z / 2.0),
                new Vector3D(dimentions.x / 2.0, dimentions.y / 2.0, -dimentions.z / 2.0),
                new Vector3D(-dimentions.x / 2.0, dimentions.y / 2.0, -dimentions.z / 2.0),
                new Vector3D(-dimentions.x / 2.0, -dimentions.y / 2.0, dimentions.z / 2.0),
                new Vector3D(dimentions.x / 2.0, -dimentions.y / 2.0, dimentions.z / 2.0),
                new Vector3D(dimentions.x / 2.0, dimentions.y / 2.0, dimentions.z / 2.0),
                new Vector3D(-dimentions.x / 2.0, dimentions.y / 2.0, dimentions.z / 2.0)
            };

            List<int> tList = new List<int>();
            tList.AddRange(new List<int> { 0, 1, 2 });
            tList.AddRange(new List<int> { 0, 2, 3 });
            tList.AddRange(new List<int> { 4, 7, 6 });
            tList.AddRange(new List<int> { 4, 6, 5 });
            tList.AddRange(new List<int> { 0, 4, 1 });
            tList.AddRange(new List<int> { 1, 4, 5 });
            tList.AddRange(new List<int> { 1, 5, 2 });
            tList.AddRange(new List<int> { 2, 5, 6 });
            tList.AddRange(new List<int> { 2, 6, 3 });
            tList.AddRange(new List<int> { 3, 6, 7 });
            tList.AddRange(new List<int> { 3, 7, 0 });
            tList.AddRange(new List<int> { 0, 7, 4 });

            List<int> triangles = new List<int>();
            if (faceSide == FaceSide.FRONT || faceSide == FaceSide.BOTH)
            {
                triangles.AddRange(tList);
            }
            if (faceSide == FaceSide.BACK || faceSide == FaceSide.BOTH)
            {
                tList.Reverse();
                triangles.AddRange(tList);
            }

            return new Mesh(cornerList, triangles);
        }

        public static double MeshDistance(Mesh m1, Mesh m2)
        {
            double minDist = double.MaxValue;
            for (int i = 0; i < m1.TriangleList.Count; i += 3)
            {
                Vector3D v0 = m1.VertexList[m1.TriangleList[i]];
                Vector3D v1 = m1.VertexList[m1.TriangleList[i + 1]];
                Vector3D v2 = m1.VertexList[m1.TriangleList[i + 2]];

                for (int j = 0; j < m2.TriangleList.Count; j += 3)
                {
                    Vector3D u0 = m2.VertexList[m2.TriangleList[j]];
                    Vector3D u1 = m2.VertexList[m2.TriangleList[j + 1]];
                    Vector3D u2 = m2.VertexList[m2.TriangleList[j + 2]];

                    // Get the min distance of each triangle pair:
                    DistTriangle3Triangle3 distTriangle3Triangle3 = new DistTriangle3Triangle3(
                                                                                    new Triangle3d(
                                                                                                    Utils.V3DToV3d(v0),
                                                                                                    Utils.V3DToV3d(v1),
                                                                                                    Utils.V3DToV3d(v2)),
                                                                                    new Triangle3d(
                                                                                                    Utils.V3DToV3d(u0),
                                                                                                    Utils.V3DToV3d(u1),
                                                                                                    Utils.V3DToV3d(u2)));
                    double dist = distTriangle3Triangle3.Get();
                    minDist = Math.Min(minDist, dist);
                }
            }

            return minDist;
        }

        public static Vector3d V3DToV3d(Vector3D v)
        {
            return new Vector3d(v.x, v.y, v.z);
        }
        public static Vector3D V3DToV3d(Vector3d v)
        {
            return new Vector3D(v.x, v.y, v.z);
        }
        public static bool MeshOverlap(Mesh m1, Mesh m2, double shrinkRatio)
        {
            Mesh m1Shrink = ShrinkMesh(m1, shrinkRatio);
            Mesh m2Shrink = ShrinkMesh(m2, shrinkRatio);
            //Mesh m1Shrink = m1;
            //Mesh m2Shrink = m2;

            for (int i = 0; i < m1Shrink.TriangleList.Count; i += 3)
            {
                Vector3D v0 = m1Shrink.VertexList[m1Shrink.TriangleList[i]];
                Vector3D v1 = m1Shrink.VertexList[m1Shrink.TriangleList[i + 1]];
                Vector3D v2 = m1Shrink.VertexList[m1Shrink.TriangleList[i + 2]];

                for (int j = 0; j < m2Shrink.TriangleList.Count; j += 3)
                {
                    Vector3D u0 = m2Shrink.VertexList[m2Shrink.TriangleList[j]];
                    Vector3D u1 = m2Shrink.VertexList[m2Shrink.TriangleList[j + 1]];
                    Vector3D u2 = m2Shrink.VertexList[m2Shrink.TriangleList[j + 2]];

                    IntrTriangle3Triangle3 intrTriangle3Triangle3 = new IntrTriangle3Triangle3(
                                                                                    new Triangle3d(
                                                                                                    V3DToV3d(v0),
                                                                                                    V3DToV3d(v1),
                                                                                                    V3DToV3d(v2)),
                                                                                    new Triangle3d(
                                                                                                    V3DToV3d(u0),
                                                                                                    V3DToV3d(u1),
                                                                                                    V3DToV3d(u2)));
                    //int counter = 0;
                    bool testResult1 = intrTriangle3Triangle3.Test();
                    //if (testResult1) { counter++; }
                    //bool testResult2 = TrianglesOverlap(v0, v1, v2, u0, u1, u2);
                    //if (testResult2) { counter++; }
                    //bool testResult3 = TriTriOverlap.TriTriIntersect(v0.GetV3(), v1.GetV3(), v2.GetV3(), u0.GetV3(), u1.GetV3(), u2.GetV3());
                    //if (testResult3) { counter++; }

                    //if (counter >= 2) { return true; }
                    if (testResult1) { return true; }
                    //if (testResult2) { return true; }
                    //if (testResult3) { return true; }
                }
            }

            // Next Two check if one is fully inside the other
            Vector3D centerM1 = Vector3D.Average(m1Shrink.VertexList.ToArray());
            if (RayIntersectsMesh(centerM1, new Vector3D(0, 0, 1), m2Shrink) &&
                RayIntersectsMesh(centerM1, new Vector3D(0, 0, -1), m2Shrink) &&
                RayIntersectsMesh(centerM1, new Vector3D(0, 1, 0), m2Shrink) &&
                RayIntersectsMesh(centerM1, new Vector3D(0, -1, 0), m2Shrink) &&
                RayIntersectsMesh(centerM1, new Vector3D(1, 0, 0), m2Shrink) &&
                RayIntersectsMesh(centerM1, new Vector3D(-1, 0, 0), m2Shrink))
            {
                return true;
            }

            Vector3D centerM2 = Vector3D.Average(m2Shrink.VertexList.ToArray());
            if (RayIntersectsMesh(centerM2, new Vector3D(0, 0, 1), m1Shrink) &&
                RayIntersectsMesh(centerM2, new Vector3D(0, 0, -1), m1Shrink) &&
                RayIntersectsMesh(centerM2, new Vector3D(0, 1, 0), m1Shrink) &&
                RayIntersectsMesh(centerM2, new Vector3D(0, -1, 0), m1Shrink) &&
                RayIntersectsMesh(centerM2, new Vector3D(1, 0, 0), m1Shrink) &&
                RayIntersectsMesh(centerM2, new Vector3D(-1, 0, 0), m1Shrink))
            {
                return true;
            }

            return false;
        }

        public static bool TrianglesOverlap(Vector3D v0, Vector3D v1, Vector3D v2, Vector3D u0, Vector3D u1, Vector3D u2)
        {
            // https://web.stanford.edu/class/cs277/resources/papers/Moller1997b.pdf
            Vector3D norm1 = Vector3D.Cross(Vector3D.Subract(v1, v0), Vector3D.Subract(v2, v0));
            double d1 = Vector3D.Dot(norm1.Neg(), v0);

            double du0 = Vector3D.Dot(norm1, u0) + d1;
            double du1 = Vector3D.Dot(norm1, u1) + d1;
            double du2 = Vector3D.Dot(norm1, u2) + d1;
            int posiCount = (du0 > 0 ? 1 : 0) + (du1 > 0 ? 1 : 0) + (du2 > 0 ? 1 : 0);
            int negaCount = (du0 < 0 ? 1 : 0) + (du1 < 0 ? 1 : 0) + (du2 < 0 ? 1 : 0);
            int zerpCount = (du0 == 0 ? 1 : 0) + (du1 == 0 ? 1 : 0) + (du2 == 0 ? 1 : 0);
            if (posiCount == 0 || negaCount == 0)
            {
                return false;
            }

            Vector3D norm2 = Vector3D.Cross(Vector3D.Subract(u1, u0), Vector3D.Subract(u2, u0));
            double d2 = Vector3D.Dot(norm2.Neg(), u0);

            double dv0 = Vector3D.Dot(norm2, v0) + d2;
            double dv1 = Vector3D.Dot(norm2, v1) + d2;
            double dv2 = Vector3D.Dot(norm2, v2) + d2;
            posiCount = (dv0 > 0 ? 1 : 0) + (dv1 > 0 ? 1 : 0) + (dv2 > 0 ? 1 : 0);
            negaCount = (dv0 < 0 ? 1 : 0) + (dv1 < 0 ? 1 : 0) + (dv2 < 0 ? 1 : 0);
            zerpCount = (dv0 == 0 ? 1 : 0) + (dv1 == 0 ? 1 : 0) + (dv2 == 0 ? 1 : 0);
            if (posiCount == 0 || negaCount == 0)
            {
                return false;
            }

            Vector3D D = Vector3D.Cross(norm1, norm2);

            double tv1 = 0, tv2 = 0;
            if ((Math.Sign(dv2) != Math.Sign(dv0)) && (Math.Sign(dv1) == Math.Sign(dv2)))
            {
                // if v0 is on the other side of v1, v2:
                GetTvalues(v1, v0, v2, D, dv1, dv0, dv2, out tv1, out tv2);
            }
            if ((Math.Sign(dv0) != Math.Sign(dv1)) && (Math.Sign(dv0) == Math.Sign(dv2)))
            {
                // if v1 is on the other side of v0, v2:
                GetTvalues(v0, v1, v2, D, dv0, dv1, dv2, out tv1, out tv2);
            }
            if ((Math.Sign(dv0) != Math.Sign(dv2)) && (Math.Sign(dv0) == Math.Sign(dv1)))
            {
                // if v2 is on the other side of v0, v1:
                GetTvalues(v2, v0, v1, D, dv2, dv0, dv1, out tv1, out tv2);
            }

            double tu1 = 0, tu2 = 0;
            if ((Math.Sign(du2) != Math.Sign(du0)) && (Math.Sign(du1) == Math.Sign(du2)))
            {
                // if u0 is on the other side of u1, u2:
                GetTvalues(u1, u0, u2, D, du1, du0, du2, out tu1, out tu2);
            }
            if ((Math.Sign(du0) != Math.Sign(du1)) && (Math.Sign(du0) == Math.Sign(du2)))
            {
                // if u1 is on the other side of u0, u2:
                GetTvalues(u0, u1, u2, D, du0, du1, du2, out tu1, out tu2);
            }
            if ((Math.Sign(du0) != Math.Sign(du2)) && (Math.Sign(du0) == Math.Sign(du1)))
            {
                // if u2 is on the other side of u0, u1:
                GetTvalues(u0, u2, u1, D, du0, du2, du1, out tu1, out tu2);
            }

            // Check if tv1, tv2, tu1, tu2 intersect:
            if (ValueInRange(tv1, tv2, tu1) || ValueInRange(tv1, tv2, tu2) || ValueInRange(tu1, tu2, tv1) || ValueInRange(tu1, tu2, tv2))
            {
                // They overlap
                return true;
            }

            if (Vector3D.AreParallel(norm1, norm2))
            {
                // INCOMPLETE?
                // Here they would be co-planer but I am not sure that this means they overlap but more that they ar touching...?
            }

            return false;
        }

        public static void GetTvalues(Vector3D vA, Vector3D vB, Vector3D vC, Vector3D D, double dvA, double dvB, double dvC, out double t1, out double t2)
        {
            // if vB is on the other side of vA, vC:
            double pvA = Vector3D.Dot(D, vA);
            double pvB = Vector3D.Dot(D, vB);
            double pvC = Vector3D.Dot(D, vC);
            t1 = pvA + (pvB - pvA) * (dvA / (dvA - dvB));
            t2 = pvC + (pvB - pvC) * (dvC / (dvC - dvB));
        }

        public static bool ValueInRange(double a, double b, double value)
        {
            return (value < a && value > b) || (value < b && value > a);
        }

        public static bool RayIntersectsTriangle(Vector3D rayOrigin, Vector3D rayVector, Vector3D vertex0, Vector3D vertex1, Vector3D vertex2)//, out Vector3D  intersectionPoint)
        {
            // https://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm
            //intersectionPoint = null;

            const double EPSILON = 0.0000001;
            Vector3D edge1 = Vector3D.Subract(vertex1, vertex0);
            Vector3D edge2 = Vector3D.Subract(vertex2, vertex0);
            Vector3D h = Vector3D.Cross(rayVector, edge2);
            double a = Vector3D.Dot(edge1, h);
            if (a > -EPSILON && a < EPSILON)
                return false;    // This ray is parallel to this triangle.
            double f = 1.0 / a;
            Vector3D s = Vector3D.Subract(rayOrigin, vertex0);
            double u = f * (Vector3D.Dot(s, h));
            if (u < 0.0 || u > 1.0)
            {
                return false;
            }
            Vector3D q = Vector3D.Cross(s, edge1);
            double v = f * Vector3D.Dot(rayVector, q);
            if (v < 0.0 || u + v > 1.0)
            {
                return false;
            }
            // At this stage we can compute t to find out where the intersection point is on the line.
            double t = f * Vector3D.Dot(edge2, q);
            if (t > EPSILON) // ray intersection
            {
                //intersectionPoint = Vector3D.Add(rayOrigin, Vector3D.Multiply(rayVector, t));
                return true;
            }
            else // This means that there is a line intersection but not a ray intersection.
            {
                return false;
            }
        }

        public static bool RayIntersectsMesh(Vector3D rayOrigin, Vector3D rayVector, Mesh m)
        {
            for (int j = 0; j < m.TriangleList.Count; j += 3)
            {
                Vector3D u0 = m.VertexList[m.TriangleList[j]];
                Vector3D u1 = m.VertexList[m.TriangleList[j + 1]];
                Vector3D u2 = m.VertexList[m.TriangleList[j + 2]];

                bool testResult = RayIntersectsTriangle(rayOrigin, rayVector, u0, u1, u2);
                if (testResult)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool MeshFacing(Mesh m1, Vector3D m1Forward, Mesh m2, Vector3D m2Forward)
        {
            for (int i = 0; i < m1.TriangleList.Count; i += 3)
            {
                Vector3D v0 = m1.VertexList[m1.TriangleList[i]];
                Vector3D v1 = m1.VertexList[m1.TriangleList[i + 1]];
                Vector3D v2 = m1.VertexList[m1.TriangleList[i + 2]];

                Vector3D vM = Vector3D.Average(new Vector3D[] { v0, v1, v2 });

                for (int j = 0; j < m2.TriangleList.Count; j += 3)
                {
                    Vector3D u0 = m2.VertexList[m2.TriangleList[j]];
                    Vector3D u1 = m2.VertexList[m2.TriangleList[j + 1]];
                    Vector3D u2 = m2.VertexList[m2.TriangleList[j + 2]];

                    Vector3D uM = Vector3D.Average(new Vector3D[] { u0, u1, u2 });

                    bool testResult = RayIntersectsTriangle(v0, m1Forward, u0, u1, u2) ||
                                      RayIntersectsTriangle(v1, m1Forward, u0, u1, u2) ||
                                      RayIntersectsTriangle(v2, m1Forward, u0, u1, u2) ||
                                      RayIntersectsTriangle(vM, m1Forward, u0, u1, u2) ||
                                      RayIntersectsTriangle(u0, m2Forward, v0, v1, v2) ||
                                      RayIntersectsTriangle(u1, m2Forward, v0, v1, v2) ||
                                      RayIntersectsTriangle(u2, m2Forward, v0, v1, v2) ||
                                      RayIntersectsTriangle(uM, m2Forward, v0, v1, v2);
                    if (testResult)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool MeshInDirectionOf(Mesh m1, Vector3D direction, Mesh m2)
        {
            // Check if m2 is in the direction of m1 (ie. m2 is being checked if it lies in the direction "direction" from m1)
            Vector3D negDirection = direction.Neg();
            for (int i = 0; i < m1.TriangleList.Count; i += 3)
            {
                Vector3D v0 = m1.VertexList[m1.TriangleList[i]];
                Vector3D v1 = m1.VertexList[m1.TriangleList[i + 1]];
                Vector3D v2 = m1.VertexList[m1.TriangleList[i + 2]];

                Vector3D vM = Vector3D.Average(new Vector3D[] { v0, v1, v2 });

                for (int j = 0; j < m2.TriangleList.Count; j += 3)
                {
                    Vector3D u0 = m2.VertexList[m2.TriangleList[j]];
                    Vector3D u1 = m2.VertexList[m2.TriangleList[j + 1]];
                    Vector3D u2 = m2.VertexList[m2.TriangleList[j + 2]];

                    Vector3D uM = Vector3D.Average(new Vector3D[] { u0, u1, u2 });

                    bool testResult = RayIntersectsTriangle(v0, direction, u0, u1, u2) ||
                                      RayIntersectsTriangle(v1, direction, u0, u1, u2) ||
                                      RayIntersectsTriangle(v2, direction, u0, u1, u2) ||
                                      RayIntersectsTriangle(vM, direction, u0, u1, u2) ||
                                      RayIntersectsTriangle(u0, negDirection, v0, v1, v2) ||
                                      RayIntersectsTriangle(u1, negDirection, v0, v1, v2) ||
                                      RayIntersectsTriangle(u2, negDirection, v0, v1, v2) ||
                                      RayIntersectsTriangle(uM, negDirection, v0, v1, v2);
                    if (testResult)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// https://www.habrador.com/tutorials/math/8-convex-hull/
        /// </summary>
        public static List<Vector3D> GetConvexHull(List<Vector3D> points)
        {
            double accuracy = 0.00001;
            if (points.Count == 3)
            {
                return points;
            }
            if (points.Count < 3)
            {
                return null;
            }
            List<Vector3D> convexHull = new List<Vector3D>();

            Vector3D startVertex = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                Vector3D testPos = points[i];
                if (testPos.x < startVertex.x || (testPos.x == startVertex.x && testPos.y < startVertex.y))
                {
                    startVertex = points[i];
                }
            }
            convexHull.Add(startVertex);
            points.Remove(startVertex);

            Vector3D currentPoint = convexHull[0];
            List<Vector3D> colinearPoints = new List<Vector3D>();
            Random r = new Random();

            int counter = 0;
            while (true)
            {
                if (counter == 2)
                {
                    points.Add(convexHull[0]);
                }

                Vector3D nextPoint = points[r.Next(0, points.Count)];
                Vector3D a = currentPoint.Get2D(0);
                Vector3D b = nextPoint.Get2D(0);

                for (int i = 0; i < points.Count; i++)
                {
                    if (Vector3D.Distance(points[i], nextPoint) < accuracy)
                    {
                        continue;
                    }

                    Vector3D c = points[i].Get2D(0);
                    double relation = IsAPointLeftOfVectorOrOnTheLine(a, b, c);

                    if (relation < accuracy && relation > -accuracy)
                    {
                        colinearPoints.Add(points[i]);
                    }
                    else if (relation < 0f)
                    {
                        nextPoint = points[i];

                        b = nextPoint.Get2D(0);
                        colinearPoints.Clear();
                    }
                }

                if (colinearPoints.Count > 0)
                {
                    colinearPoints.Add(nextPoint);
                    colinearPoints = colinearPoints.OrderBy(n => Vector3D.Distance(n, currentPoint)).ToList();
                    convexHull.AddRange(colinearPoints);
                    currentPoint = colinearPoints[colinearPoints.Count - 1];
                    for (int i = 0; i < colinearPoints.Count; i++)
                    {
                        points.Remove(colinearPoints[i]);
                    }

                    colinearPoints.Clear();
                }
                else
                {
                    convexHull.Add(nextPoint);
                    points.Remove(nextPoint);
                    currentPoint = nextPoint;
                }

                if (Vector3D.Distance(currentPoint, convexHull[0]) < accuracy)
                {
                    convexHull.RemoveAt(convexHull.Count - 1);
                    break;
                }

                counter += 1;
            }

            return convexHull;
        }

        /// <summary>
        /// https://www.habrador.com/tutorials/math/9-useful-algorithms/
        /// </summary>
        public static double IsAPointLeftOfVectorOrOnTheLine(Vector3D a, Vector3D b, Vector3D p)
        {
            double determinant = (a.x - p.x) * (b.y - p.y) - (a.y - p.y) * (b.x - p.x);

            return determinant;
        }

        public static string ReturnKeyword(string name)
        {
            Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>();
            keyValuePairs.Add("Stand", new List<string>() { "stand" });
            keyValuePairs.Add("Shelf", new List<string>() { "shelf" });
            keyValuePairs.Add("CoffeeTable", new List<string>() { "CoffeeTable" });
            keyValuePairs.Add("EndTable", new List<string>() { "endtable" });
            keyValuePairs.Add("Countertop", new List<string>() { "countertop", "counter top" });
            keyValuePairs.Add("Refrigerator", new List<string>() { "fridge", "refrigerator" });
            keyValuePairs.Add("Sink", new List<string>() { "sink" });
            keyValuePairs.Add("Cornercabinet", new List<string>() { "Corner" });
            keyValuePairs.Add("Cooktop", new List<string>() { "cooktop", "cook top" });
            keyValuePairs.Add("Cabinet", new List<string>() { "cabinet", "wall unit", "tower unit", "base built-in" });
            keyValuePairs.Add("Drawer", new List<string>() { "drawer" });
            keyValuePairs.Add("Lamp", new List<string>() { "lamp" });
            keyValuePairs.Add("Stove", new List<string>() { "stove" });
            keyValuePairs.Add("RangeHood", new List<string>() { "rangehood" });
            keyValuePairs.Add("Range", new List<string>() { "range" });
            keyValuePairs.Add("Oven", new List<string>() { "oven" });
            keyValuePairs.Add("TV", new List<string>() { "tv" });
            keyValuePairs.Add("Microwave", new List<string>() { "microwave" });
            keyValuePairs.Add("Bath", new List<string>() { "bath" });
            keyValuePairs.Add("Shower", new List<string>() { "shower" });
            keyValuePairs.Add("Washer", new List<string>() { "washer" });
            keyValuePairs.Add("Dryer", new List<string>() { "dryer" });
            keyValuePairs.Add("Plant", new List<string>() { "plant" });
            keyValuePairs.Add("Rug", new List<string>() { "rug" });
            keyValuePairs.Add("Table", new List<string>() { "table" });
            keyValuePairs.Add("Chair", new List<string>() { "chair" });
            keyValuePairs.Add("Bed", new List<string>() { "bed" });
            keyValuePairs.Add("Couch", new List<string>() { "couch", "sofa" });
            keyValuePairs.Add("Floor", new List<string>() { "floor" });
            keyValuePairs.Add("Wall", new List<string>() { "wall" });
            keyValuePairs.Add("Window", new List<string>() { "window" });
            keyValuePairs.Add("Muntin", new List<string>() { "muntin" });

            foreach (KeyValuePair<string, List<string>> kvp in keyValuePairs)
            {
                foreach (string s in kvp.Value)
                {
                    if (name.ToUpper().Contains(s.ToUpper()))
                    {
                        return kvp.Key;
                    }
                }
            }

            return "Unknown";
        }
    }
}