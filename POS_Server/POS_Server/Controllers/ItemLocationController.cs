using LinqKit;
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
    }
}