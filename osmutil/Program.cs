using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace osmutil
{
    class Program
    {
        static uint apiid = x; // Get this from Ed
        static string token = "xxx"; // Get this from Ed

        static uint userid = x; // You'll get this programmatically from authorising
        static string secret = "xxx";// You'll get this programmatically from authorising

        static uint termid = x;
        static uint sectionid = x;

        static string basestring = "https://www.onlinescoutmanager.co.uk/";


        static void Main(string[] args)
        {
            
        }

        private static string authorise(string email, string password)
        {
            List<datakeypair> datakeypairs = new List<datakeypair>();
            datakeypairs.Add(new datakeypair("password", password));
            datakeypairs.Add(new datakeypair("email", email));

            return perform_query("users.php?action=authorise", datakeypairs, true);
        }

        public static string perform_query(string requeststring, List<datakeypair> datakeypairs, bool POST)
        {
            //global $apiid, $token, $base, $myEmail, $myPassword, $userid, $secret;
            datakeypairs.Add(new datakeypair("token", token));
            datakeypairs.Add(new datakeypair("apiid", apiid.ToString()));

            if (userid > 0)
            {
                datakeypairs.Add(new datakeypair("userid", userid.ToString()));
            }

            if (secret.Length == 32)
            {
                datakeypairs.Add(new datakeypair("secret", secret));
            }

            string data = "";
            foreach (datakeypair dkp in datakeypairs)
            {
                data += "&" + dkp.key + "=" + Uri.EscapeDataString(dkp.data);
            }

            WebRequest request = WebRequest.Create(basestring + requeststring);
            request.Method = "POST";
            request.Timeout = 2000; //2 second timeout
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] databytearray = Encoding.UTF8.GetBytes(data.Substring(1)); //Cuts the first & off, as per Ed's code
            request.ContentLength = databytearray.Length;
            Stream requeststream = request.GetRequestStream();
            requeststream.Write(databytearray, 0, databytearray.Length);
            requeststream.Close();

            WebResponse response = request.GetResponse();
            this.Text = ((HttpWebResponse)response).StatusDescription; //Use the title bar for http responses :)

            Stream responsestream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responsestream);
            string retval = reader.ReadToEnd();
            reader.Close();
            responsestream.Close();
            response.Close();
            return retval;
        }

        private void getsecret()
        {
            string test = authorise();
            StreamWriter output = new StreamWriter("D:\\osm.txt"); //Output the userid and secret to file in raw JSON
            output.Write(test);
            output.Close();
        }
    }

    public struct datakeypair
    {
        public string data;
        public string key;
        public datakeypair(string key, string data)
        {
            this.data = data;
            this.key = key;
        }
    }
}
