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
    
    public partial class SalesInvoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SalesInvoice()
        {
            this.SaleInvoiceItem = new HashSet<SaleInvoiceItem>();
        }
    
        public long InvoiceId { get; set; }
        public string InvNumber { get; set; }
        public string InvType { get; set; }
        public Nullable<long> CustomerId { get; set; }
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
        public Nullable<long> InvoiceMainId { get; set; }
        public string Notes { get; set; }
        public Nullable<int> PosId { get; set; }
        public Nullable<byte> IsApproved { get; set; }
        public bool IsActive { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal RealShippingCost { get; set; }
        public Nullable<decimal> Remain { get; set; }
        public Nullable<int> ShippingCompanyId { get; set; }
        public Nullable<long> ShipUserId { get; set; }
        public bool IsPrePaid { get; set; }
        public bool IsShipPaid { get; set; }
        public bool IsFreeShip { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleInvoiceItem> SaleInvoiceItem { get; set; }
    }
}
