using DBMS.Controllers.DBControllers;
using DBMS.Filters;
using DbmsApi.API;
using DbmsApi.Mongo;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBMS.Controllers.APIControllers
{
    public class ModelPermissionsController : ApiController
    {
        protected MongoDbController db;
        public ModelPermissionsController() { db = MongoDbController.Instance; }
        public ModelPermissionsController(MongoDbController db) { this.db = db; }

        public HttpResponseMessage Get()
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (!(user.IsAdmin))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be admin to access full model list");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, db.RetrieveAvailableModels());
        }

        public HttpResponseMessage Get(string id)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            ModelPermission modelPermissions = db.GetModelPermissions(id);
            if (modelPermissions == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "No model exists with the given Id");
            }

            if (!(user.IsAdmin || modelPermissions.Owner == user.Username))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Cannot access this model's permision");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, modelPermissions);
        }

        public HttpResponseMessage Put([FromBody] ModelPermission newModelPermission)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (newModelPermission == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Missing ModelPermission");
            }

            // Non-admin users may only set permissions on models they own
            ModelPermission previousModelPermision = db.GetModelPermissions(newModelPermission.ModelId);
            if (!(user.IsAdmin || previousModelPermision.Owner == user.Username))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Cannot set ownership or permissions to this model");
            }

            // Some input sanitation
            newModelPermission.Owner = newModelPermission.Owner.ToLower();
            newModelPermission.UsersWithAccess.RemoveAll(s => s == newModelPermission.Owner);
            newModelPermission.UsersWithAccess = newModelPermission.UsersWithAccess.Select(s => s.ToLower()).Distinct().ToList();

            User owner = db.GetUser(newModelPermission.Owner);
            Model model = db.GetModel(newModelPermission.ModelId);
            if (model == null || owner == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.NotFound, "Model or Owner does not exist");
            }

            foreach (string username in newModelPermission.UsersWithAccess)
            {
                if (db.GetUser(username) == null)
                {
                    return Request.CreateResponseDBMS(HttpStatusCode.NotFound, "One or more access users do not exist");
                }
            }

            // Update the permissions
            db.SetModelPermissions(newModelPermission.ModelId, newModelPermission.UsersWithAccess);
            db.SetModelOwner(newModelPermission.ModelId, newModelPermission.Owner);

            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Update Successful");
        }
    }
}