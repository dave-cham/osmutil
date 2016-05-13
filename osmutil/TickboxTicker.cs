using System;
using System.Collections.Generic;
using System.Linq;
using osmutil.DataModel;

namespace osmutil
{
    public class TickboxTicker
    {
        private readonly Authorisation _auth;
        public TickboxTicker()
        {
            _auth = new Authorisation("", ""); // FILL IN
        }

        public void DoIt()
        {
            var count = 0;
            var membersAndTickboxStatus = GetSectionsAndCurrentTerms()
                .Select(term => GetMembers(term.Value.sectionid, term.Value.termid))
                .SelectMany(m => m.items)
                .Select(m1 =>
                {
                    var furtherDetails = GetFurtherDetails(m1.sectionid, m1.scoutid);
                    var primaryContacts = furtherDetails.data.Where(fd => fd.identifier.Contains("contact_primary"));
                    var dataBlockAndColumns = primaryContacts.Select(pc => new { block = pc, cols = pc.columns.Where(col => col.varname.Contains("_leaders")) });

                    count++;
                    Console.WriteLine($"Checking details for {m1.firstname} {m1.lastname} from {furtherDetails.meta.section_name}");

                    return new
                    {
                        member = m1,
                        dataBlockAndColumns = dataBlockAndColumns
                    };
                }).ToList();

            Console.WriteLine($"I make that a total of {count} members");
            var columnsNeedingTicking = membersAndTickboxStatus
                .SelectMany(m => m.dataBlockAndColumns
                    .Where(bc => bc.block.identifier == "contact_primary_1" || bc.block.identifier == "contact_primary_2")
                    .Select(bc => new { b = bc.block, c = bc.cols.Where(c => c.value != "yes") })
                    .Where(x => x.c.Count() > 0)
                    .Select(x => new { member = m.member, col = x.c, block = x.b }));

            foreach (var x in columnsNeedingTicking)
            {
                foreach(var col in x.col)
                {
                    col.value = "yes";
                }
                UpdateMemberCustomData(x.member, x.block);
            }
        }

        private Dictionary<string, Term[]> GetTerms()
        {
            return Helpers.QueryServer<Dictionary<string, Term[]>>("api.php?action=getTerms", null, _auth);
        }

        private Dictionary<string, Term> GetSectionsAndCurrentTerms()
        {
            return GetTerms()
                .Select(kv => new KeyValuePair<string, Term>(kv.Key, kv.Value
                    .OrderByDescending(t => t.startdate)
                    .First(t => t.past)))
                    .ToDictionary(k => k.Key, v => v.Value);
        }

        private Members GetMembers(string sectionId, string termId)
        {
            var returnData = Helpers.QueryServer<Members>(Helpers.FormUrl("/ext/members/contact/?action=getListOfMembers", new[]
                { Helpers.NewPair("sectionid",sectionId),
                Helpers.NewPair("termid", termId),
                Helpers.NewPair("sort","dob"),
                Helpers.NewPair("section", "cubs") }), null, _auth);

            Console.WriteLine($"Section {sectionId} contains {returnData.items.Count()} members.");
            return returnData;
        }

        private object GetMemberDetails(string sectionId, string termId, string scoutId)
        {
            return Helpers.QueryServer(Helpers.FormUrl("/ext/members/contact/?action=getIndividual", new[]
                { Helpers.NewPair("sectionid",sectionId),
                Helpers.NewPair("termid", termId),
                Helpers.NewPair("scoutid",scoutId),
                Helpers.NewPair("_section_id", sectionId),
                Helpers.NewPair("context", "members")}), null, _auth);
        }

        private MemberCustomData GetFurtherDetails(string sectionId, string scoutId)
        {
            return Helpers.QueryServer<MemberCustomData>(Helpers.FormUrl("/ext/customdata/?action=getData", new[]
                { Helpers.NewPair("section_id",sectionId) }), new[]
                { Helpers.NewPair("associated_id",scoutId),
                  Helpers.NewPair("associated_type","member"),
                  Helpers.NewPair("context", "members")
                }, _auth);
        }

        private void UpdateMemberCustomData(Member member, MemberCustomDataBlock block)
        {
            Console.WriteLine($"Updating data for for block {block.identifier} for {member.firstname} {member.lastname}");

            var dataColumns = block.columns
                .Where(c => !c.varname.Contains("last_updated"))
                .Select(c => Helpers.NewPair($"data[{c.varname}]", c.value));

            var ret = Helpers.QueryServer(Helpers.FormUrl("/ext/customdata/?action=update", new[]
                { Helpers.NewPair("section_id",member.sectionid) }), dataColumns.Concat(new[]
                { Helpers.NewPair("associated_id",member.scoutid),
                  Helpers.NewPair("associated_type","member"),
                  Helpers.NewPair("context", "members"),
                  Helpers.NewPair("group_id", block.group_id)
                }), _auth);
        }
    }
}
