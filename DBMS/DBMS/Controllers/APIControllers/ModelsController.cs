using DBMS.Controllers.DBControllers;
using DBMS.Filters;
using DbmsApi.API;
using DbmsApi.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBMS.Controllers.APIControllers
{
    public class ModelsController : ApiController
    {
        protected MongoDbController db;
        public ModelsController() { db = MongoDbController.Instance; }
        public ModelsController(MongoDbController db) { this.db = db; }

        public HttpResponseMessage Get()
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, db.RetrieveAvailableModels(user.Username));
        }

        public HttpResponseMessage Get(string id, string lod)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            LevelOfDetail levelOfDetail = LevelOfDetail.LOD100;
            try
            {
                levelOfDetail = (LevelOfDetail)Enum.Parse(typeof(LevelOfDetail), lod);
            }
            catch
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing Level of Detail");
            }

            MongoModel model = db.GetModel(id);
            if (model == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "No model exists with the given Id");
            }

            if (!(user.AccessibleModels.Contains(model.Id) || user.OwnedModels.Contains(model.Id) || user.IsAdmin))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Cannot access this model");
            }

            Model fullModel = new Model
            {
                Id = model.Id,
                Name = model.Name,
                Tags = model.Tags,
                Properties = model.Properties,
                ModelObjects = model.ModelObjects,
                Relations = model.Relations,
            };

            foreach (CatalogObjectReference catalogRef in model.CatalogObjects)
            {
                MongoCatalogObject mongoCO = db.GetCatalogObject(catalogRef.CatalogId);
                ModelCatalogObject catalogObject = new ModelCatalogObject()
                {
                    Id = catalogRef.Id,
                    CatalogId = catalogRef.CatalogId,
                    Location = catalogRef.Location,
                    Orientation = catalogRef.Orientation,
                    Tags = catalogRef.Tags,
                    Components = (mongoCO.MeshReps.Any(c => c.LevelOfDetail == levelOfDetail) ? mongoCO.MeshReps.First(c => c.LevelOfDetail == levelOfDetail) : mongoCO.MeshReps.FirstOrDefault()).Components,
                    Name = mongoCO.Name,
                    Properties = mongoCO.Properties,
                    TypeId = mongoCO.TypeId
                };

                fullModel.ModelObjects.Add(catalogObject);
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, fullModel);
        }

        public HttpResponseMessage Post([FromBody] Model model)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (model == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Model missing");
            }

            MongoModel fullModel = new MongoModel(model, user.PublicName);
            fullModel.Id = null;
            string id = db.CreateModel(fullModel);
            db.AddOwnedModel(user.Username, id);

            // Return the id of the new model
            return Request.CreateResponseDBMS(HttpStatusCode.Created, id);
        }

        public HttpResponseMessage Put([FromBody] Model model)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (model == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Model missing");
            }

            if (!(user.AccessibleModels.Contains(model.Id) || user.OwnedModels.Contains(model.Id) || user.IsAdmin))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Cannot access this model");
            }

            MongoModel fullModel = new MongoModel(model, user.PublicName);
            db.UpdateModel(fullModel);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Update Successful");
        }

        public HttpResponseMessage Delete(string id)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!(user.OwnedModels.Contains(id) || user.IsAdmin))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Cannot delete this model");
            }

            db.DeleteModel(id);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Delete Successful");
        }
    }
}