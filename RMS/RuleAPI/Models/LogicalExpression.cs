using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RuleAPI.Models
{
    public class LogicalExpression
    {
        public List<ObjectCheck> ObjectChecks { get; set; }
        public List<RelationCheck> RelationChecks { get; set; }
        public List<LogicalExpression> LogicalExpressions { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LogicalOperator LogicalOperator { get; set; }

        public LogicalExpression(List<ObjectCheck> objectChecks, List<RelationCheck> relationChecks, List<LogicalExpression> logicalExpressions, LogicalOperator logicalOperator)
        {
            ObjectChecks = objectChecks;
            RelationChecks = relationChecks;
            LogicalExpressions = logicalExpressions;
            LogicalOperator = logicalOperator;
        }

        public string String()
        {
            List<string> checksStrings = new List<string>();
            foreach (ObjectCheck oc in ObjectChecks)
            {
                checksStrings.Add(oc.String());
            }
            foreach (RelationCheck rc in RelationChecks)
            {
                checksStrings.Add(rc.String());
            }
            foreach (LogicalExpression le in LogicalExpressions)
            {
                checksStrings.Add(le.String());
            }

            string operatorString = " " + LogicalOperator + " ";
            return "(" + string.Join(operatorString, checksStrings) + ")";
        }

        public LogicalExpression Copy()
        {
            List<ObjectCheck> newObjectChecks = new List<ObjectCheck>();
            foreach (ObjectCheck ec in ObjectChecks)
            {
                newObjectChecks.Add(ec.Copy());
            }
            List<RelationCheck> newRelationChecks = new List<RelationCheck>();
            foreach (RelationCheck ec in RelationChecks)
            {
                newRelationChecks.Add(ec.Copy());
            }
            List<LogicalExpression> newLogicalExpressions = new List<LogicalExpression>();
            foreach (LogicalExpression ec in LogicalExpressions)
            {
                newLogicalExpressions.Add(ec.Copy());
            }

            return new LogicalExpression(newObjectChecks, newRelationChecks, newLogicalExpressions, this.LogicalOperator);
        }
    }
}
