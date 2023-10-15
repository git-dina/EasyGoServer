using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class setValuesModel
    {
        public int ValId { get; set; }
        public string Value { get; set; }
        public Nullable<int> IsDefault { get; set; }
        public Nullable<int> IsSystem { get; set; }
        public string Notes { get; set; }
        public Nullable<int> SettingId { get; set; }
        //setting
        public string Name { get; set; }
    }
}