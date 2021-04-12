using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathPackage
{
    public class Vector3D
    {
        public double x, y, z;

        public Vector3D()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector3D(double[] array)
        {
            if (array.Length == 3)
            {
                x = array[0];
                y = array[1];
                z = array[2];
            }
        }

        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Scale(double scale)
        {
            x = x * scale;
            y = y * scale;
            z = z * scale;
        }

        public double[] AsArray()
        {
            return new double[] { x, y, z };
        }

        public Vector4D Get4D(double w)
        {
            return new Vector4D(x, y, z, w);
        }

        public Vector3D Get2D(double z)
        {
            return new Vector3D(x, y, z);
        }

        public string Print()
        {
            return "(" + string.Join(",", this.AsArray()) + ")";
        }

        public Vector3D Norm()
        {
            double mag = this.Length();
            return new Vector3D(x / mag, y / mag, z / mag);
        }

        public Vector3D Neg()
        {
            return new Vector3D(-x, -y, -z);
        }

        public double Length()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        //public Vector3 UnityVetor()
        //{
        //    return new Vector3(x, y, z);
        //}

        // Static:

        public static double Distance(Vector3D v1, Vector3D v2)
        {
            return (double)Math.Sqrt(Math.Pow(v2.x - v1.x, 2.0) + Math.Pow(v2.y - v1.y, 2.0) + Math.Pow(v2.z - v1.z, 2.0));
        }

        public static Vector3D Subract(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3D Add(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3D Scale(Vector3D v1, double scale)
        {
            return new Vector3D(v1.x * scale, v1.y * scale, v1.z * scale);
        }

        public static Vector3D Cross(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.y * v2.z - v1.z * v2.y,
                              -(v1.x * v2.z - v1.z * v2.x),
                                v1.x * v2.y - v1.y * v2.x);
        }

        public static double Dot(Vector3D v1, Vector3D v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static double AngleRad(Vector3D v1, Vector3D v2)
        {
            return Math.Acos(Dot(v1, v2) / (v1.Length() * v2.Length()));
        }

        public static bool AreParallel(Vector3D v1, Vector3D v2)
        {
            double angle = AngleRad(v1, v2);
            return (angle == Math.PI || angle == 0.0);
        }

        public static Vector3D Multiply(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3D Multiply(Vector3D v1, double d)
        {
            return new Vector3D(v1.x * d, v1.y * d, v1.z * d);
        }

        public static Vector3D Divide(Vector3D v1, double d)
        {
            return new Vector3D(v1.x / d, v1.y / d, v1.z / d);
        }
        
        public static bool AreOpposite(Vector3D v1, Vector3D v2)
        {
            return v1.x == -v2.x && v1.y == -v2.y && v1.z == -v2.z;
        }

        public static Vector3D Average(Vector3D[] vects)
        {
            double x = 0, y = 0, z = 0;
            for (int i = 0; i < vects.Length; i++)
            {
                x += vects[i].x;
                y += vects[i].y;
                z += vects[i].z;
            }

            return new Vector3D(x / vects.Length, y / vects.Length, z / vects.Length);
        }

        public static Vector3D Average(Vector3D v1, Vector3D v2)
        {
            return new Vector3D((v1.x + v2.x) / 2.0, (v1.y + v2.y) / 2.0, (v1.z + v2.z) / 2.0);
        }

        public static Vector3D X
        {
            get { return new Vector3D(1, 0, 0); }
        }

        public static Vector3D Y
        {
            get { return new Vector3D(0, 1, 0); }
        }

        public static Vector3D Z
        {
            get { return new Vector3D(0, 0, 1); }
        }

        public static bool AreEqual(Vector3D v1, Vector3D v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public Vector3D Copy()
        {
            return new Vector3D(x, y, z);
        }

        public Vector3 GetV3()
        {
            return new Vector3((float)x, (float)y, (float)z);
        }
    }

    public class Vector4D : Vector3D
    {
        public double w;

        public Vector4D() : base()
        {
            w = 0;
        }

        public Vector4D(double[] array)
        {
            if (array.Length == 4)
            {
                x = array[0];
                y = array[1];
                z = array[2];
                w = array[3];
            }
        }

        public Vector4D(double x, double y, double z, double w) : base(x, y, z)
        {
            this.w = w;
        }

        public Vector4D(Vector3D v, double w)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = w;
        }

        public new double[] AsArray()
        {
            return new double[] { x, y, z, w };
        }

        public Vector3D Get3D()
        {
            return new Vector3D(x, y, z);
        }

        public new Vector4D Norm()
        {
            double mag = this.Length();
            return new Vector4D(x / mag, y / mag, z / mag, w / mag);
        }

        public new Vector4D Neg()
        {
            return new Vector4D(-x, -y, -z, -w);
        }

        public new double Length()
        {
            return Math.Sqrt(x * x + y * y + z * z + w * w);
        }

        //public new Vector4 UnityVetor()
        //{
        //    return new Vector4(x, y, z, w);
        //}

        // Static:

        public static double Distance(Vector4D v1, Vector4D v2)
        {
            return (double)Math.Sqrt(Math.Pow(v2.x - v1.x, 2.0) + Math.Pow(v2.y - v1.y, 2.0) + Math.Pow(v2.z - v1.z, 2.0) + Math.Pow(v2.w - v1.w, 2.0));
        }

        public static Vector4D Subract(Vector4D v1, Vector4D v2)
        {
            return new Vector4D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        public static Vector4D Scale(Vector4D v1, double scale)
        {
            return new Vector4D(v1.x * scale, v1.y * scale, v1.z * scale, v1.w * scale);
        }

        public static double Dot(Vector4D v1, Vector4D v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w * v2.w;
        }
    }

    public class Matrix4
    {
        private double[,] matrix = new double[4, 4];

        public int Dim = 4;

        public Matrix4(Vector4D c1, Vector4D c2, Vector4D c3, Vector4D c4)
        {
            SetColumn(0, c1);
            SetColumn(1, c2);
            SetColumn(2, c3);
            SetColumn(3, c4);
        }

        public Matrix4()
        {
            SetColumn(0, new Vector4D());
            SetColumn(1, new Vector4D());
            SetColumn(2, new Vector4D());
            SetColumn(3, new Vector4D());
        }

        public Vector4D GetColumn(int index)
        {
            return new Vector4D(matrix[0, index],
                                matrix[1, index],
                                matrix[2, index],
                                matrix[3, index]);
        }

        public Vector4D GetRow(int index)
        {
            return new Vector4D(matrix[index, 0],
                                matrix[index, 1],
                                matrix[index, 2],
                                matrix[index, 3]);
        }

        public double GetCell(int rowIndex, int columnIndex)
        {
            return matrix[rowIndex, columnIndex];
        }

        public void SetColumn(int index, Vector4D v)
        {
            matrix[0, index] = v.x;
            matrix[1, index] = v.y;
            matrix[2, index] = v.z;
            matrix[3, index] = v.w;
        }

        public void SetRow(int index, Vector4D v)
        {
            matrix[index, 0] = v.x;
            matrix[index, 1] = v.y;
            matrix[index, 2] = v.z;
            matrix[index, 3] = v.w;
        }

        public void SetCell(int rowIndex, int columnIndex, double v)
        {
            matrix[rowIndex, columnIndex] = v;
        }

        public Matrix4 GetTranspose()
        {
            Matrix4 matrix4 = new Matrix4();
            matrix4.SetRow(0, GetColumn(0));
            matrix4.SetRow(1, GetColumn(1));
            matrix4.SetRow(2, GetColumn(2));
            matrix4.SetRow(3, GetColumn(3));

            return matrix4;
        }

        public Matrix4 GetInverse(out bool hasInverse)
        {
            return MatrixInverse(this, out hasInverse);
        }

        // Static:

        public static Matrix4 IdentityMatrix()
        {
            return new Matrix4(
                                new Vector4D(1, 0, 0, 0),
                                new Vector4D(0, 1, 0, 0),
                                new Vector4D(0, 0, 1, 0),
                                new Vector4D(0, 0, 0, 1)
                                );
        }

        public static Vector4D Multiply(Matrix4 m, Vector4D v)
        {
            double[] sum = new double[] { 0, 0, 0, 0 };
            for (int rowNum = 0; rowNum < 4; rowNum++)
            {
                sum[rowNum] = Vector4D.Dot(m.GetRow(rowNum), v);
            }

            return new Vector4D(sum);
        }

        public static Matrix4 Multiply(Matrix4 m1, Matrix4 m2)
        {
            List<Vector4D> newColList = new List<Vector4D>();
            for (int m2ColNum = 0; m2ColNum < 4; m2ColNum++)
            {
                newColList.Add(Multiply(m1, m2.GetColumn(m2ColNum)));
            }

            return new Matrix4(newColList[0], newColList[1], newColList[2], newColList[3]);
        }

        public string Print()
        {
            List<string> rowStrings = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                List<string> columnStrings = new List<string>();
                for (int j = 0; j < 4; j++)
                {
                    columnStrings.Add(matrix[i, j].ToString());
                }
                rowStrings.Add("[ " + string.Join(", \t", columnStrings) + "]");
            }
            return string.Join("\n", rowStrings);
        }

        // http://quaetrix.com/Matrix/code.html
        public static Matrix4 MatrixInverse(Matrix4 matrix, out bool hasInverse)
        {
            hasInverse = true;
            if (MatrixDeterminant(matrix) == 0)
            {
                hasInverse = false;
                return matrix;
            }

            // assumes determinant is not 0
            // that is, the matrix does have an inverse
            int n = matrix.Dim;
            Matrix4 result = new Matrix4(); // make a copy of matrix
            for (int i = 0; i < n; ++i)
                for (int j = 0; j < n; ++j)
                    result.SetCell(i, j, matrix.GetCell(i, j));

            Matrix4 lum; // combined lower & upper
            int[] perm;
            int toggle;
            toggle = MatrixDecompose(matrix, out lum, out perm);

            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                    if (i == perm[j])
                        b[j] = 1.0f;
                    else
                        b[j] = 0.0f;

                double[] x = Helper(lum, b); // 
                for (int j = 0; j < n; ++j)
                    result.SetCell(j, i, x[j]);
            }
            return result;
        } // MatrixInverse

        public static int MatrixDecompose(Matrix4 m, out Matrix4 lum, out int[] perm)
        {
            // Crout's LU decomposition for matrix determinant and inverse
            // stores combined lower & upper in lum[][]
            // stores row permuations into perm[]
            // returns +1 or -1 according to even or odd number of row permutations
            // lower gets dummy 1.0s on diagonal (0.0s above)
            // upper gets lum values on diagonal (0.0s below)

            int toggle = +1; // even (+1) or odd (-1) row permutatuions
            int n = m.Dim;

            // make a copy of m[][] into result lu[][]
            lum = new Matrix4();
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    lum.SetCell(i, j, m.GetCell(i, j));
                }
            }

            // make perm[]
            perm = new int[n];
            for (int i = 0; i < n; ++i)
                perm[i] = i;

            for (int j = 0; j < n - 1; ++j) // process by column. note n-1 
            {
                double max = Math.Abs(lum.GetCell(j, j));
                int piv = j;

                for (int i = j + 1; i < n; ++i) // find pivot index
                {
                    double xij = Math.Abs(lum.GetCell(i, j));
                    if (xij > max)
                    {
                        max = xij;
                        piv = i;
                    }
                } // i

                if (piv != j)
                {
                    Vector4D tmp = lum.GetRow(piv); // swap rows j, piv
                    lum.SetRow(piv, lum.GetRow(j));
                    lum.SetRow(j, tmp);

                    int t = perm[piv]; // swap perm elements
                    perm[piv] = perm[j];
                    perm[j] = t;

                    toggle = -toggle;
                }

                double xjj = lum.GetCell(j, j);
                if (xjj != 0.0)
                {
                    for (int i = j + 1; i < n; ++i)
                    {
                        double xij = lum.GetCell(i, j) / xjj;
                        lum.SetCell(i, j, xij);
                        for (int k = j + 1; k < n; ++k)
                            lum.SetCell(i, k, lum.GetCell(i, k) - xij * lum.GetCell(j, k));
                    }
                }

            } // j

            return toggle;
        } // MatrixDecompose

        public static double[] Helper(Matrix4 luMatrix, double[] b)
        {
            int n = luMatrix.Dim;
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix.GetCell(i, j) * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix.GetCell(n - 1, n - 1);
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix.GetCell(i, j) * x[j];
                x[i] = sum / luMatrix.GetCell(i, i);
            }

            return x;
        } // Helper

        public static double MatrixDeterminant(Matrix4 matrix)
        {
            Matrix4 lum;
            int[] perm;
            int toggle = MatrixDecompose(matrix, out lum, out perm);
            double result = toggle;
            for (int i = 0; i < lum.Dim; ++i)
                result *= lum.GetCell(i, i);
            return result;
        }
    }

    public class Plane
    {
        public Vector3D Normal { get; private set; }
        public Vector3D PointOnPlane { get; private set; }

        public Plane(Vector3D normal, Vector3D pointOnPlane)
        {
            Normal = normal;
            PointOnPlane = pointOnPlane;
        }
    }

    public class Line
    {
        public Vector3D End0 { get; private set; }
        public Vector3D End1 { get; private set; }

        public Line(Vector3D end0, Vector3D end1)
        {
            End0 = end0;
            End1 = end1;
        }

        public Vector3D GetVect()
        {
            return Vector3D.Cross(End0, End1);
        }

        public double Length()
        {
            return Vector3D.Distance(End0, End1);
        }

        public Line Flatten()
        {
            return new Line(End0.Get2D(0.0), End1.Get2D(0.0));
        }

        public Vector3D DirectionVect()
        {
            return Vector3D.Subract(End1, End0);
        }

        public Vector3D LineEquationXY()
        {
            double A = End1.y - End0.y;
            double B = End0.x - End1.x;
            double C = A * End0.x + B * End0.y;
            return new Vector3D(A, B, C);
        }

        public static Vector3D UnboundIntersectionXY(Line l1, Line l2)
        {
            Vector3D l1V = l1.LineEquationXY();
            Vector3D l2V = l2.LineEquationXY();
            double A1 = l1V.x;
            double A2 = l2V.x;
            double B1 = l1V.y;
            double B2 = l2V.y;
            double C1 = l1V.z;
            double C2 = l2V.z;

            double det = A1 * B2 - A2 * B1;
            if (det == 0)
            {
                //Lines are parallel
                return null;
            }
            else
            {
                double x = (B2 * C1 - B1 * C2) / det;
                double y = (A1 * C2 - A2 * C1) / det;
                return new Vector3D(x, y, 0.0);
            }
        }
    }

    public class Mesh
    {
        public List<Vector3D> VertexList { get; set; }
        public List<int> TriangleList { get; set; }

        public Mesh(List<Vector3D> vertexList, List<int> triangleList)
        {
            VertexList = vertexList;
            TriangleList = triangleList;
        }

        public Mesh()
        {
            VertexList = new List<Vector3D>();
            TriangleList = new List<int>();
        }
    }
}