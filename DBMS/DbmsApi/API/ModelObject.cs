using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathPackage;

namespace DbmsApi.API
{
    public abstract class BaseObject
    {
        public string Id; // This is the ID within the model, not the catalogID
        public Vector3D Location;
        public Vector4D Orientation;
        public List<KeyValuePair<string, string>> Tags = new List<KeyValuePair<string, string>>();
    }

    public class ModelObject : BaseObject
    {
        public string Name;
        public ObjectTypes TypeId;

        public Properties Properties;
        public List<Component> Components = new List<Component>();
    }

    public class ModelCatalogObject : ModelObject
    {
        public string CatalogId;
    }
}