﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class PurInvoiceModel
    {

        public long InvoiceId { get; set; }
        public string InvNumber { get; set; }
        public string InvType { get; set; }
        public Nullable<long> SupplierId { get; set; }
        public string DiscountType { get; set; }
        public Nullable<decimal> DiscountValue { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> TotalNet { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<decimal> Deserved { get; set; }
        public Nullable<System.DateTime> DeservedDate { get; set; }
        public Nullable<int> BranchCreatorId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public string TaxType { get; set; }
        public decimal TaxPercentage { get; set; }
        public Nullable<System.DateTime> InvDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<int> InvoiceMainId { get; set; }
        public string Notes { get; set; }
        public string VendorInvNum { get; set; }
        public Nullable<System.DateTime> VendorInvDate { get; set; }
        public Nullable<int> PosId { get; set; }
        public Nullable<byte> IsApproved { get; set; }
        public decimal ManualDiscountValue { get; set; }
        public string ManualDiscountType { get; set; }
        public bool IsActive { get; set; }
        public decimal ShippingCost { get; set; }

        //extra
        public string SupplierName { get; set; }
        public List<PurInvoiceItemModel> InvoiceItems { get; set; }
    }

    public class PurInvoiceItemModel
    {
        public int InvItemId { get; set; }
        public Nullable<long> InvoiceId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<long> ItemUnitId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public  ItemUnitModel ItemUnit { get; set; }
    }
}