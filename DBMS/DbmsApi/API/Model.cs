using DbmsApi.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.API
{
    public class Model : MongoDocument
    {
        public string Name;
        public Properties Properties = new Properties();
        public List<ModelObject> ModelObjects = new List<ModelObject>();
        public List<Relation> Relations = new List<Relation>();
        public List<KeyValuePair<string, string>> Tags = new List<KeyValuePair<string, string>>();
    }
}
