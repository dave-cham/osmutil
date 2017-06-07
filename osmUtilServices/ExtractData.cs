using osmutil.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace osmutil
{
    public class ExtractData : IOperation
    {
        private List<string> _sectionFilter;
        private Service _service;

        public ExtractData(Service service, List<string> sectionFilter)
        {
            _service = service;
            _sectionFilter = sectionFilter;
        }

        public void DoIt(Action<string, bool> feedback, bool dryRun)
        {
            foreach (var s in _service.GetRequiredSections(_sectionFilter))
            {
                feedback("------------------------------------------------------", true);
                feedback("", true);
                feedback("Section : " + s.sectionname + ":", true);
                feedback("", true);

                foreach (var m in _service.GetMembers(s.sectionid, _service.GetLatestTermIdForSection(s.sectionid)).items)
                {
                    feedback("Child: "+ m.firstname + " " + m.lastname, true);
                    var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                    var primaryContact1 = furtherDetails.data.Where(fd => fd.identifier == "contact_primary_1");
                    var phoneNumberColumns1 = primaryContact1.SelectMany(c => c.columns.Where(col => col.varname == "phone1" || col.varname == "phone2" && !string.IsNullOrEmpty(col.value)));
                    if (phoneNumberColumns1.Any())
                    {
                        feedback(primaryContact1.First().columns.First(c => c.varname == "firstname").value, false);
                        feedback(" " + primaryContact1.First().columns.First(c => c.varname == "lastname").value + ": ", false);
                        bool first = true;
                        foreach (var c in phoneNumberColumns1)
                        {
                            if (!first) feedback(", ", false);
                            feedback(c.value, false);
                            first = false;
                        }
                        feedback("", true);
                    }

                    var primaryContact2 = furtherDetails.data.Where(fd => fd.identifier == "contact_primary_2");
                    var phoneNumberColumns2 = primaryContact2.SelectMany(c => c.columns.Where(col => col.varname == "phone1" || col.varname == "phone2" && !string.IsNullOrEmpty(col.value)));
                    if (phoneNumberColumns2.Any())
                    {
                        feedback(primaryContact2.First().columns.First(c => c.varname == "firstname").value, false);
                        feedback(" " + primaryContact2.First().columns.First(c => c.varname == "lastname").value + ": ", false);
                        bool first = true;
                        foreach (var c in phoneNumberColumns2)
                        {
                            if (!first) feedback(", ", false);
                            feedback(c.value, false);
                            first = false;
                        }
                        feedback("", true);
                    }
                    feedback("", true);
                }
            }
        }
    }
}
