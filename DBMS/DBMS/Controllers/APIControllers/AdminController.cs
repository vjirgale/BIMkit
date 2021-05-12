using DBMS.Controllers.DBControllers;
using DBMS.Filters;
using DbmsApi.API;
using DbmsApi.Mongo;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBMS.Controllers.APIControllers
{
    public class AdminController : ApiController
    {
        protected MongoDbController db;
        public AdminController() { db = MongoDbController.Instance; }
        public AdminController(MongoDbController db) { this.db = db; }

        public HttpResponseMessage Get()
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

            return Request.CreateResponseDBMS(HttpStatusCode.OK, db.RetrieveAllUserData());
        }

        public HttpResponseMessage Get(string id)
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

            User userData = db.GetUser(id);
            if (userData == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "User does not exist");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, db.RetrieveAvailableModels(userData.Username));
        }

        public HttpResponseMessage Put([FromBody] AuthModel auth)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            User lookupUser = db.GetUser(auth.Username);
            if (lookupUser == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "No user with with that username exists");
            }
            if (!(user.IsAdmin || user.Username == lookupUser.Username))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin or the user");
            }

            db.UpdateUserPassword(auth);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Update Successful");
        }
    }
}