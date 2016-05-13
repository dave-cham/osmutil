using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osmutil.DataModel;

namespace osmutil
{
    public class TickboxTicker
    {
        private readonly Authorisation _auth;
        public TickboxTicker()
        {
            _auth = new Authorisation("", ""); here
        }

        public void DoIt()
        {
            var terms = Helpers.QueryServer<Dictionary<string, Term[]>>("api.php?action=getTerms", null, _auth, Operation.Post);

            // Find the latest term for each section
            var sectionsWithTerms = terms.Select(kv => new KeyValuePair<string,Term>(kv.Key, kv.Value.OrderByDescending(t => t.startdate).First(t => t.past))).ToDictionary(k => k.Key, v => v.Value);

            var first = sectionsWithTerms.First().Value;
            var members = Helpers.QueryServer<MemberDetails>(Helpers.FormUrl("/ext/members/contact/?action=getListOfMembers", new[]
                { Helpers.NewPair("sectionid",first.sectionid),
                Helpers.NewPair("termid", first.termid),
                Helpers.NewPair("sort","dob"),
                Helpers.NewPair("section", "cubs") }), null, _auth, Operation.Post);


            Console.ReadLine();

            now get details.
            //https://www.onlinescoutmanager.co.uk/ext/members/contact/?action=getIndividual&sectionid=12207&scoutid=187162&termid=26443&_section_id=12207&context=members
        }
    }
}
