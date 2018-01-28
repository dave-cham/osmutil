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
        Dictionary<GroupSection, int> _memberCount = new Dictionary<GroupSection, int>();
        Dictionary<GroupSection, int> _femaleCount = new Dictionary<GroupSection, int>();
        Dictionary<GroupSection, Dictionary<int, int>> _ageCount = new Dictionary<GroupSection, Dictionary<int, int>>();

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
                _memberCount.Add(s, 0);
                _femaleCount.Add(s, 0);
                _ageCount.Add(s, new Dictionary<int, int>());
                foreach (var m in _service.GetMembers(s.sectionid, _service.GetLatestTermIdForSection(s.sectionid)).items)
                {
                    if (m.patrol == "Leaders") continue;

                    _memberCount[s]++;
                    var furtherDetails = _service.GetFurtherDetails(m.sectionid, m.scoutid);
                    var gender = furtherDetails.data.Single(_ => _.identifier == "floating").columns.Single(_ => _.label == "Gender").value;
                    var age = int.Parse(m.age.Split(' ')[0]);

                    if (gender == "Female")
                    {
                        _femaleCount[s]++;
                    }

                    Dictionary<int, int> ageLookup = _ageCount[s];
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
            _feedback($"{section}:", true);

            foreach (var s in _memberCount.Keys.Where(s => s.section == section))
            {
                _feedback($"Members of {s.sectionname}: {_memberCount[s]}", true);
                _feedback($"Female memers of {s.sectionname}: {_femaleCount[s]}", true);
                _feedback("Ages:", true);
                foreach (var dic in _ageCount[s].OrderBy(k => k.Key))
                {
                    _feedback($"{dic.Key} : {dic.Value}", true);
                }
            }
        }
    }
}
