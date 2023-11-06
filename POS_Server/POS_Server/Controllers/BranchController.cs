using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using POS_Server.Classes;
using POS_Server.Models;
using POS_Server.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/Branch")]
    public class BranchController : ApiController
    {
        CountriesController cc = new CountriesController();
        [HttpPost]
        [Route("Get")]
        public string Get(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            string type = "";
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {

                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {


                    var branchesList = entity.Branch
                        .Where(b => b.IsActive == true)
                   .Select(b => new BranchModel
                   {
                       BranchId = b.BranchId,
                       Address = b.Address,
                       CreateDate = b.CreateDate,
                       CreateUserId = b.CreateUserId,
                       Email = b.Email,
                       Mobile = b.Mobile,
                       Name = b.Name,
                       Code = b.Code,
                       Notes = b.Notes,
                       ParentId = b.ParentId,
                       Phone = b.Phone,
                       UpdateDate = b.UpdateDate,
                       UpdateUserId = b.UpdateUserId,
                       IsActive = b.IsActive,
                       Type = b.Type
                   })
                   .ToList();

                  
                    return TokenManager.GenerateToken(branchesList);
                }
            }
        }
      

    }
}