using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DbmsApi.API
{
    //[JsonConverter(typeof(PropertiesConverter))]
    public class Properties
    {
        public Dictionary<string, Property> Items { get; set; }
        [JsonConstructor]
        private Properties(Dictionary<string, Property> props)
        {
            Items = props??new Dictionary<string, Property>();
        }
        public Properties()
        {
            Items = new Dictionary<string, Property>();
        }
        public Property this[string key]
        {
            get
            {
                return Items[key];
            }
        }
        public bool Add(string key, string value)
        {
            return Add(new PropertyString(key, value));
        }
        public bool Add(string key, bool value)
        {
            return Add(new PropertyBool(key, value));
        }
        public bool Add(string key, double value)
        {
            return Add(new PropertyNum(key, value));
        }
        public bool Add(Property property)
        {
            if (Items.ContainsKey(property.Name))
            {
                return false;
            }
            else
            {
                Items.Add(property.Name, property);
                return true;
            }
        }
        public bool ContainsKey(string key)
        {
            return Items.ContainsKey(key);
        }
        public IEnumerator<Property> GetEnumerator()
        {
            return Items.Select(d => d.Value).ToList().GetEnumerator();
        }
    }


    //public class PropertiesConverter : JsonConverter
    //{
    //    static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new PropertiesSpecifiedConcreteClassConverter() };

    //    public override bool CanConvert(Type objectType)
    //    {
    //        return (objectType == typeof(Properties));
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        JObject jo = JObject.Load(reader);
    //        Properties props = JsonConvert.DeserializeObject<Properties>(jo.ToString(), SpecifiedSubclassConversion);
    //        return props;
    //        //switch (jo["Type"].Value<string>())
    //        //{
    //        //    case "BOOL":
    //        //        return JsonConvert.DeserializeObject<PropertyBool>(jo.ToString(), SpecifiedSubclassConversion);
    //        //    case "STRING":
    //        //        return JsonConvert.DeserializeObject<PropertyString>(jo.ToString(), SpecifiedSubclassConversion);
    //        //    case "NUM":
    //        //        return JsonConvert.DeserializeObject<PropertyNum>(jo.ToString(), SpecifiedSubclassConversion);
    //        //    default:
    //        //        throw new Exception();
    //        //}
    //        //throw new NotImplementedException();
    //    }

    //    public override bool CanWrite
    //    {
    //        get { return false; }
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException(); // won't be called because CanWrite returns false
    //    }
    //}
    //public class PropertiesSpecifiedConcreteClassConverter : DefaultContractResolver
    //{
    //    protected override JsonConverter ResolveContractConverter(Type objectType)
    //    {
    //        if (typeof(Property).IsAssignableFrom(objectType) && !objectType.IsAbstract)
    //            return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
    //        return base.ResolveContractConverter(objectType);
    //    }
    //}

    //public class Properties
    //{
    //    private List<Property> _properties = new List<Property>();

    //    [JsonConstructor]
    //    private Properties(List<Property> properties)
    //    {
    //        _properties = properties;
    //    }

    //    public Properties() {
    //        _properties = new List<Property>();
    //    }

    //    public ReadOnlyCollection<Property> Data()
    //    {
    //        return _properties.AsReadOnly();
    //    }
    //    public IEnumerable<string> Keys()
    //    {
    //        return _properties.Select(p => p.Name);
    //    }
    //    public IEnumerable<string> Values()
    //    {
    //        return _properties.Select(p => p.GetValueString());
    //    }
    //    public int Count()
    //    {
    //        return _properties.Count;
    //    }

    //    public Property this[string key]
    //    {
    //        get
    //        {
    //            return _properties.First(p => p.Name == key);
    //        }
    //    }
    //    public Property this[int index]
    //    {
    //        get
    //        {
    //            return _properties[index];
    //        }
    //    }

    //    public bool Add(string key, string value)
    //    {
    //        return Add(new PropertyString(key, value));
    //    }
    //    public bool Add(string key, bool value)
    //    {
    //        return Add(new PropertyBool(key, value));
    //    }
    //    public bool Add(string key, double value)
    //    {
    //        return Add(new PropertyNum(key, value));
    //    }
    //    public bool Add(Property property)
    //    {
    //        if (_properties.Any(p => p.Name == property.Name))
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            _properties.Add(property);
    //            return true;
    //        }
    //    }

    //    public void Clear()
    //    {
    //        _properties.Clear();
    //    }

    //    public bool Contains(Property property)
    //    {
    //        return _properties.Any(p => p.Name == property.Name && p.GetValueString() == property.GetValueString());
    //    }

    //    public bool ContainsKey(string key)
    //    {
    //        return _properties.Any(p => p.Name == key);
    //    }

    //    public void CopyTo(Property[] array, int arrayIndex)
    //    {
    //        _properties.CopyTo(array, arrayIndex);
    //    }

    //    public IEnumerator<Property> GetEnumerator()
    //    {
    //        return _properties.GetEnumerator();
    //    }

    //    public bool Remove(Property property)
    //    {
    //        return _properties.Remove(property);
    //    }

    //    public bool Remove(string key)
    //    {
    //        Property property = _properties.FirstOrDefault(p => p.Name == key);
    //        return _properties.Remove(property);
    //    }

    //    public bool TryGetValue(string key, out string value)
    //    {
    //        if (ContainsKey(key))
    //        {
    //            value = this[key].GetValueString();
    //            return true;
    //        }
    //        else
    //        {
    //            value = default;
    //            return false;
    //        }
    //    }

    //    public bool TryGetProperty(string key, out Property value)
    //    {
    //        if (ContainsKey(key))
    //        {
    //            value = this[key];
    //            return true;
    //        }
    //        else
    //        {
    //            value = default;
    //            return false;
    //        }
    //    }
    //}
}