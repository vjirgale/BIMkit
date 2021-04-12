using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RuleAPI.Models
{
    public static class RuleJsonSettings
    {
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            Converters = { new RuleJsonConverter() },
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            Formatting = Formatting.Indented
        };
    }

    public class RuleJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(PropertyCheck));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["PCType"].Value<string>() == PCType.BOOL.ToString())
                return jo.ToObject<PropertyCheckBool>(serializer);

            if (jo["PCType"].Value<string>() == PCType.STRING.ToString())
                return jo.ToObject<PropertyCheckString>(serializer);

            if (jo["PCType"].Value<string>() == PCType.NUM.ToString())
                return jo.ToObject<PropertyCheckNum>(serializer);

            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public static class RuleReadWrite
    {
        public static RuleSet ReadRuleSet(string fileName)
        {
            return ReadRuleSetFromString(File.ReadAllText(fileName));
        }

        public static void WriteRuleSet(RuleSet ruleSet, string fileName)
        {
            File.WriteAllText(fileName, ConvertRuleSetToString(ruleSet));
        }

        public static string ConvertRuleSetToString(RuleSet ruleSet)
        {
            return JsonConvert.SerializeObject(ruleSet, RuleJsonSettings.JsonSerializerSettings);
        }

        public static RuleSet ReadRuleSetFromString(string data)
        {
            return JsonConvert.DeserializeObject<RuleSet>(data, RuleJsonSettings.JsonSerializerSettings);
        }

        public static Rule ReadRule(string fileName)
        {
            return ReadRuleFromString(File.ReadAllText(fileName));
        }

        public static void WriteRule(Rule rule, string fileName)
        {
            File.WriteAllText(fileName, ConvertRuleToString(rule));
        }

        public static string ConvertRuleToString(Rule rule)
        {
            return JsonConvert.SerializeObject(rule, RuleJsonSettings.JsonSerializerSettings);
        }

        public static Rule ReadRuleFromString(string data)
        {
            return JsonConvert.DeserializeObject<Rule>(data, RuleJsonSettings.JsonSerializerSettings);
        }
    }
}