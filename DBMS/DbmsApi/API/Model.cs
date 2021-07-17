using DbmsApi.Mongo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.API
{
    public class Model : MongoDocument
    {
        public string Name;
        public Properties Properties;
        public List<ModelObject> ModelObjects = new List<ModelObject>();
        public List<Relation> Relations = new List<Relation>();
        public List<KeyValuePair<string, string>> Tags = new List<KeyValuePair<string, string>>();

        public Model() { }

        [JsonConstructor]
        private Model(string id, string name, Properties properties, List<ModelCatalogObject> modelObjects, List<Relation> relations, List<KeyValuePair<string, string>> tags)
        {
            Id = id;
            Name = name;
            Properties = properties;
            ModelObjects = modelObjects.Where(mco => string.IsNullOrWhiteSpace(mco.CatalogId)).Select(mco => ModelObjectConverter(mco)).ToList();
            ModelObjects.AddRange(modelObjects.Where(mco => !string.IsNullOrWhiteSpace(mco.CatalogId)).ToList());
            Relations = relations;
            Tags = tags;
        }

        private ModelObject ModelObjectConverter(ModelCatalogObject mco)
        {
            return new ModelObject() {
                Components = mco.Components ,
                Id = mco.Id,
                Location = mco.Location,
                Name = mco.Name,
                Orientation = mco.Orientation,
                Properties = mco.Properties,
                Tags = mco.Tags,
                TypeId = mco.TypeId
            };
        }
    }
}
