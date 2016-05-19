using osmutil.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osmutil
{
    public class Service
    {
        private Authorisation _auth;
        private bool _dryRun;

        public Service(bool dryRun = true)
        {
            _dryRun = dryRun;
        }

        public void Authorise()
        {
            _auth = new Authorisation("", ""); // FILL IN
        }

        public Dictionary<string, Term[]> GetTerms()
        {
            return QueryHelpers.QueryServer<Dictionary<string, Term[]>>("api.php?action=getTerms", null, _auth);
        }

        public Dictionary<string, Term> GetSectionsAndCurrentTerms()
        {
            return GetTerms()
                .Select(kv => new KeyValuePair<string, Term>(kv.Key, kv.Value
                    .OrderByDescending(t => t.startdate)
                    .First(t => t.past)))
                    .ToDictionary(k => k.Key, v => v.Value);
        }

        public Members GetMembers(string sectionId, string termId)
        {
            return QueryHelpers.QueryServer<Members>(QueryHelpers.FormUrl("/ext/members/contact/?action=getListOfMembers", new[]
                { QueryHelpers.NewPair("sectionid",sectionId),
                QueryHelpers.NewPair("termid", termId),
                QueryHelpers.NewPair("sort","dob"),
                QueryHelpers.NewPair("section", "cubs") }), null, _auth);
        }

        public IEnumerable<Member> GetAllMembersInAllSectionsForLatestTerm()
        {
            return GetSectionsAndCurrentTerms()
                .Select(term => GetMembers(term.Value.sectionid, term.Value.termid))
                .SelectMany(m => m.items);
        }

        public object GetMemberDetails(string sectionId, string termId, string scoutId)
        {
            return QueryHelpers.QueryServer(QueryHelpers.FormUrl("/ext/members/contact/?action=getIndividual", new[]
                { QueryHelpers.NewPair("sectionid",sectionId),
                QueryHelpers.NewPair("termid", termId),
                QueryHelpers.NewPair("scoutid",scoutId),
                QueryHelpers.NewPair("_section_id", sectionId),
                QueryHelpers.NewPair("context", "members")}), null, _auth);
        }

        public MemberCustomData GetFurtherDetails(string sectionId, string scoutId)
        {
            return QueryHelpers.QueryServer<MemberCustomData>(QueryHelpers.FormUrl("/ext/customdata/?action=getData", new[]
                { QueryHelpers.NewPair("section_id",sectionId) }), new[]
                { QueryHelpers.NewPair("associated_id",scoutId),
                  QueryHelpers.NewPair("associated_type","member"),
                  QueryHelpers.NewPair("context", "members")
                }, _auth);
        }

        public void UpdateMemberCustomData(Member member, MemberCustomDataBlock block)
        {
            Console.WriteLine($"Updating data for for block {block.identifier} for {member.firstname} {member.lastname}");

            var dataColumns = block.columns
                .Where(c => !c.varname.Contains("last_updated"))
                .Select(c => QueryHelpers.NewPair($"data[{c.varname}]", c.value));

            if (!_dryRun)
            {
                var ret = QueryHelpers.QueryServer(QueryHelpers.FormUrl("/ext/customdata/?action=update", new[]
                    { QueryHelpers.NewPair("section_id",member.sectionid) }), dataColumns.Concat(new[]
                    { QueryHelpers.NewPair("associated_id",member.scoutid),
                  QueryHelpers.NewPair("associated_type","member"),
                  QueryHelpers.NewPair("context", "members"),
                  QueryHelpers.NewPair("group_id", block.group_id)
                    }), _auth);

                // TODO: Test Return value.
            }
        }
    }
}
