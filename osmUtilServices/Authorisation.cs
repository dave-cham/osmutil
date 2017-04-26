using System;
using System.Collections.Generic;

namespace osmutil
{
    public class Authorisation
    {
        public AuthData Data { get; }
        public Authorisation(string username, string password)
        {
            Data = QueryHelpers.QueryServer<AuthData>("users.php?action=authorise", new[]
            {
                QueryHelpers.NewPair("email", username),
                QueryHelpers.NewPair("password", password)
            }, null);

            if (Data.secret == null)
            {
                throw new ApplicationException("Can't log on");
            }
        }
    }
}
