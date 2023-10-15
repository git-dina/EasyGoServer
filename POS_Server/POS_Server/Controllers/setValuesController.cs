using Newtonsoft.Json;
using POS_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using System.Net.Http.Headers;
using POS_Server.Models.VM;
using System.Security.Claims;
using Newtonsoft.Json.Converters;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/AppSettingValue")]
    public class setValuesController : ApiController
    {
        CountriesController cc = new CountriesController();
        // GET api/<controller> get all AppSettingValue
        [HttpPost]
        [Route("Get")]
       public string Get(string token)
        {
            // public ResponseVM GetPurinv(string token)

           
            
            
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


                        var list = entity.AppSettingValue

                   .Select(c => new
                   {
                       c.ValId,
                       c.Value,
                       c.IsDefault,
                       c.IsSystem,
                       c.Notes,
                       c.SettingId,

                   })
                               .ToList();

                        return TokenManager.GenerateToken(list);

                    }

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }

            }

          
        }
        // email
        [HttpPost]
        [Route("GetBySetName")]
      public string   GetBySetName(string token)
        {

            
          token = TokenManager.readToken(HttpContext.Current.Request); 
 var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
               string Name = "";
               

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "Name")
                    {
                        Name = c.Value;
                    }
                  

                }

                // DateTime cmpdate = DateTime.Now.AddDays(newdays);
                try
                {


                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        AppSetting sett = entity.AppSetting.Where(s => s.Name == Name).FirstOrDefault();
                        var list = entity.AppSettingValue.Where(x => sett.SettingId == x.SettingId)
                             .Select(X => new
                             {
                                 X.ValId,
                                 X.Value,
                                 X.IsDefault,
                                 X.IsSystem,
                                 X.SettingId,
                                 X.Notes,

                             })
                             .ToList();
                        return TokenManager.GenerateToken(list);

                    }

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }

            }


           
        }



        public string GetBySettingName(string settingName)
        {

            AppSettingValue sv = new AppSettingValue();
          List<AppSettingValue> svl = new List<AppSettingValue>();

                // DateTime cmpdate = DateTime.Now.AddDays(newdays);
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        AppSetting sett = entity.AppSetting.Where(s => s.Name == settingName).FirstOrDefault();

                    var svlv = entity.AppSettingValue.ToList();
                    svl=svlv.Where(x => sett.SettingId == x.SettingId)
                         .Select(X => new AppSettingValue
                         {
                            ValId= X.ValId,
                             Value=  X.Value,
                             IsDefault=   X.IsDefault,
                             IsSystem=  X.IsSystem,
                             SettingId=   X.SettingId,
                             Notes= X.Notes,

                         }).ToList();
                    sv = svl.FirstOrDefault();
                    return sv.Value;
                    }

                }
                catch 
                {
               // return ex.ToString();
              return "0";
                }
         
        }
        public AppSettingValue GetRowBySettingName(string settingName)
        {

            AppSettingValue sv = new AppSettingValue();
            List<AppSettingValue> svl = new List<AppSettingValue>();

            // DateTime cmpdate = DateTime.Now.AddDays(newdays);
            try
            {
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    AppSetting sett = entity.AppSetting.Where(s => s.Name == settingName).FirstOrDefault();

                    var svlv = entity.AppSettingValue.ToList();
                    svl = svlv.Where(x => sett.SettingId == x.SettingId)
                         .Select(X => new AppSettingValue
                         {
                             ValId = X.ValId,
                             Value = X.Value,
                             IsDefault = X.IsDefault,
                             IsSystem = X.IsSystem,
                             SettingId = X.SettingId,
                             Notes = X.Notes,

                         }).ToList();
                    sv = svl.FirstOrDefault();
                    return sv ;
                }

            }
            catch
            {
                sv = new AppSettingValue();
                // return ex.ToString();
                return sv;
            }

        }


        [HttpPost]
        [Route("GetBySetvalNote")]
      public string   GetBySetvalNote(string token)
      {          
            token = TokenManager.readToken(HttpContext.Current.Request); 
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                string setvalnote = "";


                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "setvalnote")
                    {
                        setvalnote = c.Value;
                    }


                }

                // DateTime cmpdate = DateTime.Now.AddDays(newdays);
                try
                {


                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var list = entity.AppSettingValue.ToList().Where(x => x.Notes == setvalnote)
                             .Select(X => new
                             {
                                 X.ValId,
                                 X.Value,
                                 X.IsDefault,
                                 X.IsSystem,
                                 X.SettingId,
                                 X.Notes,
                                 Name = entity.AppSetting.ToList().Where(s => s.SettingId == X.SettingId).FirstOrDefault().Name,

                             })
                             .ToList();

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
            //        //AppSetting sett = entity.AppSetting.Where(s => s.Name == Name).FirstOrDefault();
            //        var setValuesList = entity.AppSettingValue.ToList().Where(x => x.Notes == setvalnote)
            //             .Select(X => new {
            //                 X.ValId,
            //                 X.Value,
            //                 X.IsDefault,
            //                 X.IsSystem,
            //                 X.SettingId,
            //                 X.Notes,
            //                 Name= entity.AppSetting.ToList().Where(s => s.SettingId == X.SettingId).FirstOrDefault().Name,

            //    })
            //             .ToList();

            //        if (setValuesList == null)
            //            return NotFound();
            //        else
            //            return Ok(setValuesList);
            //    }
            //}
            ////else
            //return NotFound();
        }


        // GET api/<controller>  Get medal By ID 
        [HttpPost]
        [Route("GetByID")]
      public string   GetByID(string token)
        {
            // public ResponseVM GetPurinv(string token)Id

           
            
            
          token = TokenManager.readToken(HttpContext.Current.Request); 
 var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int Id =0;


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
                   
                        var item = entity.AppSettingValue
                   .Where(c => c.ValId == Id)
                   .Select(c => new
                   {
                       c.ValId,
                       c.Value,
                       c.IsDefault,
                       c.IsSystem,
                       c.Notes,
                       c.SettingId,


                   }).FirstOrDefault();
                        return TokenManager.GenerateToken(item);

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
            //        var list = entity.AppSettingValue
            //       .Where(c => c.ValId == cId)
            //       .Select(c => new {
            //           c.ValId,
            //           c.Value,
            //           c.IsDefault,
            //           c.IsSystem,
            //           c.Notes,
            //           c.SettingId,


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


        // add or update medal 
        [HttpPost]
        [Route("Save")]
      public string   Save(string token)
        {
            //string Object string newObject
            string message = "";
           
            
            
          token = TokenManager.readToken(HttpContext.Current.Request); 
 var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                string Object = "";
                AppSettingValue newObject = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "Object")
                    {
                        //Object = c.Value.Replace("\\", string.Empty);
                        //Object = Object.Trim('"');
                        Object = c.Value;
                        newObject = JsonConvert.DeserializeObject<AppSettingValue>(Object, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                if (newObject != null)
                {


                    AppSettingValue tmpObject=null;


                    try
                    {
                        if (newObject.SettingId == 0 || newObject.SettingId == null)
                        {
                            Nullable<int> id = null;
                            newObject.SettingId = id;
                        }
                        using (EasyGoDBEntities entity = new EasyGoDBEntities())
                        {
                            var sEntity = entity.Set<AppSettingValue>();
                            AppSettingValue defItem = entity.AppSettingValue.Where(p => p.SettingId == newObject.SettingId && p.IsDefault == 1).FirstOrDefault();

                            if (newObject.ValId == 0)
                            {
                                if (newObject.IsDefault == 1)
                                { // get the row with same SettingId of newObject
                                    if (defItem != null)
                                    {
                                        defItem.IsDefault = 0;
                                        entity.SaveChanges();
                                    }
                                }
                                else //Object.IsDefault ==0 
                                {
                                    if (defItem == null)//other values IsDefault not 1 
                                    {
                                        newObject.IsDefault = 1;
                                    }

                                }
                                sEntity.Add(newObject);
                                message = newObject.ValId.ToString();
                                entity.SaveChanges();
                            }
                            else
                            {
                                if (newObject.IsDefault == 1)
                                {
                                    defItem.IsDefault = 0;//reset the other default to 0 if exist
                                }
                                tmpObject = entity.AppSettingValue.Where(p => p.ValId == newObject.ValId).FirstOrDefault();
                                tmpObject.ValId = newObject.ValId;
                                tmpObject.Notes = newObject.Notes;
                                tmpObject.Value = newObject.Value;
                                tmpObject.IsDefault = newObject.IsDefault;
                                tmpObject.IsSystem = newObject.IsSystem;

                                tmpObject.SettingId = newObject.SettingId;
                                entity.SaveChanges();
                                message = tmpObject.ValId.ToString();
                            }


                        }
                       
                        return TokenManager.GenerateToken(message);

                    }
                    catch
                    {
                        message = "0";
                      return TokenManager.GenerateToken(message);
                    }


                }

              return TokenManager.GenerateToken(message);

            }

            //var re = Request;
            //
            //string token = "";
            //string message ="";
            //if (headers.Contains("APIKey"))
            //{
            //    token = headers.GetValues("APIKey").First();
            //}
            //Validation validation = new Validation();
            //bool valid = validation.CheckApiKey(token);

            //if (valid)
            //{
            //    newObject = newObject.Replace("\\", string.Empty);
            //    newObject = newObject.Trim('"');
            //    AppSettingValue Object = JsonConvert.DeserializeObject<AppSettingValue>(newObject, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
            //    try
            //    {
            //        if (Object.SettingId == 0 || Object.SettingId == null)
            //        {
            //            Nullable<int> id = null;
            //            Object.SettingId = id;
            //        }
            //        using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //        {
            //            var sEntity = entity.Set<AppSettingValue>();
            //            AppSettingValue defItem = entity.AppSettingValue.Where(p => p.SettingId == Object.SettingId && p.IsDefault == 1).FirstOrDefault();

            //            if (Object.ValId == 0)
            //            {     
            //                if (Object.IsDefault == 1 )
            //                { // get the row with same SettingId of newObject
            //                     if (defItem != null)
            //                    {
            //                        defItem.IsDefault = 0;
            //                        entity.SaveChanges();
            //                    }
            //                }
            //                else //Object.IsDefault ==0 
            //                {
            //                    if (defItem == null)//other values IsDefault not 1 
            //                    {
            //                        Object.IsDefault =1;
            //                    }

            //                }
            //                    sEntity.Add(Object);
            //              message = Object.ValId.ToString();
            //                entity.SaveChanges();
            //            }
            //            else
            //            {
            //                if (Object.IsDefault == 1)
            //                {
            //                    defItem.IsDefault = 0;//reset the other default to 0 if exist
            //                }
            //                var tmps = entity.AppSettingValue.Where(p => p.ValId == Object.ValId).FirstOrDefault();
            //                tmps.ValId = Object.ValId;                          
            //                tmps.Notes = Object.Notes;
            //                tmps.Value = Object.Value;
            //                tmps.IsDefault=Object.IsDefault;
            //                tmps.IsSystem=Object.IsSystem;

            //                tmps.SettingId=Object.SettingId;
            //                entity.SaveChanges();
            //                message = tmps.ValId.ToString();
            //            }


            //        }
            //        return message; ;
            //    }

            //    catch
            //    {
            //        return "-1";
            //    }
            //}
            //else
            //    return "-1";
        }

        // add or update medal 
        [HttpPost]
        [Route("SaveList")]
        public string SaveList(string token)
        {
            //string Object string newObject
            string message = "";



            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                string Object = "";
               List< AppSettingValue> newObject = new List<AppSettingValue>();
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "Object")
                    {
                        //Object = c.Value.Replace("\\", string.Empty);
                        //Object = Object.Trim('"');
                        Object = c.Value;
                        newObject = JsonConvert.DeserializeObject<List<AppSettingValue>>(Object, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                if (newObject != null)
                {


                   


                    try
                    {
                        int res = 0;
                        if (newObject.Count()>0)
                        {
                            foreach (AppSettingValue valrow in newObject)
                            {
                                res = Save(valrow);
                            }
                        }
                     
                        return TokenManager.GenerateToken(res.ToString());

                    }
                    catch
                    {
                        message = "0";
                        return TokenManager.GenerateToken(message);
                    }


                }

                return TokenManager.GenerateToken(message);

            }

      
        }



        //email temp  
        [HttpPost]
        [Route("SaveValueByNotes")]
      public string   SaveValueByNotes(string token)
        {

            //string Object string newObject
            string message = "";
           
            
            
          token = TokenManager.readToken(HttpContext.Current.Request); 
 var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                string Object = "";
                AppSettingValue newObject = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "Object")
                    {
                        //Object = c.Value.Replace("\\", string.Empty);
                        //Object = Object.Trim('"');
                        Object = c.Value;
                        newObject = JsonConvert.DeserializeObject<AppSettingValue>(Object, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                if (newObject != null)
                {


                    AppSettingValue tmpObject = null;


                    try
                    {
                        if (newObject.SettingId == 0 || newObject.SettingId == null)
                        {
                            Nullable<int> id = null;
                            newObject.SettingId = id;
                        }
                        using (EasyGoDBEntities entity = new EasyGoDBEntities())
                        {
                            AppSettingValue defItem = new AppSettingValue();
                            var sEntity = entity.Set<AppSettingValue>();

                            defItem = entity.AppSettingValue.Where(p => p.SettingId == newObject.SettingId).FirstOrDefault();



                            if (newObject.ValId == 0)
                            {
                                if (newObject.IsDefault == 1)
                                {
                                    // get the row with same SettingId of newObject
                                    if (defItem != null)
                                    {
                                        defItem.IsDefault = 0;
                                        entity.SaveChanges();
                                    }
                                }
                                else //newObject.IsDefault ==0 
                                {
                                    if (defItem == null)//other values IsDefault not 1 
                                    {
                                        newObject.IsDefault = 1;
                                    }

                                }
                                sEntity.Add(newObject);
                                message = newObject.ValId.ToString();
                                entity.SaveChanges();
                            }
                            else
                            {
                                if (newObject.IsDefault == 1)
                                {
                                    defItem.IsDefault = 0;//reset the other default to 0 if exist
                                }
                                var tmps1 = sEntity.ToList();
                                tmpObject = tmps1.Where(p => p.Notes == newObject.Notes && p.SettingId == newObject.SettingId && p.ValId == newObject.ValId).FirstOrDefault();
                                //   tmpObject.ValId = newObject.ValId;
                                // tmpObject.Notes = newObject.Notes;
                                tmpObject.Value = newObject.Value;
                                tmpObject.IsDefault = newObject.IsDefault;
                                tmpObject.IsSystem = newObject.IsSystem;

                                tmpObject.SettingId = newObject.SettingId;
                                entity.SaveChanges();
                                message = tmpObject.ValId.ToString();
                            }


                        }
                       

                        return TokenManager.GenerateToken(message);

                    }
                    catch
                    {
                        message = "0";
                      return TokenManager.GenerateToken(message);
                    }


                }

              return TokenManager.GenerateToken(message);

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
            //    newObject = newObject.Replace("\\", string.Empty);
            //    newObject = newObject.Trim('"');
            //    AppSettingValue Object = JsonConvert.DeserializeObject<AppSettingValue>(newObject, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
            //    try
            //    {
            //        if (Object.SettingId == 0 || Object.SettingId == null)
            //        {
            //            Nullable<int> id = null;
            //            Object.SettingId = id;
            //        }
            //        using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //        {
            //            AppSettingValue defItem = new AppSettingValue();
            //            var sEntity = entity.Set<AppSettingValue>();

            //                defItem = entity.AppSettingValue.Where(p => p.SettingId == Object.SettingId ).FirstOrDefault();



            //            if (Object.ValId == 0)
            //            {
            //                if (Object.IsDefault == 1)
            //                {
            //                    // get the row with same SettingId of newObject
            //                    if (defItem != null)
            //                    {
            //                        defItem.IsDefault = 0;
            //                        entity.SaveChanges();
            //                    }
            //                }
            //                else //Object.IsDefault ==0 
            //                {
            //                    if (defItem == null)//other values IsDefault not 1 
            //                    {
            //                        Object.IsDefault = 1;
            //                    }

            //                }
            //                sEntity.Add(Object);
            //                message = Object.ValId.ToString();
            //                entity.SaveChanges();
            //            }
            //            else
            //            {
            //                if (Object.IsDefault == 1)
            //                {
            //                    defItem.IsDefault = 0;//reset the other default to 0 if exist
            //                }
            //                var tmps1 = sEntity.ToList();
            //                var tmps = tmps1.Where(p => p.Notes == Object.Notes &&  p.SettingId == Object.SettingId && p.ValId == Object.ValId).FirstOrDefault();
            //             //   tmps.ValId = Object.ValId;
            //               // tmps.Notes = Object.Notes;
            //                tmps.Value = Object.Value;
            //                tmps.IsDefault = Object.IsDefault;
            //                tmps.IsSystem = Object.IsSystem;

            //                tmps.SettingId = Object.SettingId;
            //                entity.SaveChanges();
            //                message = tmps.ValId.ToString();
            //            }


            //        }
            //        return message; ;
            //    }

            //    catch (Exception ex)
            //    {
            //        return ex.ToString();
            //    }
            //}
            //else
            //    return "-2";
        }

        [HttpPost]
        [Route("Delete")]
      public string   Delete(string token)
        {


            // public ResponseVM Delete(string token)int Id, int userId
            //int Id, int userId
            string message = "";
           
            
            
          token = TokenManager.readToken(HttpContext.Current.Request); 
 var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int Id = 0;
                int userId = 0;


                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "Id")
                    {
                        Id = int.Parse(c.Value);
                    }
                    else if (c.Type == "userId")
                    {
                        userId = int.Parse(c.Value);
                    }

                }

                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        AppSettingValue sObj = entity.AppSettingValue.Find(Id);

                        entity.AppSettingValue.Remove(sObj);
                        message = entity.SaveChanges().ToString();

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
            //if (headers.Contains("APIKey"))
            //{
            //    token = headers.GetValues("APIKey").First();
            //}

            //Validation validation = new Validation();
            //bool valid = validation.CheckApiKey(token);
            //if (valid)
            //{
               
            //        try
            //        {
            //            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //            {
            //                AppSettingValue sObj = entity.AppSettingValue.Find(Id);
                       
            //                entity.AppSettingValue.Remove(sObj);
            //                entity.SaveChanges();

            //                return Ok("medal is Deleted Successfully");
            //            }
            //        }
            //        catch
            //        {
            //            return NotFound();
            //        }
                
                

               
            //}
            //else
            //    return NotFound();
        }
        // image
        #region Image

        [Route("PostImage")]
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

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png, .jfif, .bmp , .jpeg ,.tiff");
                            return Ok(message);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            return Ok(message);
                        }
                        else
                        {
                            //  check if image exist
                            var pathCheck = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\setvalues"), imageWithNoExt);
                            var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\setvalues"), imageWithNoExt + ".*");
                            if (files.Length > 0)
                            {
                                File.Delete(files[0]);
                            }

                            //Userimage myfolder Name where i want to save my image
                            var filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\setvalues"), imageName);
                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Ok(message1);
                }
                var res = string.Format("Please Upload a image.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");

                return Ok(res);
            }
        }

        //[HttpGet]
        //[Route("GetImage")]
        //public HttpResponseMessage GetImage(string imageName)
        //{
        //    if (String.IsNullOrEmpty(imageName))
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);

        //    string localFilePath;

        //    localFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\setvalues"), imageName);

        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
        //    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    response.Content.Headers.ContentDisposition.FileName = imageName;

        //    return response;
        //}

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
                    localFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\setvalues"), imageName);

                    byte[] b = System.IO.File.ReadAllBytes(localFilePath);
                    return TokenManager.GenerateToken(Convert.ToBase64String(b));
                }
                catch
                {
                    return TokenManager.GenerateToken(null);

                }
            }
        }
        // update database record
        [HttpPost]
        [Route("UpdateImage")]
        public string UpdateImage(string token)
        {
            //SetValuesObject
            string message = "";



            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                string Object = "";
                AppSettingValue newObject = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "Object")
                    {
                        //Object = c.Value.Replace("\\", string.Empty);
                        //Object = Object.Trim('"');
                        Object = c.Value;
                        newObject = JsonConvert.DeserializeObject<AppSettingValue>(Object, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                      
                    }
                }
                if (newObject != null)
                {

                    try
                    {
                        AppSettingValue Setvalue;
                        using (EasyGoDBEntities entity = new EasyGoDBEntities())
                        {
                            var Entity = entity.Set<AppSettingValue>();
                            Setvalue = entity.AppSettingValue.Where(p => p.ValId == newObject.ValId).First();
                            Setvalue.Value = newObject.Value;
                            entity.SaveChanges();
                        }
                       // return Setvalue.ValId;
                        return TokenManager.GenerateToken(Setvalue.ValId.ToString());
                    }


                    catch
                    {
                        message = "0";
                        return TokenManager.GenerateToken(message);
                    }



                }
                else
                {
                    return TokenManager.GenerateToken(message);
                }

            }

            //var re = Request;
            //var headers = re.Headers;
            //string token = "";
            //if (headers.Contains("APIKey"))
            //{
            //    token = headers.GetValues("APIKey").First();
            //}
            //Validation validation = new Validation();
            //bool valid = validation.CheckApiKey(token);

            //SetValuesObject = SetValuesObject.Replace("\\", string.Empty);
            //SetValuesObject = SetValuesObject.Trim('"');

            //AppSettingValue setvalObj = JsonConvert.DeserializeObject<AppSettingValue>(SetValuesObject, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
            ///*
            //if (userObj.updateUserId == 0 || userObj.updateUserId == null)
            //{
            //    Nullable<int> id = null;
            //    userObj.updateUserId = id;
            //}
            //if (userObj.createUserId == 0 || userObj.createUserId == null)
            //{
            //    Nullable<int> id = null;
            //    userObj.createUserId = id;
            //}
            //*/
            //if (valid)
            //{
            //    try
            //    {
            //        AppSettingValue Setvalue;
            //        using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //        {
            //            var Entity = entity.Set<AppSettingValue>();
            //            Setvalue = entity.AppSettingValue.Where(p => p.ValId == setvalObj.ValId).First();
            //            Setvalue.Value = setvalObj.Value;
            //            entity.SaveChanges();
            //        }
            //        return Setvalue.ValId;
            //    }

            //    catch
            //    {
            //        return 0;
            //    }
            //}
            //else
            //    return 0;
        }

        #endregion 
        public int Save(AppSettingValue newObject)
        {
            //string Object string newObject
            string message = "";
            int res = 0;

            if (newObject != null)
            {


                AppSettingValue tmpObject = null;


                try
                {
                    if (newObject.SettingId == 0 || newObject.SettingId == null)
                    {
                        Nullable<int> id = null;
                        newObject.SettingId = id;
                    }
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var sEntity = entity.Set<AppSettingValue>();
                        AppSettingValue defItem = entity.AppSettingValue.Where(p => p.SettingId == newObject.SettingId && p.IsDefault == 1).FirstOrDefault();

                        if (newObject.ValId == 0)
                        {
                            if (newObject.IsDefault == 1)
                            { // get the row with same SettingId of newObject
                                if (defItem != null)
                                {
                                    defItem.IsDefault = 0;
                                    entity.SaveChanges();
                                }
                            }
                            else //Object.IsDefault ==0 
                            {
                                if (defItem == null)//other values IsDefault not 1 
                                {
                                    newObject.IsDefault = 1;
                                }

                            }
                            sEntity.Add(newObject);
                            res = newObject.ValId;

                            message = res.ToString();
                            entity.SaveChanges();
                        }
                        else
                        {
                            //update
                            if (newObject.IsDefault == 1)
                            {
                                defItem.IsDefault = 0;//reset the other default to 0 if exist
                            }
                            tmpObject = entity.AppSettingValue.Where(p => p.ValId == newObject.ValId).FirstOrDefault();
                            tmpObject.ValId = newObject.ValId;
                            tmpObject.Notes = newObject.Notes;
                            tmpObject.Value = newObject.Value;
                            tmpObject.IsDefault = newObject.IsDefault;
                            tmpObject.IsSystem = newObject.IsSystem;

                            tmpObject.SettingId = newObject.SettingId;
                            entity.SaveChanges();
                            res = tmpObject.ValId;
                            message = res.ToString();
                        }


                    }

                    return (res);

                }
                catch
                {
                    message = "0";
                    return 0;
                }


            }

            return res;



        }

    }
}