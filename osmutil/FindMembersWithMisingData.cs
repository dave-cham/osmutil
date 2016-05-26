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
                foreach (var m in _service.GetMembers(s.sectionid, _service.GetLatestTermIdForSection(s.sectionid)).items)
                { 
                    var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                    var primaryContact1 = furtherDetails.data.First(fd => fd.identifier == "contact_primary_1");
                    var phoneNumberColumns = primaryContact1.columns.Where(col => col.varname == "phone1" || col.varname == "phone2");
                    var nameColumns = primaryContact1.columns.Where(col => col.varname == "firstname" || col.varname == "lastname");
                    var email1Column = primaryContact1.columns.First(col => col.varname == "email1");

                    if (email1Column.value.Length == 0)
                    {
                        Console.WriteLine($"!!!!!! {m.firstname} {m.lastname} ({s.sectionname}) {email1Column.label} is missing");
                    }

                    foreach (var col in nameColumns)
                    {
                        if (col.value.Length == 0)
                        {
                            Console.WriteLine($"{m.firstname} {m.lastname} ({s.sectionname}) {col.label} is missing");
                        }
                    }

                    foreach (var col in phoneNumberColumns)
                    {
                        if (col.value.Length == 0)
                        {
                            Console.WriteLine($"{m.firstname} {m.lastname} ({s.sectionname}) {col.label} is missing");
                        }
                        else if (col.value[0] != '0')
                        {
                            Console.WriteLine($"{m.firstname} {m.lastname} ({s.sectionname}) {col.label} does not start with a 0");
                        }
                    }
                }
            }
        }
    }
}
