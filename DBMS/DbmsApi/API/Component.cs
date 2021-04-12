using MathPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.API
{
    public class Component
    {
        public string MaterialId;
        public List<Vector3D> Vertices = new List<Vector3D>();
        public List<int[]> Triangles = new List<int[]>();
    }
}