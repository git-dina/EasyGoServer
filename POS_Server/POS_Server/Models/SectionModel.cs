﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class SectionModel
    {
        public int SectionId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> IsFreeZone { get; set; }
    }
}