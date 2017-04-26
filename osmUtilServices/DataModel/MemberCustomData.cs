using System.Collections.Generic;
using System.Linq;

namespace osmutil.DataModel
{
    public class MemberCustomData
    {
        public bool status { get; set; }
        public string error { get; set; }
        public IEnumerable<MemberCustomDataBlock> data { get; set; }
        public MemberMetaData meta { get; set; }

        public MemberCustomDataBlock ExtractDataBlock(string blockName)
        {
            return data.First(fd => fd.identifier == blockName);
        }
    }

    public class MemberCustomDataBlock
    {
        public string group_id { get; set; }
        public string group_type { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string is_considered_core { get; set; }
        public string allow_new_columns { get; set; }
        public string display { get; set; }
        public IEnumerable<ColumnDesc> columns { get; set; }

        public ColumnDesc ExtractColumn(string name)
        {
            return columns.FirstOrDefault(col => col.varname == name);
        }
    }

    public class ColumnDesc
    {
        public string column_id { get; set; }
        public string type { get; set; }
        public string required { get; set; }
        public string display_in_advanced_view { get; set; }
        public string display_if_empty { get; set; }
        public string hide_from_group_display { get; set; }
        public string varname { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public string is_core { get; set; }
        public int order { get; set; }
        public string force_read_only { get; set; }
        public string special_permissions { get; set; }
        public string orig_label { get; set; }
    }

    public class MemberMetaData
    {
        public string group_name { get; set; }
        public string section_name { get; set; }
    }
}
