using System;
using System.Collections.Generic;

namespace osmutil
{
    public class Authorisation
    {
        public AuthData Data { get; }
        public Authorisation(string username, string password)
        {
            Data = Helpers.QueryServer<AuthData>("users.php?action=authorise", new[]
            {
                new KeyValuePair<string,string>("email", username),
                new KeyValuePair<string,string>("password", password)
            }, null, Operation.Post);

            if (Data.secret == null)
            {
                throw new ApplicationException("Can't log on");
            }
        }
    }
}
