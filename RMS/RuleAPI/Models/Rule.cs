using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RuleAPI.Models
{
    public class Rule : BaseRuleItem
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorLevel ErrorLevel { get; set; }
        public Dictionary<string, ExistentialClause> ExistentialClauses { get; set; }
        public LogicalExpression LogicalExpression { get; set; }

        [JsonConstructor]
        private Rule(string id, string name, string description, ErrorLevel errorLevel, Dictionary<string, ExistentialClause> existentialClauses, LogicalExpression logicalExpression)
        {
            Id = id;
            Name = name;
            Description = description;
            ErrorLevel = errorLevel;
            ExistentialClauses = existentialClauses;
            LogicalExpression = logicalExpression;
        }

        public Rule(string name, string description, ErrorLevel errorLevel, Dictionary<string, ExistentialClause> existentialClauses, LogicalExpression logicalExpression)
        {
            Name = name;
            Description = description;
            ErrorLevel = errorLevel;
            ExistentialClauses = existentialClauses;
            LogicalExpression = logicalExpression;
        }

        public string String()
        {
            string returnString = "";
            foreach (var ec in ExistentialClauses)
            {
                returnString += ec.Value.String(ec.Key) + "\n";
            }
            returnString += LogicalExpression.String();
            return returnString;
        }

        public Rule Copy()
        {
            Dictionary<string, ExistentialClause> newExistentialClauses = new Dictionary<string, ExistentialClause>();
            foreach (var ec in ExistentialClauses)
            {
                newExistentialClauses.Add(ec.Key, ec.Value.Copy());
            }

            return new Rule(this.Name, this.Description, this.ErrorLevel, newExistentialClauses, LogicalExpression.Copy());
        }
    }

    public class RuleSet : BaseRuleItem
    {
        public List<Rule> Rules { get; set; }

        public RuleSet(string name, string description, List<Rule> rules)
        {
            Name = name;
            Description = description;
            Rules = rules;
        }

        public RuleSet Copy()
        {
            List<Rule> newRules = new List<Rule>();
            foreach (Rule rule in Rules)
            {
                newRules.Add(rule.Copy());
            }

            return new RuleSet(this.Name, this.Description, newRules);
        }
    }

    public class MongoRuleSet : BaseRuleItem
    {
        public List<string> RuleIds { get; set; }
    }

    public class BaseRuleItem : MongoDocument
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
    }

    public abstract class MongoDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;
    }
}