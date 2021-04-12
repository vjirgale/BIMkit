using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.API
{
    public class CatalogObject
    {
        public string CatalogID;
        public string Name;
        public ObjectTypes TypeId;

        public Properties Properties = new Properties();
        public List<Component> Components = new List<Component>();
    }
}