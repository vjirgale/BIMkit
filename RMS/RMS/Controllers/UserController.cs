using RMS.Services;
using RuleAPI.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMS.Controllers
{
    public class UserController : ApiController
    {
        RuleDBService db = RuleDBService.Instance;

        // Check for a username (lazy login)
        public HttpResponseMessage Get(string id)
        {
            RuleUser user = db.GetUser(id);
            if (user == null)
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username does not exist");
            }

            return Request.CreateResponseRMS(HttpStatusCode.OK, user);
        }

        // Create a User
        public HttpResponseMessage Post([FromBody] RuleUser newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.PublicName))
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Invalid Username and/or Public Name");
            }
            if (db.GetUser(newUser.Username) != null)
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username not available");
            }

            RuleUser createdUser = db.AddUser(newUser.Username, newUser.PublicName);
            return Request.CreateResponseRMS(HttpStatusCode.Created, createdUser);
        }

        // Update User info
        public HttpResponseMessage Put(string id, [FromBody] RuleUser newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.PublicName))
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Invalid Username and/or Public Name");
            }
            // Check that the username has changed and is not in use other then the current user:
            if (id.ToLower() != newUser.Username.ToLower() && db.GetUser(newUser.Username) != null)
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username not available");
            }

            db.UpdateUser(id, newUser);
            return Request.CreateResponseRMS(HttpStatusCode.OK, null);
        }
    }
}