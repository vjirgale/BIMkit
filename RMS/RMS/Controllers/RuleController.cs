using RMS.Services;
using RuleAPI.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMS.Controllers
{
    public class RuleController : ApiController
    {
        RuleDBService db = RuleDBService.Instance;

        /// <summary>
        /// Gets a list of Rules that match the id passed through (all indicates get all rules)
        /// </summary>
        public HttpResponseMessage Get(string id)
        {
            if (id == "all")
            {
                return Request.CreateResponseRMS(HttpStatusCode.OK, db.GetRules());
            }

            RuleUser user = db.GetUser(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username does not exist or not logged in");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "No Id");
            }

            Rule mongoRule = db.GetRule(id);
            if (mongoRule == null)
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Not a Rule");
            }

            return Request.CreateResponseRMS(HttpStatusCode.OK, mongoRule);
        }

        /// <summary>
        /// Create the Rule, making sure the user that uploaded it is the owner
        /// </summary>
        public HttpResponseMessage Post([FromBody] Rule rule)
        {
            // Create the rule
            if (rule != null)
            {
                // Allow the DB to assign an ID to the rule
                rule.Id = null;
                try
                {
                    RuleUser user = db.GetUser(ActionContext.Request.Headers.Authorization.Parameter);
                    if (user == null)
                    {
                        return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username does not exist or not logged in");
                    }
                    rule.Owner = user.PublicName;
                    string ruleId = db.Create(rule);
                    db.AddRuleToUser(user.Username, ruleId);
                    return Request.CreateResponseRMS(HttpStatusCode.Created, ruleId);
                }
                catch
                {
                    return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Unknown Error");
                }
            }

            return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Missing Rule");
        }

        /// <summary>
        /// update the Rule, making sure the user that uploaded it is the owner
        /// </summary>
        public HttpResponseMessage Put([FromBody] Rule rule)
        {
            // Create the rule
            if (rule != null)
            {
                try
                {
                    RuleUser user = db.GetUser(ActionContext.Request.Headers.Authorization.Parameter);
                    if (user == null)
                    {
                        return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username does not exist or not logged in");
                    }
                    if (db.CheckRuleOwnership(rule.Id, user.Username))
                    {
                        rule.Owner = user.PublicName;
                        db.Update(rule.Id, rule);
                        return Request.CreateResponseRMS(HttpStatusCode.OK, rule.Id);
                    }
                    else
                    {
                        return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "User not owner of this Rule");
                    }
                }
                catch
                {
                    return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Unknown Error");
                }
            }

            return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Missing Rule");
        }

        /// <summary>
        /// Delete the Rules that match the ruleIds, all rules that can be deleted will be, but cannot delete the rules that are not owend by the deleter
        /// </summary>
        public HttpResponseMessage Delete(string id)
        {
            RuleUser user = db.GetUser(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username does not exist or not logged in");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "No Id");
            }

            // Make sure the user is allowed to delete the rule:
            if (user.RuleOwnership.Contains(id))
            {
                db.DeleteRule(id);
                db.RemoveRuleOwnership(user.Username, id);
                return Request.CreateResponseRMS(HttpStatusCode.OK, null);
            }

            return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "User is not Rule owner");
        }
    }
}