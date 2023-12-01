using POS_Server.Classes;
using POS_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/CashTransfer")]
    public class CashTransferController : ApiController
    {
        CountriesController cc = new CountriesController();

        [NonAction]
        public List<PayedInvClass> GetPayedByInvId(long invId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                List<CashTransferModel> cachtrans = new List<CashTransferModel>();
                cachtrans = (from C in entity.CashTransfer
                             join b in entity.Card on C.CardId equals b.CardId into jb

                             from Card in jb.DefaultIfEmpty()
                             select new CashTransferModel()
                             {
                                 CashTransId = C.CashTransId,
                                 TransType = C.TransType,
                                 InvId = C.InvId,
                                 Cash = C.Cash,
                                 DocNum = C.DocNum,
                                 ProcessType = C.ProcessType,
                                 CardId = C.CardId,
                                 CommissionRatio = C.CommissionRatio,
                                 CommissionValue = C.CommissionValue,
                                 CardName = C.ProcessType == "card" ? Card.Name : C.ProcessType,
                                // HasProcessNum = Card.hasProcessNum,
                             }).Where(C => C.InvId == invId && (C.ProcessType == "card" || C.ProcessType == "cash" || C.ProcessType == "admin")).ToList();

                int i = 0;
                List<PayedInvClass> Payedlist = new List<PayedInvClass>();
                //cash
                List<PayedInvClass> cachprocess = cachtrans.GroupBy(x => x.ProcessType).Select(x => new PayedInvClass
                {
                    ProcessType = x.FirstOrDefault().ProcessType,

                    Cash = x.Sum(c => c.Cash),
                    CardId = x.FirstOrDefault().CardId == null ? 0 : x.FirstOrDefault().CardId,
                    CardName = x.FirstOrDefault().CardName,
                    Sequenc = x.FirstOrDefault().ProcessType == "cash" ? 0 : ++i,
                    CommissionRatio = x.FirstOrDefault().CommissionRatio,
                    CommissionValue = x.FirstOrDefault().CommissionValue,
                    DocNum = x.FirstOrDefault().DocNum,
                }).OrderBy(c => c.Sequenc).ToList().Where(x => x.ProcessType == "cash").ToList();
                //admin
                List<PayedInvClass> adminprocess = cachtrans.GroupBy(x => x.ProcessType).Select(x => new PayedInvClass
                {
                    ProcessType = x.FirstOrDefault().ProcessType,

                    Cash = x.Sum(c => c.Cash),
                    CardId = x.FirstOrDefault().CardId == null ? 0 : x.FirstOrDefault().CardId,
                    CardName = x.FirstOrDefault().CardName,
                    Sequenc = x.FirstOrDefault().ProcessType == "cash" ? 0 : ++i,
                    CommissionRatio = x.FirstOrDefault().CommissionRatio,
                    CommissionValue = x.FirstOrDefault().CommissionValue,
                    DocNum = x.FirstOrDefault().DocNum,
                }).OrderBy(c => c.Sequenc).ToList().Where(x => x.ProcessType == "admin").ToList();

                //all card
                List<PayedInvClass> cardprocess = cachtrans.Select(x => new PayedInvClass
                {
                    ProcessType = x.ProcessType,
                    Cash = x.Cash,
                    CardId = x.CardId,
                    CardName = x.CardName,
                    Sequenc = x.ProcessType == "cash" ? 0 : ++i,
                    CommissionRatio = x.CommissionRatio,
                    CommissionValue = x.CommissionValue,
                    DocNum = x.DocNum,
                }).OrderBy(c => c.CardId).ToList().Where(x => x.ProcessType == "card").ToList();
                //card has process num -no group
                List<PayedInvClass> cardhasprocessnum = cardprocess.Where(x => x.ProcessType == "card" && (x.DocNum != "" && x.DocNum != null)).ToList();
                // card has No Process num - group
                List<PayedInvClass> cardhasNoprocessnum = cardprocess.Where(x => x.ProcessType == "card" && (x.DocNum == "" || x.DocNum == null)).ToList();
                //group
                List<PayedInvClass> NoprocessnumGroup = cardhasNoprocessnum.GroupBy(x => x.CardId).Select(x => new PayedInvClass
                {
                    ProcessType = x.FirstOrDefault().ProcessType,
                    Cash = x.Sum(c => c.Cash),
                    CardId = x.FirstOrDefault().CardId,
                    CardName = x.FirstOrDefault().CardName,
                    Sequenc = x.FirstOrDefault().Sequenc,
                    CommissionRatio = x.FirstOrDefault().CommissionRatio,
                    CommissionValue = x.FirstOrDefault().CommissionValue,
                    DocNum = x.FirstOrDefault().DocNum,
                }).OrderBy(c => c.CardId).ToList();
                //add cash row
                if (cachprocess.Count() > 0)
                {
                    Payedlist.Add(cachprocess.FirstOrDefault());
                }
                if (adminprocess.Count > 0)
                {
                    Payedlist.AddRange(adminprocess);
                }
                //add card with process num
                Payedlist.AddRange(cardhasprocessnum);
                //add card no process num
                Payedlist.AddRange(NoprocessnumGroup);

                return Payedlist;
            }
        }

        public string addCashTransfer(CashTransfer newObject)
        {
            string message = "";
            if (newObject.UpdateUserId == 0 || newObject.UpdateUserId == null)
            {
                Nullable<long> id = null;
                newObject.UpdateUserId = id;
            }
            if (newObject.CreateUserId == 0 || newObject.CreateUserId == null)
            {
                Nullable<long> id = null;
                newObject.CreateUserId = id;
            }

            if (newObject.PosIdCreator == 0 || newObject.PosIdCreator == null)
            {
                Nullable<int> id = null;
                newObject.PosIdCreator = id;
            }


            CashTransfer cashtr;
            DateTime datenow = cc.AddOffsetTodate(DateTime.Now);
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var cEntity = entity.Set<CashTransfer>();
                if (newObject.CashTransId == 0)
                {

                    if (newObject.ProcessType == "statement")
                    {
                        DateTime DT = new DateTime(datenow.Year, 1, 1, 0, 0, 0);
                        newObject.CreateDate = DT;
                        newObject.UpdateDate = DT;
                    }
                    else
                    {
                        newObject.CreateDate = datenow;
                        newObject.UpdateDate = datenow;
                    }

                    newObject.TransNum = generateCashNumber(newObject.TransNum);

                    newObject.UpdateUserId = newObject.CreateUserId;
                    cashtr = cEntity.Add(newObject);
                }
                else
                {
                    cashtr = entity.CashTransfer.Where(p => p.CashTransId == newObject.CashTransId).First();
                    cashtr.TransType = newObject.TransType;
                    cashtr.PosId = newObject.PosId;
                    cashtr.UserId = newObject.UserId;
                    cashtr.AgentId = newObject.AgentId;
                    cashtr.InvId = newObject.InvId;
                    cashtr.TransNum = newObject.TransNum;
                    cashtr.CreateDate = newObject.CreateDate;
                    cashtr.UpdateDate = datenow;// server current date
                    cashtr.Cash = newObject.Cash;
                    cashtr.UpdateUserId = newObject.UpdateUserId;
                    cashtr.Notes = newObject.Notes;
                    cashtr.PosIdCreator = newObject.PosIdCreator;
                    cashtr.IsConfirm = newObject.IsConfirm;
                    cashtr.CashTransIdSource = newObject.CashTransIdSource;
                    cashtr.Side = newObject.Side;

                   // cashtr.DocName = newObject.DocName;
                    cashtr.DocNum = newObject.DocNum;
                    //cashtr.DocImage = newObject.DocImage;
                    cashtr.BankId = newObject.BankId;

                    cashtr.ProcessType = newObject.ProcessType;
                    cashtr.CardId = newObject.CardId;
                    //cashtr.BondId = newObject.BondId;
                    cashtr.ShippingCompanyId = newObject.ShippingCompanyId;
                    cashtr.OtherSide = newObject.OtherSide;
                }
                entity.SaveChanges();
            }
            message = cashtr.CashTransId.ToString();
            return message;
        }

        public string generateCashNumber(string cashCode)
        {
            #region check if last of code is num
            string num = cashCode.Substring(cashCode.LastIndexOf("-") + 1);

            if (!num.Equals(cashCode))
                return cashCode;

            #endregion

            List<string> numberList;
            int sequence = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                numberList = entity.CashTransfer.Where(b => b.TransNum.Contains(cashCode + "-")).Select(b => b.TransNum).ToList();

                for (int i = 0; i < numberList.Count; i++)
                {
                    string code = numberList[i];
                    string s = code.Substring(code.LastIndexOf("-") + 1);
                    numberList[i] = s;
                }
                if (numberList.Count > 0)
                {
                    numberList.Sort();
                    sequence = int.Parse(numberList[numberList.Count - 1]);
                }
            }
            sequence++;

            string strSeq = sequence.ToString();
            if (sequence <= 999999)
                strSeq = sequence.ToString().PadLeft(6, '0');
            string transNum = cashCode + "-" + strSeq;
            return transNum;
        }

        [NonAction]
        public async void AddCardCommission(CashTransfer basicCash)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var card = entity.Card.Find(basicCash.CardId);

                decimal commissionValue = 0;
                if (card.CommissionValue > 0)
                    commissionValue += (decimal)card.CommissionValue;
                if (card.CommissionValue > 0)
                {
                    Calculate calc = new Calculate();
                    commissionValue += calc.percentValue(basicCash.Cash, card.CommissionRatio);
                }

                if (commissionValue > 0)
                {
                    var cashTransfer = new CashTransfer()
                    {
                        InvId = basicCash.InvId,
                        CashTransIdSource = basicCash.CashTransId,
                       CardId = basicCash.CardId,
                        PosId = basicCash.PosId,
                        Cash = commissionValue,
                        Deserved = 0,
                        Paid = commissionValue,
                        ProcessType = "commissionCard",
                        TransType = "d",
                        Side = "card",
                        IsCommissionPaid = 1,
                        CommissionValue = card.CommissionValue,
                        CommissionRatio = card.CommissionRatio,
                        TransNum = generateCashNumber("d" + "u"),
                        CreateUserId = basicCash.CreateUserId,
                        UpdateUserId = basicCash.CreateUserId,
                        CreateDate = cc.AddOffsetTodate(DateTime.Now),
                        UpdateDate = cc.AddOffsetTodate(DateTime.Now),
                    };

                    entity.CashTransfer.Add(cashTransfer);
                    entity.SaveChanges();
                }

            }
        }

    }
}