using osmutil.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osmutil
{
    public class Service
    {
        private Authorisation _auth;
        private Dictionary<string, Term[]> _terms;
        private GroupSection[] _sections;

        public Service(string userName, string password)
        {
            _auth = new Authorisation(userName, password);
            _terms = GetTerms();
            _sections = GetSections();
        }

        private Dictionary<string, Term[]> GetTerms()
        {
            return QueryHelpers.QueryServer<Dictionary<string, Term[]>>("api.php?action=getTerms", null, _auth);
        }

        private GroupSection[] GetSections()
        {
            return QueryHelpers.QueryServer<GroupSection[]>("api.php?action=getUserRoles", null, _auth);
        }

        public GroupSection[] Sections => _sections;

        public IEnumerable<GroupSection> GetRequiredSections(List<string> sectionFilter)
        {
            return _sections.Where(s => sectionFilter==null || sectionFilter.Any(sf => s.sectionname.Contains(sf)));
        }

        public string GetLatestTermIdForSection(string sectionId)
        {
            Term[] terms;
            if (!_terms.TryGetValue(sectionId, out terms))
                return "-1";

            var today = DateTime.UtcNow.Date;
            var term =  terms.FirstOrDefault(t => t.startdate <= today && t.enddate >= today)?.termid;

            if(term==null)
            {
                term = terms.Aggregate((i, j) => i.startdate > j.startdate ? i : j)?.termid;
            }

            return term;
        }

        public Members GetMembers(string sectionId, string termId)
        {
            return QueryHelpers.QueryServer<Members>(QueryHelpers.FormUrl("/ext/members/contact/?action=getListOfMembers", new[]
                { QueryHelpers.NewPair("sectionid",sectionId),
                QueryHelpers.NewPair("termid", termId),
                QueryHelpers.NewPair("sort","dob"),
                QueryHelpers.NewPair("section", _sections.Single(s=>s.sectionid==sectionId).section) }), null, _auth);
        }

        public MemberDetails GetMemberDetails(string sectionId, string termId, string scoutId)
        {
            return QueryHelpers.QueryServer<MemberDetails>(QueryHelpers.FormUrl("/ext/members/contact/?action=getIndividual", new[]
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

        public string UpdateMemberCustomData(Member member, MemberCustomDataBlock block, bool dryRun)
        {
            var dataColumns = block.columns
                .Where(c => !c.varname.Contains("last_updated"))
                .Select(c => QueryHelpers.NewPair($"data[{c.varname}]", c.value));

            if (!dryRun)
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
            return $"Updating data for for block {block.identifier} for {member.firstname} {member.lastname}";
        }
    }
}
