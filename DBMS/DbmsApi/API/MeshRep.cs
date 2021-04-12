using DbmsApi.Mongo;
using System;
using System.Collections.Generic;

namespace DbmsApi.API
{
    public enum LevelOfDetail { LOD100 = 100, LOD200 = 200, LOD300 = 300, LOD400 = 400, LOD500 = 500 };

    public class MeshRep
    {
        public LevelOfDetail LevelOfDetail;
        public List<Component> Components = new List<Component>();
        public List<Joint> Joints = new List<Joint>();
    }
}