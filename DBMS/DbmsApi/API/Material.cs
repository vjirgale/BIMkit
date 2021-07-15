using DbmsApi.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.API
{
    public class Material : MongoDocument
    {
        public string Name;
        public Properties Properties;
    }
}
