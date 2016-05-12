using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace osmutil
{
    public enum Operation
    {
        Post,
        Get
    }

    public static class Helpers
    {
        public static object QueryServer(string requeststring, IEnumerable<KeyValuePair<string, string>> data, Authorisation auth, Operation operation)
        {
            var rawData = QueryServerRaw(requeststring, data, auth, operation);
            return JsonConvert.DeserializeObject(rawData);
        }

        public static T QueryServer<T>(string requeststring, IEnumerable<KeyValuePair<string, string>> data, Authorisation auth, Operation operation)
        {
            var rawData = QueryServerRaw(requeststring, data, auth, operation);
            return JsonConvert.DeserializeObject<T>(rawData);
        }

        private static string QueryServerRaw(string requeststring, IEnumerable<KeyValuePair<string, string>> data, Authorisation auth, Operation operation)
        {
            var authData = auth == null ? new KeyValuePair<string, string>[] { } : new[]
            {
               NewPair("userid", auth.Data.userid),
               NewPair("secret", auth.Data.secret)
            };

            var apiTokenData = new[]
            {
                NewPair("token", "e6c1ea1ff1609e2375c28794627ca8de"),
                NewPair("apiid", "181")
            };

            var queryData = data ?? new KeyValuePair<string, string>[] { };

            var allQueryData = queryData.Concat(apiTokenData).Concat(authData);

            var payload = string.Join("", allQueryData.Select(d => $"&{d.Key}={Uri.EscapeDataString(d.Value)}"));
            var getData = operation == Operation.Get ? payload : "";
            var postData = Encoding.UTF8.GetBytes(payload.Substring(1)); //Cuts the first & off, as per Ed's code

            var request = WebRequest.Create($"https://www.onlinescoutmanager.co.uk/{requeststring}{getData}");
            request.Method = operation == Operation.Get ? "GET" : "POST";
            request.Timeout = 2000; //2 second timeout
            request.ContentType = "application/x-www-form-urlencoded";

            Have a look to make sure the headers are the same.
            if (operation == Operation.Post)
            {
                request.ContentLength = postData.Length;
                var requeststream = request.GetRequestStream();
                requeststream.Write(postData, 0, postData.Length);
                requeststream.Close();
            }

            var response = request.GetResponse();

            var responsestream = response.GetResponseStream();
            var reader = new StreamReader(responsestream);
            var retval = reader.ReadToEnd();
            reader.Close();
            responsestream.Close();
            response.Close();
            return retval;
        }

        public static KeyValuePair<string, string> NewPair(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
