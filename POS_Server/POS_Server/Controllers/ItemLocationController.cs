using LinqKit;
using POS_Server.Models;
using POS_Server.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/ItemLocation")]
    public class ItemLocationController : ApiController
    {
        CountriesController cc = new CountriesController();

        [HttpPost]
        [Route("getAmountInBranch")]
        public string getAmountInBranch(string token)
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
                int branchId = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemUnitId")
                    {
                        itemUnitId = long.Parse(c.Value);

                    }
                    else if (c.Type == "branchId")
                    {
                        branchId = int.Parse(c.Value);

                    }
                }

                try
                {

                    int amount = getBranchAmount(itemUnitId, branchId);
                    return TokenManager.GenerateToken(amount.ToString());
                }
                catch
                {
                    message = "0";
                    return TokenManager.GenerateToken(message);
                }
            }
        }

        public int getBranchAmount(long itemUnitId, int branchId)
        {

            int amount = 0;
            amount += getItemUnitAmount(itemUnitId, branchId); // from bigger unit
            amount += getSmallItemUnitAmount(itemUnitId, branchId);

            return amount;
        }

        private int getItemUnitAmount(long itemUnitId, int branchId)
        {
            int amount = 0;

            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                //var searchPredicate = PredicateBuilder.New<ItemLocation>();

                var itemInLocs = (from //b in entity.Branch
                                  //where b.BranchId == branchId
                                  //join s in entity.Section on b.BranchId equals s.BranchId
                                  //join l in entity.Location on s.SectionId equals l.SectionId
                                  //join
                                  il in entity.ItemLocation //on l.LocationId equals il.LocationId
                                  where il.ItemUnitId == itemUnitId && il.Quantity > 0
                                  && il.Location.BranchId == branchId
                                  select new
                                  {
                                      il.ItemLocId,
                                      il.Quantity,
                                      il.ItemUnitId,
                                      il.LocationId,
                                      il.Location.SectionId,
                                     // s.SectionId,
                                  }).ToList();
                for (int i = 0; i < itemInLocs.Count; i++)
                {
                    amount += (int)itemInLocs[i].Quantity;
                }

                var unit = entity.ItemUnit.Where(x => x.ItemUnitId == itemUnitId).Select(x => new { x.UnitId, x.ItemId }).FirstOrDefault();
                var upperUnit = entity.ItemUnit.Where(x => x.SubUnitId == unit.UnitId && x.ItemId == unit.ItemId && x.SubUnitId != x.UnitId).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();

                if (upperUnit == null)
                    return amount;
                if ((upperUnit != null && itemUnitId == upperUnit.ItemUnitId))
                    return amount;
                if (upperUnit != null)
                    amount += (int)upperUnit.UnitValue * getItemUnitAmount(upperUnit.ItemUnitId, branchId);

                return amount;
            }
        }

        private int getSmallItemUnitAmount(long itemUnitId, int branchId)
        {
            int amount = 0;

            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {

                var unit = entity.ItemUnit.Where(x => x.ItemUnitId == itemUnitId).Select(x => new { x.SubUnitId, x.UnitId, x.UnitValue, x.ItemId }).FirstOrDefault();

                var smallUnit = entity.ItemUnit.Where(x => x.UnitId == unit.SubUnitId && x.ItemId == unit.ItemId).Select(x => new { x.ItemUnitId }).FirstOrDefault();
                if (smallUnit == null || smallUnit.ItemUnitId == itemUnitId)
                {
                    return 0;
                }
                else
                {
                    var itemInLocs = (from //b in entity.Branch
                                      //where b.BranchId == branchId
                                      //join s in entity.Section on b.BranchId equals s.BranchId
                                     // join l in entity.Location on s.SectionId equals l.SectionId
                                      //join
                                      il in entity.ItemLocation //on l.LocationId equals il.LocationId
                                      where il.ItemUnitId == smallUnit.ItemUnitId && il.Quantity > 0
                                      && il.Location.BranchId == branchId
                                      select new
                                      {
                                          il.ItemLocId,
                                          il.Quantity,
                                          il.ItemUnitId,
                                          il.LocationId,
                                          il.Location.Section.SectionId,
                                      }).ToList();
                    for (int i = 0; i < itemInLocs.Count; i++)
                    {
                        amount += (int)itemInLocs[i].Quantity;
                    }
                    if (unit.UnitValue != 0)
                        amount = amount / (int)unit.UnitValue;

                    else
                        amount += getSmallItemUnitAmount(smallUnit.ItemUnitId, branchId) / (int)unit.UnitValue;

                    return amount;
                }
            }
        }
        [NonAction]
        public void receiptInvoice(int branchId, List<PurInvoiceItemModel> newObject, long userId, string objectName, string notificationObj)
        {
            NotificationController notificationController = new NotificationController();
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var freeZoneLocation = (from l in entity.Location.Where(x => x.BranchId == branchId && x.IsFreeZone == true)
                                        select l.LocationId).SingleOrDefault();
                foreach (var item in newObject)
                {
                    increaseItemQuantity(item.ItemUnitId.Value, freeZoneLocation, (int)item.Quantity, userId);


                    bool isExcedded = isExceddMaxQuantity((long)item.ItemUnitId, branchId, userId);
                    if (isExcedded == true) //add notification
                    {
                        notificationController.addNotifications(objectName, notificationObj, branchId, item.ItemName);
                    }
                }

            }
        }

        private void increaseItemQuantity(long itemUnitId, int locationId, int quantity, long userId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var itemUnit = (from il in entity.ItemLocation
                                where il.ItemUnitId == itemUnitId && il.LocationId == locationId 
                                select new { il.ItemLocId }
                                ).FirstOrDefault();
                ItemLocation itemL = new ItemLocation();
                if (itemUnit == null)//add item in new location
                {
                    itemL.ItemUnitId = itemUnitId;
                    itemL.LocationId = locationId;
                    itemL.Quantity = quantity;
                    itemL.CreateDate = cc.AddOffsetTodate(DateTime.Now);
                    itemL.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                    itemL.CreateUserId = userId;
                    itemL.UpdateUserId = userId;

                    entity.ItemLocation.Add(itemL);
                }
                else
                {
                    itemL = entity.ItemLocation.Find(itemUnit.ItemLocId);
                    itemL.Quantity += quantity;
                    itemL.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                    itemL.UpdateUserId = userId;
                }
                entity.SaveChanges();
            }
        }

        public bool isExceddMaxQuantity(long itemUnitId, int branchId, long userId)
        {
            ItemUnitController itemUnitController = new ItemUnitController();
            bool isExcedded = false;
            try
            {
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var itemId = entity.ItemUnit.Where(x => x.ItemUnitId == itemUnitId).Select(x => x.ItemId).Single();
                    var item = entity.Item.Find(itemId);
                    long maxUnitId = (long)item.MaxUnitId;
                    int maxQuantity = (int)item.Max;
                    if (maxQuantity == 0)
                        return false;
                    var maxUnit = entity.ItemUnit.Where(x => x.ItemId == itemId && x.UnitId == maxUnitId).FirstOrDefault();
                    if (maxUnit == null)
                        isExcedded = false;
                    else
                    {
                        int itemUnitQuantity = getItemAmount(maxUnit.ItemUnitId, branchId);
                        if (itemUnitQuantity >= maxQuantity)
                        {
                            isExcedded = true;
                        }
                        if (isExcedded == false)
                        {
                            long smallestItemUnit = entity.ItemUnit.Where(x => x.ItemId == itemId && x.SubUnitId == x.UnitId).Select(x => x.ItemUnitId).Single();
                            int smallUnitQuantity = getLevelItemUnitAmount(smallestItemUnit, maxUnit.ItemUnitId, branchId);
                            int unitValue = itemUnitController.getLargeUnitConversionQuan(smallestItemUnit, maxUnit.ItemUnitId);
                            int quantity = 0;
                            if (unitValue != 0)
                                quantity = smallUnitQuantity / unitValue;

                            quantity += itemUnitQuantity;
                            if (quantity >= maxQuantity)
                            {
                                isExcedded = true;
                            }
                        }

                    }
                }
            }
            catch
            {
            }
            return isExcedded;
        }

        private int getItemAmount(long itemUnitId, int branchId)
        {
            int amount = 0;

            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var itemInLocs = (from //b in entity.Branch
                                  //where b.branchId == branchId
                                 // join s in entity.sections on b.branchId equals s.branchId
                                 // join l in entity.locations on s.sectionId equals l.sectionId
                                  //join 
                                  il in entity.ItemLocation //on l.locationId equals il.LocationId
                                  where il.ItemUnitId == itemUnitId && il.Quantity > 0 && il.Location.BranchId == branchId
                                  select new
                                  {
                                      il.ItemLocId,
                                      il.Quantity,
                                      il.ItemUnitId,
                                      il.LocationId,
                                      il.Location.SectionId,
                                     // s.sectionId,
                                  }).ToList();
                for (int i = 0; i < itemInLocs.Count; i++)
                {
                    amount += (int)itemInLocs[i].Quantity;
                }

                var unit = entity.ItemUnit.Where(x => x.ItemUnitId == itemUnitId).Select(x => new { x.UnitId, x.ItemId }).FirstOrDefault();
                var upperUnit = entity.ItemUnit.Where(x => x.SubUnitId == unit.UnitId && x.ItemId == unit.ItemId && x.SubUnitId != x.UnitId).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();

                if ((upperUnit != null && itemUnitId == upperUnit.ItemUnitId) || upperUnit == null)
                    return amount;
                if (upperUnit != null)
                    amount += (int)upperUnit.UnitValue * getItemUnitAmount(upperUnit.ItemUnitId, branchId);

                return amount;
            }
        }

        private int getLevelItemUnitAmount(long itemUnitId, long topLevelUnit, int branchId)
        {
            int amount = 0;

            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var itemInLocs = (from //b in entity.branches
                                  //where b.branchId == branchId
                                  //join s in entity.sections on b.branchId equals s.branchId
                                  //join l in entity.locations on s.sectionId equals l.sectionId
                                  //join
                                  il in entity.ItemLocation //on l.locationId equals il.locationId
                                  where il.ItemUnitId == itemUnitId && il.Quantity > 0 && il.Location.BranchId == branchId
                                  select new
                                  {
                                      il.ItemLocId,
                                      il.Quantity,
                                      il.ItemUnitId,
                                      il.LocationId,
                                      il.Location.SectionId,
                                  }).ToList();
                for (int i = 0; i < itemInLocs.Count; i++)
                {
                    amount += (int)itemInLocs[i].Quantity;
                }

                var unit = entity.ItemUnit.Where(x => x.ItemUnitId == itemUnitId).Select(x => new { x.UnitId, x.ItemId }).FirstOrDefault();
                var upperUnit = entity.ItemUnit.Where(x => x.SubUnitId == unit.UnitId && x.ItemId == unit.ItemId && x.SubUnitId != x.UnitId).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();

                if ((upperUnit != null && itemUnitId == upperUnit.ItemUnitId) || upperUnit == null)
                    return amount;
                if (upperUnit != null && upperUnit.ItemUnitId != topLevelUnit)
                    amount += (int)upperUnit.UnitValue * getLevelItemUnitAmount(upperUnit.ItemUnitId, topLevelUnit, branchId);

                return amount;
            }
        }

        public string checkItemsAmounts(List<PurInvoiceItemModel> billDetails, int branchId)
        {
            string res = "";
            foreach (var item in billDetails)
            {
                int availableAmount = getBranchAmount((long)item.ItemUnitId, branchId);
                if (availableAmount < item.Quantity && item.ItemType != "sr")
                {
                    res = item.ItemName;
                    return res;
                }
            }
            return res;
        }
        [HttpPost]
        [Route("GetFreeZoneItems")]
        public string GetFreeZoneItems(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int branchId = 0;


                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "branchId")
                    {
                        branchId = int.Parse(c.Value);
                    }

                }
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var docImageList = (from b in entity.ItemLocation
                                            where b.Quantity > 0 
                                            //join u in entity.itemsUnits on b.itemUnitId equals u.itemUnitId
                                            //join i in entity.items on u.itemId equals i.itemId
                                            //join l in entity.locations on b.locationId equals l.locationId
                                            //join s in entity.sections on l.sectionId equals s.sectionId
                                            where b.Location.BranchId == branchId && b.Location.IsFreeZone == true

                                            select new ItemLocationModel
                                            {
                                                CreateDate = b.CreateDate,
                                                CreateUserId = b.CreateUserId,
                                                EndDate = b.EndDate,
                                                ItemLocId = b.ItemLocId,
                                               ItemUnitId = b.ItemUnitId,
                                                LocationId = b.LocationId,
                                                Notes = b.Notes,
                                                Quantity = (long)b.Quantity,
                                                StartDate = b.StartDate,

                                                UpdateDate = b.UpdateDate,
                                                UpdateUserId = b.UpdateUserId,
                                                ItemName = b.ItemUnit.Item.Name,
                                                SectionId = (int)b.Location.SectionId,
                                                IsFreeZone = b.Location.IsFreeZone,
                                                ItemType = b.ItemUnit.Item.Type,
                                                LocationName = b.Location.x + b.Location.y + b.Location.z,
                                                SectionName = b.Location.Section.Name,
                                                UnitName = b.ItemUnit.Unit.Name,
                                                IsExpired = b.ItemUnit.Item.IsExpired,
                                            }).ToList();


                        return TokenManager.GenerateToken(docImageList);

                    }
                }
                catch
                {
                    return TokenManager.GenerateToken("failed");
                }
            }
        }


        [HttpPost]
        [Route("ChangeUnitExpireDate")]
        public string ChangeUnitExpireDate(string token)
        {

            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                long itemLocId = 0;
                long userId = 0;
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemLocId")
                    {
                        itemLocId = long.Parse(c.Value);

                    }
                    else if (c.Type == "userId")
                    {
                        userId = long.Parse(c.Value);

                    }
                    else if (c.Type == "startDate")
                    {
                        startDate = DateTime.Parse(c.Value);
                    }
                    else if (c.Type == "endDate")
                    {
                        endDate = DateTime.Parse(c.Value);
                    }

                }

                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var itemLoc = entity.ItemLocation.Find(itemLocId);

                        itemLoc.StartDate = startDate;
                        itemLoc.EndDate = endDate;
                        itemLoc.UpdateUserId = userId;
                        entity.SaveChanges().ToString();

                        return TokenManager.GenerateToken("success");
                    }
                }
                catch
                {
                    return TokenManager.GenerateToken("failed");
                }
            }

        } 
        [HttpPost]
        [Route("SaveItemNotes")]
        public string SaveItemNotes(string token)
        {

            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                long itemLocId = 0;
                long userId = 0;
                string notes = "";

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemLocId")
                    {
                        itemLocId = long.Parse(c.Value);

                    }
                    else if (c.Type == "userId")
                    {
                        userId = long.Parse(c.Value);

                    }
                    else if (c.Type == "notes")
                    {
                        notes =c.Value;
                    }
                    
                }

                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var itemLoc = entity.ItemLocation.Find(itemLocId);

                        itemLoc.Notes = notes;
                        itemLoc.UpdateUserId = userId;
                        entity.SaveChanges().ToString();

                        return TokenManager.GenerateToken("success");
                    }
                }
                catch
                {
                    return TokenManager.GenerateToken("failed");
                }
            }

        }
    }
}