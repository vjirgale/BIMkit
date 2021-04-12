using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RuleAPI.Models
{
    public class RelationCheck
    {
        public string Obj1Name { get; set; }
        public string Obj2Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Negation Negation { get; set; }
        public PropertyCheck PropertyCheck { get; set; }

        public RelationCheck(string obj1Name, string obj2Name, Negation negation, PropertyCheck propertyCheck)
        {
            Obj1Name = obj1Name;
            Obj2Name = obj2Name;
            Negation = negation;
            PropertyCheck = propertyCheck;
        }

        public string String()
        {
            return Obj1Name + " and " + Obj2Name + " " + Negation + " " + PropertyCheck.String();
        }

        public RelationCheck Copy()
        {
            return new RelationCheck(this.Obj1Name, this.Obj2Name, this.Negation, this.PropertyCheck.Copy());
        }
    }
}
