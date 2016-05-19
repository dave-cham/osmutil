using System;
using System.Linq;

namespace osmutil
{
    public class FindMembersWithMisingData
    {
        private Service _service;

        public FindMembersWithMisingData()
        {
            _service = new Service();
            _service.Authorise();
        }

        public void DoIt()
        {
            foreach (var m in _service.GetAllMembersInAllSectionsForLatestTerm())
            {
                var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                var primaryContact1 = furtherDetails.data.First(fd => fd.identifier == "contact_primary_1");
                var phoneNumberColumns = primaryContact1.columns.Where(col => col.varname == "phone1" || col.varname == "phone2");
                var nameColumns = primaryContact1.columns.Where(col => col.varname == "firstname" || col.varname == "lastname");
                var email1Column = primaryContact1.columns.First(col => col.varname == "email1");

                if (email1Column.value.Length == 0)
                {
                    Console.WriteLine($"{m.firstname} {m.lastname} {email1Column.label} is missing");
                }

                foreach (var col in nameColumns)
                {
                    if (col.value.Length == 0)
                    {
                        Console.WriteLine($"{m.firstname} {m.lastname} {col.label} is missing");
                    }
                }

                foreach (var col in phoneNumberColumns)
                {
                    if (col.value.Length == 0)
                    {
                        Console.WriteLine($"{m.firstname} {m.lastname} {col.label} is missing");
                    }
                    else if (col.value[0] != '0')
                    {
                        Console.WriteLine($"{m.firstname} {m.lastname} {col.label} does not start with a 0");
                    }
                }
            }
        }
    }
}
