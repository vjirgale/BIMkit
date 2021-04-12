using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.API
{
    public class Relation
    {
        public string ObjectId1; // This is the ID within the model, not the catalogID
        public string ObjectId2; // This is the ID within the model, not the catalogID
        public Properties Properties = new Properties();
    }
}
