using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCheckPackage
{
    public class RuleResult
    {
        public Rule Rule { get; private set; }
        public double PassVal { get; private set; }
        public List<RuleInstance> RuleInstances { get; private set; }
        public TimeSpan Runtime { get; set; }

        public RuleResult(Rule rule, double passVal, List<RuleInstance> ruleInstances)
        {
            Rule = rule;
            PassVal = passVal;
            RuleInstances = ruleInstances;
            Runtime = new TimeSpan();
        }

        public override string ToString()
        {
            return Rule.Name + ": " + (PassVal == 1 ? "Passed" : "Failed") + " (" + PassVal.ToString("f2") + ") \t(Runtime:" + Runtime.ToString() + ")";
        }
    }
}
