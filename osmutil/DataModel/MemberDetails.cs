using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osmutil.DataModel
{
    public class MemberDetails
    {
        public string identifier { get; set; }
        public bool photos { get; set; }
        public IEnumerable<MemberDetail> items { get; set; }
    }

    public class MemberDetail
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string patrolid { get; set; }
        public string patrol { get; set; }
        public string sectionid { get; set; }
        public DateTime? enddate { get; set; }
        public string age { get; set; }
        public string patrol_role_level_label { get; set; }
        public bool active { get; set; }
        public string scoutid { get; set; }
        public bool pic { get; set; }

    }
}
