using DBMS.Controllers.DBControllers;
using DBMS.Filters;
using DbmsApi.API;
using DbmsApi.Mongo;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBMS.Controllers
{
    /// <summary>
    /// Controller for /users endpoint
    /// </summary>
    public class UsersController : ApiController
    {
        MongoDbController db;
        public UsersController() { this.db = MongoDbController.Instance; }
        public UsersController(MongoDbController db) { this.db = db; }

        public HttpResponseMessage Get()
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, db.RetrieveAllUserNames());
        }

        public HttpResponseMessage Get(string id)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            UserData userData = db.RetrieveUserData(id);
            if (userData == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Username does not exist");
            }

            if (!(user.IsAdmin || user.Username == userData.Username))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin or the user");
            }

            return Request.CreateResponseDBMS(HttpStatusCode.OK, userData);
        }

        public HttpResponseMessage Post([FromBody] NewUser newUserData)
        {
            if (newUserData == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "UserData missing");
            }

            // Basic data validation
            if (db.GetUser(newUserData.Username) != null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Conflict, "Username not available.");
            }

            if (string.IsNullOrEmpty(newUserData.Username) || string.IsNullOrEmpty(newUserData.Password))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Username and password cannot be Empty.");
            }

            db.AddUser(newUserData, false);
            return Request.CreateResponseDBMS(HttpStatusCode.Created, db.RetrieveUserData(newUserData.Username));
        }

        public HttpResponseMessage Put([FromBody] UserData userData)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            if (userData == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "UserData missing");
            }

            string oldUsername = userData.Username.ToLower();
            if (!(user.IsAdmin || user.Username == oldUsername))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin or the user");
            }

            // Do some cleansing of the request
            userData.Username = userData.Username.ToLower();

            if (!user.IsAdmin)
            {
                // undo their request to change access to certain properties
                userData.OwnedModels = user.OwnedModels;
                userData.AccessibleModels = user.AccessibleModels;
                userData.IsAdmin = user.IsAdmin;
            }

            db.UpdateUserData(userData);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Update Successful");
        }

        public HttpResponseMessage Delete(string id)
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            User deleteUser = db.GetUser(id);
            if (deleteUser == null)
            {
                return Request.CreateResponseDBMS(HttpStatusCode.BadRequest, "Username does not exist");
            }

            if (!(user.IsAdmin || user.Username == deleteUser.Username))
            {
                return Request.CreateResponseDBMS(HttpStatusCode.Unauthorized, "Must be an Admin or the user");
            }

            db.DeleteUser(deleteUser.Username);
            return Request.CreateResponseDBMS(HttpStatusCode.OK, "Delete Successful");
        }
    }
}