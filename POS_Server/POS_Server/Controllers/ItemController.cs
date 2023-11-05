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

                DateTime cmpdate = cc.AddOffsetTodate(DateTime.Now).AddDays(newdays);
                DateTime datenow = cc.AddOffsetTodate(DateTime.Now);
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var itemsList = (from I in entity.Item where I.IsActive == true

                                    // join c in entity.Category on I.CategoryId equals c.CategoryId into lj
                                    // from x in lj.DefaultIfEmpty()
                                     select new ItemModel()
                                     {
                                         ItemId = I.ItemId,
                                         Name = I.Name,
                                         Code = I.Code,
                                         CategoryId = I.CategoryId,

                                         CategoryName = I.Category.Name,
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
                                         Notes = I.Notes,
                                         AvgPurchasePrice = I.AvgPurchasePrice,
                                         IsExpired = I.IsExpired,
                                         ItemUnits = (from IU in entity.ItemUnit
                                                      where (IU.ItemId == I.ItemId && IU.IsActive == true)
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
                                                          PackCost = IU.PackCost,
                                                          UnitCount = IU.UnitCount,
                                                          PurchasePrice = IU.PurchasePrice,
                                                          SmallestUnitId = IU.SmallestUnitId,
                                                          SmallestUnitName = entity.Unit.Where(x => x.UnitId == IU.SmallestUnitId).Select(x => x.Name).FirstOrDefault(),
                                                      }).ToList(),
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
                    //                           isTaxExempt = iu.Item.isTaxExempt,
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
                    //               MaxUnitId = itemtb.MaxUnitId,
                    //               MinUnitId = itemtb.MinUnitId,
                    //               min = itemtb.min,

                    //               ParentId = itemtb.ParentId,
                    //               isActive = itemtb.isActive,

                    //               isOffer = 0,
                    //               desPrice = 0,

                    //               offerName = "",
                    //               CreateDate = itemtb.CreateDate,
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

                    //            itemsList[i].AvgPurchasePrice = itemsList[i].AvgPurchasePrice;
                    //            itemsList[i].discountType = itofflist.discountType;
                    //            itemsList[i].discountValue = itofflist.discountValue;
                    //        }
                    //    }
                    //    // is new
                    //    int res = DateTime.Compare((DateTime)itemsList[i].CreateDate, cmpdate);
                    //    if (res >= 0)
                    //    {
                    //        itemsList[i].isNew = 1;
                    //    }

                    //}

                    return TokenManager.GenerateToken(itemsList);
                }
            }
        }

         [HttpPost]
        [Route("GetWithUnits")]
        public string GetWithUnits(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {

                DateTime cmpdate = cc.AddOffsetTodate(DateTime.Now).AddDays(newdays);
                DateTime datenow = cc.AddOffsetTodate(DateTime.Now);
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var itemsList = (from I in entity.Item where I.IsActive == true

                                    // join c in entity.Category on I.CategoryId equals c.CategoryId into lj
                                    // from x in lj.DefaultIfEmpty()
                                     select new ItemModel()
                                     {
                                         ItemId = I.ItemId,
                                         Name = I.Name,
                                         Code = I.Code,
                                         CategoryId = I.CategoryId,

                                         CategoryName = I.Category.Name,
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
                                         Notes = I.Notes,
                                         AvgPurchasePrice = I.AvgPurchasePrice,
                                         IsExpired = I.IsExpired,
                                         ItemUnits = (from IU in entity.ItemUnit
                                                      where (IU.ItemId == I.ItemId && IU.IsActive == true)
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
                                                          PackCost = IU.PackCost,
                                                          UnitCount = IU.UnitCount,
                                                          PurchasePrice = IU.PurchasePrice,
                                                          SmallestUnitId = IU.SmallestUnitId,
                                                          SmallestUnitName = entity.Unit.Where(x => x.UnitId == IU.SmallestUnitId).Select(x => x.Name).FirstOrDefault(),
                                                      }).ToList(),
                                     }).Where(x => x.ItemUnits.Count > 0)
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
                    //                           isTaxExempt = iu.Item.isTaxExempt,
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
                    //               MaxUnitId = itemtb.MaxUnitId,
                    //               MinUnitId = itemtb.MinUnitId,
                    //               min = itemtb.min,

                    //               ParentId = itemtb.ParentId,
                    //               isActive = itemtb.isActive,

                    //               isOffer = 0,
                    //               desPrice = 0,

                    //               offerName = "",
                    //               CreateDate = itemtb.CreateDate,
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

                    //            itemsList[i].AvgPurchasePrice = itemsList[i].AvgPurchasePrice;
                    //            itemsList[i].discountType = itofflist.discountType;
                    //            itemsList[i].discountValue = itofflist.discountValue;
                    //        }
                    //    }
                    //    // is new
                    //    int res = DateTime.Compare((DateTime)itemsList[i].CreateDate, cmpdate);
                    //    if (res >= 0)
                    //    {
                    //        itemsList[i].isNew = 1;
                    //    }

                    //}

                    return TokenManager.GenerateToken(itemsList);
                }
            }
        }


        // add or update item
        [HttpPost]
        [Route("Save")]
        public string Save(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            string message = "";
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                Item itemObj = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        itemObj = JsonConvert.DeserializeObject<Item>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }

                if (itemObj.UpdateUserId == 0 || itemObj.UpdateUserId == null)
                {
                    Nullable<int> id = null;
                    itemObj.UpdateUserId = id;
                }
                if (itemObj.CreateUserId == 0 || itemObj.CreateUserId == null)
                {
                    Nullable<int> id = null;
                    itemObj.CreateUserId = id;
                }
                if (itemObj.CategoryId == 0 || itemObj.CategoryId == null)
                {
                    Nullable<int> id = null;
                    itemObj.CategoryId = id;
                }
                if (itemObj.MinUnitId == 0 || itemObj.MinUnitId == null)
                {
                    Nullable<int> id = null;
                    itemObj.MinUnitId = id;
                }
                if (itemObj.MaxUnitId == 0 || itemObj.MaxUnitId == null)
                {
                    Nullable<int> id = null;
                    itemObj.MaxUnitId = id;
                }
                if (itemObj.AvgPurchasePrice == null)
                {
                    itemObj.AvgPurchasePrice = 0;
                }
               try
                {
                    Item itemModel;
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var ItemEntity = entity.Set<Item>();
                        if (itemObj.ItemId == 0)
                        {
                            //ProgramInfo programInfo = new ProgramInfo();
                            //int itemMaxCount = programInfo.getItemCount();
                            //int itemsCount = entity.Item.Count();
                            //if (itemsCount >= itemMaxCount && itemMaxCount != -1)
                            //{
                            //    message = "upgrade";
                            //}
                            //else
                            {
                                itemObj.CreateDate = cc.AddOffsetTodate(DateTime.Now);
                                itemObj.UpdateDate = itemObj.CreateDate;
                                itemObj.UpdateUserId = itemObj.CreateUserId;

                                itemModel = ItemEntity.Add(itemObj);
                                entity.SaveChanges();
                                message = itemObj.ItemId.ToString();
                            }
                        }
                        else
                        {
                            itemModel = entity.Item.Where(p => p.ItemId == itemObj.ItemId).First();
                            itemModel.Code = itemObj.Code;
                            itemModel.CategoryId = itemObj.CategoryId;
                            itemModel.Details = itemObj.Details;
                            itemModel.Image = itemObj.Image;
                            itemModel.Max = itemObj.Max;
                            itemModel.MaxUnitId = itemObj.MaxUnitId;
                            itemModel.Min = itemObj.Min;
                            itemModel.MinUnitId = itemObj.MinUnitId;
                            itemModel.Name = itemObj.Name;

                            itemModel.Taxes = itemObj.Taxes;
                            itemModel.Type = itemObj.Type;
                            itemModel.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            itemModel.UpdateUserId = itemObj.UpdateUserId;
                            itemModel.AvgPurchasePrice = itemObj.AvgPurchasePrice;
                            itemModel.IsExpired = itemObj.IsExpired;

                            entity.SaveChanges();
                            message = itemModel.ItemId.ToString();
                        }
                    }
                    return TokenManager.GenerateToken(message);
                }
                catch
                {
                    message = "failed";
                    return TokenManager.GenerateToken(message);
                }
            }
        }

        [HttpPost]
        [Route("UpdateImage")]
        public string UpdateImage(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            string message = "";
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                long itemId = 0;
                string fileName = "";

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemId")
                    {
                        itemId = long.Parse(c.Value);
                    }
                    else if (c.Type == "image")
                    {
                        fileName = c.Value;
                    }

                }
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var item = entity.Item.Find(itemId);
                        item.Image = fileName;
                        entity.SaveChanges();
                        message = item.ItemId.ToString();
                    }
                   
                    return TokenManager.GenerateToken(message);
                }
                catch
                {
                    message = "faild";
                    return TokenManager.GenerateToken(message);
                }
            }
        }

        [Route("PostItemImage")]
        public IHttpActionResult PostItemImage()
        {

            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    string imageName = postedFile.FileName;
                    string imageWithNoExt = Path.GetFileNameWithoutExtension(postedFile.FileName);

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".bmp", ".jpeg", ".tiff", ".jfif" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();

                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload Image of type .jpg,.gif,.png.");
                            return Ok(message);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            return Ok(message);
                        }
                        else
                        {
                            var dir = System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\item");
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);

                            //  check if Image exist
                            var pathCheck = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\item"), imageWithNoExt);
                            var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\item"), imageWithNoExt + ".*");
                            if (files.Length > 0)
                            {
                                File.Delete(files[0]);
                            }

                            //Userimage myfolder Name where i want to save my Image
                            var filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\item"), imageName);
                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Ok(message1);
                }
                var res = string.Format("Please Upload a Image.");

                return Ok(res);
            }
            catch
            {
                var res = string.Format("faild");

                return Ok(res);
            }
        }

        [HttpPost]
        [Route("GetImage")]
        public string GetImage(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                string imageName = "";
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "imageName")
                    {
                        imageName = c.Value;
                    }
                }
                if (String.IsNullOrEmpty(imageName))
                    return TokenManager.GenerateToken("faild");

                string localFilePath;

                try
                {
                    localFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\item"), imageName);

                    byte[] b = System.IO.File.ReadAllBytes(localFilePath);
                    return TokenManager.GenerateToken(Convert.ToBase64String(b));
                }
                catch
                {
                    return TokenManager.GenerateToken("failed");

                }
            }
        }

        [HttpPost]
        [Route("Delete")]
        public string Delete(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            string message = "success";
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                long itemId = 0;
                long userId = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemId")
                    {
                        itemId = long.Parse(c.Value);
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

                        var tmp = entity.Item.Find(itemId);
                        entity.Item.Remove(tmp);
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
                            var tmp = entity.Item.Find(itemId);

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