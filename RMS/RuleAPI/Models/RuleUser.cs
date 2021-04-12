using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleAPI.Models
{
    public class RuleUser
    {
        [BsonId]
        [BsonElement("Name")]
        public string Username { get; set; }
        public string PublicName { get; set; }
        public List<string> RuleOwnership { get; set; }
        public List<string> RuleSetOwnership { get; set; }
    }
}