using Newtonsoft.Json;
using POS_Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data.Entity.Migrations;
using POS_Server.Models.VM;
using System.Security.Claims;
using Newtonsoft.Json.Converters;
using System.Web;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/Category")]
    public class CategoryController : ApiController
    {
        CountriesController cc = new CountriesController();
        // GET api/category
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
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var categoriesList = (from p in entity.Category 
                            where p.IsActive == true
                             select new CategoryModel()
                             {
                                 CategoryId = p.CategoryId,
                                 Name = p.Name,
                                 Code = p.Code,
                                 CreateDate = p.CreateDate,
                                 CreateUserId = p.CreateUserId,
                                 Details = p.Details,
                                 Image = p.Image,
                                 Notes = p.Notes,
                                 ParentId = p.ParentId,
                                 UpdateDate = p.UpdateDate,
                                 UpdateUserId = p.UpdateUserId,
                                 IsActive = p.IsActive,
                                 
                             }).ToList();                   
                    
                    return TokenManager.GenerateToken(categoriesList);

                }
            }
        }

      
        // add or update category
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
                Category newObject = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        newObject = JsonConvert.DeserializeObject<Category>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                if (newObject.UpdateUserId == 0 || newObject.UpdateUserId == null)
                {
                    Nullable<int> id = null;
                    newObject.UpdateUserId = id;
                }
                if (newObject.CreateUserId == 0 || newObject.CreateUserId == null)
                {
                    Nullable<int> id = null;
                    newObject.CreateUserId = id;
                }
                try
                {
                    Category tmpCategory;                    
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var categoryEntity = entity.Set<Category>();

                        if (newObject.CategoryId == 0)
                        {
                            newObject.CreateDate = cc.AddOffsetTodate(DateTime.Now);
                            newObject.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            newObject.UpdateUserId = newObject.CreateUserId;

                            tmpCategory = categoryEntity.Add(newObject);
                          
                           entity.SaveChanges();
                        }
                        else
                        {
                            tmpCategory = entity.Category.Where(p => p.CategoryId == newObject.CategoryId).First();
                            tmpCategory.Code = newObject.Code;
                            tmpCategory.Details = newObject.Details;
                            tmpCategory.Name = newObject.Name;
                            tmpCategory.Notes = newObject.Notes;
                            tmpCategory.ParentId = newObject.ParentId;

                            tmpCategory.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            tmpCategory.UpdateUserId = newObject.UpdateUserId;
                            tmpCategory.IsActive = newObject.IsActive;
                            entity.SaveChanges();
                            int CategoryId = tmpCategory.CategoryId;
                            bool? isActivecat = tmpCategory.IsActive;
                            long? updateuser = tmpCategory.UpdateUserId;
                            //update is active sons and items sons
                            // get all sub Category of CategoryId

                            List<Category> categoriesList = entity.Category
                             .ToList()
                              .Select(p => new Category
                              {
                                  CategoryId = p.CategoryId,
                                  Name = p.Name,
                                  ParentId = p.ParentId,
                              })
                             .ToList();

                            categoriesId = new List<int>();
                            List<int> catIdlist = new List<int>();
                            categoriesId.Add(CategoryId);
                            ItemsController icls = new ItemsController();

                            var result = Recursive(categoriesList, CategoryId).ToList();


                            foreach (var r in result)
                            {
                                catIdlist.Add(r.CategoryId);

                            }

                            // end sub cat
                            // disactive selected category
                       
                            // disactive subs Category

                            List<Category> sonList = entity.Category.Where(U => catIdlist.Contains(U.CategoryId)).ToList();

                            if (sonList.Count > 0)
                            {
                                for (int i = 0; i < sonList.Count; i++)
                                {

                                    sonList[i].IsActive = isActivecat;
                                    sonList[i].UpdateUserId = updateuser;
                                    sonList[i].UpdateDate = cc.AddOffsetTodate(DateTime.Now);


                                    entity.Category.AddOrUpdate(sonList[i]);

                                }
                                entity.SaveChanges();
                            }
                            if (tmpCategory.fixedTax == 1)
                            {
                                var Category = entity.Category.Where(U => catIdlist.Contains((int)U.CategoryId)).ToList();
                                Category.ForEach(a => a.taxes = tmpCategory.taxes);
                            }
                            // disactive items related to selected category and subs
                            catIdlist.Add(CategoryId);
                           
                            var catitems = entity.items.Where(U => catIdlist.Contains((int)U.CategoryId)).ToList();
                            if (catitems.Count > 0)
                            {
                                for (int i = 0; i < catitems.Count; i++)
                                {
                                    if(tmpCategory.fixedTax == 1)
                                        catitems[i].taxes = tmpCategory.taxes;
                                    catitems[i].IsActive = (byte)isActivecat;
                                    catitems[i].UpdateUserId = updateuser;
                                    catitems[i].UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                                    entity.items.AddOrUpdate(catitems[i]);

                                }
                                entity.SaveChanges();
                            }
                        }
                    }
                    message =  tmpCategory.CategoryId.ToString();
                    return TokenManager.GenerateToken(message);
                }
                catch
                {
                    message = "0";
                    return TokenManager.GenerateToken(message);
                }
            }
        }
        [HttpPost]
        [Route("Delete")]
        public string Delete(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            string message = "0";
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int CategoryId = 0;
                int userId = 0;
                Boolean final = false;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemId")
                    {
                        CategoryId = int.Parse(c.Value);
                    }
                    else if (c.Type == "userId")
                    {
                        userId = int.Parse(c.Value);
                    }
                    else if (c.Type == "final")
                    {
                        final = bool.Parse(c.Value);
                    }
                }
                if (final)
                {
                    try
                    {
                        using (EasyGoDBEntities entity = new EasyGoDBEntities())
                        {
                            var childCategories = entity.Category.Where(u => u.ParentId == CategoryId && u.IsActive == 1).FirstOrDefault();

                            if (childCategories == null)
                            {
                               // entity.categoryuser.RemoveRange(entity.categoryuser.Where(x => x.CategoryId == CategoryId));

                                var tmpCategory = entity.Category.Where(p => p.CategoryId == CategoryId).First();
                                entity.Category.Remove(tmpCategory);

                                message = entity.SaveChanges().ToString();
                                return TokenManager.GenerateToken(message);
                            }
                            else
                                message = "0";
                            return TokenManager.GenerateToken(message);
                        }
                    }
                    catch
                    {
                        message = "0";
                        return TokenManager.GenerateToken(message);
                    }
                }
                else
                {
                    try
                    {
                        using (EasyGoDBEntities entity = new EasyGoDBEntities())
                        {  // get all sub Category of CategoryId
                            List<Category> categoriesList = entity.Category
                             .ToList()
                              .Select(p => new Category
                              {
                                  CategoryId = p.CategoryId,
                                  Name = p.Name,
                                  ParentId = p.ParentId,
                              })
                             .ToList();

                            categoriesId = new List<int>();
                            List<int>  catIdlist = new List<int>();
                            categoriesId.Add(CategoryId);
                            ItemsController icls = new ItemsController();
                           
                            var result =Recursive(categoriesList, CategoryId).ToList();
                           
                            
                            foreach (var r in result)
                            {
                                catIdlist.Add(r.CategoryId);
                             
                            }
                            
                            // end sub cat
                            // disactive selected category
                            var tmpCategory = entity.Category.Where(p => p.CategoryId == CategoryId).First();
                            tmpCategory.IsActive = 0;
                            tmpCategory.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            tmpCategory.UpdateUserId = userId;
                            entity.Category.AddOrUpdate(tmpCategory);
                            entity.SaveChanges();

                       // disactive subs Category

                           List<Category> sonList = entity.Category.Where(U => catIdlist.Contains(U.CategoryId)).ToList();

                            if (sonList.Count > 0)
                            {
                                for (int i = 0; i < sonList.Count; i++)
                                {
                                    sonList[i].IsActive = 0;
                                    sonList[i].UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                                    sonList[i].UpdateUserId = userId;
                                    entity.Category.AddOrUpdate(sonList[i]);

                                }
                                entity.SaveChanges();
                            }
                            // disactive items related to selected category and subs
                            catIdlist.Add(CategoryId);
                              var catitems = entity.items.Where(U => catIdlist.Contains((int)U.CategoryId)).ToList();
                                if (catitems.Count > 0)
                                {
                                    for (int i = 0; i < catitems.Count; i++)
                                    {
                                    catitems[i].IsActive = 0;
                                    catitems[i].UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                                    catitems[i].UpdateUserId = userId;
                                    entity.items.AddOrUpdate(catitems[i]);
                                   
                                    }
                                   entity.SaveChanges();

                                }



                            message = "1";
                            return TokenManager.GenerateToken(message);

                        }
                    }
                    catch
                    {
                        message = "0";
                        return TokenManager.GenerateToken(message);
                    }

                }
            }
        }


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
                string categoryObject = "";
                Category catObj = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        categoryObject = c.Value.Replace("\\", string.Empty);
                        categoryObject = categoryObject.Trim('"');
                        catObj = JsonConvert.DeserializeObject<Category>(categoryObject, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                try
                {
                    Category category;
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var agentEntity = entity.Set<agents>();
                        category = entity.Category.Where(p => p.CategoryId == catObj.CategoryId).First();
                        category.Image = catObj.Image;
                        entity.SaveChanges();
                    }
                    message =  category.CategoryId.ToString();
                    return TokenManager.GenerateToken(message);
                }

                catch
                {
                    message = "0";
                    return TokenManager.GenerateToken(message);
                }
            }
        }
        [Route("PostCategoryImage")]
        public IHttpActionResult PostCategoryImage()
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

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".bmp", ".jpeg", ".tiff",".jfif" };
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
                            //  check if Image exist
                            var pathCheck = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\category"), imageWithNoExt);
                            var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\category"), imageWithNoExt + ".*");
                            if (files.Length > 0)
                            {
                                File.Delete(files[0]);
                            }

                            //Userimage myfolder Name where i want to save my Image
                            var filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\category"), imageName);
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

        //    localFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\category"), imageName);

        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    if (System.IO.File.Exists(localFilePath))
        //    {
        //        response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
        //        response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //        response.Content.Headers.ContentDisposition.FileName = imageName;
        //    }
        //    else
        //    {
        //        response.Content = null;
        //    }
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
                    localFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\category"), imageName);

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
        public IEnumerable<Category> Recursive(List<Category> categoriesList, int toplevelid)
        {
            List<Category> inner = new List<Category>();

            foreach (var t in categoriesList.Where(item => item.ParentId == toplevelid))
            {
                categoriesId.Add(t.CategoryId);
                inner.Add(t);
                inner = inner.Union(Recursive(categoriesList, t.CategoryId)).ToList();
            }

            return inner;
        }

        }
}