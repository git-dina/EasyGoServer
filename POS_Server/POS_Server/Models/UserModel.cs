using System;

namespace POS_Server.Models
{
    public class UserModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? CreateUserId { get; set; }
        public long? UpdateUserId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public bool? IsOnline { get; set; }
        public Boolean canDelete { get; set; }
        public string Image { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<byte> BalanceType { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public int? canLogin { get; set; }
        public int? branchId { get; set; }
        public long? userLogInID { get; set; }
        //public Nullable<int> groupId { get; set; }

       // public string groupName { get; set; }
        public bool HasCommission { get; set; }
        public Nullable<decimal> CommissionValue { get; set; }
        public Nullable<decimal> CommissionRatio { get; set; }
        public long? RoleId { get; set; }
    }

    public class UserSettings
    {
        public string userLang { get; set; }
        public string UserMenu { get; set; }
        public string firstPath { get; set; }
        public string secondPath { get; set; }
        public int? firstPathId { get; set; }
        public int? secondPathId { get; set; }
        public string messageContent { get; set; }
        public string messageTitle { get; set; }
        public int invoiceSlice { get; set; }

        //general info
        public string accuracy { get; set; }
        public string dateFormat { get; set; }
        public string Currency { get; set; }
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        //default system info

        public string companyName { get; set; }
        public string Address { get; set; }

        public string com_name_ar { get; set; }
        public string com_address_ar { get; set; }

        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string logoImage { get; set; }
        public string com_website { get; set; }
        public string com_social { get; set; }
        public string com_social_icon { get; set; }


        public decimal StorageCost { get; set; }
        // for print
        public string sale_copy_count { get; set; }
        public string pur_copy_count { get; set; }
        public string print_on_save_sale { get; set; }
        public string print_on_save_pur { get; set; }
        public string email_on_save_sale { get; set; }
        public string email_on_save_pur { get; set; }
        public string rep_print_count { get; set; }
        public string Allow_print_inv_count { get; set; }
        public string show_header { get; set; }
        public string itemtax_note { get; set; }
        public string sales_invoice_note { get; set; }
        public string print_on_save_directentry { get; set; }
        public string directentry_copy_count { get; set; }
        public string rep_printer_name { get; set; }
        public string sale_printer_name { get; set; }
        public string salePaperSize { get; set; }
        public string docPapersize { get; set; }
        public string Reportlang { get; set; }
        public string invoice_lang { get; set; }
        //tax
        public bool? invoiceTax_bool { get; set; }
        public decimal? invoiceTax_decimal { get; set; }
        public bool? itemsTax_bool { get; set; }
        public decimal? itemsTax_decimal { get; set; }

        //support
        public string activationSite { get; set; }
        public bool canSkipProperties { get; set; }
        public bool canSkipSerialsNum { get; set; }
        public int returnPeriod { get; set; }
        public bool freeDelivery { get; set; }
    }
}