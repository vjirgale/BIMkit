using RMS.Services;
using RuleAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMS.Controllers
{
    public class RuleSetController : ApiController
    {
        RuleDBService db = RuleDBService.Instance;

        /// <summary>
        /// Gets a list of RuleSets that match the ruleSetIds passed through
        /// </summary>
        public HttpResponseMessage Get(string id)
        {
            RuleUser user = db.GetUser(ActionContext.Request.Headers.Authorization.Parameter);
            if (user == null)
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username does not exist or not logged in");
            }

            if (id == "all")
            {
                return Request.CreateResponseRMS(HttpStatusCode.OK, db.GetRuleSets().Select(rs => ConvertMongoRStoRS(rs)));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "No Id");
            }
            MongoRuleSet mongoRuleSet = db.GetRuleSet(id);
            if (mongoRuleSet == null)
            {
                return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Not a RuleSet");
            }
            return Request.CreateResponseRMS(HttpStatusCode.OK, ConvertMongoRStoRS(mongoRuleSet));
        }

        /// <summary>
        /// Create the RuleSets, making sure the user that uploaded it is the owner
        /// </summary>
        public HttpResponseMessage Post([FromBody] RuleSet ruleset)
        {
            // Create the rule
            if (ruleset != null)
            {
                // Allow the DB to assign an ID to the rule
                ruleset.Id = null;
                try
                {
                    RuleUser user = db.GetUser(ActionContext.Request.Headers.Authorization.Parameter);
                    if (user == null)
                    {
                        return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username does not exist or not logged in");
                    }

                    List<string> ruleIds = new List<string>();
                    foreach (Rule rule in ruleset.Rules)
                    {
                        string ruleId = rule.Id;
                        Rule mongoRule = db.GetRule(ruleId);
                        // Rule is new so add it to the list:
                        if (mongoRule == null)
                        {
                            rule.Id = null;
                            rule.Owner = user.PublicName;
                            ruleId = db.Create(rule);
                            db.AddRuleToUser(user.Username, ruleId);
                        }
                        else
                        {
                            // Should ideally check if the rule is changed or not, if it is then make a new one with this owner else don't update it
                            if (db.CheckRuleOwnership(rule.Id, user.Username))
                            {
                                rule.Owner = user.PublicName;
                                db.Update(rule.Id, rule);
                            }
                        }

                        ruleIds.Add(ruleId);
                    }

                    MongoRuleSet newRuleSet = new MongoRuleSet()
                    {
                        Id = ruleset.Id,
                        RuleIds = ruleIds,
                        Name = ruleset.Name,
                        Owner = user.PublicName,
                        Description = ruleset.Description
                    };
                    string ruleSetId = db.Create(newRuleSet);
                    db.AddRuleSetToUser(user.Username, ruleSetId);
                    return Request.CreateResponseRMS(HttpStatusCode.Created, ruleset.Id);
                }
                catch
                {
                    return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Unknown Error");
                }
            }

            return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Missing RuleSet");
        }

        /// <summary>
        /// update the RuleSet, making sure the user that uploaded it is the owner
        /// </summary>
        public HttpResponseMessage Put([FromBody] RuleSet ruleset)
        {
            // Create the rule
            if (ruleset != null)
            {
                try
                {
                    RuleUser user = db.GetUser(ActionContext.Request.Headers.Authorization.Parameter);
                    if (user == null)
                    {
                        return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Username does not exist or not logged in");
                    }
                    if (db.CheckRuleSetOwnership(ruleset.Id, user.Username))
                    {
                        List<string> ruleIds = new List<string>();
                        foreach (Rule rule in ruleset.Rules)
                        {
                            string ruleId = rule.Id;
                            Rule mongoRule = db.GetRule(ruleId);
                            // Rule is new so add it to the list:
                            if (mongoRule == null)
                            {
                                rule.Id = null;
                                rule.Owner = user.PublicName;
                                ruleId = db.Create(rule);
                                db.AddRuleToUser(user.Username, ruleId);
                            }
                            else
                            {
                                // Should ideally check if the rule is changed or not, if it is then make a new one with this owner else don't update it
                                if (db.CheckRuleOwnership(rule.Id, user.Username))
                                {
                                    rule.Owner = user.PublicName;
                                    db.Update(rule.Id, rule);
                                }
                            }

                            ruleIds.Add(ruleId);
                        }

                        MongoRuleSet newRuleSet = new MongoRuleSet()
                        {
                            Id = ruleset.Id,
                            RuleIds = ruleIds,
                            Name = ruleset.Name,
                            Owner = user.PublicName,
                            Description = ruleset.Description
                        };
                        db.Update(newRuleSet.Id, newRuleSet);
                        return Request.CreateResponseRMS(HttpStatusCode.OK, ruleset.Id);
                    }
                    else
                    {
                        return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "User not owner of this RuleSet");
                    }
                }
                catch
                {
                    return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Unknown Error");
                }
            }

            return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "Missing RuleSet");
        }

        /// <summary>
        /// Delete the RuleSets that match the ruleSetIds, all ruleSets that can be deleted will be, but cannot delete the ruleSets that are not owend by the deleter
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
            if (user.RuleSetOwnership.Contains(id))
            {
                db.DeleteRuleSet(id);
                db.RemoveRuleSetOwnership(user.Username, id);
                return Request.CreateResponseRMS(HttpStatusCode.OK, null);
            }

            return Request.CreateResponseRMS(HttpStatusCode.BadRequest, "User is not RuleSet owner");
        }

        private RuleSet ConvertMongoRStoRS(MongoRuleSet mongoRuleSet)
        {
            List<Rule> rules = db.GetRules(mongoRuleSet.RuleIds);
            RuleSet ruleSet = new RuleSet(mongoRuleSet.Name, mongoRuleSet.Description, rules)
            {
                Owner = mongoRuleSet.Owner,
                Id = mongoRuleSet.Id
            };

            // Remove rules no longer available from the ruleSet. (this also removes duplicates)
            List<string> existingRules = rules.Select(r => r.Id).ToList();
            mongoRuleSet.RuleIds = existingRules;
            db.Update(mongoRuleSet.Id, mongoRuleSet);

            return ruleSet;
        }
    }
}