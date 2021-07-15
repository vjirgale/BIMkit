using DbmsApi.API;
using DbmsApi.Mongo;
using MongoDB.Driver;
using SimpleCrypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDocument = DbmsApi.Mongo.MongoDocument;

namespace DBMS.Controllers.DBControllers
{
    /// <summary>
    /// Controller for database operations
    /// </summary>
    public class MongoDbController
    {
        // Singleton stuff
        private static readonly Lazy<MongoDbController> lazy = new Lazy<MongoDbController>(() => new MongoDbController());
        public static MongoDbController Instance => lazy.Value;

        // MongoDB stuff
        private IMongoDatabase mongoDatabase;
        private MongoClient mongoClient;

        private IMongoCollection<User> userCollection;
        private IMongoCollection<TokenData> tokenCollection;
        private IMongoCollection<Material> materialCollection;
        private IMongoCollection<MongoModel> modelCollection;
        private IMongoCollection<MongoCatalogObject> catalogObjectCollection;

        private MongoDbController()
        {
            mongoClient = new MongoClient(new MongoUrl(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString));
            mongoDatabase = mongoClient.GetDatabase("BIMkit");
            RetrieveCollections();
            CreateDefaults();
        }

        private void RetrieveCollections()
        {
            userCollection = mongoDatabase.GetCollection<User>("users");
            tokenCollection = mongoDatabase.GetCollection<TokenData>("tokens");
            materialCollection = mongoDatabase.GetCollection<Material>("materials");
            modelCollection = mongoDatabase.GetCollection<MongoModel>("models");
            catalogObjectCollection = mongoDatabase.GetCollection<MongoCatalogObject>("catalogObjects");
        }

        private void CreateDefaults()
        {
            if (userCollection.EstimatedDocumentCount() == 0.0)
            {
                AddUser(new NewUser("Admin", "Administrator", "admin"), true);
            };
            if (materialCollection.EstimatedDocumentCount() == 0.0)
            {
                CreateMaterial(new Material() { Name = "Default", Properties = new Properties() });
            };
        }

        #region USER

        public User CheckUser(string username, string password)
        {
            User user = GetUser(username);
            if (user != null)
            {
                // Compare hashed passwords
                string passHash = new PBKDF2().Compute(password, user.Salt);
                if (!passHash.Equals(user.PassHash))
                {
                    return null;
                }
            }

            return user;
        }

        public User GetUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            // Make sure username is lowercase
            username = username.ToLower();

            return userCollection.Find(u => u.Username == username).FirstOrDefault();
        }

        public User AddUser(NewUser newUser, bool admin)
        {
            ICryptoService cs = new PBKDF2();
            User user = new User()
            {
                Username = newUser.Username.ToLower(),
                PublicName = newUser.PublicName,
                PassHash = cs.Compute(newUser.Password),
                Salt = cs.Salt,
                IsAdmin = admin,
                Properties = new Properties(),
                Tags = new List<KeyValuePair<string, string>>()
            };

            userCollection.InsertOne(user);
            return user;
        }

        public void DeleteUser(string username)
        {
            // Get users owned models
            List<string> ownedModels = GetUser(username).OwnedModels;

            // Unlink users models from any other users
            ownedModels.ForEach(m => userCollection.UpdateMany(u => u.AccessibleModels.Contains(m), Builders<User>.Update.Pull(_u => _u.AccessibleModels, m)));

            // Delete any models owned by the user
            modelCollection.DeleteMany(m => ownedModels.Contains(m.Id));

            // Delete the user account
            userCollection.DeleteOne(u => u.Username == username.ToLower());
        }

        public void UpdateUserData(UserData userData)
        {
            User user = GetUser(userData.Username);
            user.PublicName = userData.PublicName;
            user.AccessibleModels = userData.AccessibleModels;
            user.OwnedModels = userData.OwnedModels;
            user.IsAdmin = userData.IsAdmin;
            user.Properties = userData.Properties;
            userCollection.ReplaceOne(u => u.Username == user.Username, user);
        }

        public void UpdateUserPassword(AuthModel auth)
        {
            User user = GetUser(auth.Username);

            ICryptoService cs = new PBKDF2();
            userCollection.UpdateOne(u => u.Username == user.Username, Builders<User>.Update.Set((u) => u.PassHash, cs.Compute(auth.Password)).Set((u) => u.Salt, cs.Salt));
        }

        public TokenData LoginUser(AuthModel userdata)
        {
            User user = GetUser(userdata.Username);
            if (user == null)
            {
                return null;
            }
            TokenData tokenData = new TokenData() { username = user.Username, expiry = DateTime.Now + (new TimeSpan(1, 0, 0)) };
            tokenCollection.InsertOne(tokenData);
            return tokenData;
        }

        public void LogoutUser(string username)
        {
            User user = GetUser(username);
            if (user == null)
            {
                return;
            }
            tokenCollection.DeleteMany(t => t.username == user.Username);
        }

        public User GetUserFromToken(string token)
        {
            TokenData tokenData = VerifyToken(token);
            if (tokenData == null)
            {
                return null;
            }
            return GetUser(tokenData.username);
        }

        public TokenData VerifyToken(string token)
        {
            // Find the token
            TokenData tokenData = GetToken(token);

            if (tokenData != null)
            {
                // If the token is expired, delete it
                if (tokenData.expiry < DateTime.Now)
                {
                    RemoveExpiredToken(tokenData.Id);
                    tokenData = null;
                }
            }

            return tokenData;
        }

        private TokenData GetToken(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            return tokenCollection.Find(t => t.Id == id).FirstOrDefault();
        }

        public void RemoveExpiredToken(string id)
        {
            tokenCollection.DeleteOne(t => t.Id == id);
        }

        public void RemoveAllExpiredTokens()
        {
            tokenCollection.DeleteMany(token => token.expiry < DateTime.Now);
        }

        public void UpdateUserAccess(string username, List<string> accessIds)
        {
            User user = this.GetUser(username);
            user.AccessibleModels = accessIds;
            userCollection.UpdateOne(u => u.Username == user.Username, Builders<User>.Update.Set((u) => u.AccessibleModels, accessIds));
        }

        public void AddOwnedModel(string username, string ownedId)
        {
            userCollection.UpdateOne(u => u.Username == username, Builders<User>.Update.Push(u => u.OwnedModels, ownedId));
        }

        public List<UserData> RetrieveAllUserData()
        {
            return userCollection.AsQueryable().ToList().Select(u => new UserData(u)).ToList();
        }

        public List<string> RetrieveAllUserNames()
        {
            return userCollection.AsQueryable().ToList().Select(u => u.Username).ToList();
        }

        public UserData RetrieveUserData(string username)
        {
            User user = GetUser(username);
            if (user == null)
            {
                return null;
            }
            return new UserData(user);
        }

        #endregion

        #region ModelControl

        public string CreateModel(MongoModel model)
        {
            modelCollection.InsertOne(model);
            return model.Id;
        }

        public MongoModel GetModel(string id)
        {
            return modelCollection.Find(m => m.Id == id).Limit(1).FirstOrDefault();
        }

        public void UpdateModel(MongoModel model)
        {
            ReplaceOneResult result = modelCollection.ReplaceOne(m => m.Id == model.Id, model);
        }

        public void DeleteModel(string id)
        {
            // Remove shared access
            userCollection.UpdateMany(u => u.AccessibleModels.Contains(id), Builders<User>.Update.Pull(u => u.AccessibleModels, id));

            // Remove ownership from the old owner
            userCollection.UpdateOne(u => u.OwnedModels.Contains(id), Builders<User>.Update.Pull(u => u.OwnedModels, id));

            // Delete document
            modelCollection.DeleteOne(m => m.Id == id);
        }

        public List<ModelMetadata> RetrieveAvailableModels(string username)
        {
            User user = this.GetUser(username);
            List<MongoModel> availableModels = modelCollection.Find(m => user.AccessibleModels.Contains(m.Id) || user.OwnedModels.Contains(m.Id)).ToList();
            return availableModels.Select(m => new ModelMetadata(m, GetModelPermissions(m.Id).Owner)).ToList();
        }

        public List<ModelMetadata> RetrieveAvailableModels()
        {
            List<MongoModel> availableModels = modelCollection.Find(m => true).ToList();
            return availableModels.Select(m => new ModelMetadata(m, GetModelPermissions(m.Id).Owner)).ToList();
        }

        public void SetModelPermissions(string modelId, List<string> UsersWithAccess)
        {
            UsersWithAccess = UsersWithAccess.Select(u => u.ToLower()).ToList();

            // Modify users who have access who shouldn't
            userCollection.UpdateMany(u => u.AccessibleModels.Contains(modelId) && !UsersWithAccess.Contains(u.Username), Builders<User>.Update.Pull(u => u.AccessibleModels, modelId));

            // Modify users who don't have access who should
            userCollection.UpdateMany(u => UsersWithAccess.Contains(u.Username) && !u.AccessibleModels.Contains(modelId), Builders<User>.Update.Push(u => u.AccessibleModels, modelId));
        }

        public void SetModelOwner(string modelId, string newOwnerId)
        {
            newOwnerId = newOwnerId.ToLower();

            // Remove ownership from the old owner
            userCollection.FindOneAndUpdate(u => u.OwnedModels.Contains(modelId), Builders<User>.Update.Pull(u => u.OwnedModels, modelId));

            // Add ownership to the new owner
            userCollection.FindOneAndUpdate(u => u.Username == newOwnerId, Builders<User>.Update.Push(u => u.OwnedModels, modelId));
        }

        public ModelPermission GetModelPermissions(string modelId)
        {
            MongoModel model = GetModel(modelId);
            if (model == null)
            {
                return null;
            }
            List<string> usersWithAccess = userCollection.Find(u => u.AccessibleModels.Contains(modelId)).ToList().Select(u => u.Username).ToList();
            User owner = userCollection.Find(u => u.OwnedModels.Contains(modelId)).Limit(1).FirstOrDefault();
            if (owner == null)
            {
                return null;
            }
            return new ModelPermission
            {
                ModelId = modelId,
                Owner = owner.Username,
                UsersWithAccess = usersWithAccess
            };
        }

        #endregion

        #region CatalogControl

        public string CreateCatalogObject(MongoCatalogObject co)
        {
            catalogObjectCollection.InsertOne(co);
            return co.Id;
        }

        public MongoCatalogObject GetCatalogObject(string id)
        {
            return catalogObjectCollection.Find(co => co.Id == id).Limit(1).FirstOrDefault();
        }

        public void UpdateCatalogObject(MongoCatalogObject model)
        {
            catalogObjectCollection.ReplaceOne(co => co.Id == model.Id, model);
        }

        public void DeleteCatalogObject(string id)
        {
            // Delete document
            catalogObjectCollection.DeleteOne(co => co.Id == id);
        }

        public void UpdateCatalogObjectMetaData(CatalogObjectMetadata catalogObject)
        {
            catalogObjectCollection.FindOneAndUpdate(c => c.Id == catalogObject.CatalogObjectId, 
                                                    Builders<MongoCatalogObject>.Update.Set(c => c.Name, catalogObject.Name)
                                                                                       .Set(c => c.TypeId, catalogObject.Type)
                                                                                       .Set(c => c.Properties, catalogObject.Properties));
        }

        public List<CatalogObjectMetadata> RetrieveAvailableCatalogObjects()
        {
            return catalogObjectCollection.AsQueryable().ToList().Select(co => new CatalogObjectMetadata(co)).ToList();
        }

        #endregion

        #region Materials

        public string CreateMaterial(Material material)
        {
            materialCollection.InsertOne(material);
            return material.Id;
        }

        public Material RetrieveMaterial(string id)
        {
            return materialCollection.Find(m => m.Id == id).Limit(1).FirstOrDefault();
        }

        public void UpdateMaterial(Material material)
        {
            materialCollection.ReplaceOne(m => m.Id == material.Id, material);
        }

        public void DeleteMaterial(string id)
        {
            materialCollection.DeleteOne(m => m.Id == id);
        }

        public List<Material> GetAllAvailableMaterials()
        {
            return materialCollection.Find(m => true).ToList();
        }

        #endregion
    }
}