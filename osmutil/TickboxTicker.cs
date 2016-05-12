using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osmutil
{
    public class Term
    {
        public string termid { get; set; }
        public string sectionid { get; set; }
        public string name { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string master_term { get; set; }
        public string past { get; set; }
    }
    public class TickboxTicker
    {
        private readonly Authorisation _auth;
        public TickboxTicker()
        {
            _auth = new Authorisation("", "");
        }

        public void DoIt()
        {
            var terms = Helpers.QueryServer<Dictionary<string, Term[]>>("api.php?action=getTerms", null, _auth);

            // Find the latest term for each section
            //var sectionsWithTerms = terms.TermArray.GroupBy(t => t.sectionid).Select(g => g.OrderBy(t => DateTime.Parse(t.startdate)).First(t => t.past == "True"));

            var first = terms.First().Value[0];
            var members = Helpers.QueryServer("ext/members/contact/?action=getListOfMembers", new[]
                { Helpers.NewPair("sectionid",first.sectionid),
                Helpers.NewPair("termid", first.termid),
                Helpers.NewPair("sort","dob"),
                Helpers.NewPair("section", "scouts") }, _auth);


            Console.ReadLine();
            //Helpers.QueryServer("ext/members/contact/?action=getListOfMembers&sort=dob&sectionid=8762&termid=140818&section=cubs")
        }
    }
}
