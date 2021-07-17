using DbmsApi.API;
using DbmsApi.Mongo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleAPI.Models
{
    public class RuleCheckRelation
    {
        public RuleCheckObject FirstObj;
        public RuleCheckObject SecondObj;
        public Properties Properties;

        public RuleCheckRelation(Relation relation, List<RuleCheckObject> ruleCheckObjects)
        {
            Properties = relation.Properties;
            foreach (RuleCheckObject ruleCheckObject in ruleCheckObjects)
            {
                if (ruleCheckObject.ID == relation.ObjectId1)
                {
                    FirstObj = ruleCheckObject;
                }
                if (ruleCheckObject.ID == relation.ObjectId2)
                {
                    SecondObj = ruleCheckObject;
                }
            }
        }

        [JsonConstructor]
        public RuleCheckRelation(RuleCheckObject o1, RuleCheckObject o2, Properties props = null)
        {
            FirstObj = o1;
            SecondObj = o2;
            Properties = props == null ? new Properties() : props;
        }
    }
}