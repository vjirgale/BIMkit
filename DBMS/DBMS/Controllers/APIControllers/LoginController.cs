using DBMS.Controllers.DBControllers;
using DbmsApi.API;
using DbmsApi.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace DBMS.Controllers
{
    public class LoginController : ApiController
    {
        protected MongoDbController db;
        public LoginController() { db = MongoDbController.Instance; }
        public LoginController(MongoDbController db) { this.db = db; }

        public HttpResponseMessage Post([FromBody] AuthModel authModel)
        {
            if (authModel == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "AuthModel Missing");
            }

            User user = db.CheckUser(authModel.Username, authModel.Password); ;
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Incorrect Username or Password");
            }

            // Create token
            TokenData token = db.LoginUser(authModel);
            return Request.CreateResponse(HttpStatusCode.OK, token);
        }

        public HttpResponseMessage Delete()
        {
            User user = db.GetUserFromToken(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Not Logged in or Session has ended");
            }

            db.LogoutUser(user.Username);
            return Request.CreateResponse(HttpStatusCode.OK, "User has been logged out");
        }
    }
}