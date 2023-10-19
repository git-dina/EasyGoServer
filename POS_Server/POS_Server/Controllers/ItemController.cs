using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using POS_Server.Models;
using System.Web;
using System.IO;
using LinqKit;
using Microsoft.Ajax.Utilities;
using POS_Server.Classes;
using POS_Server.Models.VM;
using System.Security.Claims;
using Newtonsoft.Json.Converters;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/Item")]
    public class ItemController : ApiController
    {
        CountriesController cc = new CountriesController();
        private Classes.Calculate Calc = new Classes.Calculate();
        public int newdays = -15;
        List<int> categoriesId = new List<int>();

        [HttpPost]
        [Route("GetAllItems")]
        public string GetAllItems(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                Boolean canDelete = false;

                DateTime cmpdate = cc.AddOffsetTodate(DateTime.Now).AddDays(newdays);
                DateTime datenow = cc.AddOffsetTodate(DateTime.Now);
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var itemsList = (from I in entity.Item

                                     join c in entity.Category on I.CategoryId equals c.CategoryId into lj
                                     from x in lj.DefaultIfEmpty()
                                     select new ItemModel()
                                     {
                                         ItemId = I.ItemId,
                                         Name = I.Name,
                                         Code = I.Code,
                                         CategoryId = I.CategoryId,

                                         CategoryName = x.Name,
                                         Max = I.Max,
                                         MaxUnitId = I.MaxUnitId,
                                         MinUnitId = I.MinUnitId,
                                         Min = I.Min,
                                         IsActive = I.IsActive,
                                         Image = I.Image,
                                         Type = I.Type,
                                         Details = I.Details,
                                         Taxes = I.Taxes,
                                         CreateDate = I.CreateDate,
                                         UpdateDate = I.UpdateDate,
                                         CreateUserId = I.CreateUserId,
                                         UpdateUserId = I.UpdateUserId,
                                         IsNew = 0,
                                         MinUnitName = entity.Unit.Where(m => m.UnitId == I.MinUnitId).FirstOrDefault().Name,
                                         MaxUnitName = entity.Unit.Where(m => m.UnitId == I.MinUnitId).FirstOrDefault().Name,

                                         AvgPurchasePrice = I.AvgPurchasePrice,
                                         IsExpired = I.IsExpired,
                                         AlertDays = I.AlertDays,
                                         IsTaxExempt = I.IsTaxExempt,
                                     })
                                   .ToList();

                    //var itemsofferslist = (from off in entity.offers

                    //                       join itof in entity.itemsOffers on off.offerId equals itof.offerId // itemsOffers and offers 

                    //                       join iu in entity.ItemUnit on itof.iuId equals iu.itemUnitId
                    //                       select new ItemSalePurModel()
                    //                       {
                    //                           ItemId = iu.ItemId,
                    //                           itemUnitId = itof.iuId,
                    //                           offerName = off.Name,
                    //                           offerId = off.offerId,
                    //                           discountValue = off.discountValue,
                    //                           isNew = 0,
                    //                           isOffer = 1,
                    //                           isActiveOffer = off.isActive,
                    //                           startDate = off.startDate,
                    //                           endDate = off.endDate,
                    //                           unitId = iu.unitId,

                    //                           price = iu.price,
                    //                           discountType = off.discountType,
                    //                           desPrice = iu.price,
                    //                           defaultSale = iu.defaultSale,
                    //                           isTaxExempt = iu.items.isTaxExempt,
                    //                       }).ToList();

                    //itemsofferslist = itemsofferslist.Where(IO => (IO.isActiveOffer == 1 && DateTime.Compare(((DateTime)IO.startDate).Date, datenow.Date) <= 0 && System.DateTime.Compare(((DateTime)IO.endDate).Date, datenow.Date) >= 0 && IO.defaultSale == 1)
                    //                        && (((DateTime)IO.startDate)).TimeOfDay <= datenow.TimeOfDay && ((DateTime)IO.endDate).TimeOfDay >= datenow.TimeOfDay)
                    //                        .Distinct().ToList();

                    //var unt = (from unitm in entity.ItemUnit
                    //           join untb in entity.Unit on unitm.unitId equals untb.unitId
                    //           join itemtb in entity.Item on unitm.ItemId equals itemtb.ItemId

                    //           select new ItemSalePurModel()
                    //           {
                    //               ItemId = itemtb.ItemId,
                    //               Name = itemtb.Name,
                    //               Code = itemtb.Code,


                    //               max = itemtb.max,
                    //               maxUnitId = itemtb.maxUnitId,
                    //               minUnitId = itemtb.minUnitId,
                    //               min = itemtb.min,

                    //               parentId = itemtb.parentId,
                    //               isActive = itemtb.isActive,

                    //               isOffer = 0,
                    //               desPrice = 0,

                    //               offerName = "",
                    //               createDate = itemtb.createDate,
                    //               defaultSale = unitm.defaultSale,
                    //               unitName = untb.Name,
                    //               unitId = untb.unitId,
                    //               price = unitm.price,

                    //           }).Where(a => a.defaultSale == 1).Distinct().ToList();


                    //for (int i = 0; i < itemsList.Count; i++)
                    //{

                    //    foreach (var itofflist in itemsofferslist)
                    //    {


                    //        if (itemsList[i].ItemId == itofflist.ItemId)
                    //        {

                    //            // get unit Name of item that has the offer
                    //            using (EasyGoDBEntities entitydb = new EasyGoDBEntities())
                    //            { // put it in item
                    //                var un = entitydb.units
                    //                    .Where(a => a.unitId == itofflist.unitId)
                    //                    .Select(u => new
                    //                    {
                    //                        u.Name
                    //                    ,
                    //                        u.unitId
                    //                    }).FirstOrDefault();
                    //                itemsList[i].unitName = un.Name;
                    //            }

                    //            itemsList[i].offerName = itemsList[i].offerName + "- " + itofflist.offerName;
                    //            itemsList[i].isOffer = 1;
                    //            itemsList[i].startDate = itofflist.startDate;
                    //            itemsList[i].endDate = itofflist.endDate;
                    //            itemsList[i].itemUnitId = itofflist.itemUnitId;
                    //            itemsList[i].offerId = itofflist.offerId;
                    //            itemsList[i].isActiveOffer = itofflist.isActiveOffer;

                    //            itemsList[i].price = itofflist.price;

                    //            itemsList[i].avgPurchasePrice = itemsList[i].avgPurchasePrice;
                    //            itemsList[i].discountType = itofflist.discountType;
                    //            itemsList[i].discountValue = itofflist.discountValue;
                    //        }
                    //    }
                    //    // is new
                    //    int res = DateTime.Compare((DateTime)itemsList[i].createDate, cmpdate);
                    //    if (res >= 0)
                    //    {
                    //        itemsList[i].isNew = 1;
                    //    }

                    //}

                    return TokenManager.GenerateToken(itemsList);
                }
            }
        }

       

    }
}