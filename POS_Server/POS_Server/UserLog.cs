//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POS_Server
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserLog
    {
        public long LogId { get; set; }
        public Nullable<System.DateTime> SInDate { get; set; }
        public Nullable<System.DateTime> SOutDate { get; set; }
        public Nullable<long> PosId { get; set; }
        public Nullable<long> UserId { get; set; }
    
        public virtual User User { get; set; }
    }
}
