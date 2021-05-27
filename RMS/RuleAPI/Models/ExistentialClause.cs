using Newtonsoft.Json;

namespace RuleAPI.Models
{
    using Newtonsoft.Json.Converters;

    public class ExistentialClause
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public OccurrenceRule OccurrenceRule { get; set; }
        public Characteristic Characteristic { get; set; }

        public ExistentialClause(OccurrenceRule occurrenceRule, Characteristic characteristic)
        {
            OccurrenceRule = occurrenceRule;
            Characteristic = characteristic;
        }

        public string String(string itemName)
        {
            return OccurrenceRule + " " + itemName + " = " + Characteristic.String();
        }

        public ExistentialClause Copy()
        {
            return new ExistentialClause(this.OccurrenceRule, this.Characteristic.Copy());
        }
    }
}