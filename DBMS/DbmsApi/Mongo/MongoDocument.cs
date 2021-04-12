using MongoDB.Bson.Serialization.Attributes;

namespace DbmsApi.Mongo
{
    public abstract class MongoDocument
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id;
    }
}