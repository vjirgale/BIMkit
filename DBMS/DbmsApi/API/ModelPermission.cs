using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.API
{
    public class ModelPermission
    {
        public string ModelId;
        public string Owner;
        public List<string> UsersWithAccess = new List<string>();
    }
}
