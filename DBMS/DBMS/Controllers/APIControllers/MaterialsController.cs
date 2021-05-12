using DBMS.Controllers.DBControllers;
using DBMS.Filters;
using DbmsApi.API;
using DbmsApi.Mongo;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBMS.Controllers.APIControllers
{
    public class MaterialsController : ApiController
    {
        protected MongoDbController db;
        public MaterialsController() { db = MongoDbController.Instance; }
        public MaterialsController(MongoDbController db) { this.db = db; }

        public HttpResponseMessage Get()
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, db.GetAllAvailableMaterials());
        }

        public HttpResponseMessage Get(string id)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            Material mat = db.RetrieveMaterial(id);
            if (mat == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "No Material with that ID exists");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, mat);
        }

        public HttpResponseMessage Post([FromBody] Material mat)
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

            if (mat == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing Material");
            }

            mat.Id = null;
            string matId = db.CreateMaterial(mat);
            return Request.CreateResponseDBMS(HttpStatusCode.Created, matId);
        }

        public HttpResponseMessage Put([FromBody] Material mat)
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

            if (mat == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing Material");
            }

            db.UpdateMaterial(mat);
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

            db.DeleteMaterial(id);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Delete Successful");
        }
    }
}