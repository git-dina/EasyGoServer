using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    [RoutePrefix("api/Unit")]
    public class UnitsController : ApiController
    {
        CountriesController cc = new CountriesController();
        List<int> unitsIds = new List<int>();
        // GET api/<controller>
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
                try
                {

                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var unitsList = (from u in entity.Unit
                                     where u.IsActive == true
                                     select new UnitModel()
                                     {
                                         UnitId = u.UnitId,
                                         Name = u.Name,
                                         Notes = u.Notes,
                                         CreateDate = u.CreateDate,
                                         CreateUserId = u.CreateUserId,
                                         UpdateDate = u.UpdateDate,
                                         UpdateUserId = u.UpdateUserId,
                                         IsActive = u.IsActive,
                                     }).ToList();

                   
                    return TokenManager.GenerateToken(unitsList);
                }
            }
                catch
            {
                return TokenManager.GenerateToken("failed");
            }
        }
        }

        // add or update unit
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
                Unit Object = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        Object = JsonConvert.DeserializeObject<Unit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }

                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        Unit tmpUnit = new Unit();
                        var unitEntity = entity.Set<Unit>();
                        if (Object.UnitId == 0)
                        {
                            Object.CreateDate = cc.AddOffsetTodate(DateTime.Now);
                            Object.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            Object.UpdateUserId = Object.CreateUserId;

                            unitEntity.Add(Object);
                            tmpUnit = unitEntity.Add(Object);
                            
                        }
                        else
                        {
                            tmpUnit = entity.Unit.Where(p => p.UnitId == Object.UnitId).FirstOrDefault();
                            tmpUnit.Name = Object.Name;
                            tmpUnit.Notes = Object.Notes;
                            tmpUnit.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            tmpUnit.UpdateUserId = Object.UpdateUserId;
                            tmpUnit.IsActive = Object.IsActive;

                        }
                        entity.SaveChanges();
                        message = tmpUnit.UnitId.ToString();
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
                int unitId = 0;
                long userId = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemId")
                    {
                        unitId = int.Parse(c.Value);
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

                        var tmp = entity.Unit.Find(unitId);
                        entity.Unit.Remove(tmp);
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
                            var tmp = entity.Unit.Find(unitId);

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