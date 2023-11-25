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
    }
}