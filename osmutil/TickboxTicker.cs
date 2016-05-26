using System;
using System.Collections.Generic;
using System.Linq;

namespace osmutil
{
    public class TickboxTicker
    {
        private List<string> _sectionFilter;
        private Service _service;

        public TickboxTicker(Service service, List<string> sectionFilter)
        {
            _service = service;
            _sectionFilter = sectionFilter;
        }

        public void DoIt()
        {
            var count = 0;
            var membersAndTickboxStatus = _service.GetRequiredSections(_sectionFilter)
                .SelectMany(section => _service.GetMembers(section.sectionid, _service.GetLatestTermIdForSection(section.sectionid)).items)
                .Select(m =>
                {
                    var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                    var primaryContacts = furtherDetails.data.Where(fd => fd.identifier == "contact_primary_1" || fd.identifier == "contact_primary_2");
                    var dataBlockAndTickboxColumns = primaryContacts.Select(pc => new { block = pc, cols = pc.columns.Where(col => col.varname.Contains("_leaders")) });

                    count++;
                    Console.WriteLine($"Checking details for {m.firstname} {m.lastname} from {furtherDetails.meta.section_name}");

                    return new
                    {
                        member = m,
                        dataBlockAndTickboxColumns = dataBlockAndTickboxColumns
                    };
                }).ToList();

            Console.WriteLine($"I make that a total of {count} members");
            var columnsNeedingTicking = membersAndTickboxStatus
                .SelectMany(m => m.dataBlockAndTickboxColumns
                    .Select(bc => new { b = bc.block, c = bc.cols.Where(c => c.value != "yes") })
                    .Where(x => x.c.Count() > 0)
                    .Select(x => new { member = m.member, col = x.c, block = x.b }));

            foreach (var x in columnsNeedingTicking)
            {
                foreach(var col in x.col)
                {
                    col.value = "yes";
                }
                _service.UpdateMemberCustomData(x.member, x.block);
            }
        }
    }
}
