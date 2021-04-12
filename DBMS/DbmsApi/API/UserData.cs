using DbmsApi.Mongo;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DbmsApi.API
{
    public class UserData
    {
        public string Username;
        public string PublicName;
        public bool IsAdmin;
        public List<string> AccessibleModels;
        public List<string> OwnedModels;
        public Properties Properties;

        [JsonConstructor]
        public UserData() { }
        public UserData(User user)
        {
            this.Username = user.Username;
            this.PublicName = user.PublicName;
            this.IsAdmin = user.IsAdmin;
            this.AccessibleModels = user.AccessibleModels;
            this.OwnedModels = user.OwnedModels;
            this.Properties = user.Properties;
        }
    }
}