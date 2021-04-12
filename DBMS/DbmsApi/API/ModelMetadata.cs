using DbmsApi.Mongo;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace DbmsApi.API
{
    public class ModelMetadata
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ModelId;

        public int ObjectCount;
        public string Name;
        public string Owner;
        public Properties Properties;

        [JsonConstructor]
        [BsonConstructor]
        public ModelMetadata() { }

        public ModelMetadata(MongoModel model, string owner)
        {
            ModelId = model.Id;
            ObjectCount = model.CatalogObjects.Count + model.ModelObjects.Count;
            Name = model.Name;
            Owner = owner;
            Properties = model.Properties;
        }

        public override string ToString()
        {
            return Name + " (" + ModelId + ")";
        }
    }
}
