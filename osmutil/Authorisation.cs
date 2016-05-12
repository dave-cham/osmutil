using System;
using System.Collections.Generic;

namespace osmutil
{
    public class Authorisation
    {
        public class AuthRetData
        {
            public string secret { get; set; }
            public string userid { get; set; }
        }

        public AuthRetData AuthData { get; }
        public Authorisation(string username, string password)
        {
            AuthData = Helpers.QueryServer<AuthRetData>("users.php?action=authorise", new[]
                {
                new KeyValuePair<string,string>("email", username),
                new KeyValuePair<string,string>("password", password)
            }, null);
        }
    }
}
