using System;

namespace osmutil
{
    public class Term
    {
        public string termid { get; set; }
        public string sectionid { get; set; }
        public string name { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string master_term { get; set; }
        public bool past { get; set; }
    }
}