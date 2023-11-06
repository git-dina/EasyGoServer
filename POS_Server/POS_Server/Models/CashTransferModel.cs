using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class CashTransferModel
    {
        public long CashTransId { get; set; }
        public string TransType { get; set; }
        public Nullable<int> PosId { get; set; }
        public Nullable<long> UserId { get; set; }
        public Nullable<long> AgentId { get; set; }
        public Nullable<long> InvId { get; set; }
        public string TransNum { get; set; }
        public Nullable<decimal> Cash { get; set; }
        public string Notes { get; set; }
        public Nullable<int> PosIdCreator { get; set; }
        public Nullable<byte> IsConfirm { get; set; }
        public Nullable<int> CashTransIdSource { get; set; }
        public string Side { get; set; }
        public string DocNum { get; set; }
        public Nullable<int> BankId { get; set; }
        public string ProcessType { get; set; }
        public Nullable<int> CardId { get; set; }
        public Nullable<int> ShippingCompanyId { get; set; }
        public Nullable<decimal> CommissionValue { get; set; }
        public Nullable<decimal> CommissionRatio { get; set; }
        public int IsCommissionPaid { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<decimal> Deserved { get; set; }
        public string Purpose { get; set; }
        public bool IsInvPurpose { get; set; }
        public string OtherSide { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<long> CreateUserId { get; set; }

        //extra
        public string BankName { get; set; }
        public string AgentName { get; set; }
        public string PosName { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
        public string CreateUserLName { get; set; }
        public string CardName { get; set; }
        public string InvNumber { get; set; }
    }
}