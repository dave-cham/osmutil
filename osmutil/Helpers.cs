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
        public static object QueryServer(string requeststring, IEnumerable<KeyValuePair<string, string>> data, Authorisation auth)
        {
            var rawData = QueryServerRaw(requeststring, data, auth);
            return JsonConvert.DeserializeObject(rawData);
        }

        public static T QueryServer<T>(string requeststring, IEnumerable<KeyValuePair<string, string>> data, Authorisation auth)
        {
            var rawData = QueryServerRaw(requeststring, data, auth);
            return JsonConvert.DeserializeObject<T>(rawData);
        }

        private static string QueryServerRaw(string requeststring, IEnumerable<KeyValuePair<string, string>> data, Authorisation auth)
        {
            var authData = auth == null ? new KeyValuePair<string, string>[] { } : new[]
            {
               NewPair("userid", auth.AuthData.userid),
               NewPair("secret", auth.AuthData.secret)
            };

            var queryData = data ?? new KeyValuePair<string, string>[] { };

            var allQueryData = authData.Concat(queryData.Concat(new[]
            {
                NewPair("token", ""), //here
                NewPair("apiid", "181")
            }));

            var postData = string.Join("", allQueryData.Select(d => $"&{d.Key}={Uri.EscapeDataString(d.Value)}"));

            WebRequest request = WebRequest.Create($"https://www.onlinescoutmanager.co.uk/{requeststring}");
            request.Method = "POST";
            request.Timeout = 2000; //2 second timeout
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] databytearray = Encoding.UTF8.GetBytes(postData.Substring(1)); //Cuts the first & off, as per Ed's code
            request.ContentLength = databytearray.Length;
            Stream requeststream = request.GetRequestStream();
            requeststream.Write(databytearray, 0, databytearray.Length);
            requeststream.Close();

            WebResponse response = request.GetResponse();
            //this.Text = ((HttpWebResponse)response).StatusDescription; //Use the title bar for http responses :)

            Stream responsestream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responsestream);
            string retval = reader.ReadToEnd();
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
