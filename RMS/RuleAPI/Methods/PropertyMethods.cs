using MathPackage;
using RuleAPI.Models;
using System;
using System.Linq;

namespace RuleAPI.Methods
{
    public class PropertyMethods
    {
        #region return double functions single: ================================================================

        public static double Width(RuleCheckObject obj)
        {
            double minX = obj.LocalVerticies.Min(v => v.x);
            double maxX = obj.LocalVerticies.Max(v => v.x);

            return maxX - minX;
        }
        public static double Depth(RuleCheckObject obj)
        {
            double minZ = obj.LocalVerticies.Min(v => v.z);
            double maxZ = obj.LocalVerticies.Max(v => v.z);

            return maxZ - minZ;
        }
        public static double Height(RuleCheckObject obj)
        {
            double minY = obj.LocalVerticies.Min(v => v.y);
            double maxY = obj.LocalVerticies.Max(v => v.y);

            return maxY - minY;
        }
        public static double MinEdgeLength(RuleCheckObject obj)
        {
            double minLen = double.MaxValue;
            for (int i = 0; i < obj.Triangles.Count; i += 3)
            {
                Vector3D v0 = obj.GlobalVerticies[obj.Triangles[i]];
                Vector3D v1 = obj.GlobalVerticies[obj.Triangles[i + 1]];
                Vector3D v2 = obj.GlobalVerticies[obj.Triangles[i + 2]];

                double d01 = Vector3D.Distance(v0, v1);
                double d02 = Vector3D.Distance(v0, v2);
                double d12 = Vector3D.Distance(v1, v2);
                minLen = Math.Min(Math.Min(minLen, d01), Math.Min(d02, d12));
            }
            return minLen;
        }
        public static double MaxEdgeLength(RuleCheckObject obj)
        {
            double maxLen = double.MinValue;
            for (int i = 0; i < obj.Triangles.Count; i += 3)
            {
                Vector3D v0 = obj.GlobalVerticies[obj.Triangles[i]];
                Vector3D v1 = obj.GlobalVerticies[obj.Triangles[i + 1]];
                Vector3D v2 = obj.GlobalVerticies[obj.Triangles[i + 2]];

                double d01 = Vector3D.Distance(v0, v1);
                double d02 = Vector3D.Distance(v0, v2);
                double d12 = Vector3D.Distance(v1, v2);
                maxLen = Math.Max(Math.Max(maxLen, d01), Math.Max(d02, d12));
            }
            return maxLen;
        }
        public static double TotalEdgeLength(RuleCheckObject obj)
        {
            double totalLen = 0;
            for (int i = 0; i < obj.Triangles.Count; i += 3)
            {
                Vector3D v0 = obj.GlobalVerticies[obj.Triangles[i]];
                Vector3D v1 = obj.GlobalVerticies[obj.Triangles[i + 1]];
                Vector3D v2 = obj.GlobalVerticies[obj.Triangles[i + 2]];

                double d01 = Vector3D.Distance(v0, v1);
                double d02 = Vector3D.Distance(v0, v2);
                double d12 = Vector3D.Distance(v1, v2);
                totalLen += d01 + d02 + d12;
            }
            return totalLen;
        }

        #endregion

        #region return bool functions single: ================================================================

        public static bool HasDoor(RuleCheckObject obj)
        {
            string name = obj.Name;
            string keyWord = Utils.ReturnKeyword(name);
            return keyWord == "Range" || keyWord == "Refrigerator" || keyWord == "Oven";
        }

        #endregion

        #region return string functions single: ================================================================

        public static string FunctionOfObj(RuleCheckObject obj)
        {
            string name = obj.Name;
            return Utils.ReturnKeyword(name);
        }

        #endregion
    }
}
