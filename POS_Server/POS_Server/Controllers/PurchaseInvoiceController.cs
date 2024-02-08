using LinqKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
                          saveInvoiceItems(invoiceModel.InvoiceItems, InvoiceId);
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


        [HttpPost]
        [Route("savePurchaseInvoice")]
        public string savePurchaseInvoice(string token)
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
                string amountNotStr = "";
                //string waitNotStr = "";
                //string Object = "";
                int posId = 0;

                PurchaseInvoice newObject = null;
                PurInvoiceModel invoiceModel = null;
                NotificationUserModel amountNot = null;
                //NotificationUserModel waitNotUser = null;
               // Notification waitNot = null;
                List<PurInvoiceItem> transferObject = new List<PurInvoiceItem>();
                CashTransfer PosCashTransfer = null;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);

                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        newObject = JsonConvert.DeserializeObject<PurchaseInvoice>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
                        invoiceModel = JsonConvert.DeserializeObject<PurInvoiceModel>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
                    }
                    else if (c.Type == "amountNot")
                    {
                        amountNotStr = c.Value;
                        amountNot = JsonConvert.DeserializeObject<NotificationUserModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    }
                    //else if (c.Type == "waitNot")
                    //{
                    //    waitNotStr = c.Value.Replace("\\", string.Empty);
                    //    waitNotStr = waitNotStr.Trim('"');
                    //    waitNotUser = JsonConvert.DeserializeObject<NotificationUserModel>(waitNotStr, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    //    waitNot = JsonConvert.DeserializeObject<Notification>(waitNotStr, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    //}
                    else if (c.Type == "PosCashTransfer")
                    {
                        PosCashTransfer = JsonConvert.DeserializeObject<CashTransfer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    }
                    else if (c.Type == "posId")
                    {
                        posId = int.Parse(c.Value);
                    }

                }
                #endregion
                try
                {
                   // ProgramDetailsController pc = new ProgramDetailsController();
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {

                        #region check pos balance
                        var pos = entity.POS.Find(posId);
                        foreach (var c in invoiceModel.ListPayments)
                        {
                            if (c.ProcessType == "cash" && pos.Balance < c.Cash)
                            {
                                message = "lowBalance";
                                result += "Result:" + message;
                                result += "}";
                                return TokenManager.GenerateToken(result);

                            }
                        }
                        #endregion

                        newObject =  SaveInvoice(newObject);

                        long invoiceId = newObject.InvoiceId;

                        newObject.UpdateDate = newObject.UpdateDate;
                        message = newObject.InvoiceId.ToString();
                        newObject.InvoiceId = invoiceId;
                        if (!invoiceId.Equals(0))
                        {
                            //save items transfer
                            saveInvoiceItems(invoiceModel.InvoiceItems, invoiceId);

                            #region enter items to store and notification

                            if (newObject.BranchCreatorId.Equals(newObject.BranchId))
                            {
                                ItemLocationController ilc = new ItemLocationController();
                                ilc.receiptInvoice((int)newObject.BranchId, invoiceModel.InvoiceItems, (long)newObject.UpdateUserId, amountNot.objectName, amountNotStr);

                                saveAvgPrice(invoiceModel.InvoiceItems);
                            }
                            //else
                            //{
                            //    NotificationController nc = new NotificationController();
                            //    nc.save(waitNot, waitNotUser.objectName, waitNotUser.prefix, (int)waitNotUser.branchId);
                            //}
                            #endregion

                            #region save payments

                            #region save pos cash transfer
                            CashTransferController cc = new CashTransferController();

                            PosCashTransfer.InvId = invoiceId;
                            cc.addCashTransfer(PosCashTransfer);
                            #endregion

                            var inv = entity.PurchaseInvoice.Find(invoiceId);

                            foreach (var item in invoiceModel.ListPayments)
                            {
                                item.InvId = invoiceId;
                                savePurchaseCash(newObject, item, posId);
                            }

                            #endregion
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
               // result += ",InvTime:'" + newObject.InvTime + "'";
                result += ",UpdateDate:'" + DateTime.Parse(newObject.UpdateDate.ToString()).ToShortDateString() + "'";
                #region get purchase draft count
                List<string> invoiceType = new List<string>() { "pd ", "pbd" };
                int draftCount = getDraftCount((long)newObject.UpdateUserId, invoiceType);

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

        [HttpPost]
        [Route("savePurchaseBounce")]
        public async Task<string> savePurchaseBounce(string token)
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
              //  string Object = "";
                int posId = 0;
                int branchId = 0;
                PurchaseInvoice newObject = null;
                PurInvoiceModel invoiceModel = null;
                NotificationUserModel notificationUser = null;
                Notification notification = null;
              //  List<itemsTransfer> transferObject = new List<itemsTransfer>();
               // List<ItemTransferModel> billDetails = new List<ItemTransferModel>();
               // List<cashTransfer> listPayments = new List<cashTransfer>();
                CashTransfer PosCashTransfer = new CashTransfer();

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);

                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        newObject = JsonConvert.DeserializeObject<PurchaseInvoice>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
                        invoiceModel = JsonConvert.DeserializeObject<PurInvoiceModel>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
                    }
                    //else if (c.Type == "itemTransferObject")
                    //{
                    //    Object = c.Value.Replace("\\", string.Empty);
                    //    Object = Object.Trim('"');
                    //    transferObject = JsonConvert.DeserializeObject<List<itemsTransfer>>(Object, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    //    billDetails = JsonConvert.DeserializeObject<List<ItemTransferModel>>(Object, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    //}
                    //else if (c.Type == "listPayments")
                    //{
                    //    Object = c.Value.Replace("\\", string.Empty);
                    //    Object = Object.Trim('"');
                    //    listPayments = JsonConvert.DeserializeObject<List<cashTransfer>>(Object, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    //}
                    else if (c.Type == "posCashTransfer")
                    {
                        PosCashTransfer = JsonConvert.DeserializeObject<CashTransfer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    }
                    else if (c.Type == "notification")
                    {
                        notificationUser = JsonConvert.DeserializeObject<NotificationUserModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        notification = JsonConvert.DeserializeObject<Notification>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    }
                    else if (c.Type == "posId")
                    {
                        posId = int.Parse(c.Value);
                    }
                    else if (c.Type == "branchId")
                    {
                        branchId = int.Parse(c.Value);
                    }
                }
                #endregion
                try
                {
                   // ProgramDetailsController pc = new ProgramDetailsController();
                   // ItemsTransferController it = new ItemsTransferController();
                    ItemUnitController iuc = new ItemUnitController();

                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        #region caculate available amount in basic invoice
                        //get purchase invoice   
                      //  var mainInvId = entity.PurchaseInvoice.Where(i => i.InvoiceId == newObject.InvoiceMainId).FirstOrDefault().InvoiceId;

                        //purchase invoice items
                        var mainInvoiceItems = GetInvoiceItems((long)newObject.InvoiceMainId);

                        var returnedItems = invoiceModel.InvoiceItems.Select(x => x.ItemId).Distinct().ToList();
                        foreach (var item in returnedItems)
                        {
                            var returnedItemUnits = invoiceModel.InvoiceItems.Where(x => x.ItemId == item).Select(x => new { x.ItemUnitId, x.Quantity }).ToList();
                            var saledItemUnits = mainInvoiceItems.Where(x => x.ItemId == item).ToList();
                            int returnedQuantity = 0;
                            int purchasedQuantity = 0;

                            foreach (var itemUnit in returnedItemUnits)
                            {
                                int multiplyFactor = multiplyFactorWithSmallestUnit((long)item, (long)itemUnit.ItemUnitId);
                                returnedQuantity += multiplyFactor * (int)itemUnit.Quantity;
                            }

                            foreach (var itemUnit in saledItemUnits)
                            {
                                int multiplyFactor = multiplyFactorWithSmallestUnit((long)item, (long)itemUnit.ItemUnitId);
                                purchasedQuantity += multiplyFactor * (int)itemUnit.Quantity;
                            }
                            if (returnedQuantity > purchasedQuantity)
                            {
                                message = "lowReturnQty";
                                result += "Result:" + message;
                                result += "}";
                                return TokenManager.GenerateToken(result);
                            }
                        }

                        #endregion

                        #region check items quantity in store
                        ItemLocationController itc = new ItemLocationController();
                        string res = itc.checkItemsAmounts(invoiceModel.InvoiceItems, branchId);

                        if (!res.Equals(""))
                        {
                            message = "lowQty";
                            result += "Result:" + message;

                            res = System.Web.Helpers.Json.Encode(res).Substring(1, System.Web.Helpers.Json.Encode(res).Length - 2);
                            result += ",Message:'" + res + "'";
                            result += "}";

                            return TokenManager.GenerateToken(result);
                        }
                        #endregion

                        newObject =  SaveInvoice(newObject);
                        long invoiceId = newObject.InvoiceId;
                        newObject.InvoiceId = invoiceId;
                        message = invoiceId.ToString();

                        if (!invoiceId.Equals(0))
                        {
                            #region save return invoice items
                            saveInvoiceItems(invoiceModel.InvoiceItems, invoiceId);

                            #endregion

                            #region save payments

                            #region save pos cash transfer
                            CashTransferController cc = new CashTransferController();

                            PosCashTransfer.InvId = invoiceId;

                            cc.addCashTransfer(PosCashTransfer);
                            #endregion
                            decimal paid = 0;
                            decimal deserved = 0;

                            foreach (var item in invoiceModel.ListPayments)
                            {
                                ConfiguredReturnCashTrans(newObject, item, posId);

                                if (item.ProcessType != "balance")
                                {
                                    paid += (decimal)item.Cash;
                                    deserved += (decimal)item.Cash;
                                }
                            }
                            var inv = entity.PurchaseInvoice.Find(invoiceId);
                            inv.Paid += paid;
                            inv.Deserved -= deserved;
                            entity.SaveChanges();

                            foreach (var item in invoiceModel.ListPayments)
                            {
                                if (item.ProcessType == "balance")
                                {
                                    var basicInvId =  entity.PurchaseInvoice.Where(i => i.InvoiceId == newObject.InvoiceMainId).FirstOrDefault().InvoiceId;
                                    var basicInv = entity.PurchaseInvoice.Find(basicInvId);
                                    var returnInv = entity.PurchaseInvoice.Find(invoiceId);

                                    decimal salesPaid = 0;
                                    if (basicInv.Deserved >= item.Cash)
                                        salesPaid = (decimal)item.Cash;
                                    else
                                    {
                                        salesPaid = (decimal)basicInv.Deserved;
                                        //decrease agent balance
                                        var agent = entity.Supplier.Find(newObject.SupplierId);
                                        decimal newBalance = 0;
                                        if (agent.BalanceType == 0)
                                        {
                                            if (salesPaid <= (decimal)agent.Balance)
                                            {
                                                newBalance = (decimal)agent.Balance - (decimal)item.Cash;
                                                agent.Balance = newBalance;
                                            }
                                            else
                                            {
                                                newBalance = (decimal)item.Cash - (decimal)agent.Balance;
                                                agent.Balance = newBalance;
                                                agent.BalanceType = 1;
                                            }


                                        }
                                        else if (agent.BalanceType == 1)
                                        {
                                            newBalance = (decimal)agent.Balance + (decimal)item.Cash;
                                            agent.Balance = newBalance;
                                        }
                                    }

                                    basicInv.Deserved -= salesPaid;
                                    basicInv.Paid += salesPaid;

                                    returnInv.Deserved -= salesPaid;
                                    returnInv.Paid += salesPaid;
                                    entity.SaveChanges();

                                    break;
                                }
                            }
                            #endregion

                            #region save notification
                            NotificationController nc = new NotificationController();
                            notification.UpdateUserId = notification.CreateUserId;

                            nc.save(notification, notificationUser.objectName, notificationUser.prefix, (int)notificationUser.branchId);
                            #endregion
                        }
                    }
                }

                catch
                {
                    message = "0";
                }
                result += "Result:" + message;
                string temp = System.Web.Helpers.Json.Encode(newObject.InvNumber).Substring(1, System.Web.Helpers.Json.Encode(newObject.InvNumber).Length - 2);
                result += ",Message:'" + temp + "'";

                #region get sales draft count
                List<string> invoiceType = new List<string>() { "pd ", "pbd" };
                int draftCount = getDraftCount((long)newObject.UpdateUserId, invoiceType);

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
            string InvNumber =  generateInvNumber(newObject.InvType, BranchId);

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
                    newObject.IsActive = true;

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
                    tmpInvoice.Remain = newObject.Remain;

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
               // var branch = entity.Branch.Find(BranchId);

               // branchCode = branch.Code;

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
           // string invoiceNum = invoiceCode + "-" + branchCode + "-" + strSeq;
            string invoiceNum = invoiceCode + "-" + strSeq;
            return invoiceNum;
        }

        public string saveInvoiceItems(List<PurInvoiceItemModel> newObject, long invoiceId)
        {
            string message = "";
            try
            {
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {

                    List<PurInvoiceItem> items = entity.PurInvoiceItem.Where(x => x.InvoiceId == invoiceId).ToList();
                    entity.PurInvoiceItem.RemoveRange(items);
                    entity.SaveChanges();

                    var invoice = entity.PurchaseInvoice.Find(invoiceId);
                    for (int i = 0; i < newObject.Count; i++)
                    {
                        PurInvoiceItem t;
                        if (newObject[i].CreateUserId == 0 || newObject[i].CreateUserId == null)
                        {
                            Nullable<long> id = null;
                            newObject[i].CreateUserId = id;
                        }

                        var transferEntity = entity.Set<PurInvoiceItem>();

                        newObject[i].InvoiceId = invoiceId;
                        newObject[i].CreateDate = countryc.AddOffsetTodate(DateTime.Now);
                        newObject[i].UpdateDate = countryc.AddOffsetTodate(DateTime.Now);
                        newObject[i].UpdateUserId = newObject[i].CreateUserId;

                        var myContent = JsonConvert.SerializeObject(newObject[i]);
                        var item = JsonConvert.DeserializeObject<PurInvoiceItem>(myContent, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        t = entity.PurInvoiceItem.Add(item);
                        entity.SaveChanges(); 
                    }
                    entity.SaveChanges();
                    message = "1";
                }
            }
            catch { message = "0"; }
            return message;
        }

        public string saveAvgPrice(List<PurInvoiceItemModel> newObject)
        {
            try
            {
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var set = entity.AppSetting.Where(x => x.Name == "item_cost").FirstOrDefault();
                    string invoiceNum = "0";
                    if (set != null)
                        invoiceNum = entity.AppSettingValue.Where(x => x.SettingId == (long)set.SettingId).Select(x => x.Value).Single();
                    foreach (var item in newObject)
                    {
                        var itemId = entity.ItemUnit.Where(x => x.ItemUnitId == (long)item.ItemUnitId).Select(x => x.ItemId).Single();

                        decimal price = GetAvgPrice((long)item.ItemUnitId, (long)itemId, int.Parse(invoiceNum));
                        var itemO = entity.Item.Find(itemId);
                        itemO.AvgPurchasePrice = price;

                    }
                    entity.SaveChanges();
                }
                return "1";
            }
            catch
            {
                return "0";
            }
        }

        private decimal GetAvgPrice(long itemUnitId, long itemId, int numInvoice)
        {
            decimal price = 0;
            int totalNum = 0;
            decimal smallUnitPrice = 0;

            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var itemUnits = (from i in entity.ItemUnit where (i.ItemId == itemId) select (i.ItemUnitId)).ToList();
                List<long> invoicesIds = new List<long>();
                if (numInvoice == 0)
                {
                    invoicesIds = (from p in entity.PurchaseInvoice
                                   where p.IsActive == true && (p.InvType == "p" || p.InvType == "is")
                                   select p).Select(x => x.InvoiceId).ToList();
                }
                else
                {
                    var invoices = (from p in entity.PurchaseInvoice
                                    where p.IsActive == true && (p.InvType == "p" || p.InvType == "is")
                                    orderby p.InvDate descending
                                    select p).Take(numInvoice);
                    invoicesIds = invoices.Select(x => x.InvoiceId).ToList();


                }
                price += getLastPrice(itemUnits, invoicesIds);
                totalNum = getItemUnitLastNum(itemUnits, invoicesIds);

                if (totalNum != 0)
                    smallUnitPrice = price / totalNum;
                return smallUnitPrice;

            }
        }

        private decimal getLastPrice(List<long> itemUnits, List<long> invoiceIds)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var sumPrice = (from s in entity.PurInvoiceItem.Where(x => itemUnits.Contains((long)x.ItemUnitId) && invoiceIds.Contains((long)x.InvoiceId))
                                select s.Quantity * s.Price).Sum();

                if (sumPrice != null)
                    return (decimal)sumPrice;
                else
                    return 0;
            }
        }

        private int getItemUnitLastNum(List<long> itemUnits, List<long> invoiceIds)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {

                var smallestUnitId = (from iu in entity.ItemUnit
                                      where (itemUnits.Contains((long)iu.ItemUnitId) && iu.UnitId == iu.SubUnitId)
                                      select iu.ItemUnitId).FirstOrDefault();

                if (smallestUnitId == null || smallestUnitId == 0)
                {
                    smallestUnitId = (from u in entity.ItemUnit
                                      where !entity.ItemUnit.Any(y => u.SubUnitId == y.UnitId)
                                      where (itemUnits.Contains((long)u.ItemUnitId))
                                      select u.ItemUnitId).FirstOrDefault();
                }
                var lst = entity.PurInvoiceItem.Where(x => x.ItemUnitId == smallestUnitId && invoiceIds.Contains((long)x.InvoiceId))
                           .Select(t => new ItemLocationModel
                           {
                               Quantity = (long)t.Quantity,
                           }).ToList();
                long sumNum = 0;
                if (lst.Count > 0)
                    sumNum = lst.Sum(x => x.Quantity);


                if (sumNum == null)
                    sumNum = 0;

                var unit = entity.ItemUnit.Where(x => x.ItemUnitId == smallestUnitId).Select(x => new { x.UnitId, x.ItemId }).FirstOrDefault();
                if (unit != null)
                {
                    var upperUnit = entity.ItemUnit.Where(x => x.SubUnitId == unit.UnitId && x.ItemId == unit.ItemId && x.SubUnitId != x.UnitId).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();

                    if (upperUnit != null && upperUnit.ItemUnitId != smallestUnitId)
                        sumNum += (int)upperUnit.UnitValue * getLastNum(upperUnit.ItemUnitId, invoiceIds);
                }

                try
                {
                    return (int)sumNum;
                }
                catch
                {
                    return 0;
                }
            }
        }

        private long getLastNum(long itemUnitId, List<long> invoiceIds)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var lst = entity.PurInvoiceItem.Where(x => x.ItemUnitId == itemUnitId && invoiceIds.Contains((long)x.InvoiceId))
                           .Select(t => new ItemLocationModel
                           {
                               Quantity =(long) t.Quantity,
                           }).ToList();
                long sumNum = 0;
                if (lst.Count > 0)
                    sumNum = lst.Sum(x => x.Quantity);
                if (sumNum == null)
                    sumNum = 0;

                var unit = entity.ItemUnit.Where(x => x.ItemUnitId == itemUnitId).Select(x => new { x.UnitId, x.ItemId }).FirstOrDefault();
                var upperUnit = entity.ItemUnit.Where(x => x.SubUnitId == unit.UnitId && x.ItemId == unit.ItemId && x.SubUnitId != x.UnitId).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();

                if (upperUnit != null)
                    sumNum += (int)upperUnit.UnitValue * getLastNum(upperUnit.ItemUnitId, invoiceIds);

                if (sumNum != null) return (long)sumNum;
                else
                    return 0;
            }
        }

        private void savePurchaseCash(PurchaseInvoice inv, CashTransfer cashTransfer, int posId)
        {
            CashTransferController cc = new CashTransferController();

            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var invoice = entity.PurchaseInvoice.Find(inv.InvoiceId);
                switch (cashTransfer.ProcessType)
                {
                    case "cash":// cash: update pos balance  
                        var pos = entity.POS.Find(posId);
                        if (pos.Balance > 0)
                        {
                            if (pos.Balance >= cashTransfer.Cash)
                            {
                                pos.Balance -= cashTransfer.Cash;
                                invoice.Paid = cashTransfer.Cash;
                                invoice.Deserved -= cashTransfer.Cash;
                            }
                            else
                            {
                                invoice.Paid = pos.Balance;
                                cashTransfer.Cash = pos.Balance;
                                invoice.Deserved -= pos.Balance;
                                pos.Balance = 0;
                            }
                            entity.SaveChanges();
                            cc.addCashTransfer(cashTransfer); //add cash transfer  
                        }
                        break;
                    case "balance":// balance: update supplier balance
                         recordConfiguredSupplierCash(invoice, "pi", cashTransfer, posId);

                        break;
                    case "card": // card  
                         cc.addCashTransfer(cashTransfer); //add cash transfer 
                        invoice.Paid += cashTransfer.Cash;
                        invoice.Deserved -= cashTransfer.Cash;
                        entity.SaveChanges();
                        break;
                }
            }
        }


        public PurchaseInvoice recordConfiguredSupplierCash(PurchaseInvoice invoice, string invType, CashTransfer cashTransfer, int posId)
        {
            CashTransferController cc = new CashTransferController();
            decimal newBalance = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var agent = entity.Supplier.Find(invoice.SupplierId);
                var inv = entity.PurchaseInvoice.Find(invoice.InvoiceId);

                #region agent Cash transfer
                cashTransfer.PosId = posId;
                cashTransfer.AgentId = invoice.SupplierId;
                cashTransfer.InvId = invoice.InvoiceId;
                cashTransfer.CreateUserId = invoice.CreateUserId;
                cashTransfer.Side = "v"; // vendor
                #endregion
                switch (invType)
                {
                    #region purchase
                    case "pi"://purchase invoice                 
                        cashTransfer.TransType = "p";
                        cashTransfer.TransNum = "pv";
                       
                        if (agent.BalanceType == 1)
                        {
                            if (cashTransfer.Cash <= (decimal)agent.Balance)
                            {

                                newBalance = (decimal)agent.Balance - (decimal)cashTransfer.Cash;
                                agent.Balance = newBalance;

                                inv.Paid += cashTransfer.Cash;
                                inv.Deserved -= cashTransfer.Cash;
                                ////
                                entity.SaveChanges();
                                ///
                            }
                            else
                            {
                                inv.Paid += (decimal)agent.Balance;
                                inv.Deserved -= (decimal)agent.Balance;
                                //////
                                ///
                                newBalance = (decimal)cashTransfer.Cash - (decimal)agent.Balance;
                                agent.Balance = newBalance;
                                agent.BalanceType = 0;
                                entity.SaveChanges();

                            }
                            cashTransfer.TransType = "p"; //pull

                            if (cashTransfer.ProcessType != "balance")
                                cc.addCashTransfer(cashTransfer); //add agent cash transfer

                        }
                        else if (agent.BalanceType == 0)
                        {
                            newBalance = (decimal)agent.Balance + (decimal)cashTransfer.Cash;
                            agent.Balance = newBalance;
                            entity.SaveChanges();
                        }

                        break;
                    #endregion
                    #region purchase bounce
                    case "pb"://purchase bounce invoice
                        cashTransfer.TransType = "d";
                        
                        cashTransfer.TransNum = cc.generateCashNumber("dv");

                       
                        if (agent.BalanceType == 0)
                        {
                            if (cashTransfer.Cash <= (decimal)agent.Balance)
                            {

                                newBalance = (decimal)agent.Balance - (decimal)cashTransfer.Cash;
                                agent.Balance = newBalance;

                                entity.SaveChanges();
                            }
                            else
                            {
                                newBalance = (decimal)cashTransfer.Cash - (decimal)agent.Balance;
                                agent.Balance = newBalance;
                                agent.BalanceType = 1;
                                entity.SaveChanges();

                            }
                            cashTransfer.TransType = "d"; //deposit

                            if (cashTransfer.Cash > 0 && cashTransfer.ProcessType != "balance")
                            {
                                 cc.addCashTransfer(cashTransfer); //add cash transfer     
                            }
                        }
                        else if (agent.BalanceType == 1)
                        {
                            newBalance = (decimal)agent.Balance + (decimal)cashTransfer.Cash;
                            agent.Balance = newBalance;
                            entity.SaveChanges();
                        }


                        break;
                        #endregion
                }
            }

            return invoice;
        }
        public int multiplyFactorWithSmallestUnit(long itemId, long itemUnitId)
        {
            int multiplyFactor = 1;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {

                var smallestUnit = entity.ItemUnit.Where(iu => iu.ItemId == itemId && iu.UnitId == iu.SubUnitId && iu.IsActive == true).FirstOrDefault();

                if (smallestUnit != null && smallestUnit.ItemUnitId.Equals(itemUnitId))
                    return multiplyFactor;
                if (smallestUnit != null)
                {
                    if (!smallestUnit.Equals(itemUnitId))
                        multiplyFactor = getUnitConversionQuan(itemUnitId, smallestUnit.ItemUnitId);
                }
                return multiplyFactor;
            }
        }

        private int getUnitConversionQuan(long fromItemUnit, long toItemUnit)
        {
            int amount = 0;

            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var unit = entity.ItemUnit.Where(x => x.ItemUnitId == toItemUnit).Select(x => new { x.UnitId, x.ItemId }).FirstOrDefault();
                var upperUnit = entity.ItemUnit.Where(x => x.SubUnitId == unit.UnitId && x.ItemId == unit.ItemId && x.SubUnitId != x.UnitId && x.IsActive == true).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();
                if (upperUnit != null)
                    amount = (int)upperUnit.UnitValue;
                if (fromItemUnit == upperUnit.ItemUnitId)
                    return amount;
                if (upperUnit != null)
                    amount += (int)upperUnit.UnitValue * getUnitConversionQuan(fromItemUnit, upperUnit.ItemUnitId);

                return amount;
            }
        }

        private CashTransfer ConfiguredReturnCashTrans(PurchaseInvoice invoice, CashTransfer cashTransfer, int posId)
        {
            CashTransferController cc = new CashTransferController();
            cashTransfer.CreateUserId = invoice.UpdateUserId;
            switch (cashTransfer.ProcessType)
            {
                case "cash":// cash: update pos balance  
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var pos = entity.POS.Find(posId);
                        pos.Balance += cashTransfer.Cash;
                        entity.SaveChanges();
                    }

                    cashTransfer.TransType = "d"; //deposit
                    cashTransfer.PosId = posId;
                    cashTransfer.AgentId = invoice.SupplierId;
                    cashTransfer.InvId = invoice.InvoiceId;
                    cashTransfer.TransNum = "dc";
                    cashTransfer.Side = "c"; // customer                    
                    cashTransfer.CreateUserId = invoice.UpdateUserId;
                    cc.addCashTransfer(cashTransfer);
                    break;
                case "balance":// balance: update customer balance

                       recordConfiguredSupplierCash(invoice, "si", cashTransfer, posId);

                    break;
                case "card": // card
                    cashTransfer.TransType = "d"; //deposit
                    cashTransfer.PosId = posId;
                    cashTransfer.AgentId = invoice.SupplierId;
                    cashTransfer.InvId = invoice.InvoiceId;
                    cashTransfer.TransNum = "dc";
                    cashTransfer.Side = "c"; // customer
                    cashTransfer.CreateUserId = invoice.UpdateUserId;
                    cc.addCashTransfer(cashTransfer); //add cash transfer

                    cc.AddCardCommission(cashTransfer);
                    break;
            }

            return cashTransfer;
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
                                                Remain = b.Remain,

                                            }).ToList();

                        if (invoicesList != null)
                        {
                            for (int i = 0; i < invoicesList.Count; i++)
                            {
                                long invoiceId = invoicesList[i].InvoiceId;
                                invoicesList[i].InvoiceItems = GetInvoiceItems(invoiceId);
                                invoicesList[i].ItemsCount = invoicesList[i].InvoiceItems.Count;
                                invoicesList[i].cachTrans = cashTransferController.GetPayedByInvId(invoiceId);

                                var NInvoice = GetNextInvoice(invoiceId, invoicesList[i].InvType, createUserId, duration);
                                if (NInvoice != null)
                                    invoicesList[i].HasNextInvoice = true;

                                var PInvoice = GetPreviouseInvoice(invoiceId, invoicesList[i].InvType, createUserId, duration);
                                if (PInvoice != null)
                                    invoicesList[i].HasPrevInvoice = true;
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
                                        Total = t.Total,
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

        [HttpPost]
        [Route("GetNextInvoice")]
        public string GetNextInvoice(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                CashTransferController cashTransferController = new CashTransferController();
                long invoiceId = 0;
                long createUserId = 0;
                int duration = 0;
                string invType = "";
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "invoiceId")
                    {
                        invoiceId = long.Parse(c.Value);
                    }
                    else if (c.Type == "createUserId")
                    {
                        createUserId = long.Parse(c.Value);
                    } 
                    else if (c.Type == "duration")
                    {
                        duration = int.Parse(c.Value);
                    }
                    else if (c.Type == "invType")
                    {
                        invType = c.Value;
                    }
                }
               var invoice = GetNextInvoice(invoiceId, invType, createUserId, duration);

                if (invoice != null)
                {
                    invoice.InvoiceItems = GetInvoiceItems(invoice.InvoiceId);
                    invoice.ItemsCount = invoice.InvoiceItems.Count;
                    invoice.cachTrans = cashTransferController.GetPayedByInvId(invoiceId);

                    var NInvoice = GetNextInvoice(invoice.InvoiceId, invType, createUserId, duration);
                    if (NInvoice != null)
                        invoice.HasNextInvoice = true; 
                    
                    var PInvoice = GetPreviouseInvoice(invoice.InvoiceId, invType, createUserId, duration);
                    if (PInvoice != null)
                        invoice.HasPrevInvoice = true;
                }
                return TokenManager.GenerateToken(invoice);

               
            }
        }

        [NonAction]
        private PurInvoiceModel GetNextInvoice(long invoiceId,string invType,long createUserId,int duration)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {

                var searchPredicate = PredicateBuilder.New<PurchaseInvoice>();

                searchPredicate = searchPredicate.And(inv => inv.InvType.Equals(invType));
                searchPredicate = searchPredicate.And(inv => inv.CreateUserId == createUserId);
                searchPredicate = searchPredicate.And(inv => inv.IsActive == true);
                searchPredicate = searchPredicate.And(inv => inv.InvoiceId > invoiceId);

                if (duration > 0)
                {
                    DateTime dt = Convert.ToDateTime(DateTime.Today.AddDays(-duration).ToShortDateString());
                    searchPredicate = searchPredicate.And(inv => inv.UpdateDate >= dt);
                }

                var invoice = (from b in entity.PurchaseInvoice.Where(searchPredicate)
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
                                   Tax = (decimal)b.Tax,
                                   TaxType = b.TaxType,
                                   TaxPercentage = b.TaxPercentage,
                                   IsApproved = b.IsApproved,
                                   BranchName = x.Name,
                                   BranchCreatorId = b.BranchCreatorId,

                                   BranchCreatorName = y.Name,
                                   PosId = b.PosId,
                                   ShippingCost = b.ShippingCost,
                                   Remain = b.Remain,

                               }).OrderBy(x => x.InvoiceId).FirstOrDefault();

                return invoice;
            }
        }

        [HttpPost]
        [Route("GetPreviousInvoice")]
        public string GetPreviousInvoice(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                CashTransferController cashTransferController = new CashTransferController();
                long invoiceId = 0;
                long createUserId = 0;
                int duration = 0;
                string invType = "";
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "invoiceId")
                    {
                        invoiceId = long.Parse(c.Value);
                    }
                    else if (c.Type == "createUserId")
                    {
                        createUserId = long.Parse(c.Value);
                    }
                    else if (c.Type == "duration")
                    {
                        duration = int.Parse(c.Value);
                    }
                    else if (c.Type == "invType")
                    {
                        invType = c.Value;
                    }
                }
                var invoice = GetPreviouseInvoice(invoiceId, invType, createUserId, duration);

                if (invoice != null)
                {
                    invoice.InvoiceItems = GetInvoiceItems(invoice.InvoiceId);
                    invoice.ItemsCount = invoice.InvoiceItems.Count;
                    invoice.cachTrans = cashTransferController.GetPayedByInvId(invoiceId);

                    var NInvoice = GetNextInvoice(invoice.InvoiceId, invType, createUserId, duration);
                    if (NInvoice != null)
                        invoice.HasNextInvoice = true;

                    var PInvoice = GetPreviouseInvoice(invoice.InvoiceId, invType, createUserId, duration);
                    if (PInvoice != null)
                        invoice.HasPrevInvoice = true;
                }
                return TokenManager.GenerateToken(invoice);


            }
        }
        [NonAction]
        private PurInvoiceModel GetPreviouseInvoice(long invoiceId, string invType, long createUserId, int duration)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {

                var searchPredicate = PredicateBuilder.New<PurchaseInvoice>();

                searchPredicate = searchPredicate.And(inv => inv.InvType.Equals(invType));
                searchPredicate = searchPredicate.And(inv => inv.CreateUserId == createUserId);
                searchPredicate = searchPredicate.And(inv => inv.IsActive == true);
                searchPredicate = searchPredicate.And(inv => inv.InvoiceId < invoiceId);

                if (duration > 0)
                {
                    DateTime dt = Convert.ToDateTime(DateTime.Today.AddDays(-duration).ToShortDateString());
                    searchPredicate = searchPredicate.And(inv => inv.UpdateDate >= dt);
                }

                var invoice = (from b in entity.PurchaseInvoice.Where(searchPredicate)
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
                                   Tax = (decimal)b.Tax,
                                   TaxType = b.TaxType,
                                   TaxPercentage = b.TaxPercentage,
                                   IsApproved = b.IsApproved,
                                   BranchName = x.Name,
                                   BranchCreatorId = b.BranchCreatorId,

                                   BranchCreatorName = y.Name,
                                   PosId = b.PosId,
                                   ShippingCost = b.ShippingCost,
                                   Remain = b.Remain,

                               }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();

                return invoice;
            }
        }

        [HttpPost]
        [Route("GetInvoiceToReturn")]
        public string GetInvoiceToReturn(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                CashTransferController cashTransferController = new CashTransferController();
                long invoiceId = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "invoiceId")
                    {
                        invoiceId = long.Parse(c.Value);
                    }
                   
                }

                var invoice = GetInvoiceToReturn(invoiceId);
                return TokenManager.GenerateToken(invoice);

            }
        }
       private PurInvoiceModel GetInvoiceToReturn(long invoiceId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var returned = entity.PurchaseInvoice.Where(x => x.InvoiceMainId == invoiceId && x.InvType!="pbd" && x.IsActive == true).ToList();
                var invoice = (from b in entity.PurchaseInvoice.Where(x => x.InvoiceId == invoiceId)
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
                                  // Total = b.Total,
                                  // TotalNet = b.TotalNet,
                                   //Paid = b.Paid,
                                   //Deserved = b.Deserved,
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
                                   DiscountValue = 0,
                                   DiscountType = b.DiscountType,
                                   DiscountPercentage = 0,
                                   Tax = 0,
                                  TaxType = b.TaxType,
                                   TaxPercentage = 0,
                                   IsApproved = b.IsApproved,
                                   BranchName = x.Name,
                                   BranchCreatorId = b.BranchCreatorId,

                                   BranchCreatorName = y.Name,
                                   PosId = b.PosId,
                                   ShippingCost = b.ShippingCost,
                                  // Remain = b.Remain,

                               }).FirstOrDefault();

                var mainInvoiceItems = GetInvoiceItems(invoiceId);
                if (returned.Count == 0)
                {

                    invoice.InvoiceItems = mainInvoiceItems;
                    invoice.ItemsCount = invoice.InvoiceItems.Count;
                    //invoice.cachTrans = cashTransferController.GetPayedByInvId(invoiceId);


                }
                else
                {
                    // decrease returned quantity from purchase invoice
                    foreach (var inv in returned)
                    {
                        var invItems = GetInvoiceItems(inv.InvoiceId);
                        foreach (var item in invItems)
                        {
                            mainInvoiceItems = updateItemQuantity(mainInvoiceItems, (long)item.ItemUnitId, item.Quantity);
                        }
                    }
                    invoice.InvoiceItems = mainInvoiceItems;

                }

                invoice.Total = invoice.TotalNet = invoice.InvoiceItems.Sum(x => x.Quantity * x.Price);
                return invoice;
            }
        }
        public List<PurInvoiceItemModel> updateItemQuantity(List<PurInvoiceItemModel> invoiceItems, long itemUnitId,  int requiredAmount)
        {

            Dictionary<string, int> dic = new Dictionary<string, int>();
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var unitInInvoice = invoiceItems.Find(x => x.ItemUnitId == itemUnitId);
              // foreach(var item in unitInInvoice)
                {
                    int availableAmount =(int)unitInInvoice.Quantity;

                    if (availableAmount >= requiredAmount)
                    {
                        unitInInvoice.Quantity = availableAmount - requiredAmount;
                        requiredAmount = 0;
                    }
                    else if (availableAmount > 0)
                    {
                        unitInInvoice.Quantity = 0;
                        requiredAmount = requiredAmount - availableAmount;
                    }

                    if (requiredAmount == 0)
                        return invoiceItems;
                }
                if (requiredAmount != 0)
                {
                    dic = checkUpperUnit(invoiceItems, itemUnitId,  requiredAmount);

                    var unit = entity.ItemUnit.Where(x => x.ItemUnitId == itemUnitId).Select(x => new { x.UnitId, x.ItemId }).FirstOrDefault();
                    var upperUnit = entity.ItemUnit.Where(x => x.SubUnitId == unit.UnitId && x.ItemId == unit.ItemId && x.SubUnitId != x.UnitId).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();


                    if (dic["remainQuantity"] > 0)
                    {
                        unitInInvoice = invoiceItems.Find(x => x.ItemUnitId == itemUnitId);
                        if (unitInInvoice != null)
                        {
                            unitInInvoice.Quantity = dic["remainQuantity"];
                        }
                        else
                        {
                            var itemUnit = entity.ItemUnit.Find(itemUnitId);
                            PurInvoiceItemModel itemL = new PurInvoiceItemModel();
                            itemL.ItemUnitId = itemUnitId;
                            itemL.Quantity = dic["remainQuantity"];
                            itemL.Price =(decimal)itemUnit.Price;
                            itemL.Total = (int)itemL.Quantity * itemL.Price;
                            itemL.ItemName = itemUnit.Item.Name;
                            itemL.UnitName = itemUnit.Unit.Name;
                            itemL.UnitId = itemUnit.UnitId;
                           invoiceItems.Add(itemL);
                        }
                    }
                    if (dic["requiredQuantity"] > 0)
                    {
                        checkLowerUnit(invoiceItems, itemUnitId, dic["requiredQuantity"]);
                    }

                }
            }
            return invoiceItems;

        }

        private Dictionary<string, int> checkUpperUnit(List<PurInvoiceItemModel> invoiceItems, long itemUnitId,  int requiredAmount)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("remainQuantity", 0);
            dic.Add("requiredQuantity", 0);
            dic.Add("isConsumed", 0);
            int remainQuantity = 0;
            int firstRequir = requiredAmount;
            decimal newQuant = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var unit = entity.ItemUnit.Where(x => x.ItemUnitId == itemUnitId).Select(x => new { x.UnitId, x.ItemId }).FirstOrDefault();
                var upperUnit = entity.ItemUnit.Where(x => x.SubUnitId == unit.UnitId && x.ItemId == unit.ItemId && x.SubUnitId != x.UnitId).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();

                if (upperUnit != null)
                {
                    decimal unitValue = (decimal)upperUnit.UnitValue;
                    int breakNum = (int)Math.Ceiling(requiredAmount / unitValue);
                    newQuant = (decimal)(breakNum * upperUnit.UnitValue);
                    var itemL = invoiceItems.Find(il => il.ItemUnitId == upperUnit.ItemUnitId && il.Quantity > 0);

                    if(itemL != null)
                    {
                        dic["isConsumed"] = 1;
                        //var smallUnitLocId = invoiceItems.Where(x => x.ItemUnitId == itemUnitId).FirstOrDefault();

                        if (breakNum <= itemL.Quantity)
                        {
                            itemL.Quantity = itemL.Quantity - breakNum;

                            remainQuantity = (int)newQuant - firstRequir;
                            requiredAmount = 0;

                            dic["remainQuantity"] = remainQuantity;
                            dic["requiredQuantity"] = 0;

                            return dic;
                        }
                        else
                        {
                            itemL.Quantity = 0;
                            breakNum = (int)(breakNum - itemL.Quantity);
                            requiredAmount = requiredAmount - ((int)itemL.Quantity * (int)upperUnit.UnitValue);
                            entity.SaveChanges();
                        }
                       
                    }
                    if (breakNum != 0)
                    {
                        dic = new Dictionary<string, int>();
                        dic = checkUpperUnit(invoiceItems,upperUnit.ItemUnitId,  breakNum);
                        //var item = (from s in entity.sections
                        //            where s.branchId == branchId
                        //            join l in entity.locations on s.sectionId equals l.sectionId
                        //            join il in entity.itemsLocations on l.locationId equals il.locationId
                        //            where il.itemUnitId == upperUnit.itemUnitId && il.invoiceId == null
                        //            select new
                        //            {
                        //                il.itemsLocId,
                        //            }).FirstOrDefault();
                        if (itemL != null)
                        {
                            itemL.Quantity = dic["remainQuantity"];
                        }
                        else
                        {
                            var itemUnit = entity.ItemUnit.Find(upperUnit.ItemUnitId);
                            itemL = new PurInvoiceItemModel();
                            itemL.ItemUnitId = itemUnitId;
                            itemL.Quantity = dic["remainQuantity"];
                            itemL.Price = (decimal)itemUnit.Price;
                            itemL.Total = (int)itemL.Quantity * itemL.Price;
                            itemL.ItemName = itemUnit.Item.Name;
                            itemL.UnitName = itemUnit.Unit.Name;
                            itemL.UnitId = itemUnit.UnitId;
                            invoiceItems.Add(itemL);

                        }
                        ///////////////////
                        if (dic["isConsumed"] == 0)
                        {
                            dic["requiredQuantity"] = requiredAmount;
                            dic["remainQuantity"] = 0;
                        }
                        else
                        {
                            dic["remainQuantity"] = (int)newQuant - firstRequir;
                            dic["requiredQuantity"] = breakNum * (int)upperUnit.UnitValue;
                        }
                        return dic;
                    }
                }
                else
                {
                    dic["remainQuantity"] = 0;
                    dic["requiredQuantity"] = requiredAmount;

                    return dic;
                }
            }
            return dic;
        }

        private Dictionary<string, int> checkLowerUnit(List<PurInvoiceItemModel> invoiceItems,long itemUnitId,  int requiredAmount)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            int remainQuantity = 0;
            int firstRequir = requiredAmount;
            decimal newQuant = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var unit = entity.ItemUnit.Where(x => x.ItemUnitId == itemUnitId).Select(x => new { x.UnitId, x.ItemId, x.SubUnitId, x.UnitValue }).FirstOrDefault();
                var lowerUnit = entity.ItemUnit.Where(x => x.UnitId == unit.SubUnitId && x.ItemId == unit.ItemId).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();

                if (lowerUnit != null)
                {
                    decimal unitValue = (decimal)unit.UnitValue;
                    int breakNum = (int)requiredAmount * (int)unitValue;
                    newQuant = (decimal)Math.Ceiling(breakNum / (decimal)lowerUnit.UnitValue);
                    var itemL = invoiceItems.Find(x => x.ItemUnitId == lowerUnit.ItemUnitId);    

                    if (itemL != null)
                    {

                        if (breakNum <= itemL.Quantity)
                        {
                            itemL.Quantity = itemL.Quantity - breakNum;
                            remainQuantity = (int)newQuant - firstRequir;
                            requiredAmount = 0;
                            dic.Add("remainQuantity", remainQuantity);

                            return dic;
                        }
                        else
                        {
                            itemL.Quantity = 0;
                            breakNum = (int)(breakNum - itemL.Quantity);
                            requiredAmount = requiredAmount - ((int)itemL.Quantity / (int)unit.UnitValue);
                            entity.SaveChanges();
                        }
                    }
                    if (itemUnitId == lowerUnit.ItemUnitId)
                        return dic;
                    if (breakNum != 0)
                    {
                        dic = new Dictionary<string, int>();
                        dic = checkLowerUnit(invoiceItems, lowerUnit.ItemUnitId,  breakNum);

                        dic["remainQuantity"] = (int)newQuant - firstRequir;
                        dic["requiredQuantity"] = breakNum;
                        return dic;
                    }
                }
            }
            return dic;
        }

        [HttpPost]
        [Route("GetInvoiceToReturnByNum")]
        public string GetInvoiceToReturnByNum(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                BranchController bc = new BranchController();
                string invNum = "";
                int branchId = 0;
                long userId = 0;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "invNum")
                    {
                        invNum = c.Value;
                    }
                    else if (c.Type == "branchId")
                    {
                        branchId = int.Parse(c.Value);
                    }
                    else if (c.Type == "userId")
                    {
                        userId = long.Parse(c.Value);
                    }
                }
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    //get user branches permission
                    var branches = bc.BrListByBranchandUser(branchId, userId);
                    

                    List<int> branchesIds = new List<int>();
                    for (int i = 0; i < branches.Count; i++)
                        branchesIds.Add(branches[i].BranchId);

                    if (branchesIds.Count.Equals(0))
                        branchesIds.Add(branchId);

                    var invoiceId = entity.PurchaseInvoice.Where(b => b.InvNumber == invNum
                                  && b.IsActive == true
                                  && branchesIds.Contains((int)b.BranchId))
                                    .Select(b => b.InvoiceId).FirstOrDefault();

                    if (invoiceId != null)
                    {
                       var invoice = GetInvoiceToReturn(invoiceId);
                        return TokenManager.GenerateToken(invoice);

                    }


                    return TokenManager.GenerateToken(new PurchaseInvoice());
                }
            }
        }
    }
}