using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osmutil.DataModel
{
    public class MemberDetails
    {
        public MemberDetailsData data { get; set; }
    }

    public class MemberDetailsData
    {
        public string scoutid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email1 { get; set; }
        public string email2 { get; set; }
        public string email3 { get; set; }
        public string email4 { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string phone3 { get; set; }
        public string phone4 { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public DateTime dob { get; set; }
        public DateTime started { get; set; }
        public string joining_in_yrs { get; set; }
        public string parents { get; set; }
        public string notes { get; set; }
        public string medical { get; set; }
        public string religion { get; set; }
        public string school { get; set; }
        public string ethnicity { get; set; }
        public string subs { get; set; }
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public string custom4 { get; set; }
        public string custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string lastupdated_time { get; set; }
        public string patrolid { get; set; }
        public string patrolleader { get; set; }
        public DateTime? startedsection { get; set; }
        public DateTime? enddate { get; set; }
        public string age { get; set; }
        public string pic { get; set; }
        public string meetings { get; set; }
    }
}
