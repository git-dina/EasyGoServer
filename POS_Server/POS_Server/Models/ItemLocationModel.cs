using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class ItemLocationModel
    {
        public long ItemLocId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public long Quantity { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<long> ItemUnitId { get; set; }
        public string Notes { get; set; }

        //extra
        public string ItemName { get; set; }
        public string LocationName { get; set; }
        public string SectionName { get; set; }
        public string UnitName { get; set; }
        public string ItemType { get; set; }
        public bool IsFreeZone { get; set; }
        public bool IsExpired { get; set; }
    }
}