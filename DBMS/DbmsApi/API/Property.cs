using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace DbmsApi.API
{
    public enum PropertyType { BOOL = 0, STRING = 1, NUM = 2 }

    [JsonConverter(typeof(PropertyConverter))]
    [BsonKnownTypes(typeof(PropertyBool), typeof(PropertyString), typeof(PropertyNum))]
    public abstract class Property
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PropertyType Type { get; set; }

        public Property(string name)
        {
            Name = name;
        }

        public string GetValueString()
        {
            string returnString = "";
            if (this.GetType() == typeof(PropertyString))
            {
                returnString = (this as PropertyString).Value;
            }
            if (this.GetType() == typeof(PropertyBool))
            {
                returnString = (this as PropertyBool).Value.ToString();
            }
            if (this.GetType() == typeof(PropertyNum))
            {
                returnString = (this as PropertyNum).Value.ToString();
            }
            return returnString;
        }

        public string String(int tabCount = 0)
        {
            string tabs = "";
            for (int i = 0; i < tabCount; i++)
            {
                tabs += "\t";
            }
            return Name + ": " + tabs + GetValueString();
        }
    }
    public class PropertyString : Property
    {
        public string Value { get; set; }

        public PropertyString(string name, string value) : base(name)
        {
            Value = value;
            Type = PropertyType.STRING;
        }
    }
    public class PropertyBool : Property
    {
        public bool Value { get; set; }

        public PropertyBool(string name, bool value) : base(name)
        {
            Value = value;
            Type = PropertyType.BOOL;
        }
    }
    public class PropertyNum : Property
    {
        public double Value { get; set; }

        public PropertyNum(string name, double value) : base(name)
        {
            Value = value;
            Type = PropertyType.NUM;
        }
    }

    public class PropertyConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Property));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            switch (jo["Type"].Value<string>())
            {
                case "BOOL":
                    return JsonConvert.DeserializeObject<PropertyBool>(jo.ToString(), SpecifiedSubclassConversion);
                case "STRING":
                    return JsonConvert.DeserializeObject<PropertyString>(jo.ToString(), SpecifiedSubclassConversion);
                case "NUM":
                    return JsonConvert.DeserializeObject<PropertyNum>(jo.ToString(), SpecifiedSubclassConversion);
                default:
                    throw new Exception();
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }

    public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(Property).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }
}