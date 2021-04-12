using DbmsApi.API;
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
        public double PassVal { get; private set; }
        public Rule Rule { get; private set; }

        public RuleInstance(List<RuleCheckObject> objs, double passVal, Rule rule)
        {
            Objs = objs;
            PassVal = passVal;
            Rule = rule;
        }

        public override string ToString()
        {
            return string.Join("-", Objs.Select(o => o.Name)) + ": " + PassVal;
        }
    }
}
