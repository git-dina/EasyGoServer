﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class ItemUnitModel
    {
        public long ItemUnitId { get; set; }
        public Nullable<long> ItemId { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<int> SubUnitId { get; set; }
        public Nullable<int> UnitValue { get; set; }
        public Nullable<bool> IsDefaultSale { get; set; }
        public Nullable<bool> IsDefaultPurchase { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Barcode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> CreateUserId { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<decimal> purchasePrice { get; set; }
        //extra
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string UnitName { get; set; }
        //public Nullable<int> storageCostId { get; set; }


       // public List<itemsPropModel> ItemProperties { get; set; }
        public string ItemType { get; set; }
       // public Nullable<long> BranchId { get; set; }

        public Nullable<long> Quantity { get; set; }
        public Nullable<long> SerialsCount { get; set; }
        public Nullable<long> PropertiesCount { get; set; }

    }
}