using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class UsersLogsModel
    {

        public long LogId { get; set; }
        public Nullable<System.DateTime> SInDate { get; set; }
        public Nullable<System.DateTime> SOutDate { get; set; }
        public Nullable<long> PosId { get; set; }
        public Nullable<long> UserId { get; set; }
        public bool canDelete { get; set; }
    }
}