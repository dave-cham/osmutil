using osmutil.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osmutil
{
    public class FindMembersWithMisingData
    {
        private List<string> _sectionFilter;
        private Service _service;

        public FindMembersWithMisingData(Service service, List<string> sectionFilter)
        {
            _service = service;
            _sectionFilter = sectionFilter;
        }

        public void DoIt()
        {
            foreach (var s in _service.GetRequiredSections(_sectionFilter))
            {
                if (s.sectionname == "Adults")
                {
                    Console.WriteLine("Skipping adults section");
                }
                else
                {
                    foreach (var m in _service.GetMembers(s.sectionid, _service.GetLatestTermIdForSection(s.sectionid)).items)
                    {
                        var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                        CheckForMissingData(m, s, furtherDetails, "contact_primary_1", "phone1", "firstname", "lastname", "email1", "email1_leaders");
                        CheckForMissingData(m, s, furtherDetails, "contact_primary_2", "phone1", "firstname", "lastname", "email1", "email1_leaders");
                        CheckForMissingData(m, s, furtherDetails, "customisable_data", "cf_special_needs", "cf_ethnic_origin", "cf_religion", "cf_nationality");
                        CheckForMissingData(m, s, furtherDetails, "floating", "gender");
                    }
                }
            }
        }

        private void CheckForMissingData(Member m, GroupSection s, MemberCustomData data, string blockName, params string[] columns)
        {
            var block = data.ExtractDataBlock(blockName);
            foreach (var colName in columns)
            {
                var col = block.ExtractColumn(colName);
                if (col == null)
                {
                    Console.WriteLine($"{m.firstname} {m.lastname} ({s.sectionname}) {block.name}-{colName} COLUMN is missing");
                }
                else
                {
                    switch (col.type)
                    {
                        case "text":
                        case "select":
                        case "email":
                            if (col.value.Length == 0)
                            {
                                Console.WriteLine($"{m.firstname} {m.lastname} ({s.sectionname}) {block.name}-{col.label} is missing");
                            }
                            break;
                        case "checkbox":
                            if (col.value != "yes")
                            {
                                Console.WriteLine($"{m.firstname} {m.lastname} ({s.sectionname}) {block.name}-{col.label} is not checked");
                            }
                            break;
                        default:
                            Console.WriteLine($"{col.type} is not known in columns {block.name}-{col.label}");
                            break;
                    }
                }
            }
        }
    }
}
