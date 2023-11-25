using LinqKit;
using Newtonsoft.Json;
using POS_Server.Models;
using POS_Server.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/PurchaseInvoice")]
    public class PurchaseInvoiceController : ApiController
    {
        CountriesController countryc = new CountriesController();

        [HttpPost]
        [Route("savePurchaseDraft")]
        public async Task<string> savePurchaseDraft(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            string message = "";
            string result = "{";
            var strP = TokenManager.GetPrincipal(token);

            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                #region parameters
                int posId = 0;
                PurchaseInvoice newObject = null;
                PurInvoiceModel invoiceModel = null;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);

                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        invoiceModel = JsonConvert.DeserializeObject<PurInvoiceModel>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
                        newObject = JsonConvert.DeserializeObject<PurchaseInvoice>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
                    }
                    else if (c.Type == "posId")
                    {
                        posId = int.Parse(c.Value);
                    }

                }
                #endregion
                try
                {
                    //ProgramDetailsController pc = new ProgramDetailsController();
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {

                        if (newObject.InvoiceMainId == 0)
                            newObject.InvoiceMainId = null;

                        newObject =  SaveInvoice(newObject);

                        message = newObject.InvoiceId.ToString();
                        long InvoiceId = newObject.InvoiceId;
                        if (!InvoiceId.Equals(0))
                        {
                            //save items transfer
                          // saveWithSerials(transferObject, InvoiceId);
                        }
                    }
                }

                catch
                {
                    message = "failed";
                }
                result += "Result:" + message;
                string temp = System.Web.Helpers.Json.Encode(newObject.InvNumber).Substring(1, System.Web.Helpers.Json.Encode(newObject.InvNumber).Length - 2);
                result += ",Message:'" + temp + "'";
               // result += ",InvTime:'" + newObject.invTime + "'";
                result += ",UpdateDate:'" + DateTime.Parse(newObject.UpdateDate.ToString()).ToString() + "'";

                #region get purchase draft count
                List<string> invoiceType = new List<string>() { "pd", "pbd" };
                int draftCount = 0;
                if (!invoiceType.Contains(newObject.InvType))
                {
                    invoiceType = new List<string>() { "pod", "pos" };

                    if (!invoiceType.Contains(newObject.InvType))
                        invoiceType = new List<string>() { "isd" };
                }

                draftCount = getDraftCount((int)newObject.CreateUserId, invoiceType);

                result += ",PurchaseDraftCount:" + draftCount;
                #endregion

                #region return pos Balance
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var pos = entity.POS.Find(posId);
                    result += ",PosBalance:" + pos.Balance;
                }
                #endregion

                result += "}";
                return TokenManager.GenerateToken(result);

            }
        }

        private PurchaseInvoice SaveInvoice(PurchaseInvoice newObject)
        {
            string message = "";
            PurchaseInvoice tmpInvoice;
            #region generate InvNumber

            int BranchId = (int)newObject.BranchCreatorId;
            string InvNumber =  generateInvNumber(newObject.InvNumber, BranchId);

            #endregion
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var invoiceEntity = entity.Set<PurchaseInvoice>();

                if (newObject.InvoiceMainId == 0)
                    newObject.InvoiceMainId = null;
                if (newObject.InvoiceId == 0)
                {

                    newObject.InvDate = countryc.AddOffsetTodate(DateTime.Now);
                    //newObject.invTime = DateTime.Now.TimeOfDay.Add(countryc.offsetTime());
                    newObject.UpdateDate = countryc.AddOffsetTodate(DateTime.Now);
                    newObject.UpdateUserId = newObject.CreateUserId;
                    newObject.InvNumber = InvNumber;

                    tmpInvoice = invoiceEntity.Add(newObject);
                    entity.SaveChanges();
                    entity.Dispose();
                    message = tmpInvoice.InvoiceId.ToString();
                }
                else
                {
                    tmpInvoice = entity.PurchaseInvoice.Where(p => p.InvoiceId == newObject.InvoiceId).FirstOrDefault();
                    tmpInvoice.InvNumber = InvNumber;
                    tmpInvoice.SupplierId = newObject.SupplierId;
                    tmpInvoice.InvType = newObject.InvType;
                    tmpInvoice.Total = newObject.Total;
                    tmpInvoice.TotalNet = newObject.TotalNet;
                    tmpInvoice.Paid = newObject.Paid;
                    tmpInvoice.Deserved = newObject.Deserved;
                    tmpInvoice.DeservedDate = newObject.DeservedDate;
                    tmpInvoice.InvoiceMainId = newObject.InvoiceMainId;
                    tmpInvoice.Notes = newObject.Notes;

                    tmpInvoice.VendorInvNum = newObject.VendorInvNum;
                    tmpInvoice.VendorInvDate = newObject.VendorInvDate;
                    tmpInvoice.UpdateDate = countryc.AddOffsetTodate(DateTime.Now);
                    tmpInvoice.UpdateUserId = newObject.UpdateUserId;
                    tmpInvoice.BranchId = newObject.BranchId;
                    tmpInvoice.DiscountType = newObject.DiscountType;
                    tmpInvoice.DiscountValue = newObject.DiscountValue;
                    tmpInvoice.DiscountPercentage = newObject.DiscountPercentage;
                    tmpInvoice.Tax = newObject.Tax;
                    tmpInvoice.TaxType = newObject.TaxType;
                    tmpInvoice.TaxPercentage = newObject.TaxPercentage;
                    tmpInvoice.IsApproved = newObject.IsApproved;
                    tmpInvoice.BranchCreatorId = newObject.BranchCreatorId;

                    tmpInvoice.ShippingCost = newObject.ShippingCost;
                    entity.SaveChanges();
                    message = tmpInvoice.InvoiceId.ToString();
                }

            }
            return tmpInvoice;
        }

        public string generateInvNumber(string invoiceCode, int BranchId)
        {
            #region check if last of code is num
            var num = invoiceCode.Substring(invoiceCode.LastIndexOf("-") + 1);

            if (!num.Equals(invoiceCode))
                return invoiceCode;

            #endregion
            int sequence = 0;
            string branchCode = "";

            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var branch = entity.Branch.Find(BranchId);

                branchCode = branch.Code;

                var numberList = entity.PurchaseInvoice.Where(b => b.InvNumber.Contains(invoiceCode + "-") && b.BranchCreatorId == BranchId).Select(b => b.InvNumber).ToList();
                for (int i = 0; i < numberList.Count; i++)
                {
                    string code = numberList[i];
                    string s = code.Substring(code.LastIndexOf("-") + 1);

                    numberList[i] = s;
                }
                if (numberList.Count > 0)
                {
                    numberList.Sort();
                    try
                    {
                        sequence = int.Parse(numberList[numberList.Count - 1]);
                    }
                    catch
                    { sequence = 0; }
                }
            }
            sequence++;

            string strSeq = sequence.ToString();
            if (sequence <= 999999)
                strSeq = sequence.ToString().PadLeft(6, '0');
            string invoiceNum = invoiceCode + "-" + branchCode + "-" + strSeq;
            return invoiceNum;
        }

        private int getDraftCount(long CreateUserId, List<string> invoiceType)
        {

            int duration = 2;
            int draftCount = GetCountByCreator(invoiceType, duration, CreateUserId);
            return draftCount;
        }

        private int GetCountByCreator(List<string> invTypeL, int duration, long CreateUserId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var searchPredicate = PredicateBuilder.New<PurchaseInvoice>();

                if (duration > 0)
                {
                    DateTime dt = Convert.ToDateTime(DateTime.Today.AddDays(-duration).ToShortDateString());
                    searchPredicate = searchPredicate.And(inv => inv.UpdateDate >= dt);
                }
                searchPredicate = searchPredicate.And(inv => invTypeL.Contains(inv.InvType));
                searchPredicate = searchPredicate.And(inv => inv.CreateUserId == CreateUserId);
                searchPredicate = searchPredicate.And(inv => inv.IsActive == true);

                var invoicesCount =  entity.PurchaseInvoice.Where(searchPredicate)
                                     .ToList().Where(inv => inv.InvoiceId == entity.PurchaseInvoice.Where(i => i.InvNumber == inv.InvNumber).ToList().OrderBy(i => i.InvoiceId).FirstOrDefault().InvoiceId).ToList().Count;
                return invoicesCount;
            }
        }

        [HttpPost]
        [Route("GetInvoicesByCreator")]
        public async Task<string> GetInvoicesByCreator(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                #region params
                string invType = "";
                long createUserId = 0;
                int duration = 0;
                int hours = 0;
                List<string> invTypeL = new List<string>();

                CashTransferController cashTransferController = new CashTransferController();
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "invoiceType")
                    {
                        invType = c.Value;
                        string[] invTypeArray = invType.Split(',');
                        foreach (string s in invTypeArray)
                            invTypeL.Add(s.Trim());
                    }
                    else if (c.Type == "createUserId")
                    {
                        createUserId = long.Parse(c.Value);
                    }
                    else if (c.Type == "duration")
                    {
                        duration = int.Parse(c.Value);
                    }
                }
                #endregion
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var searchPredicate = PredicateBuilder.New<PurchaseInvoice>();

                        searchPredicate = searchPredicate.And(inv => invTypeL.Contains(inv.InvType));
                        searchPredicate = searchPredicate.And(inv => inv.CreateUserId == createUserId);
                        searchPredicate = searchPredicate.And(inv => inv.IsActive == true);

                        if (duration > 0)
                        {
                            DateTime dt = Convert.ToDateTime(DateTime.Today.AddDays(-duration).ToShortDateString());
                            searchPredicate = searchPredicate.And(inv => inv.UpdateDate >= dt);
                        }
                        if (hours > 0)
                        {
                            DateTime dt = Convert.ToDateTime(DateTime.Now.AddHours(-hours));
                            searchPredicate = searchPredicate.And(x => x.UpdateDate >= dt);
                        }


                        var invoicesList = (from b in entity.PurchaseInvoice.Where(searchPredicate)
                                            join l in entity.Branch on b.BranchId equals l.BranchId into lj
                                            join m in entity.Branch on b.BranchCreatorId equals m.BranchId into bj
                                            from x in lj.DefaultIfEmpty()
                                            from y in bj.DefaultIfEmpty()
                                            select new PurInvoiceModel()
                                            {
                                                InvoiceId = b.InvoiceId,
                                                InvNumber = b.InvNumber,
                                                SupplierId = b.SupplierId,
                                                SupplierName = b.Supplier.Name,
                                                InvType = b.InvType,
                                                Total = b.Total,
                                                TotalNet = b.TotalNet,
                                                Paid = b.Paid,
                                                Deserved = b.Deserved,
                                                DeservedDate = b.DeservedDate,
                                                InvDate = b.InvDate,
                                                InvoiceMainId = b.InvoiceMainId,
                                               Notes = b.Notes,
                                                VendorInvNum = b.VendorInvNum,
                                                VendorInvDate = b.VendorInvDate,
                                                CreateUserId = b.CreateUserId,
                                                UpdateDate = b.UpdateDate,
                                                UpdateUserId = b.UpdateUserId,
                                                BranchId = b.BranchId,
                                                DiscountValue = b.DiscountValue,
                                                DiscountType = b.DiscountType,
                                                DiscountPercentage = b.DiscountPercentage,
                                                Tax =(decimal) b.Tax,
                                                TaxType = b.TaxType,
                                                TaxPercentage = b.TaxPercentage,
                                                IsApproved = b.IsApproved,
                                                BranchName = x.Name,
                                                BranchCreatorId = b.BranchCreatorId,

                                                BranchCreatorName = y.Name,
                                                PosId = b.PosId,
                                                ShippingCost = b.ShippingCost,

                                            }).ToList();

                        if (invoicesList != null)
                        {
                            for (int i = 0; i < invoicesList.Count; i++)
                            {
                                long invoiceId = invoicesList[i].InvoiceId;
                                invoicesList[i].InvoiceItems = GetInvoiceItems(invoiceId);
                                invoicesList[i].cachTrans = cashTransferController.GetPayedByInvId(invoiceId);

                            }
                        }

                        return TokenManager.GenerateToken(invoicesList);
                    }
                }
                catch (Exception ex)
                {
                    return TokenManager.GenerateToken(ex.ToString());

                }
            }
        }

        public List<PurInvoiceItemModel> GetInvoiceItems(long invoiceId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var transferList = (from t in entity.PurInvoiceItem.Where(x => x.InvoiceId == invoiceId )
                                    select new PurInvoiceItemModel()
                                    {
                                        InvItemId = t.InvItemId,
                                        ItemId = t.ItemUnit.ItemId,
                                        ItemName = t.ItemUnit.Item.Name,
                                        Quantity = t.Quantity,

                                        CreateUserId = t.CreateUserId,
                                        UpdateUserId = t.UpdateUserId,
                                        Notes = t.Notes,
                                        CreateDate = t.CreateDate,
                                        UpdateDate = t.UpdateDate,
                                        ItemUnitId = t.ItemUnitId,
                                        Price = t.Price,
                                        UnitName = t.ItemUnit.Unit.Name,
                                        UnitId = t.ItemUnit.UnitId,
                                        Barcode = t.ItemUnit.Barcode,
                                        ItemType = t.ItemUnit.Item.Type,
                                        //PackageItems = (from S in entity.packages
                                        //                join IU in entity.itemsUnits on S.childIUId equals IU.itemUnitId
                                        //                join I in entity.items on IU.itemId equals I.itemId
                                        //                where S.parentIUId == u.itemUnitId
                                        //                select new ItemModel()
                                        //                {
                                        //                    isActive = S.isActive,
                                        //                    name = I.name,
                                        //                    type = I.type,
                                        //                    unitName = IU.units.name,
                                        //                    itemCount = S.quantity,
                                        //                    itemUnitId = IU.itemUnitId,
                                        //                }).ToList(),
                                    })
                                    .ToList();

                return transferList;
            }

        }
    }
}