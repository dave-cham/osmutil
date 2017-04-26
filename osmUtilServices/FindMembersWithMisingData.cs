using osmutil.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public string DoIt()
        {
            var ret=new StringBuilder();
            foreach (var s in _service.GetRequiredSections(_sectionFilter))
            {
                foreach (var m in _service.GetMembers(s.sectionid, _service.GetLatestTermIdForSection(s.sectionid)).items)
                {
                    var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                    ret.AppendLine(CheckForMissingData(m, s, furtherDetails, "contact_primary_1", "phone1", "firstname", "lastname", "email1", "email1_leaders"));
                    ret.AppendLine(CheckForMissingData(m, s, furtherDetails, "contact_primary_2", "phone1", "firstname", "lastname", "email1", "email1_leaders"));
                    ret.AppendLine(CheckForMissingData(m, s, furtherDetails, "customisable_data", "cf_special_needs", "cf_ethnic_origin", "cf_religion", "cf_nationality"));
                    ret.AppendLine(CheckForMissingData(m, s, furtherDetails, "floating", "gender"));
                }
            }
            return ret.ToString();
        }

        private string CheckForMissingData(Member m, GroupSection s, MemberCustomData data, string blockName, params string[] columns)
        {
            var ret = new StringBuilder();
            var block = data.ExtractDataBlock(blockName);
            foreach (var colName in columns)
            {
                var col = block.ExtractColumn(colName);
                if (col == null)
                {
                    ret.AppendLine($"{m.firstname} {m.lastname} ({s.sectionname}) {block.name}-{colName} COLUMN is missing");
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
                                ret.AppendLine($"{m.firstname} {m.lastname} ({s.sectionname}) {block.name}-{col.label} is missing");
                            }
                            break;
                        case "checkbox":
                            if (col.value != "yes")
                            {
                                ret.AppendLine($"{m.firstname} {m.lastname} ({s.sectionname}) {block.name}-{col.label} is not checked");
                            }
                            break;
                        default:
                            ret.AppendLine($"{col.type} is not known in columns {block.name}-{col.label}");
                            break;
                    }
                }
            }
            return ret.ToString();
        }
    }
}
