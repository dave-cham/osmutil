﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace osmutil
{
    public class ReportEmailAddresses
    {
        private List<string> _sectionFilter;
        private Service _service;

        public ReportEmailAddresses(Service service, List<string> sectionFilter)
        {
            _service = service;
            _sectionFilter = sectionFilter;
        }

        public void DoIt()
        {
            foreach (var m in _service.GetAllMembersInAllSectionsForLatestTerm())
            {
                var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                if (_sectionFilter == null || _sectionFilter.Any(s => furtherDetails.meta.section_name.ToLower().Contains(s)))
                {
                    var primaryContacts = furtherDetails.data.Where(fd => fd.identifier == "contact_primary_1" || fd.identifier == "contact_primary_2");
                    var phoneNumberColumns = primaryContacts.SelectMany(c => c.columns.Where(col => col.varname == "email1" && !string.IsNullOrEmpty(col.value)));

                    foreach(var c in phoneNumberColumns)
                    {
                        Console.Write(c.value + ",");
                    }
                }
            }
        }       
    }
}
