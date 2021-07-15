using DbmsApi.API;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DbmsApi.Mongo
{
    public class User
    {
        /// <summary>
        /// Username will be stored in db all lowercase
        /// </summary>
        [BsonId]
        [BsonElement("Name")]
        public string Username;
        public string PublicName;

        public string PassHash;
        public string Salt;
        public bool IsAdmin = false;

        public List<string> AccessibleModels = new List<string>();
        public List<string> OwnedModels = new List<string>();

        public Properties Properties;
        public List<KeyValuePair<string, string>> Tags = new List<KeyValuePair<string, string>>();
    }
}