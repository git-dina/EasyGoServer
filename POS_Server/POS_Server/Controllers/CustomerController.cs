using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using POS_Server.Classes;
using POS_Server.Models;
using POS_Server.Models.VM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        CountriesController cc = new CountriesController();
        // GET api/Customer
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
                    var agentsList = entity.Customer
                   .Where(p =>  p.IsActive == true)
                   .Select(p => new CustomerModel
                   {
                       CustomerId = p.CustomerId,
                       Name = p.Name,
                       Code = p.Code,
                       Company = p.Company,
                       Address = p.Address,
                       Email = p.Email,
                       Mobile = p.Mobile,
                       Image = p.Image,
                       Balance = p.Balance,
                       BalanceType = p.BalanceType,
                       Notes = p.Notes,
                       IsActive = p.IsActive,
                       CreateDate = p.CreateDate,
                       UpdateDate = p.UpdateDate,
                       MaxDeserve = p.MaxDeserve,
                       Fax = p.Fax,
                       IsLimited = p.IsLimited,
                       PayType = p.PayType
                    })
                   .ToList();
                   
                    return TokenManager.GenerateToken(agentsList);
                }
            }
        }
       
        [HttpPost]
        [Route("GetActiveForAccount")]
        public string GetActiveForAccount(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);

            string Type = "";
            string PayType = "";
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {

                    if (c.Type == "PayType")
                    {
                        PayType = c.Value;
                    }
                }
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {

                    var agentsList = entity.Customer
                   .Where(p =>  (p.IsActive == true || 
                                                 (p.IsActive == false && PayType == "p" && p.BalanceType == 0) || 
                                                 (p.IsActive == false && PayType == "d" && p.BalanceType == 1)))
                   .Select(p => new
                   {
                       p.CustomerId,
                       p.Name,
                       p.Code,
                       p.Company,
                       p.Address,
                       p.Email,
                       p.Mobile,
                       p.Image,
                       p.Balance,
                       p.BalanceType,
                       p.Notes,
                       p.MaxDeserve,
                       p.Fax,
                       p.IsActive,
                       p.CreateDate,
                       p.IsLimited,
                       p.PayType
                   })
                   .ToList();

                   

                    return TokenManager.GenerateToken(agentsList);

                }
            }
        }

        [HttpPost]
        [Route("GetCustomerByID")]
        public string GetCustomerByID(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                    long agentId = 0;
                    IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                    foreach (Claim c in claims)
                    {
                        if (c.Type == "agentId")
                        {
                            agentId = long.Parse(c.Value);
                        }
                    }
                    var agent = GetCustomerByID(agentId);
                return TokenManager.GenerateToken(agent);

            }
        }

        public CustomerModel GetCustomerByID(long agentId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var agent = entity.Customer
               .Where(p => p.CustomerId == agentId)
               .Select(p => new CustomerModel
               {
                   CustomerId = p.CustomerId,
                   Name = p.Name,
                   Code = p.Code,
                   Company = p.Company,
                   Address = p.Address,
                   Email = p.Email,
                   Mobile = p.Mobile,
                   Image = p.Image,
                   Balance = p.Balance,
                   BalanceType = p.BalanceType,
                   Notes = p.Notes,
                   IsActive = p.IsActive,
                   CreateDate = p.CreateDate,
                   UpdateDate = p.UpdateDate,
                   MaxDeserve = p.MaxDeserve,
                   Fax = p.Fax,
                   IsLimited = p.IsLimited,
                   PayType = p.PayType
               }).FirstOrDefault();
                return agent;
            }
        }
        // add or update agent
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
                Customer agentObj = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        agentObj = JsonConvert.DeserializeObject<Customer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                try
                {
                    Customer agent;
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var agentEntity = entity.Set<Customer>();
                        if (agentObj.CustomerId == 0)
                        {
                            //ProgramInfo programInfo = new ProgramInfo();
                            //int agentMaxCount = 0;
                            //if (agentObj.Type == "c")
                            //    agentMaxCount = programInfo.getCustomerCount();
                            //else if (agentObj.Type == "v")
                            //    agentMaxCount = programInfo.getVendorCount();

                            //int agentCount = entity.Customer.Where(x => x.Type == agentObj.Type).Count();
                            //if (agentCount >= agentMaxCount && agentMaxCount != -1)
                            //{
                            //    message = "upgrade";
                            //    return TokenManager.GenerateToken(message);
                            //}
                            //else
                            {
                           
                                agentObj.CreateDate = cc.AddOffsetTodate(DateTime.Now);
                                agentObj.UpdateDate = agentObj.CreateDate;
                                agentObj.UpdateUserId = agentObj.CreateUserId;
                                agentObj.BalanceType = 0;
                                agentObj.Code = GetLastNumOfCode();
                                agent = agentEntity.Add(agentObj);
                            }
                        }
                        else
                        {
                            agent = entity.Customer.Where(p => p.CustomerId == agentObj.CustomerId).First();

                            agent.Address = agentObj.Address;
                            agent.Company = agentObj.Company;
                            agent.Email = agentObj.Email;
                            agent.Image = agentObj.Image;
                            agent.Mobile = agentObj.Mobile;
                            agent.Name = agentObj.Name;
                            agent.Notes = agentObj.Notes;
                            agent.MaxDeserve = agentObj.MaxDeserve;
                            agent.Fax = agentObj.Fax;
                            agent.UpdateDate = cc.AddOffsetTodate(DateTime.Now); ;// server current date
                            agent.UpdateUserId = agentObj.UpdateUserId;
                            agent.IsActive = agentObj.IsActive;
                            agent.Balance = agentObj.Balance;
                            agent.BalanceType = agentObj.BalanceType;
                            agent.IsLimited = agentObj.IsLimited;
                            agent.PayType = agentObj.PayType;
                        }
                        entity.SaveChanges();
                        message = agent.CustomerId.ToString();

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
        private string GetLastNumOfCode()
        {
            long lastNum = 0;
            List<string> numberList;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                numberList = entity.Customer.Where(b => b.Code.Contains("c-")).Select(b => b.Code).ToList();

                for (int i = 0; i < numberList.Count; i++)
                {
                    string Code = numberList[i];
                    string s = Code.Substring(Code.LastIndexOf("-") + 1);
                    numberList[i] = s;
                }
                if (numberList.Count > 0)
                {
                    numberList.Sort();
                    lastNum = int.Parse(numberList[numberList.Count - 1]);
                }
            }
            lastNum++;
            string strSeq = lastNum.ToString();
            if (lastNum <= 999999)
                strSeq = lastNum.ToString().PadLeft(6, '0');
            string transNum =  "c-" + strSeq;
            return transNum;
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
                long agentId = 0;
                long userId = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemId")
                    {
                        agentId = long.Parse(c.Value);
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

                        Customer tmp = entity.Customer.Find(agentId);
                        entity.Customer.Remove(tmp);
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
                            Customer tmp = entity.Customer.Find(agentId);

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

        [Route("PostUserImage")]
        public  IHttpActionResult PostUserImage()
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

                            var message = string.Format("Please Upload Image of Type .jpg,.gif,.png, .jfif, .bmp , .jpeg ,.tiff");
                            return Ok(message);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            return Ok(message);
                        }
                        else
                        {
                            var dir = System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\customer");
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);
                            //  check if Image exist
                            var pathCheck = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\customer"), imageWithNoExt);
                            var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\customer"), imageWithNoExt + ".*");
                            if (files.Length > 0)
                            {
                                File.Delete(files[0]);
                            }

                            //Userimage myfolder Name where i want to save my Image
                            var filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\customer"), imageName);
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
                    localFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\customer"), imageName);

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
            string message = "";
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                Customer agentObj = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        agentObj = JsonConvert.DeserializeObject<Customer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                try
                {
                    Customer agent;
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var agentEntity = entity.Set<Customer>();                      
                        agent = entity.Customer.Where(p => p.CustomerId == agentObj.CustomerId).First();
                        agent.Image = agentObj.Image;
                        entity.SaveChanges();
                    }
                    message = agent.CustomerId.ToString();
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
        [Route("UpdateBalance")]
        public string UpdateBalance(string token)
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
                long agentId = 0;
                decimal balance = 0;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "agentId")
                    {
                        agentId = long.Parse(c.Value);
                    }
                    else if (c.Type == "balance")
                    {
                        balance = int.Parse(c.Value);
                    }
                }
                try
                {
                    Customer agent;
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var agentEntity = entity.Set<Customer>();
                        agent = entity.Customer.Where(p => p.CustomerId == agentId).First();
                        agent.Balance = balance;
                        entity.SaveChanges();
                    }
                    message = agent.CustomerId.ToString();
                    return TokenManager.GenerateToken(message);
                }

                catch
                {
                    message = "0";
                    return TokenManager.GenerateToken(message);
                }
            }
        }

      
       

    }
}
