using DbmsApi.API;
using System.Collections.Generic;

namespace DbmsApi.Mongo
{
    public class MongoModel : Model
    {
        public List<CatalogObjectReference> CatalogObjects = new List<CatalogObjectReference>();

        public MongoModel(Model model, string userName)
        {
            List<CatalogObjectReference> catalogObjs = new List<CatalogObjectReference>();
            List<ModelObject> nonCatalogObjs = new List<ModelObject>();
            foreach (ModelObject mo in model.ModelObjects)
            {
                if (mo.GetType() == typeof(ModelCatalogObject))
                {
                    ModelCatalogObject mco = mo as ModelCatalogObject;
                    catalogObjs.Add(new CatalogObjectReference()
                    {
                        CatalogId = mco.CatalogId,
                        Id = mo.Id,
                        Location = mo.Location,
                        Orientation = mo.Orientation,
                        Tags = mo.Tags
                    });
                }
                else
                {
                    nonCatalogObjs.Add(mo);
                }
            }

            Name = model.Name;
            Tags = model.Tags;
            Properties = model.Properties;
            Relations = model.Relations;
            CatalogObjects = catalogObjs;
            ModelObjects = nonCatalogObjs;
        }
    }
}