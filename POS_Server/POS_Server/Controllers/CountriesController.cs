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

namespace POS_Server.Controllers
{
    [RoutePrefix("api/Countries")]
    public class CountriesController : ApiController
    {
        // GET api/<controller>
        [HttpPost]
        [Route("GetAllCountries")]
      public string   GetAllCountries(string token)
        {

            // public ResponseVM GetPurinv(string token)

            //int mainBranchId, int UserId    DateTime? date=new DateTime?();
           
            
            
          token = TokenManager.readToken(HttpContext.Current.Request); 
 var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                //int mainBranchId = 0;
                //int UserId = 0;

                //IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                //foreach (Claim c in claims)
                //{
                //    if (c.Type == "mainBranchId")
                //    {
                //        mainBranchId = int.Parse(c.Value);
                //    }
                //    else if (c.Type == "UserId")
                //    {
                //        UserId = int.Parse(c.Value);
                //    }

                //}

                // DateTime cmpdate = DateTime.Now.AddDays(newdays);
                try
                {

                   
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {


                        var list = entity.CountryCode
                         .Select(c => new
                         {
                             c.CountryId,
                             c.Code,
                         }).ToList();



                        return TokenManager.GenerateToken(list);

                    }

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }

            }

            //var re = Request;
            //
            //string token = "";

            //if (headers.Contains("APIKey"))
            //{
            //    token = headers.GetValues("APIKey").First();
            //}
            //Validation validation = new Validation();
            //bool valid = validation.CheckApiKey(token);

            //if (valid) // APIKey is valid
            //{
            //    using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //    {
            //        var countrylist = entity.CountryCode
            //             .Select(c => new {
            //                 c.CountryId,
            //                 c.Code,
            //             }).ToList();


            //        if (countrylist == null)
            //           return NotFound();
            //        else
            //            return Ok(countrylist);
            //    }
            //}
            //else
            //    return NotFound();


        }


        //[HttpPost]
        //[Route("GetAllCities")]
        //public IHttpActionResult GetAllCities()
        //{
        //   
        //    
        //    string token = "";

        //    if (headers.Contains("APIKey"))
        //    {
        //        token = headers.GetValues("APIKey").First();
        //    }
        //    Validation validation = new Validation();
        //    bool valid = validation.CheckApiKey(token);

        //    if (valid) // APIKey is valid
        //    {
        //        using (EasyGoDBEntities entity = new EasyGoDBEntities())
        //        {
        //            var countrylist = entity.CountryCode
        //                 .Select(c => new {
        //                     c.CountryId,
        //                     c.Code,
        //                     c.IsDefault,
        //                 }).ToList();


        //            if (countrylist == null)
        //            { return Ok(countrylist); }
        //            //return ("no");
        //            //return NotFound();
        //            else
        //            { return Ok(countrylist);}
                        
        //        }
        //    }
        //    else
        //        return NotFound();
        //}



        [HttpPost]
        [Route("GetAllRegion")]
      public string   GetAllRegion(string token)
        {
            // public ResponseVM GetPurinv(string token)

            //int mainBranchId, int UserId    DateTime? date=new DateTime?();
           
            
            
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

                        var list = entity.CountryCode
                         .Select(c => new
                         {
                             c.CountryId,
                             c.Code,
                             c.Currency,
                             c.Name,
                             c.IsDefault,
                             c.CurrencyId,

                         }).ToList();


                        
                        return TokenManager.GenerateToken(list);
                    }

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }

            }

            //var re = Request;
            //
            //string token = "";

            //if (headers.Contains("APIKey"))
            //{
            //    token = headers.GetValues("APIKey").First();
            //}
            //Validation validation = new Validation();
            //bool valid = validation.CheckApiKey(token);

            //if (valid) // APIKey is valid
            //{
            //    using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //    {
            //        var countrylist = entity.CountryCode
            //             .Select(c => new {
            //                 c.CountryId,
            //                 c.Code,
            //                 c.Currency,
            //                 c.Name,
            //                 c.IsDefault,
            //                 c.CurrencyId,

            //             }).ToList();


            //        if (countrylist == null)
            //            return NotFound();
            //        else
            //            return Ok(countrylist);
            //    }
            //}
            //else
            //    return NotFound();
        }

        [HttpPost]
        [Route("UpdateIsdefault")]
      public string   UpdateIsdefault(string token)
        {
            //int CountryId
            string message = "";
           
            
            
          token = TokenManager.readToken(HttpContext.Current.Request); 
 var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int CountryId = 0;
             
               
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "CountryId")
                    {
                        CountryId = int.Parse(c.Value);
                    }
                   
                }
                
                    try
                    {
                        using (EasyGoDBEntities entity = new EasyGoDBEntities())
                        {
                       // reset all to 0
                                    List<CountryCode> objectlist = entity.CountryCode.Where(x => x.IsDefault == 1).ToList();
                        if (objectlist.Count > 0)
                        {
                            for (int i = 0; i < objectlist.Count; i++)
                            {
                                objectlist[i].IsDefault = 0;

                            }
                            entity.SaveChanges();
                        }
                        // set is selected to isdefault=1
                        CountryCode objectrow = entity.CountryCode.Find(CountryId);

                        if (objectrow != null)
                        {
                            objectrow.IsDefault = 1;
                           
       
                           int res=  entity.SaveChanges();
                            if (res > 0)
                            {
                                message = objectrow.CountryId.ToString();
                            }
                            else
                            {
                                return TokenManager.GenerateToken("0");
                            }
                        }
                        else
                        {
                            return TokenManager.GenerateToken("0");
                        }
                        //  entity.SaveChanges();



                      


                        }
                        return TokenManager.GenerateToken(message);
                    }
                    catch
                    {
                        return TokenManager.GenerateToken("0");
                    }
                
                
            }



            //var re = Request;
            //
            //string token = "";
            //string message = "";
            //if (headers.Contains("APIKey"))
            //{
            //    token = headers.GetValues("APIKey").First();
            //}
            //Validation validation = new Validation();
            //bool valid = validation.CheckApiKey(token);

            //if (valid)
            //{


            //    try
            //    {
            //        using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //        {
            //            // reset all to 0
            //            List<CountryCode> objectlist = entity.CountryCode.Where(x=>x.IsDefault==1).ToList();
            //            if (objectlist.Count > 0)
            //            {
            //                for(int i=0;i< objectlist.Count; i++)
            //                {
            //                    objectlist[i].IsDefault = 0;

            //                }
            //                entity.SaveChanges();
            //            }
            //            // set is selected to isdefault=1
            //            CountryCode objectrow = entity.CountryCode.Find(CountryId);

            //            if (objectrow != null)
            //            {
            //                objectrow.IsDefault = 1;

            //                message = objectrow.CountryId.ToString();
            //                entity.SaveChanges();
            //            }
            //            else
            //            {
            //                message = "-1";
            //            }
            //            //  entity.SaveChanges();
            //        }
            //    }
            //    catch
            //    {
            //        message = "-1";
            //    }
            //}
            //return message;
        }

        [HttpPost]
        [Route("GetByID")]
      public string   GetByID(string token)
        {

            // public ResponseVM GetPurinv(string token)

            //int mainBranchId, int UserId    DateTime? date=new DateTime?();
           
            
            
          token = TokenManager.readToken(HttpContext.Current.Request); 
 var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int Id = 0;
               // int UserId = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "Id")
                    {
                        Id = int.Parse(c.Value);
                    }
                 

                }

                // DateTime cmpdate = DateTime.Now.AddDays(newdays);
                try
                {


                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {


                        var list = entity.CountryCode
                   .Where(c => c.CountryId == Id)
                   .Select(c => new
                   {
                       c.CountryId,
                       c.Code,
                       c.Currency,
                       c.Name,
                       c.IsDefault,
                       c.CurrencyId,
                   })
                   .FirstOrDefault();

                        return TokenManager.GenerateToken(list);

                    }

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }

            }


            //var re = Request;
            //
            //string token = "";
            //int cId = 0;
            //if (headers.Contains("APIKey"))
            //{
            //    token = headers.GetValues("APIKey").First();
            //}
            //if (headers.Contains("Id"))
            //{
            //    cId = Convert.ToInt32(headers.GetValues("Id").First());
            //}
            //Validation validation = new Validation();
            //bool valid = validation.CheckApiKey(token);

            //if (valid)
            //{
            //    using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //    {
            //        var list = entity.CountryCode
            //       .Where(c => c.CountryId == cId)
            //       .Select(c => new {
            //           c.CountryId,
            //           c.Code,
            //           c.Currency,
            //           c.Name,
            //           c.IsDefault,
            //           c.CurrencyId,
            //       })
            //       .FirstOrDefault();

            //        if (list == null)
            //            return NotFound();
            //        else
            //            return Ok(list);
            //    }
            //}
            //else
            //    return NotFound();
        }

        [HttpPost]
        [Route("GetisDefault")]
      public string   GetisDefault(string token)
        {

            // public ResponseVM GetPurinv(string token)

            //int mainBranchId, int UserId    DateTime? date=new DateTime?();
           
            
            
          token = TokenManager.readToken(HttpContext.Current.Request); 
 var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                //int mainBranchId = 0;
               int IsDefault = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "IsDefault")
                    {
                        IsDefault = int.Parse(c.Value);
                    }
               

                }

                // DateTime cmpdate = DateTime.Now.AddDays(newdays);
                try
                {


                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {


                        var list = entity.CountryCode
                   .Where(c => c.IsDefault == IsDefault)
                   .Select(c => new
                   {
                       c.CountryId,
                       c.Code,
                       c.Currency,
                       c.Name,
                       c.IsDefault,
                       c.CurrencyId,
                     
                   })
                   .FirstOrDefault();


                        return TokenManager.GenerateToken(list);

                    }

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }

            }



            //var re = Request;
            //
            //string token = "";
            //int cId = 0;
            //if (headers.Contains("APIKey"))
            //{
            //    token = headers.GetValues("APIKey").First();
            //}
            //if (headers.Contains("IsDefault"))
            //{
            //    cId = Convert.ToInt32(headers.GetValues("IsDefault").First());
            //}
            //Validation validation = new Validation();
            //bool valid = validation.CheckApiKey(token);

            //if (valid)
            //{
            //    using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //    {
            //        var list = entity.CountryCode
            //       .Where(c => c.IsDefault == cId)
            //       .Select(c => new {
            //           c.CountryId,
            //           c.Code,
            //           c.Currency,
            //           c.Name,
            //           c.IsDefault,
            //           c.CurrencyId,
            //       })
            //       .FirstOrDefault();

            //        if (list == null)
            //            return NotFound();
            //        else
            //            return Ok(list);
            //    }
            //}
            //else
            //    return NotFound();
        }

       public string TimeZoneDiff(string tzone1name, string tzone2name)
        {
            //program-servertimez
            var tzone1 = TimeZoneInfo.FindSystemTimeZoneById
                (tzone1name);
            var tzone2 = TimeZoneInfo.FindSystemTimeZoneById
                (tzone2name);
            var now = DateTimeOffset.UtcNow;
            TimeSpan tzone1span = tzone1.GetUtcOffset(now);
            TimeSpan tzone2span = tzone2.GetUtcOffset(now);
            TimeSpan difference = tzone1span - tzone2span;

            return difference.ToString();
        }
        public TimeSpan offsetTime()
        {
            CountryModel country = new CountryModel();
            
            //server timezone
            TimeZone serverTimeZone = TimeZone.CurrentTimeZone;
            string ServerStandardName = serverTimeZone.StandardName;
            //program time zone
            country = GetDefaultCountry();
            string programStandardName = country.TimeZoneName;
        string timeoffset=TimeZoneDiff(programStandardName, ServerStandardName);
            TimeSpan offset = TimeSpan.Parse(timeoffset);
            return offset;
        }
        public DateTime AddOffsetTodate(DateTime date )
        {
            TimeSpan ts = new TimeSpan();
            ts = offsetTime();
            date = date.AddHours(ts.TotalHours);
            return date;
        }
        public CountryModel GetDefaultCountry()
        {
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                    CountryModel item = entity.CountryCode
                   .Where(c => c.IsDefault == 1)
                   .Select(c => new CountryModel
                   {
                       CountryId = c.CountryId,
                       Code = c.Code,
                       Currency = c.Currency,
                       Name = c.Name,
                       IsDefault = c.IsDefault,
                       CurrencyId = c.CurrencyId,
                       TimeZoneName=c.TimeZoneName,
                       TimeZoneOffset=c.TimeZoneOffset,
                   }).FirstOrDefault();

                        return item;
                    }
                }
                catch
                {
                    CountryModel cntry = new CountryModel();
                    return  cntry ;
                }
        }

        [HttpPost]
        [Route("GetOffsetTime")]
        public string GetOffsetTime(string token)
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
                    TimeSpan ts = new TimeSpan();
                    ts = offsetTime();
                    return TokenManager.GenerateToken(ts);
                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }

            }
        }

    }
}