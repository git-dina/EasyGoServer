using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class ItemModel
    {
        public long ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Min { get; set; }
        public Nullable<int> Max { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public bool IsExpired { get; set; }
        public Nullable<decimal> Taxes { get; set; }
        public Nullable<int> MinUnitId { get; set; }
        public Nullable<int> MaxUnitId { get; set; }
        public Nullable<decimal> AvgPurchasePrice { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public string CategoryName { get; set; }
        public List<ItemUnitModel> ItemUnits { get; set; }


        public Nullable<int> itemCount { get; set; }

        // new units and offers an is new
        //units
        public Nullable<int> UnitId { get; set; }
        public string UnitName { get; set; }
        public Nullable<decimal> Price { get; set; }
        //offer
        public Nullable<decimal> DesPrice { get; set; }
        public Nullable<int> IsNew { get; set; }
        public Nullable<int> IsOffer { get; set; }
        public string OfferName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public byte? IsActiveOffer { get; set; }
        public Nullable<int> ItemUnitId { get; set; }
        public Nullable<int> OfferId { get; set; }
        public Nullable<decimal> PriceTax { get; set; }

        public string ParentName { get; set; }
        public string MinUnitName { get; set; }
        public string MaxUnitName { get; set; }
        public bool CanUpdate { get; set; }




    }
}