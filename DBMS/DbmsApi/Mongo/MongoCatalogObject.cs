using DbmsApi.API;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DbmsApi.Mongo
{
    public class MongoCatalogObject : MongoDocument
    {
        public string Name;
        public ObjectTypes TypeId;

        public Properties Properties = new Properties();
        public List<MeshRep> MeshReps = new List<MeshRep>();
    }

    public class CatalogObjectReference : BaseObject
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CatalogId;
    }
}