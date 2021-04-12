using Newtonsoft.Json;

namespace RuleAPI.Models
{
    using Newtonsoft.Json.Converters;

    public class ExistentialClause
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public OccurenceRule OccurenceRule { get; set; }
        public Characteristic Characteristic { get; set; }

        public ExistentialClause(OccurenceRule occuranceRule, Characteristic characteristic)
        {
            OccurenceRule = occuranceRule;
            Characteristic = characteristic;
        }

        public string String(string itemName)
        {
            return OccurenceRule + " " + itemName + " = " + Characteristic.String();
        }

        public ExistentialClause Copy()
        {
            return new ExistentialClause(this.OccurenceRule, this.Characteristic.Copy());
        }
    }
}