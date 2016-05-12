using System;

public class Authorisation
{
	public Authorisation()
	{
	}

    public bool Authorise(string username, string password)
    {
        var ret = perform_query("users.php?action=authorise", new[] { new KeyValuePair("email", username), new KeyValuePair("password", password) }, true);
        JsonConvert.Deserialize();
    }
}
