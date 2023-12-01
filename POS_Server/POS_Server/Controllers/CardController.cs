using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using POS_Server.Models;
using POS_Server.Models.VM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/Card")]
    public class CardController : ApiController
    {
        CountriesController cc = new CountriesController();
        // GET api/<controller> get all cards
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
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var cardsList = entity.Card.Where(c => c.IsActive == true)
                   .Select(c => new CardModel()
                   {
                       CardId = c.CardId,
                       Name = c.Name,
                        Notes = c.Notes,
                       CreateDate = c.CreateDate,
                       UpdateDate = c.UpdateDate,
                       CreateUserId = c.CreateUserId,
                       UpdateUserId = c.UpdateUserId,
                       IsActive = c.IsActive,
                       HasProcessNum = c.HasProcessNum,
                       Image = c.Image,
                       CommissionValue = c.CommissionValue,
                       CommissionRatio = c.CommissionRatio,
                       Balance = c.Balance,
                       BalanceType = c.BalanceType,

                   }).ToList();

                   
                    return TokenManager.GenerateToken(cardsList);
                }
            }
        }

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
                Card Object = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        Object = JsonConvert.DeserializeObject<Card>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        Card tmpcard = new Card();
                        var cardEntity = entity.Set<Card>();
                        if (Object.CardId == 0)
                        {
                            Object.CreateDate = cc.AddOffsetTodate(DateTime.Now);
                            Object.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            Object.UpdateUserId = Object.CreateUserId;
                            tmpcard = cardEntity.Add(Object);
                            entity.SaveChanges();
                            message = tmpcard.CardId.ToString();

                        }
                        else
                        {

                            tmpcard = entity.Card.Where(p => p.CardId == Object.CardId).FirstOrDefault();
                            tmpcard.CardId = Object.CardId;
                            tmpcard.Name = Object.Name;
                            tmpcard.Notes = Object.Notes;
                            tmpcard.CreateDate = Object.CreateDate;
                            tmpcard.UpdateDate = Object.UpdateDate;
                            tmpcard.CreateUserId = Object.CreateUserId;
                            tmpcard.UpdateUserId = Object.UpdateUserId;
                            tmpcard.UpdateDate = cc.AddOffsetTodate(DateTime.Now);// server current date;
                            tmpcard.UpdateUserId = Object.UpdateUserId;
                            tmpcard.HasProcessNum = Object.HasProcessNum;
                            tmpcard.Image = Object.Image;
                            tmpcard.CommissionValue = Object.CommissionValue;
                            tmpcard.CommissionRatio = Object.CommissionRatio;

                            entity.SaveChanges();
                            message = tmpcard.CardId.ToString();
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

        [Route("PostUserImage")]
        public IHttpActionResult PostUserImage()
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

                            var message = string.Format("Please Upload Image of type .jpg,.gif,.png, .jfif, .bmp , .jpeg ,.tiff");
                            return Ok(message);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            return Ok(message);
                        }
                        else
                        {
                            var dir = System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\card");
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);
                            //  check if Image exist
                            var pathCheck = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\card"), imageWithNoExt);
                            var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\card"), imageWithNoExt + ".*");
                            if (files.Length > 0)
                            {
                                File.Delete(files[0]);
                            }

                            //Userimage myfolder FirstName where i want to save my Image
                            var filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\card"), imageName);
                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Ok(message1);
                }
                var res = string.Format("Please Upload a Image.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");

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
                    return TokenManager.GenerateToken("0");

                string localFilePath;
                try
                {
                    localFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\card"), imageName);

                    byte[] b = System.IO.File.ReadAllBytes(localFilePath);
                    return TokenManager.GenerateToken(Convert.ToBase64String(b));
                }
                catch
                {
                    return TokenManager.GenerateToken(null);

                }
            }
        }

        [HttpPost]
        [Route("UpdateImage")]
        public string UpdateImage(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            string message = "success";
            var re = Request;
            var headers = re.Headers;
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                Card Obj = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        Obj = JsonConvert.DeserializeObject<Card>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                try
                {
                    Card card;
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var cardEntity = entity.Set<Card>();
                        card = entity.Card.Where(p => p.CardId == Obj.CardId).First();
                        card.Image = Obj.Image;
                        entity.SaveChanges();
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
                int cardId = 0;
                long userId = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemId")
                    {
                        cardId = int.Parse(c.Value);
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

                        Card tmp = entity.Card.Find(cardId);
                        entity.Card.Remove(tmp);
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
                            Card tmp = entity.Card.Find(cardId);

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