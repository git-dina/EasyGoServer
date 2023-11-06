using POS_Server.Models;
using POS_Server.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }
}