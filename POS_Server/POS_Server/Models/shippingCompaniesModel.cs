﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class ShippingCompaniesModel
    {
        public int shippingCompanyId { get; set; }
        public string name { get; set; }
        public Nullable<decimal> RealDeliveryCost { get; set; }
        public Nullable<decimal> deliveryCost { get; set; }
        public string deliveryType { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<byte> isActive { get; set; }
        public bool canDelete { get; set; }
        public decimal balance { get; set; }
        public Nullable<byte> balanceType { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string fax { get; set; }
        public string address { get; set; }
    }
}