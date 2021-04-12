using DbmsApi.Mongo;
using System;

namespace DbmsApi.API
{
    public class TokenData : MongoDocument
    {
        public string username;
        public DateTime expiry;
    }
}