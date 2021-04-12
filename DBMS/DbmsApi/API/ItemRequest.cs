using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.API
{
    public class ItemRequest
    {
        public string ItemId;
        public LevelOfDetail LOD;

        public ItemRequest(string id, LevelOfDetail lod)
        {
            ItemId = id;
            LOD = lod;
        }
    }
}