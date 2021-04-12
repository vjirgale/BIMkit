using Newtonsoft.Json;

namespace RuleAPI.Models
{
    using Newtonsoft.Json.Converters;

    public class ObjectCheck
    {
        public string ObjName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Negation Negation { get; set; }
        public PropertyCheck PropertyCheck { get; set; }

        public ObjectCheck(string objName, Negation negation, PropertyCheck propertyCheck)
        {
            ObjName = objName;
            Negation = negation;
            PropertyCheck = propertyCheck;
        }

        public string String()
        {
            return ObjName + " " + Negation + " " + PropertyCheck.String();
        }

        public ObjectCheck Copy()
        {
            return new ObjectCheck(this.ObjName, this.Negation, this.PropertyCheck.Copy());
        }
    }
}