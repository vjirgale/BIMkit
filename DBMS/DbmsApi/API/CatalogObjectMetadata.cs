using DbmsApi.Mongo;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace DbmsApi.API
{
    public class CatalogObjectMetadata
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CatalogObjectId;

        public string Name;
        public Properties Properties;

        [JsonConstructor]
        [BsonConstructor]
        public CatalogObjectMetadata() { }

        public CatalogObjectMetadata(MongoCatalogObject catalogObject)
        {
            CatalogObjectId = catalogObject.Id;
            Name = catalogObject.Name;
            Properties = catalogObject.Properties;
        }

        public override string ToString()
        {
            return Name + " (" + CatalogObjectId + ")";
        }
    }
}