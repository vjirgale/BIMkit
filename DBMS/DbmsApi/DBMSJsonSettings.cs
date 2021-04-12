using DbmsApi.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace DbmsApi
{
    public static class DBMSJsonSettings
    {
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            Converters = { new DBMSJsonConverter() },
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            Formatting = Formatting.Indented
        };
    }

    public class DBMSJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Property));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            if (jo["Type"].Value<string>() == PropertyType.BOOL.ToString())
                return jo.ToObject<PropertyBool>(serializer);

            if (jo["Type"].Value<string>() == PropertyType.STRING.ToString())
                return jo.ToObject<PropertyString>(serializer);

            if (jo["Type"].Value<string>() == PropertyType.NUM.ToString())
                return jo.ToObject<PropertyNum>(serializer);

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

    public static class DBMSReadWrite
    {
        public static Model ReadModel(string fileName)
        {
            return ReadModelFromString(File.ReadAllText(fileName));
        }

        public static void WriteModel(Model model, string fileName)
        {
            File.WriteAllText(fileName, ConvertModelToString(model));
        }

        public static string ConvertModelToString(Model model)
        {
            return JsonConvert.SerializeObject(model, DBMSJsonSettings.JsonSerializerSettings);
        }

        public static Model ReadModelFromString(string data)
        {
            return JsonConvert.DeserializeObject<Model>(data, DBMSJsonSettings.JsonSerializerSettings);
        }

        public static CatalogObject ReadCatalogObject(string fileName)
        {
            return ReadCatalogObjectFromString(File.ReadAllText(fileName));
        }

        public static void WriteCatalogObject(CatalogObject catalogObject, string fileName)
        {
            File.WriteAllText(fileName, ConvertCatalogObjectToString(catalogObject));
        }

        public static string ConvertCatalogObjectToString(CatalogObject catalogObject)
        {
            return JsonConvert.SerializeObject(catalogObject, DBMSJsonSettings.JsonSerializerSettings);
        }

        public static CatalogObject ReadCatalogObjectFromString(string data)
        {
            return JsonConvert.DeserializeObject<CatalogObject>(data, DBMSJsonSettings.JsonSerializerSettings);
        }
    }
}