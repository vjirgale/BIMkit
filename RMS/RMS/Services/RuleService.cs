using MongoDB.Driver;
using RuleAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RMS.Services
{
    public class RuleDBService
    {
        // Singleton stuff
        private static readonly Lazy<RuleDBService> lazy = new Lazy<RuleDBService>(() => new RuleDBService());
        public static RuleDBService Instance => lazy.Value;

        // MongoDB stuff
        private MongoClient mongoClient;
        private IMongoDatabase mongoDatabase;

        private IMongoCollection<RuleUser> ruleUserCollection;
        private IMongoCollection<Rule> ruleCollection;
        private IMongoCollection<MongoRuleSet> ruleSetCollection;

        public RuleDBService()
        {
            mongoClient = new MongoClient(new MongoUrl(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString));
            mongoDatabase = mongoClient.GetDatabase("RuleDB");
            ruleUserCollection = mongoDatabase.GetCollection<RuleUser>("users");
            ruleCollection = mongoDatabase.GetCollection<Rule>("rules");
            ruleSetCollection = mongoDatabase.GetCollection<MongoRuleSet>("ruleSets");

            if (ruleUserCollection.EstimatedDocumentCount() == 0.0)
            {
                AddUser("admin", "Admin");
            }
        }

        #region UserMethods:

        public bool CheckRuleOwnership(string ruleId, string username)
        {
            // Find user by username
            RuleUser user = GetUser(username);

            if (user != null)
            {
                return user.RuleOwnership.Contains(ruleId);
            }

            return false;
        }

        public bool CheckRuleSetOwnership(string ruleSetId, string username)
        {
            // Find user by username
            RuleUser user = GetUser(username);

            if (user != null)
            {
                return user.RuleSetOwnership.Contains(ruleSetId);
            }

            return false;
        }

        public RuleUser GetUser(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            // Make sure username is lowercase
            username = username.ToLower();

            return ruleUserCollection.Find(u => u.Username == username).FirstOrDefault();
        }

        public RuleUser AddUser(string username, string publicName)
        {
            RuleUser newUser = new RuleUser()
            {
                Username = username.ToLower(),
                PublicName = publicName,
                RuleOwnership = new List<string>(),
                RuleSetOwnership = new List<string>()
            };
            ruleUserCollection.InsertOne(newUser);
            return newUser;
        }

        public bool CheckUser(string username)
        {
            username = username.ToLower();
            RuleUser theUser = ruleUserCollection.Find(user => user.Username == username).FirstOrDefault();
            return theUser != null;
        }

        public void UpdateUser(string username, RuleUser newUserInfo)
        {
            RuleUser user = GetUser(username);
            newUserInfo.Username = newUserInfo.Username.ToLower();
            newUserInfo.RuleOwnership = new List<string>();
            newUserInfo.RuleSetOwnership = new List<string>();
            foreach (string ruleId in user.RuleOwnership)
            {
                Rule rule = GetRule(ruleId);
                if (rule != null)
                {
                    rule.Owner = user.PublicName;
                    Update(rule.Id, rule);
                    newUserInfo.RuleOwnership.Add(ruleId);
                }
            }

            foreach (string ruleSetId in user.RuleSetOwnership)
            {
                MongoRuleSet ruleSet = GetRuleSet(ruleSetId);
                if (ruleSet != null)
                {
                    ruleSet.Owner = user.PublicName;
                    Update(ruleSet.Id, ruleSet);
                    newUserInfo.RuleSetOwnership.Add(ruleSetId);
                }
            }

            ruleUserCollection.DeleteOne(u => u.Username == user.Username);
            ruleUserCollection.InsertOne(newUserInfo);
        }

        public void AddRuleToUser(string username, string ruleId)
        {
            RuleUser user = GetUser(username);
            user.RuleOwnership.Add(ruleId);
            ruleUserCollection.ReplaceOne(u => u.Username == user.Username, user);
        }

        public void AddRuleSetToUser(string username, string ruleSetId)
        {
            RuleUser user = GetUser(username);
            user.RuleSetOwnership.Add(ruleSetId);
            ruleUserCollection.ReplaceOne(u => u.Username == user.Username, user);
        }

        public void RemoveRuleOwnership(string username, string ruleId)
        {
            RuleUser user = GetUser(username);
            user.RuleOwnership.RemoveAll(ruleItem => ruleId == ruleItem);
            ruleUserCollection.ReplaceOne(u => u.Username == user.Username, user);
        }

        public void RemoveRuleSetOwnership(string username, string ruleSetId)
        {
            RuleUser user = GetUser(username);
            user.RuleSetOwnership.RemoveAll(ruleItem => ruleSetId == ruleItem);
            ruleUserCollection.ReplaceOne(u => u.Username == user.Username, user);
        }

        #endregion

        #region RuleMethods:

        public List<Rule> GetRules()
        {
            return ruleCollection.Find(rule => true).ToList();
        }

        public List<Rule> GetRules(List<string> ruleIds)
        {
            return ruleCollection.Find(rule => ruleIds.Contains(rule.Id)).ToList();
        }

        public Rule GetRule(string id)
        {
            return ruleCollection.Find(rule => rule.Id == id).FirstOrDefault();
        }

        public string Create(Rule rule)
        {
            ruleCollection.InsertOne(rule);
            return rule.Id;
        }

        public void Update(string id, Rule ruleIn)
        {
            ruleCollection.ReplaceOne(rule => rule.Id == id, ruleIn);
        }

        public void DeleteRules(List<string> ruleIds)
        {
            ruleCollection.DeleteMany(rule => ruleIds.Contains(rule.Id));
        }

        public void DeleteRule(string ruleId)
        {
            ruleCollection.DeleteMany(rule => ruleId == rule.Id);
        }

        #endregion

        #region RuleSetMethods:

        public List<MongoRuleSet> GetRuleSets()
        {
            return ruleSetCollection.Find(ruleSet => true).ToList();
        }

        public List<MongoRuleSet> GetRuleSets(List<string> ruleSetIds)
        {
            return ruleSetCollection.Find(ruleSet => ruleSetIds.Contains(ruleSet.Id)).ToList();
        }

        public MongoRuleSet GetRuleSet(string id)
        {
            return ruleSetCollection.Find(ruleSet => ruleSet.Id == id).FirstOrDefault();
        }

        public string Create(MongoRuleSet ruleSet)
        {
            ruleSetCollection.InsertOne(ruleSet);
            return ruleSet.Id;
        }

        public void Update(string id, MongoRuleSet ruleIn)
        {
            ruleSetCollection.ReplaceOne(rule => rule.Id == id, ruleIn);
        }

        public void DeleteRuleSets(List<string> ruleSetIds)
        {
            ruleSetCollection.DeleteMany(rule => ruleSetIds.Contains(rule.Id));
        }

        public void DeleteRuleSet(string ruleSetId)
        {
            ruleSetCollection.DeleteMany(rule => ruleSetId == rule.Id);
        }

        #endregion
    }
}