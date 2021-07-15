using DbmsApi.API;
using Newtonsoft.Json;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCheckPackage
{
    public class RuleInstance
    {
        public List<RuleCheckObject> Objs { get; private set; }
        public List<RuleCheckRelation> Rels { get; private set; }
        public double PassVal { get; private set; }
        public Rule Rule { get; private set; }

        [JsonConstructor]
        public RuleInstance(List<RuleCheckObject> objs, List<RuleCheckRelation> rels, double passVal, Rule rule)
        {
            Objs = objs;
            Rels = rels;
            PassVal = passVal;
            Rule = rule;
        }

        public RuleInstance(List<RuleCheckObject> objs, double passVal, Rule rule, List<RuleCheckRelation> relations)
        {
            Objs = objs;
            Rels = relations;
            PassVal = passVal;
            Rule = rule;
        }

        public override string ToString()
        {
            return string.Join("-", Objs.Select(o => o.Name)) + ": " + PassVal.ToString("f2");
        }
    }
}
