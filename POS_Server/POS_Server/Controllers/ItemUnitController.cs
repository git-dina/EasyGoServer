using Newtonsoft.Json;
using POS_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using POS_Server.Models.VM;
using System.Security.Claims;
using System.Web;
using Newtonsoft.Json.Converters;
using System.Data.Entity.SqlServer;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/ItemUnit")]
    public class ItemUnitController : ApiController
    {
        CountriesController cc = new CountriesController();
        List<int> itemUnitsIds = new List<int>();
        private Classes.Calculate Calc = new Classes.Calculate();
        [HttpPost]
        [Route("Get")]
        public string Get(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {

                try
                {

                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var itemUnitsList = (from IU in entity.ItemUnit
                                             where (IU.IsActive == true)
                                             join u in entity.Unit on IU.UnitId equals u.UnitId into lj
                                             from v in lj.DefaultIfEmpty()
                                             join u1 in entity.Unit on IU.SubUnitId equals u1.UnitId into tj
                                             from v1 in tj.DefaultIfEmpty()
                                             select new ItemUnitModel()
                                             {
                                                 ItemUnitId = IU.ItemUnitId,
                                                 UnitId = IU.UnitId,
                                                 UnitName = v.Name,
                                                 CreateDate = IU.CreateDate,
                                                 CreateUserId = IU.CreateUserId,
                                                 IsDefaultPurchase = IU.IsDefaultPurchase,
                                                 IsDefaultSale = IU.IsDefaultSale,
                                                 Price = IU.Price,
                                                 Cost = IU.Cost,
                                                 SubUnitId = IU.SubUnitId,
                                                 SmallUnit = v1.Name,
                                                 UnitValue = IU.UnitValue,
                                                 Barcode = IU.Barcode,
                                                 UpdateDate = IU.UpdateDate,
                                                 UpdateUserId = IU.UpdateUserId,
                                                 PurchasePrice = IU.PurchasePrice,
                                                 PackCost = IU.PackCost,
                                                 Notes = IU.Notes,
                                                 ItemId = IU.ItemId,
                                                 UnitCount = IU.UnitCount,
                                                 SmallestUnitId = IU.SmallestUnitId,
                                                 SmallestUnitName = entity.Unit.Where(x => x.UnitId == IU.SmallestUnitId).Select(x => x.Name).FirstOrDefault(),
                                             }).ToList();
                        return TokenManager.GenerateToken(itemUnitsList);

                    }
                }
                catch
                {
                    return TokenManager.GenerateToken("failed");
                }
            }
        }

        [HttpPost]
        [Route("GetItemUnit")]
        public string GetItemUnit(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                long itemId = 0;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemId")
                        itemId = long.Parse(c.Value);
                }
                try
                {

                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var itemUnitsList = (from IU in entity.ItemUnit
                                             where (IU.ItemId == itemId && IU.IsActive == true)
                                             join u in entity.Unit on IU.UnitId equals u.UnitId into lj
                                             from v in lj.DefaultIfEmpty()
                                             join u1 in entity.Unit on IU.SubUnitId equals u1.UnitId into tj
                                             from v1 in tj.DefaultIfEmpty()
                                             select new ItemUnitModel()
                                             {
                                                 ItemUnitId = IU.ItemUnitId,
                                                 UnitId = IU.UnitId,
                                                 UnitName = v.Name,
                                                 CreateDate = IU.CreateDate,
                                                 CreateUserId = IU.CreateUserId,
                                                 IsDefaultPurchase = IU.IsDefaultPurchase,
                                                 IsDefaultSale = IU.IsDefaultSale,
                                                 Price = IU.Price,
                                                 Cost = IU.Cost,
                                                 SubUnitId = IU.SubUnitId,
                                                 SmallUnit = v1.Name,
                                                 UnitValue = IU.UnitValue,
                                                 Barcode = IU.Barcode,
                                                 UpdateDate = IU.UpdateDate,
                                                 UpdateUserId = IU.UpdateUserId,
                                                 PurchasePrice = IU.PurchasePrice,
                                                 PackCost = IU.PackCost,
                                                 Notes = IU.Notes,
                                                 UnitCount = IU.UnitCount,
                                                 SmallestUnitId = IU.SmallestUnitId,
                                                 SmallestUnitName = entity.Unit.Where(x => x.UnitId == IU.SmallestUnitId).Select(x => x.Name).FirstOrDefault(),
                                                 ItemId = IU.ItemId,
                                             }).ToList();
                        return TokenManager.GenerateToken(itemUnitsList);

                    }
                }
                catch
                {
                    return TokenManager.GenerateToken("failed");
                }
            }
        }
        // add or update item unit
        [HttpPost]
        [Route("Save")]
        public string Save(string token)
        {

            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                ItemUnit newObject = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "Object")
                    {

                        newObject = JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }

                try
                {
                    long ItemUnitId = 0;
                    ItemUnit tmpItemUnit = new ItemUnit();
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var itemUnitEntity = entity.Set<ItemUnit>();
                        if (newObject.ItemUnitId == 0)
                        {
                            var iu = entity.ItemUnit.Where(x => x.ItemId == newObject.ItemId).FirstOrDefault();
                            if (iu == null)
                            {
                                newObject.IsDefaultSale = true;
                                newObject.IsDefaultPurchase = true;
                            }
                            else
                            {
                                //create
                                // set the other default sale or purchase to 0 if the new object.default is 1

                                if (newObject.IsDefaultSale == true)
                                { // get the row with same ItemId of newObject
                                    ItemUnit defItemUnit = entity.ItemUnit.Where(p => p.ItemId == newObject.ItemId && p.IsDefaultSale == true).FirstOrDefault();
                                    if (defItemUnit != null)
                                    {
                                        defItemUnit.IsDefaultSale =false;
                                        entity.SaveChanges();
                                    }
                                }
                                if (newObject.IsDefaultPurchase == true)
                                {
                                    var defItemUnit = entity.ItemUnit.Where(p => p.ItemId == newObject.ItemId && p.IsDefaultPurchase == true).FirstOrDefault();
                                    if (defItemUnit != null)
                                    {
                                        defItemUnit.IsDefaultPurchase = false;
                                        entity.SaveChanges();
                                    }
                                }
                            }
                            newObject.CreateDate = cc.AddOffsetTodate(DateTime.Now);
                            newObject.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            newObject.UpdateUserId = newObject.CreateUserId;

                            tmpItemUnit = itemUnitEntity.Add(newObject);
                        }
                        else
                        {
                            //update
                            // set the other default sale or purchase to 0 if the new object.default is 1
                            ItemUnitId = newObject.ItemUnitId;
                            tmpItemUnit = entity.ItemUnit.Find(ItemUnitId);

                            if (newObject.IsDefaultSale == true)
                            {
                                ItemUnit saleItemUnit = entity.ItemUnit.Where(p => p.ItemId == tmpItemUnit.ItemId && p.IsDefaultSale == true).FirstOrDefault();
                                if (saleItemUnit != null)
                                {
                                    saleItemUnit.IsDefaultSale = false;
                                    entity.SaveChanges();
                                }
                            }
                            if (newObject.IsDefaultPurchase == true)
                            {
                                var defItemUnit = entity.ItemUnit.Where(p => p.ItemId == tmpItemUnit.ItemId && p.IsDefaultPurchase == true).FirstOrDefault();
                                if (defItemUnit != null)
                                {
                                    defItemUnit.IsDefaultPurchase = false;
                                    entity.SaveChanges();
                                }
                            }
                            tmpItemUnit.Barcode = newObject.Barcode;
                            tmpItemUnit.Price = newObject.Price;
                            tmpItemUnit.Cost = newObject.Cost;
                            tmpItemUnit.SubUnitId = newObject.SubUnitId;
                            tmpItemUnit.UnitId = newObject.UnitId;
                            tmpItemUnit.UnitValue = newObject.UnitValue;
                            tmpItemUnit.IsDefaultPurchase = newObject.IsDefaultPurchase;
                            tmpItemUnit.IsDefaultSale = newObject.IsDefaultSale;
                            tmpItemUnit.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            tmpItemUnit.UpdateUserId = newObject.UpdateUserId;
                            tmpItemUnit.PurchasePrice = newObject.PurchasePrice;
                            tmpItemUnit.PackCost = newObject.PackCost;
                            tmpItemUnit.Notes = newObject.Notes;

                        }

                        entity.SaveChanges();

                        var item = entity.Item.Where(x => x.ItemId == newObject.ItemId).FirstOrDefault();
                        if (item.Type == "package")
                        {
                            item.AvgPurchasePrice = calculatePackagePrice(tmpItemUnit.ItemUnitId);
                            entity.SaveChanges();
                        }

                        #region calculate unit count
                        var itemUnits = entity.ItemUnit.Where(x => x.IsActive == true && x.ItemId == newObject.ItemId).ToList();
                        foreach(var row in itemUnits )
                        {
                            var unitCount = multiplyFactorWithSmallestUnit((long)newObject.ItemId, row.ItemUnitId);
                            row.UnitCount = unitCount.UnitCount;
                            row.SmallestUnitId = unitCount.SmallestUnitId;
                        }
                        entity.SaveChanges();
                        #endregion
                        return TokenManager.GenerateToken(tmpItemUnit.ItemUnitId.ToString());

                    }
                }
                catch
                {
                    return TokenManager.GenerateToken("failed");
                }


            }

        }

        public ItemUnitModel multiplyFactorWithSmallestUnit(long itemId, long itemUnitId)
        {
            int multiplyFactor = 1;
            ItemUnitModel itemUnit = new ItemUnitModel();
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {

                var smallestUnit = entity.ItemUnit.Where(iu => iu.ItemId == itemId && iu.UnitId == iu.SubUnitId && iu.IsActive == true).FirstOrDefault();
                itemUnit.SmallestUnitId = smallestUnit.UnitId;

                if (smallestUnit != null && smallestUnit.ItemUnitId.Equals(itemUnitId))
                {
                    itemUnit.UnitCount = multiplyFactor;
                    //return multiplyFactor;
                    return itemUnit;
                }
                if (smallestUnit != null)
                {
                    if (!smallestUnit.Equals(itemUnitId))
                        itemUnit.UnitCount = getUnitConversionQuan(itemUnitId, smallestUnit.ItemUnitId);
                        //multiplyFactor = getUnitConversionQuan(itemUnitId, smallestUnit.ItemUnitId);
                }
                return itemUnit;
               // return multiplyFactor;
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
                    amount = (int)upperUnit.UnitValue * getUnitConversionQuan(fromItemUnit, upperUnit.ItemUnitId);

                return amount;
            }
        }
        public decimal calculatePackagePrice(long packageParentId)
        {
            return 0;
            //using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //{
            //    var packageIems = (from S in entity.packages
            //                       join IU in entity.ItemUnit on S.childIUId equals IU.ItemUnitId
            //                       join I in entity.Item on IU.ItemId equals I.ItemId
            //                       where S.parentIUId == packageParentId
            //                       select new PackageModel
            //                       {
            //                           childIUId = S.childIUId,
            //                           quantity = S.quantity,
            //                           type = I.type,
            //                           citemId = I.ItemId,
            //                           AvgPurchasePrice = I.AvgPurchasePrice,
            //                       }).ToList();

            //    decimal AvgPurchasePrice = 0;
            //    foreach (PackageModel item in packageIems)
            //    {
            //        if (!item.type.Equals("sr"))
            //        {
            //            int multiplyFactor = multiplyFactorWithSmallestUnit((int)item.citemId, (int)item.childIUId);

            //            AvgPurchasePrice += item.quantity * multiplyFactor * (decimal)item.AvgPurchasePrice;
            //        }
            //    }

            //    var packageCost = entity.ItemUnit.Find(packageParentId);

            //    if (packageCost.packCost != null)
            //        AvgPurchasePrice += (decimal)packageCost.packCost;
            //    return AvgPurchasePrice;
            //}
        }
        [HttpPost]
        [Route("Delete")]
        public string Delete(string token)
        {
            string message = "";

            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                long itemUnitId = 0;
                long userId = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemUnitId")
                    {
                        itemUnitId = long.Parse(c.Value);
                    }
                    else if (c.Type == "userId")
                    {
                        userId = long.Parse(c.Value);
                    }

                }

                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {

                        var tmp = entity.ItemUnit.Find(itemUnitId);
                        entity.ItemUnit.Remove(tmp);
                        entity.SaveChanges();
                        return TokenManager.GenerateToken(message);

                    }
                }
                catch
                {
                    try
                    {
                        using (EasyGoDBEntities entity = new EasyGoDBEntities())
                        {
                            var tmp = entity.ItemUnit.Find(itemUnitId);

                            tmp.IsActive = false;
                            tmp.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            tmp.UpdateUserId = userId;
                            entity.SaveChanges();
                            return TokenManager.GenerateToken(message);

                        }
                    }
                    catch
                    {
                        message = "failed";
                        return TokenManager.GenerateToken(message);
                    }
                }
            }
        }

       

        
    }
}