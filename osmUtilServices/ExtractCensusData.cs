using osmutil.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace osmutil
{
    public class ExtractCensusData : IOperation
    {
        private List<string> _sectionFilter;
        private Service _service;
        private Action<string, bool> _feedback;
        Dictionary<string, int> _memberCount = new Dictionary<string, int> { { "beavers", 0 }, { "cubs", 0 }, { "scouts", 0 } };
        Dictionary<string, int> _femaleCount = new Dictionary<string, int> { { "beavers", 0 }, { "cubs", 0 }, { "scouts", 0 } };
        Dictionary<string, Dictionary<int, int>> _ageCount = new Dictionary<string, Dictionary<int, int>> { { "beavers", new Dictionary<int, int>() }, { "cubs", new Dictionary<int, int>() }, { "scouts", new Dictionary<int, int>() } };

        public ExtractCensusData(Service service, List<string> sectionFilter)
        {
            _service = service;
            _sectionFilter = sectionFilter;
        }

        public void DoIt(Action<string, bool> feedback, bool dryRun)
        {
            _feedback = feedback;
            
            foreach (var s in _service.GetRequiredSections(_sectionFilter))
            {
                foreach (var m in _service.GetMembers(s.sectionid, _service.GetLatestTermIdForSection(s.sectionid)).items)
                {
                    if (m.patrol == "Leaders") continue;

                    _memberCount[s.section]++;
                    var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                    var gender = furtherDetails.data.Single(_ => _.identifier == "floating").columns.Single(_ => _.label == "Gender").value;
                    var age = int.Parse(m.age.Split(' ')[0]);

                    if (gender == "Female")
                    {
                        _femaleCount[s.section]++;
                    }

                    Dictionary<int, int> ageLookup = _ageCount[s.section];
                    int currentAgeCount;
                    if(!ageLookup.TryGetValue(age, out currentAgeCount))
                    {
                        currentAgeCount = 0;
                    }
                    ageLookup[age] = currentAgeCount + 1;
                }
            }

            Report("beavers");
            Report("cubs");
            Report("scouts");
        }

        void Report(string section)
        {
            _feedback($"Number of {section}: {_memberCount[section]}", true);
            _feedback($"Number of female {section}: {_femaleCount[section]}", true);
            _feedback("Ages:", true);
            foreach (var dic in _ageCount[section].OrderBy(k => k.Key))
            {
                _feedback($"{dic.Key} : {dic.Value}", true);
            }            
        }
    }
}
