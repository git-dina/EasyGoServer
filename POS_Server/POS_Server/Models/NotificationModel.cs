using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class NotificationModel
    {
        public long NotId { get; set; }
        public string Title { get; set; }
        public string Ncontent { get; set; }
        public string MsgType { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public string ObjectName { get; set; }
        public string Prefix { get; set; }

        public Nullable<int> RecieveId { get; set; }
        public Nullable<int> BranchId { get; set; }
    }
}