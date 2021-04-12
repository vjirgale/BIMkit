using MathPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common.Geometry;

namespace ModelConverter
{
    public static class ConverterGeneral
    {
        public enum Units
        {
            MM, CM, M, Inch, Ft
        }

        public static double precision = 0.0001;
        public static double GetScale(Units unit)
        {
            double scale = 1.0;
            switch (unit)
            {
                case (Units.MM):
                    scale = 1.0 / 1000.0;
                    break;
                case (Units.CM):
                    scale = 1.0 / 100.0;
                    break;
                case (Units.M):
                    scale = 1.0 / 1.0;
                    break;
                case (Units.Inch):
                    scale = 1.0 / 39.3701;
                    break;
                case (Units.Ft):
                    scale = 1.0 / 3.28084;
                    break;
            }
            return scale;
        }
        public static Units GetUnit(double scale)
        {
            if (Math.Abs(scale - 1.0 / 1000.0) < precision)
            {
                return Units.MM;
            }
            if (Math.Abs(scale - 1.0 / 100.0) < precision)
            {
                return Units.CM;
            }
            if (Math.Abs(scale - 1.0 / 1.0) < precision)
            {
                return Units.M;
            }
            if (Math.Abs(scale - 1.0 / 39.3701) < precision)
            {
                return Units.Inch;
            }
            if (Math.Abs(scale - 1.0 / 3.28084) < precision)
            {
                return Units.Ft;
            }
            return Units.M;
        }

        public static Vector3 VectorConverter(XbimPoint3D point)
        {
            return new Vector3((float)point.X, (float)point.Y, (float)point.Z);
        }
        public static Vector3 VectorConverter(Vector3D point)
        {
            return new Vector3((float)point.x, (float)point.y, (float)point.z);
        }
        public static Vector3 VectorConverter(XbimVector3D vector)
        {
            return new Vector3((float)vector.X, (float)vector.Y, (float)vector.Z);
        }
        public static Vector4 VectorConverter(XbimQuaternion quaternion)
        {
            return new Vector4((float)quaternion.X, (float)quaternion.Y, (float)quaternion.Z, (float)quaternion.W);
        }
        public static Vector3D VectorConverterXbim(XbimPoint3D point)
        {
            return new Vector3D((float)point.X, (float)point.Y, (float)point.Z);
        }
        public static Vector4D VectorConverterXbim(XbimQuaternion quaternion)
        {
            return new Vector4D(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }
        public static Vector3D VectorConverter(Tuple<double, double, double> point)
        {
            return new Vector3D(point.Item1, point.Item2, point.Item3);
        }
        public static Vector4D VectorConverter(Tuple<double, double, double, double> orientation)
        {
            return new Vector4D(orientation.Item1, orientation.Item2, orientation.Item3, orientation.Item4);
        }
        public static Tuple<double, double, double> VectorConverterTuple(Vector3 vector)
        {
            return new Tuple<double, double, double>(vector.X, vector.Y, vector.Z);
        }
        public static Tuple<double, double, double> VectorConverterTuple(Vector3D point)
        {
            return new Tuple<double, double, double>(point.x, point.y, point.z);
        }
        public static Tuple<double, double, double> VectorConverterTuple(XbimPoint3D point)
        {
            return new Tuple<double, double, double>(point.X, point.Y, point.Z);
        }
        public static Tuple<double, double, double> VectorConverterTuple(XbimVector3D vector)
        {
            return new Tuple<double, double, double>(vector.X, vector.Y, vector.Z);
        }
        public static Tuple<double, double, double, double> VectorConverterTuple(XbimQuaternion quaternion)
        {
            return new Tuple<double, double, double, double>(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }
    }
}