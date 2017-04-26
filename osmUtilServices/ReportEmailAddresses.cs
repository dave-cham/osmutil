using System;
using System.Collections.Generic;
using System.Linq;

namespace osmutil
{
    public class ReportEmailAddresses
    {
        private List<string> _sectionFilter;
        private Service _service;
        private bool _loggedIn;

        public ReportEmailAddresses(Service service, List<string> sectionFilter)
        {
            _service = service;
            _sectionFilter = sectionFilter;
        }

        public string DoIt()
        {
            string ret = "";
            foreach(var s in _service.GetRequiredSections(_sectionFilter))
            {
                foreach (var m in _service.GetMembers(s.sectionid, _service.GetLatestTermIdForSection(s.sectionid)).items)
                {
                    var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                    var primaryContacts = furtherDetails.data.Where(fd => fd.identifier == "contact_primary_1" || fd.identifier == "contact_primary_2");
                    var phoneNumberColumns = primaryContacts.SelectMany(c => c.columns.Where(col => col.varname == "email1" && !string.IsNullOrEmpty(col.value)));

                    foreach(var c in phoneNumberColumns)
                    {
                        ret += c.value + ",";
                    }
                }
            }
            return ret;
        }       
    }
}
