using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbmsApi.API
{
    public class NewUser
    {
        public string Username;
        public string PublicName;
        public string Password;

        public NewUser(string username, string publicName, string password)
        {
            Username = username;
            PublicName = publicName;
            Password = password;
        }
    }
}