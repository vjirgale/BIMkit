using DbmsApi.API;
using DbmsApi.Mongo;
using g3;
using MathPackage;
using RuleAPI.Models;
using System;
using System.Linq;

namespace RuleAPI.Methods
{
    public class RelationMethods
    {
        #region return double functions relations: ================================================================

        public static double Distance(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;

            return Utils.MeshDistance(obj1.GetGlobalMesh(), obj2.GetGlobalMesh());
        }
        public static double DistanceXY(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            return Utils.MeshDistance(new Mesh(obj1.GlobalVerticies.Select(v => v.Get2D(0.0)).ToList(), obj1.Triangles), new Mesh(obj2.GlobalVerticies.Select(v => v.Get2D(0.0)).ToList(), obj2.Triangles));
        }
        public static double CenterDistance(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            return Vector3D.Distance(obj1.Location, obj2.Location);
        }
        public static double CenterDistanceXY(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            return Vector3D.Distance(obj1.Location.Get2D(0), obj2.Location.Get2D(0));
        }
        public static double ClosestVertexDistance(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            double minVertexDistance = double.MaxValue;
            foreach (Vector3D v1 in obj1.GlobalVerticies)
            {
                foreach (Vector3D v2 in obj2.GlobalVerticies)
                {
                    minVertexDistance = Math.Min(minVertexDistance, Vector3D.Distance(v1, v2));
                }
            }

            return minVertexDistance;
        }
        public static double FrontCenterDistance(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            Vector3D obj1Front = Vector3D.Add(obj1.Location, Vector3D.Multiply(obj1.ForwardDirectionY, obj1.Dimentions.y / 2.0));
            Vector3D obj2Front = Vector3D.Add(obj2.Location, Vector3D.Multiply(obj2.ForwardDirectionY, obj2.Dimentions.y / 2.0));
            return Vector3D.Distance(obj1Front.Get2D(0.0), obj2Front.Get2D(0.0));
        }
        public static double AngleBetweenForwardAndAngleTo(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            Vector3D vector = Vector3D.Subract(obj2.Location.Get2D(0), obj1.Location.Get2D(0));
            double returnVal = Vector3D.AngleRad(obj1.ForwardDirectionY, vector);
            return returnVal;
        }
        public static double AngleBetweenObjects(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;

            Vector3D vector1 = Vector3D.Subract(obj2.Location.Get2D(0), obj1.Location.Get2D(0));
            double angle1 = Vector3D.AngleRad(obj1.ForwardDirectionY, vector1);

            Vector3D vector2 = Vector3D.Subract(obj1.Location.Get2D(0), obj2.Location.Get2D(0));
            double angle2 = Vector3D.AngleRad(obj2.ForwardDirectionY, vector2);

            //double returnVal = (Math.Cos(angle1) + 1) * (Math.Cos(angle2) + 1);
            double returnVal = (angle1 + angle2);
            return returnVal;
        }
        public static double AlignmentAngle(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;

            //double angle1 = Vector3D.AngleRad(obj1.BoundingBox.ForwardDirectionY, new Vector3D(1, 0, 0));
            //double angle2 = Vector3D.AngleRad(obj2.BoundingBox.ForwardDirectionY, new Vector3D(1, 0, 0));
            //double returnVal = Math.Cos(4 * (angle1 - angle2));

            double returnVal = Vector3D.AngleRad(obj1.ForwardDirectionY, obj2.ForwardDirectionY);
            if (double.IsNaN(returnVal))
            {
                int asdasd = 0;
                returnVal = Math.PI;
            }
            return returnVal;
        }

        #endregion

        #region return bool functions relations: ================================================================

        public static bool MeshOverlap(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;

            Mesh m1 = obj1.GetGlobalMesh();
            Mesh m2 = obj2.GetGlobalMesh();
            if (Utils.MeshOverlap(m1, m2, MethodFinder.SchrinkAmount))
            {
                return true;
                //return Utils.MeshOverlap(obj1.GlobalMesh(), obj2.GlobalMesh(), shrinkAmount);
            }

            return false;
        }
        public static bool Facing(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            Vector3D m1Forward = obj1.ForwardDirectionY;
            Vector3D m2Forward = obj2.ForwardDirectionY;

            if (!Vector3D.AreOpposite(m1Forward, m2Forward))
            {
                return false;
            }

            Mesh m1 = obj1.GetGlobalMesh();
            Mesh m2 = obj2.GetGlobalMesh();
            m1 = Utils.ShrinkMesh(m1, MethodFinder.SchrinkAmount);
            m2 = Utils.ShrinkMesh(m2, MethodFinder.SchrinkAmount);
            if (Utils.MeshFacing(m1, m1Forward, m2, m2Forward))
            {
                return true;
                //// Could Also do a more indepth:
                //if (Utils.MeshFacing(obj1.GlobalMesh(), m1Forward, obj2.GlobalMesh(), m2Forward))
                //{
                //    return true;
                //}
            }
            return false;
        }
        public static bool InFrontOf(RuleCheckRelation objRelation)
        {
            // obj1 in front of obj2
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            Mesh m1 = obj1.GetGlobalMesh();
            Mesh m2 = obj2.GetGlobalMesh();
            m1 = Utils.ShrinkMesh(m1, MethodFinder.SchrinkAmount);
            m2 = Utils.ShrinkMesh(m2, MethodFinder.SchrinkAmount);
            Vector3D m2Center = obj2.Location;
            Vector3D m2Forward = obj2.ForwardDirectionY;

            if (Utils.RayIntersectsMesh(m2Center, m2Forward, m1) || Utils.MeshInDirectionOf(m2, m2Forward, m1))
            {
                return true;
                //// Could Also do a more indepth:
                //Mesh m1 = obj1.GlobalMesh();
                //Mesh m2 = obj2.GlobalMesh();
                //if (Utils.RayIntersectsMesh(m2Center, m2Forward, m1) || Utils.MeshInfrontOf(m2, m2Forward, m1))
                //{
                //    return true;
                //}
            }

            return false;
        }
        public static bool IsBehind(RuleCheckRelation objRelation)
        {
            // obj1 behind obj2
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            Mesh m1 = obj1.GetGlobalMesh();
            Mesh m2 = obj2.GetGlobalMesh();
            m1 = Utils.ShrinkMesh(m1, MethodFinder.SchrinkAmount);
            m2 = Utils.ShrinkMesh(m2, MethodFinder.SchrinkAmount);
            Vector3D m2Center = obj2.Location;
            Vector3D m2Backwards = obj2.ForwardDirectionY.Neg();

            if (Utils.RayIntersectsMesh(m2Center, m2Backwards, m1) || Utils.MeshInDirectionOf(m2, m2Backwards, m1))
            {
                return true;
                //// Could Also do a more indepth:
                //Mesh m1 = obj1.GlobalMesh();
                //Mesh m2 = obj2.GlobalMesh();
                //if (Utils.RayIntersectsMesh(m2Center, m2Backwards, m1) || Utils.MeshInfrontOf(m2, m2Backwards, m1))
                //{
                //    return true;
                //}
            }

            return false;
        }
        public static bool IsAbove(RuleCheckRelation objRelation)
        {
            // obj1 above obj2
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            Mesh m1 = obj1.GetGlobalMesh();
            Mesh m2 = obj2.GetGlobalMesh();
            m1 = Utils.ShrinkMesh(m1, MethodFinder.SchrinkAmount);
            m2 = Utils.ShrinkMesh(m2, MethodFinder.SchrinkAmount);
            Vector3D m2Center = obj2.Location;
            Vector3D m2Up = obj2.UpDirectionZ;

            if (Utils.RayIntersectsMesh(m2Center, m2Up, m1) || Utils.MeshInDirectionOf(m2, m2Up, m1))
            {
                return true;
                //// Could Also do a more indepth:
                //Mesh m1 = obj1.GlobalMesh();
                //Mesh m2 = obj2.GlobalMesh();
                //if (Utils.RayIntersectsMesh(m2Center, m2Up, m1) || Utils.MeshInfrontOf(m2, m2Up, m1))
                //{
                //    return true;
                //}
            }

            return false;
        }
        public static bool IsNextTo(RuleCheckRelation objRelation)
        {
            // obj1 is next to obj2
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            Mesh m1 = obj1.GetGlobalMesh();
            Mesh m2 = obj2.GetGlobalMesh();
            m1 = Utils.ShrinkMesh(m1, MethodFinder.SchrinkAmount);
            m2 = Utils.ShrinkMesh(m2, MethodFinder.SchrinkAmount);
            Vector3D m2Center = obj2.Location;
            Vector3D m2Left = obj2.LeftDirectionX;
            Vector3D m2Right = obj2.LeftDirectionX.Neg();

            if (Utils.RayIntersectsMesh(m2Center, m2Left, m1) || Utils.MeshInDirectionOf(m2, m2Left, m1))
            {
                return true;
                //// Could Also do a more indepth:
                //Mesh m1 = obj1.GlobalMesh();
                //Mesh m2 = obj2.GlobalMesh();
                //if (Utils.RayIntersectsMesh(m2Center, m2Left, m1) || Utils.MeshInfrontOf(m2, m2Left, m1))
                //{
                //    return true;
                //}
            }
            if (Utils.RayIntersectsMesh(m2Center, m2Right, m1) || Utils.MeshInDirectionOf(m2, m2Right, m1))
            {
                return true;
                //// Could Also do a more indepth:
                //Mesh m1 = obj1.GlobalMesh();
                //Mesh m2 = obj2.GlobalMesh();
                //if (Utils.RayIntersectsMesh(m2Center, m2Right, m1) || Utils.MeshInfrontOf(m2, m2Right, m1))
                //{
                //    return true;
                //}
            }
            return false;
        }
        public static bool IsInside(RuleCheckRelation objRelation)
        {
            RuleCheckObject obj1 = objRelation.FirstObj;
            RuleCheckObject obj2 = objRelation.SecondObj;
            Mesh m2 = new Mesh(obj2.GlobalVerticies, obj2.Triangles);
            Vector3D obj1Center = obj1.Location;

            if (Utils.RayIntersectsMesh(obj1Center, Vector3D.X, m2) &&
                Utils.RayIntersectsMesh(obj1Center, Vector3D.X.Neg(), m2) &&
                Utils.RayIntersectsMesh(obj1Center, Vector3D.Y, m2) &&
                Utils.RayIntersectsMesh(obj1Center, Vector3D.Y.Neg(), m2) &&
                Utils.RayIntersectsMesh(obj1Center, Vector3D.Z, m2) &&
                Utils.RayIntersectsMesh(obj1Center, Vector3D.Z.Neg(), m2))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region return string functions relations: ================================================================

        public static string TestFunction(RuleCheckRelation objRelation)
        {
            return "null";
        }

        #endregion
    }
}
