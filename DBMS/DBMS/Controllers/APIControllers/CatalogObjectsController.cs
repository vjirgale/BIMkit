using DBMS.Controllers.DBControllers;
using DBMS.Filters;
using DbmsApi.API;
using DbmsApi.Mongo;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;

namespace DBMS.Controllers.APIControllers
{
    public class CatalogObjectsController : ApiController
    {
        protected MongoDbController db;
        public CatalogObjectsController() { db = MongoDbController.Instance; }
        public CatalogObjectsController(MongoDbController db) { this.db = db; }

        public HttpResponseMessage Get()
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, db.RetrieveAvailableCatalogObjects());
        }

        public HttpResponseMessage Get(string id)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            MongoCatalogObject co = db.GetCatalogObject(id);
            if (co == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "No Catalog Object with that ID exists");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, co);
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

            MongoCatalogObject co = db.GetCatalogObject(id);
            if (co == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "No Catalog Object with that ID exists");
            }

            CatalogObject fullCatalogObj = new CatalogObject()
            {
                CatalogID = co.Id,
                Name = co.Name,
                Properties = co.Properties,
                TypeId = co.TypeId,
                Components = GetNextLOD(co.MeshReps, levelOfDetail)
            };
            return Request.CreateResponseDBMS(HttpStatusCode.OK, fullCatalogObj);
        }

        private List<Component> GetNextLOD(List<MeshRep> mreps, LevelOfDetail lod)
        {
            List<LevelOfDetail> lodList = Enum.GetValues(typeof(LevelOfDetail)).Cast<LevelOfDetail>().ToList();
            int startIndex = lodList.IndexOf(lod);
            for (int i = 0; i < lodList.Count; i++)
            {
                int index = (startIndex - i + lodList.Count) % lodList.Count;
                if (mreps.Any(m => m.LevelOfDetail == lod))
                {
                    return mreps.First(m => m.LevelOfDetail == lod).Components;
                }
            }

            return mreps.First().Components;
        }

        public HttpResponseMessage Post([FromBody] MongoCatalogObject catalogObject)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!user.IsAdmin)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin");
            }

            if (catalogObject == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing CatalogObject");
            }

            catalogObject.Id = null;
            catalogObject.MeshReps = catalogObject.MeshReps.OrderBy(o => o.LevelOfDetail).ToList();
            string coId = db.CreateCatalogObject(catalogObject);
            return Request.CreateResponseDBMS(HttpStatusCode.Created, coId);
        }

        public HttpResponseMessage Put([FromBody] MongoCatalogObject catalogObject)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!user.IsAdmin)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin");
            }

            if (catalogObject == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing CatalogObject");
            }

            catalogObject.MeshReps = catalogObject.MeshReps.OrderBy(o => o.LevelOfDetail).ToList();
            db.UpdateCatalogObject(catalogObject);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Update Successful");
        }

        public HttpResponseMessage Patch([FromBody] CatalogObjectMetadata catalogObjectData)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!user.IsAdmin)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin");
            }

            if (catalogObjectData == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing CatalogObject Data");
            }

            db.UpdateCatalogObjectMetaData(catalogObjectData);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Update Successful");
        }

        public HttpResponseMessage Delete(string id)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!user.IsAdmin)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin");
            }

            db.DeleteCatalogObject(id);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Delete Successful");
        }
    }
}