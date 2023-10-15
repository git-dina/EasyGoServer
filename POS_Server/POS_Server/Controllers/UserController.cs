using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using POS_Server.Classes;
using POS_Server.Models;
using POS_Server.Models.VM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        CountriesController cc = new CountriesController();

        [HttpPost]
        [Route("GetActiveForAccount")]
        public string GetActiveForAccount(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            string payType = "";

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
                    if (c.Type == "payType")
                    {
                        payType = c.Value;
                    }
                }
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var usersList = entity.User.Where(u => u.UserId != 1 && (u.IsActive == true ||
                                                 (u.IsActive == false && payType == "p" && u.BalanceType == 0) ||
                                                 (u.IsActive == false && payType == "d" && u.BalanceType == 1)))
                    .Select(u => new UserModel
                    {
                        UserId = u.UserId,
                        UserName = u.UserName,
                        Password = u.Password,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        FullName = u.FirstName + " " + u.LastName,

                        CreateDate = u.CreateDate,
                        UpdateDate = u.UpdateDate,
                        CreateUserId = u.CreateUserId,
                        UpdateUserId = u.UpdateUserId,
                        Mobile = u.Mobile,
                        Email = u.Email,
                        Notes = u.Notes,
                        Address = u.Address,
                        IsActive = u.IsActive,
                        IsOnline = u.IsOnline,
                        Image = u.Image,
                        Balance = u.Balance,
                        BalanceType = u.BalanceType,
                        HasCommission = u.HasCommission,
                        CommissionValue = u.CommissionValue,
                        CommissionRatio = u.CommissionRatio,
                    })
                    .ToList();

                    return TokenManager.GenerateToken(usersList);
                }
            }
        }

        [HttpPost]
        [Route("Getloginuser")]
        public async Task<string> Getloginuser(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            List<UserModel> usersList = new List<UserModel>();
            UserModel user = new UserModel();
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                string userName = "";
                string Password = "";
                int PosId = 0;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "userName")
                    {
                        userName = c.Value;
                    }
                    else if (c.Type == "Password")
                    {
                        Password = c.Value;
                    }
                    else if (c.Type == "PosId")
                    {
                        PosId = int.Parse(c.Value);
                    }
                }

                UserModel emptyuser = new UserModel();

                try
                {

                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        usersList = entity.User.Where(u => u.IsActive == true && u.UserName == userName)
                        .Select(u => new UserModel
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            Password = u.Password,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            FullName = u.FirstName + " " + u.LastName,

                            CreateDate = u.CreateDate,
                            UpdateDate = u.UpdateDate,
                            CreateUserId = u.CreateUserId,
                            UpdateUserId = u.UpdateUserId,

                            Mobile = u.Mobile,
                            Email = u.Email,
                            Notes = u.Notes,
                            Address = u.Address,
                            IsActive = u.IsActive,
                            IsOnline = u.IsOnline,
                            Image = u.Image,
                            Balance = u.Balance,
                            BalanceType = u.BalanceType,

                            //groupName = entity.groups.Where(g => g.groupId == u.groupId).FirstOrDefault().FirstName,
                            HasCommission = u.HasCommission,
                            CommissionValue = u.CommissionValue,
                            CommissionRatio = u.CommissionRatio,
                        })
                        .ToList();

                        if (usersList == null || usersList.Count <= 0)
                        {
                            user = emptyuser;
                            // rong user
                            return TokenManager.GenerateToken(user);
                        }
                        else
                        {
                            user = usersList.Where(i => i.UserName == userName).FirstOrDefault();
                            if (user.Password.Equals(Password))
                            {
                                #region check if user can login and set other user logOut
                                user.canLogin = await CanLogIn(user.UserId, PosId);
                                if (user.canLogin == 1 || (user.UserName == "Support@Increase" && user.IsAdmin == true))
                                {

                                    //make user online
                                    var us = entity.User.Find(user.UserId);
                                    us.IsOnline = true;

                                    UsersLogsController ulc = new UsersLogsController();
                                    ulc.checkOtherUser(user.UserId);
                                    ulc.SignOutOld(user.UserId);
                                    //create lognin record
                                    UserLog UserLog = new UserLog()
                                    {
                                        PosId = PosId,
                                        UserId = user.UserId,
                                        SInDate = cc.AddOffsetTodate(DateTime.Now),

                                    };
                                    entity.UserLog.Add(UserLog);
                                    entity.SaveChanges();
                                    user.userLogInID = UserLog.LogId;
                                    //var pos = entity.pos.Find(PosId);
                                    //user.branchId = pos.branchId;
                                }
                                #endregion
                                // correct UserName and pasword
                                return TokenManager.GenerateToken(user);
                            }
                            else
                            {
                                // rong pass return just UserName
                                user = emptyuser;
                                user.UserName = userName;
                                return TokenManager.GenerateToken(user);

                            }
                        }
                    }

                }
                catch
                {
                    return TokenManager.GenerateToken(emptyuser);
                }
            }
        }

        //GetAll
        // return all User active and inactive
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
                    var usersList = entity.User.Where(x=> x.IsActive == true)
                    .Select(u => new UserModel()
                    {
                        UserId = u.UserId,
                        UserName = u.UserName,
                        Password = u.Password,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        FullName = u.FirstName + " " + u.LastName,

                        CreateDate = u.CreateDate,
                        UpdateDate = u.UpdateDate,
                        CreateUserId = u.CreateUserId,
                        UpdateUserId = u.UpdateUserId,
  
                        Mobile = u.Mobile,
                        Email = u.Email,
                        Notes = u.Notes,
                        Address = u.Address,
                        IsActive = u.IsActive,
                        IsOnline = u.IsOnline,
                        IsAdmin = u.IsAdmin,
                        Image = u.Image,
                        Balance = u.Balance,
                        BalanceType = u.BalanceType,
                        RoleId = u.RoleId,
                        //groupId = u.groupId,
                        //groupName = entity.groups.Where(g => g.groupId == u.groupId).FirstOrDefault().FirstName,
                        HasCommission = u.HasCommission,
                        CommissionValue = u.CommissionValue,
                        CommissionRatio = u.CommissionRatio,
                    })
                    .ToList();
                  
                    return TokenManager.GenerateToken(usersList.Where(u => u.UserId != 1));
                }
            }
        }

      

        [HttpPost]
        [Route("GetUserSettings")]
        public string GetUserSettings(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);

            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int UserId = 0;
                int PosId = 0;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "UserId")
                    {
                        UserId = int.Parse(c.Value);
                    }
                    else if (c.Type == "PosId")
                    {
                        PosId = int.Parse(c.Value);
                    }
                }
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    string result = "{";

                    //get all settings

                    var settingsCls = entity.AppSetting.ToList();
                    var settingsValues = entity.AppSettingValue.ToList();
                    UserSettings usersetModel = new UserSettings();
                    #region get user language 
                    var set = settingsCls.Where(l => l.Name == "language").FirstOrDefault();

                    var lang = (from c in entity.AppSettingValue.Where(x => x.SettingId == set.SettingId)
                                join us in entity.UserSettingValue.Where(x => x.UserId == UserId) on c.ValId equals us.ValId
                                select new
                                {
                                    c.ValId,
                                    c.Value,
                                    c.IsDefault,
                                    c.IsSystem,
                                    c.Notes,
                                    c.SettingId,

                                }).FirstOrDefault();

                    string langVal = "";
                    if (lang == null)
                        langVal = "en";
                    else
                        langVal = lang.Value;
                    
                    result += "userLang:'" + langVal + "'";
                    usersetModel.userLang = langVal;
                    #endregion

                    #region menuOpen
                    set = settingsCls.Where(l => l.Name == "menuIsOpen").FirstOrDefault();
                    var menu = (from c in entity.AppSettingValue.Where(x => x.SettingId == set.SettingId)
                                join us in entity.UserSettingValue.Where(x => x.UserId == UserId) on c.ValId equals us.ValId
                                select new
                                {
                                    c.ValId,
                                    c.Value,
                                    c.IsDefault,
                                    c.IsSystem,
                                    c.Notes,
                                    c.SettingId,

                                }).FirstOrDefault();

                    string menuVal = "";
                    if (menu == null)
                        menuVal = "close";
                    else
                        menuVal = menu.Value;

                    result += ",UserMenu:'" + menuVal + "'";
                    usersetModel.UserMenu = menuVal;
                    #endregion
                    #region invoiceSlice
                    var slice = entity.UserSettingValue.Where(x => x.UserId == UserId && x.Note.StartsWith("invoice_slice")).FirstOrDefault();

                    string sliceVal = "0";
                    if (slice != null)
                        sliceVal = slice.Value;

                    result += ",invoiceSlice:'" + sliceVal + "'";
                    usersetModel.invoiceSlice = int.Parse(sliceVal);
                    #endregion
                    #region user path
                    string firstPath = "";
                    int? firstId = null;
                    int? secondId = null;
                    string secondPath = "";
                    set = settingsCls.Where(l => l.Name == "user_path").FirstOrDefault();
                    var setPath = settingsValues.Where(x => x.SettingId == set.SettingId).ToList();
                    if (setPath.Count > 0)
                    {
                        firstId = setPath[0].ValId;
                        secondId = setPath[1].ValId;
                        var first = entity.UserSettingValue.Where(x => x.UserId == UserId && x.ValId == firstId).ToList();
                        var second = entity.UserSettingValue.Where(x => x.UserId == UserId && x.ValId == secondId).ToList();
                        if (first.Count > 0 && second.Count > 0)
                        {
                            firstPath = first.FirstOrDefault().Note;
                            secondPath = second.FirstOrDefault().Note;
                        }
                    }

                    result += ",firstPathId:" + firstId + ",firstPath:'" + firstPath + "',secondPathId:" + secondId + ",secondPath:'" + secondPath + "'";
                    usersetModel.firstPathId = firstId;
                    usersetModel.firstPath = firstPath;
                    usersetModel.secondPathId = secondId;
                    usersetModel.secondPath = secondPath;
                    #endregion
                    #region user Last message
                    //adminMessagesController ac = new adminMessagesController();
                    //var message = ac.GetLastMessageByUserId(UserId, PosId);
                    //if (message != null)
                    //{
                    //    result += ",messageContent:'" + message.msgContent + "'";
                    //    result += ",messageTitle:'" + message.title + "'";
                    //    //
                    //    usersetModel.messageContent = message.msgContent;
                    //    usersetModel.messageTitle = message.title;
                    //}
                    //else
                    //{
                    //    result += ",messageContent:''";
                    //    result += ",messageTitle:''";
                    //    usersetModel.messageContent = "";
                    //    usersetModel.messageTitle = "";
                    //}
                    #endregion

                    #region default system info
                    List<char> charsToRemove = new List<char>() { '@', '_', ',', '.', '-' };

                    //company FirstName
                    set = settingsCls.Where(s => s.Name == "com_name").FirstOrDefault();
                    int SettingId = set.SettingId;
                    var setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    string val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    result += ",companyName:'" + val + "'";
                    usersetModel.companyName = val;

                    //company Address
                    set = settingsCls.Where(s => s.Name == "com_address").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    result += ",Address:'" + val + "'";
                    usersetModel.Address = val;
                    //company Email
                    set = settingsCls.Where(s => s.Name == "com_email").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    result += ",Email:'" + val + "'";
                    usersetModel.Email = val;
                    //get company Mobile
                    set = settingsCls.Where(s => s.Name == "com_mobile").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                    {
                        charsToRemove.ForEach(x => setVal.Value = setVal.Value.Replace(x.ToString(), String.Empty));
                        val = setVal.Value;
                    }
                    result += ",Mobile:'" + val + "'";
                    usersetModel.Mobile = val;
                    //get company phone
                    set = settingsCls.Where(s => s.Name == "com_phone").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                    {
                        charsToRemove.ForEach(x => setVal.Value = setVal.Value.Replace(x.ToString(), String.Empty));
                        val = setVal.Value;
                    }
                    result += ",Phone:'" + val + "'";
                    usersetModel.Phone = val;
                    //get company fax
                    set = settingsCls.Where(s => s.Name == "com_fax").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                    {
                        charsToRemove.ForEach(x => setVal.Value = setVal.Value.Replace(x.ToString(), String.Empty));
                        val = setVal.Value;
                    }
                    result += ",Fax:'" + val + "'";
                    usersetModel.Fax = val;
                    //get company logo
                    set = settingsCls.Where(s => s.Name == "com_logo").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    result += ",logoImage:'" + val + "'";
                    usersetModel.logoImage = val;
                    #endregion
                    #region social
                    set = settingsCls.Where(s => s.Name == "com_website").FirstOrDefault();
                     SettingId = set.SettingId;
                      setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                   val = "";

                    if (setVal != null)
                        val = setVal.Value;
                   // result += ",com_website:'" + val + "'";
                    usersetModel.com_website = val;
                    //
                    set = settingsCls.Where(s => s.Name == "com_social").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    // result += ",com_website:'" + val + "'";
                    usersetModel.com_social = val;
                    //
                    set = settingsCls.Where(s => s.Name == "com_social_icon").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    // result += ",com_website:'" + val + "'";
                    usersetModel.com_social_icon = val;
                    #endregion
                    #region tax
                    var oneSet = settingsCls.Where(s => s.Name == "invoiceTax_bool").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",invoiceTax_bool:'" + val + "'";
                    usersetModel.invoiceTax_bool = bool.Parse(val);
                    oneSet = settingsCls.Where(s => s.Name == "invoiceTax_decimal").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "0";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",invoiceTax_decimal:" + val;
                    usersetModel.invoiceTax_decimal = decimal.Parse(val);
                    oneSet = settingsCls.Where(s => s.Name == "itemsTax_bool").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",itemsTax_bool:'" + val + "'";
                    usersetModel.itemsTax_bool = bool.Parse(val);

                    oneSet = settingsCls.Where(s => s.Name == "itemsTax_decimal").FirstOrDefault();
                    setVal = settingsValues.Where(i => i.SettingId == oneSet.SettingId).FirstOrDefault();
                    val = "0";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",itemsTax_decimal:'" + val + "'";
                    usersetModel.itemsTax_decimal = decimal.Parse(val);
 
                    #endregion
                    #region get print settings
                    var printList = entity.AppSettingValue.ToList().Where(x => x.Notes == "print")
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


                    var psetVal = printList.Where(X => X.Name == "sale_copy_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",sale_copy_count:'" + val + "'";
                    usersetModel.sale_copy_count = val;
                    psetVal = printList.Where(X => X.Name == "pur_copy_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",pur_copy_count:'" + val + "'";
                    usersetModel.pur_copy_count = val;
                    psetVal = printList.Where(X => X.Name == "print_on_save_sale").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",print_on_save_sale:'" + val + "'";
                    usersetModel.print_on_save_sale = val;
                    psetVal = printList.Where(X => X.Name == "print_on_save_pur").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",print_on_save_pur:'" + val + "'";
                    usersetModel.print_on_save_pur = val;
                    psetVal = printList.Where(X => X.Name == "email_on_save_sale").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",email_on_save_sale:'" + val + "'";
                    usersetModel.email_on_save_sale = val;
                    psetVal = printList.Where(X => X.Name == "email_on_save_pur").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",email_on_save_pur:'" + val + "'";
                    usersetModel.email_on_save_pur = val;
                    psetVal = printList.Where(X => X.Name == "rep_copy_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",rep_print_count:'" + val + "'";
                    usersetModel.rep_print_count = val;
                    psetVal = printList.Where(X => X.Name == "Allow_print_inv_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",Allow_print_inv_count:'" + val + "'";
                    usersetModel.Allow_print_inv_count = val;
                    psetVal = printList.Where(X => X.Name == "show_header").FirstOrDefault();
                    val = "1";
                    if (psetVal != null)
                    {
                        val = psetVal.Value;
                        if (val == null || val == "")
                        {
                            val = "1";
                        }
                    }
                    result += ",show_header:'" + val + "'";
                    usersetModel.show_header = val;
                    psetVal = printList.Where(X => X.Name == "itemtax_note").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",itemtax_note:'" + val + "'";
                    usersetModel.itemtax_note = val;
                    psetVal = printList.Where(X => X.Name == "sales_invoice_note").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",sales_invoice_note:'" + val + "'";
                    usersetModel.sales_invoice_note = val;
                    psetVal = printList.Where(X => X.Name == "print_on_save_directentry").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",print_on_save_directentry:'" + val + "'";
                    usersetModel.print_on_save_directentry = val;
                    psetVal = printList.Where(X => X.Name == "directentry_copy_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",directentry_copy_count:'" + val + "'";
                    usersetModel.directentry_copy_count = val;

                    //report language
                    oneSet = settingsCls.Where(s => s.Name == "report_lang").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId && i.IsDefault == 1).FirstOrDefault().Value;

                    if (val.Equals(""))
                        val = "en";
                    result += ",Reportlang:'" + val + "'";
                    usersetModel.Reportlang = val;
                    #endregion


                    #region accuracy - date form
                    oneSet = settingsCls.Where(s => s.Name == "accuracy").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId && i.IsDefault == 1).FirstOrDefault();
                    val = "0";
                    if (setVal != null)
                    {
                        val = setVal.Value;
                        if (val.Equals(""))
                            val = "0";
                    }
                    result += ",accuracy:'" + val + "'";
                    usersetModel.accuracy = val;
                    //date form
                    oneSet = settingsCls.Where(s => s.Name == "dateForm").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId && i.IsDefault == 1).FirstOrDefault();
                    val = "";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",dateFormat:'" + val + "'";
                    usersetModel.dateFormat = val;
                    //Currency info
                    var regions = entity.CountryCode.Where(x => x.IsDefault == 1).FirstOrDefault();
                    if (regions == null)
                    {
                        result += ",Currency:''" + ",CurrencyId:,CountryId:";
                        usersetModel.Currency = "";
                        usersetModel.CurrencyId = 0;
                        usersetModel.CountryId = 0;
                    }                      
                    else
                    {
                        result += ",Currency:'" + regions.Currency + "'" + ",CurrencyId:" + regions.CurrencyId + ",CountryId:" + regions.CountryId;
                        usersetModel.Currency = regions.Currency;
                        usersetModel.CurrencyId = regions.CurrencyId;
                        usersetModel.CountryId = regions.CountryId;
                    }                    
                    #endregion


                    #region storage cost
                    oneSet = settingsCls.Where(s => s.Name == "storage_cost").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;

                    if (val == "" || val == null)
                        val = "0";
                    result += ",StorageCost:" + val;
                    usersetModel.StorageCost = decimal.Parse(val);
                    #endregion
                    #region activationSite
                    oneSet = settingsCls.Where(s => s.Name == "active_site").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;


                    result += ",activationSite:'" + val + "'";
                    usersetModel.activationSite = val;

                    #endregion
                    #region invoice_lang
                    oneSet = settingsCls.Where(s => s.Name == "invoice_lang").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;
                    result += ",invoice_lang:'" + val + "'";
                    usersetModel.invoice_lang = val;
                    #endregion
                    #region com_name_ar
                    oneSet = settingsCls.Where(s => s.Name == "com_name_ar").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;
                    result += ",com_name_ar:'" + val + "'";
                    usersetModel.com_name_ar = val;
                    #endregion
                    #region com_address_ar
                    oneSet = settingsCls.Where(s => s.Name == "com_address_ar").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;
                    result += ",com_address_ar:'" + val + "'";
                    usersetModel.com_address_ar = val;
                    #endregion
                    #region Properties
                    //canSkipProperties
                    oneSet = settingsCls.Where(s => s.Name == "canSkipProperties").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",canSkipProperties:'" + val + "'";
                    usersetModel.canSkipProperties = bool.Parse(val);
                    //canSkipSerialsNum
                    oneSet = settingsCls.Where(s => s.Name == "canSkipSerialsNum").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",canSkipSerialsNum:'" + val + "'";
                    usersetModel.canSkipSerialsNum = bool.Parse(val);
                    #endregion

                    #region returnPeriod
                    oneSet = settingsCls.Where(s => s.Name == "returnPeriod").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;

                    if (val == "" || val == null)
                        val = "0";
                    result += ",returnPeriod:" + val;
                    usersetModel.returnPeriod = int.Parse(val);
                    #endregion
                    #region freeDelivery
                    oneSet = settingsCls.Where(s => s.Name == "freeDelivery").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",freeDelivery:'" + val + "'";
                    usersetModel.freeDelivery = bool.Parse(val);
                    #endregion
                    result += "}";
                    return TokenManager.GenerateToken(usersetModel);

                }
            }
        }

        [HttpPost]
        [Route("GetSettings")]
        public string GetSettings(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);

            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                UserSettings usersetModel = new UserSettings();
                usersetModel = GetSettings();
                return TokenManager.GenerateToken(usersetModel);
            }
        }

        public UserSettings GetSettings( )
        {
            
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    string result = "{";

                    //get all settings

                    var settingsCls = entity.AppSetting.ToList();
                    var settingsValues = entity.AppSettingValue.ToList();
                    UserSettings usersetModel = new UserSettings();
                   

                    #region default system info
                    List<char> charsToRemove = new List<char>() { '@', '_', ',', '.', '-' };

                    //company FirstName
                  var  set = settingsCls.Where(s => s.Name == "com_name").FirstOrDefault();
                    int SettingId = set.SettingId;
                    var setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    string val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    result += ",companyName:'" + val + "'";
                    usersetModel.companyName = val;

                    //company Address
                    set = settingsCls.Where(s => s.Name == "com_address").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    result += ",Address:'" + val + "'";
                    usersetModel.Address = val;
                    //company Email
                    set = settingsCls.Where(s => s.Name == "com_email").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    result += ",Email:'" + val + "'";
                    usersetModel.Email = val;
                    //get company Mobile
                    set = settingsCls.Where(s => s.Name == "com_mobile").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                    {
                        charsToRemove.ForEach(x => setVal.Value = setVal.Value.Replace(x.ToString(), String.Empty));
                        val = setVal.Value;
                    }
                    result += ",Mobile:'" + val + "'";
                    usersetModel.Mobile = val;
                    //get company phone
                    set = settingsCls.Where(s => s.Name == "com_phone").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                    {
                        charsToRemove.ForEach(x => setVal.Value = setVal.Value.Replace(x.ToString(), String.Empty));
                        val = setVal.Value;
                    }
                    result += ",Phone:'" + val + "'";
                    usersetModel.Phone = val;
                    //get company fax
                    set = settingsCls.Where(s => s.Name == "com_fax").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                    {
                        charsToRemove.ForEach(x => setVal.Value = setVal.Value.Replace(x.ToString(), String.Empty));
                        val = setVal.Value;
                    }
                    result += ",Fax:'" + val + "'";
                    usersetModel.Fax = val;
                    //get company logo
                    set = settingsCls.Where(s => s.Name == "com_logo").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    result += ",logoImage:'" + val + "'";
                    usersetModel.logoImage = val;
                    #endregion
                    #region social
                    set = settingsCls.Where(s => s.Name == "com_website").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    // result += ",com_website:'" + val + "'";
                    usersetModel.com_website = val;
                    //
                    set = settingsCls.Where(s => s.Name == "com_social").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    // result += ",com_website:'" + val + "'";
                    usersetModel.com_social = val;
                    //
                    set = settingsCls.Where(s => s.Name == "com_social_icon").FirstOrDefault();
                    SettingId = set.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "";

                    if (setVal != null)
                        val = setVal.Value;
                    // result += ",com_website:'" + val + "'";
                    usersetModel.com_social_icon = val;
                    #endregion
                    #region 
                    var oneSet = settingsCls.Where(s => s.Name == "invoiceTax_bool").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",invoiceTax_bool:'" + val + "'";
                    usersetModel.invoiceTax_bool = bool.Parse(val);
                    oneSet = settingsCls.Where(s => s.Name == "invoiceTax_decimal").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "0";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",invoiceTax_decimal:" + val;
                    usersetModel.invoiceTax_decimal = decimal.Parse(val);
                    oneSet = settingsCls.Where(s => s.Name == "itemsTax_bool").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",itemsTax_bool:'" + val + "'";
                    usersetModel.itemsTax_bool = bool.Parse(val);

                    #endregion
                    #region get print settings
                    var printList = entity.AppSettingValue.ToList().Where(x => x.Notes == "print")
                            .Select(X => new
                            {
                                X.ValId,
                                X.Value,
                                X.IsDefault,
                                X.IsSystem,
                                X.SettingId,
                                X.Notes,
                                FirstName = entity.AppSetting.ToList().Where(s => s.SettingId == X.SettingId).FirstOrDefault().Name,

                            })
                            .ToList();


                    var psetVal = printList.Where(X => X.FirstName == "sale_copy_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",sale_copy_count:'" + val + "'";
                    usersetModel.sale_copy_count = val;
                    psetVal = printList.Where(X => X.FirstName == "pur_copy_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",pur_copy_count:'" + val + "'";
                    usersetModel.pur_copy_count = val;
                    psetVal = printList.Where(X => X.FirstName == "print_on_save_sale").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",print_on_save_sale:'" + val + "'";
                    usersetModel.print_on_save_sale = val;
                    psetVal = printList.Where(X => X.FirstName == "print_on_save_pur").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",print_on_save_pur:'" + val + "'";
                    usersetModel.print_on_save_pur = val;
                    psetVal = printList.Where(X => X.FirstName == "email_on_save_sale").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",email_on_save_sale:'" + val + "'";
                    usersetModel.email_on_save_sale = val;
                    psetVal = printList.Where(X => X.FirstName == "email_on_save_pur").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",email_on_save_pur:'" + val + "'";
                    usersetModel.email_on_save_pur = val;
                    psetVal = printList.Where(X => X.FirstName == "rep_copy_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",rep_print_count:'" + val + "'";
                    usersetModel.rep_print_count = val;
                    psetVal = printList.Where(X => X.FirstName == "Allow_print_inv_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",Allow_print_inv_count:'" + val + "'";
                    usersetModel.Allow_print_inv_count = val;
                    psetVal = printList.Where(X => X.FirstName == "show_header").FirstOrDefault();
                    val = "1";
                    if (psetVal != null)
                    {
                        val = psetVal.Value;
                        if (val == null || val == "")
                        {
                            val = "1";
                        }
                    }
                    result += ",show_header:'" + val + "'";
                    usersetModel.show_header = val;
                    psetVal = printList.Where(X => X.FirstName == "itemtax_note").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",itemtax_note:'" + val + "'";
                    usersetModel.itemtax_note = val;
                    psetVal = printList.Where(X => X.FirstName == "sales_invoice_note").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",sales_invoice_note:'" + val + "'";
                    usersetModel.sales_invoice_note = val;
                    psetVal = printList.Where(X => X.FirstName == "print_on_save_directentry").FirstOrDefault();
                    val = "";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",print_on_save_directentry:'" + val + "'";
                    usersetModel.print_on_save_directentry = val;
                    psetVal = printList.Where(X => X.FirstName == "directentry_copy_count").FirstOrDefault();
                    val = "0";
                    if (psetVal != null)
                        val = psetVal.Value;
                    result += ",directentry_copy_count:'" + val + "'";
                    usersetModel.directentry_copy_count = val;

                    //report language
                    oneSet = settingsCls.Where(s => s.Name == "report_lang").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId && i.IsDefault == 1).FirstOrDefault().Value;

                    if (val.Equals(""))
                        val = "en";
                    result += ",Reportlang:'" + val + "'";
                    usersetModel.Reportlang = val;
                    #endregion


                    #region accuracy - date form
                    oneSet = settingsCls.Where(s => s.Name == "accuracy").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId && i.IsDefault == 1).FirstOrDefault();
                    val = "0";
                    if (setVal != null)
                    {
                        val = setVal.Value;
                        if (val.Equals(""))
                            val = "0";
                    }
                    result += ",accuracy:'" + val + "'";
                    usersetModel.accuracy = val;
                    //date form
                    oneSet = settingsCls.Where(s => s.Name == "dateForm").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId && i.IsDefault == 1).FirstOrDefault();
                    val = "";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",dateFormat:'" + val + "'";
                    usersetModel.dateFormat = val;
                    //Currency info
                    var regions = entity.CountryCode.Where(x => x.IsDefault == 1).FirstOrDefault();
                    if (regions == null)
                    {
                        result += ",Currency:''" + ",CurrencyId:,CountryId:";
                        usersetModel.Currency = "";
                        usersetModel.CurrencyId = 0;
                        usersetModel.CountryId = 0;
                    }
                    else
                    {
                        result += ",Currency:'" + regions.Currency + "'" + ",CurrencyId:" + regions.CurrencyId + ",CountryId:" + regions.CountryId;
                        usersetModel.Currency = regions.Currency;
                        usersetModel.CurrencyId = regions.CurrencyId;
                        usersetModel.CountryId = regions.CountryId;
                    }
                    #endregion


                    #region storage cost
                    oneSet = settingsCls.Where(s => s.Name == "storage_cost").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;

                    if (val == "" || val == null)
                        val = "0";
                    result += ",StorageCost:" + val;
                    usersetModel.StorageCost = decimal.Parse(val);
                    #endregion
                    #region activationSite
                    oneSet = settingsCls.Where(s => s.Name == "active_site").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;


                    result += ",activationSite:'" + val + "'";
                    usersetModel.activationSite = val;

                    #endregion
                    #region invoice_lang
                    oneSet = settingsCls.Where(s => s.Name == "invoice_lang").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;
                    result += ",invoice_lang:'" + val + "'";
                    usersetModel.invoice_lang = val;
                    #endregion
                    #region com_name_ar
                    oneSet = settingsCls.Where(s => s.Name == "com_name_ar").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;
                    result += ",com_name_ar:'" + val + "'";
                    usersetModel.com_name_ar = val;
                    #endregion
                    #region com_address_ar
                    oneSet = settingsCls.Where(s => s.Name == "com_address_ar").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;
                    result += ",com_address_ar:'" + val + "'";
                    usersetModel.com_address_ar = val;
                    #endregion
                    #region Properties
                    //canSkipProperties
                    oneSet = settingsCls.Where(s => s.Name == "canSkipProperties").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",canSkipProperties:'" + val + "'";
                    usersetModel.canSkipProperties = bool.Parse(val);
                    //canSkipSerialsNum
                    oneSet = settingsCls.Where(s => s.Name == "canSkipSerialsNum").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",canSkipSerialsNum:'" + val + "'";
                    usersetModel.canSkipSerialsNum = bool.Parse(val);
                    #endregion

                    #region returnPeriod
                    oneSet = settingsCls.Where(s => s.Name == "returnPeriod").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    val = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault().Value;

                    if (val == "" || val == null)
                        val = "0";
                    result += ",returnPeriod:" + val;
                    usersetModel.returnPeriod = int.Parse(val);
                    #endregion
                    #region freeDelivery
                    oneSet = settingsCls.Where(s => s.Name == "freeDelivery").FirstOrDefault();
                    SettingId = oneSet.SettingId;
                    setVal = settingsValues.Where(i => i.SettingId == SettingId).FirstOrDefault();
                    val = "false";
                    if (setVal != null)
                        val = setVal.Value;
                    result += ",freeDelivery:'" + val + "'";
                    usersetModel.freeDelivery = bool.Parse(val);
                    #endregion
                    result += "}";
                    return usersetModel;
                    // return TokenManager.GenerateToken(result);
                }
        
        }
        // GET api/<controller>
        [HttpPost]
        [Route("GetUserByID")]
        public string GetUserByID(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int UserId = 0;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemId")
                    {
                        UserId = int.Parse(c.Value);
                    }
                }
                var user = GetUserByID(UserId);
                return TokenManager.GenerateToken(user);

            }
        }

        public UserModel GetUserByID(int UserId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var user = entity.User
               .Where(u => u.UserId == UserId)
               .Select(u => new UserModel()
               {
                  UserId = u.UserId,
                  UserName = u.UserName,
                   Password = u.Password,
                   FirstName=  u.FirstName,
                   LastName = u.LastName,

                   CreateDate=  u.CreateDate,
                   UpdateDate = u.UpdateDate,
                   CreateUserId =  u.CreateUserId,
                   UpdateUserId = u.UpdateUserId,

                   Mobile = u.Mobile,
                   Email = u.Email,
                   Notes = u.Notes,
                   Address = u.Address,
                   IsOnline = u.IsOnline,
                   Image = u.Image,
                   IsActive = u.IsActive,
                   Balance = u.Balance,
                   BalanceType = u.BalanceType,

                   FullName = u.FirstName + " " + u.LastName,

                   HasCommission = u.HasCommission,
                   CommissionValue =  u.CommissionValue,
                   CommissionRatio = u.CommissionRatio,
               })
               .FirstOrDefault();
                return user;
            }
        }
        [HttpPost]
        [Route("editUserBalance")]
        public string editUserBalance(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                long UserId = 0;
                decimal amount = 0;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "UserId")
                    {
                        UserId = long.Parse(c.Value);
                    }
                    else if (c.Type == "amount")
                    {
                        amount = decimal.Parse(c.Value);
                    }
                }
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var user = entity.User.Find(UserId);

                        if (user.BalanceType == 0)
                        {
                            if (amount > user.Balance)
                            {
                                amount -= (decimal)user.Balance;
                                user.Balance = amount;
                                user.BalanceType = 1;
                            }
                            else
                                user.Balance -= amount;
                        }
                        else
                        {
                            user.Balance += amount;
                        }

                        entity.SaveChanges();
                    }
                    return TokenManager.GenerateToken("1");

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }
            }
        }


        [HttpPost]
        [Route("GetSalesMan")]
        public string GetSalesMan(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int branchId = 0;
                string deliveryPermission = "";
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "branchId")
                    {
                        branchId = int.Parse(c.Value);
                    }
                    else if (c.Type == "deliveryPermission")
                    {
                        deliveryPermission = c.Value;
                    }
                }
                List<UserModel> User = new List<UserModel>();
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    //var usersList = (from u in entity.User.Where(us => us.IsActive == 1 && us.UserId != 1)
                    //                 join bu in entity.branchesUsers on u.UserId equals bu.UserId
                    //                 where bu.branchId == branchId
                    //                 select new UserModel
                    //                 {
                    //                     UserId = u.UserId,
                    //                     UserName = u.UserName,
                    //                     FirstName = u.FirstName,
                    //                     LastName = u.LastName,
                    //                     FullName = u.FirstName + " " + u.LastName,
                    //                     Mobile = u.Mobile,
                    //                     Balance = u.Balance,
                    //                     BalanceType = u.BalanceType,
                                        
                    //                     HasCommission = u.HasCommission,
                    //                     CommissionValue = u.CommissionValue,
                    //                     CommissionRatio = u.CommissionRatio,
                    //                 }).ToList();

                  
                    return TokenManager.GenerateToken(User);
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
                User userObj = null;
                User newObject = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        newObject = JsonConvert.DeserializeObject<User>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
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
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var userEntity = entity.Set<User>();

                        //check user name if already exist
                        var tmp = entity.User.Where(x => x.UserName == newObject.UserName && x.UserId != newObject.UserId).FirstOrDefault();
                        if (tmp != null)
                            return TokenManager.GenerateToken("dUserName");

                        //check full name if already exist
                         tmp = entity.User.Where(x => x.FirstName == newObject.FirstName && x.LastName == newObject.LastName && x.UserId != newObject.UserId).FirstOrDefault();
                        if (tmp != null)
                            return TokenManager.GenerateToken("dFullName");

                        if (newObject.UserId == 0)
                        {

                            //ProgramInfo programInfo = new ProgramInfo();
                            //int userMaxCount = programInfo.getUserCount();
                            //int usersCount = entity.User.Count();
                            //if (usersCount >= userMaxCount && userMaxCount != -1)
                            //{
                            //    message = "upgrade";
                            //    return TokenManager.GenerateToken(message);
                            //}

                         
                            //else
                            {
                                newObject.CreateDate = cc.AddOffsetTodate(DateTime.Now);
                                newObject.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                                newObject.UpdateUserId = newObject.CreateUserId;
                                newObject.Balance = 0;
                                newObject.BalanceType = 0;
                                userObj = userEntity.Add(newObject);

                                entity.SaveChanges().ToString();
                                message = userObj.UserId.ToString();
                                return TokenManager.GenerateToken(message);

                            }
                        }
                        else
                        {
                            userObj = entity.User.Where(p => p.UserId == newObject.UserId).FirstOrDefault();
                            userObj.FirstName = newObject.FirstName;
                            userObj.UserName = newObject.UserName;
                            userObj.Password = newObject.Password;
                            userObj.FirstName = newObject.FirstName;
                            userObj.LastName = newObject.LastName;

                            userObj.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            userObj.UpdateUserId = newObject.UpdateUserId;
                            userObj.Mobile = newObject.Mobile;
                            userObj.Email = newObject.Email;
                            userObj.Notes = newObject.Notes;
                            userObj.Address = newObject.Address;
                            userObj.IsActive = newObject.IsActive;
                            userObj.Balance = newObject.Balance;
                            userObj.BalanceType = newObject.BalanceType;
                            userObj.IsOnline = newObject.IsOnline;
                            userObj.HasCommission = newObject.HasCommission;
                            userObj.CommissionValue = newObject.CommissionValue;
                            userObj.CommissionRatio = newObject.CommissionRatio;

                            message = userObj.UserId.ToString();

                            entity.SaveChanges().ToString();
                            return TokenManager.GenerateToken(message);

                        }
                    }
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
                long delUserId = 0;
                long UserId = 0;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "delUserId")
                    {
                        delUserId = long.Parse(c.Value);
                    }
                    else if (c.Type == "UserId")
                    {
                        UserId = long.Parse(c.Value);
                    }

                }

                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                         
                        User usersDelete = entity.User.Find(delUserId);
                        entity.User.Remove(usersDelete);
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
                            User userDelete = entity.User.Find(delUserId);

                            userDelete.IsActive = false;
                            userDelete.UpdateDate = cc.AddOffsetTodate(DateTime.Now);
                            userDelete.UpdateUserId = UserId;
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
                            var dir = System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\user");
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);
                            //  check if Image exist
                            var pathCheck = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\user"), imageWithNoExt);
                            var files = Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\user"), imageWithNoExt + ".*");
                            if (files.Length > 0)
                            {
                                File.Delete(files[0]);
                            }

                            //Userimage myfolder FirstName where i want to save my Image
                            var filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\user"), imageName);
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
                    localFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\user"), imageName);

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
                User userObj = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "itemObject")
                    {
                        userObj = JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                try
                {
                    User user;
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var userEntity = entity.Set<User>();
                        user = entity.User.Where(p => p.UserId == userObj.UserId).First();
                        user.Image = userObj.Image;
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
        [Route("CanLogIn")]
        public async Task<string> CanLogIn(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int PosId = 0;
                int UserId = 0;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "PosId")
                    {
                        PosId = int.Parse(c.Value);
                    }
                    else if (c.Type == "UserId")
                    {
                        UserId = int.Parse(c.Value);
                    }
                }
                List<UserModel> User = new List<UserModel>();
                try
                {
                    int can = await CanLogIn(UserId, PosId);
                    return TokenManager.GenerateToken(can.ToString());

                }

                catch
                {
                    return TokenManager.GenerateToken("0");
                }
            }
        }
        public async Task<int> CanLogIn(long UserId, int PosId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                int can = 0;
                //var usersList = (from bu in entity.branchesUsers
                //                 join B in entity.branches on bu.branchId equals B.branchId
                //                 join P in entity.pos on B.branchId equals P.branchId

                //                 where P.PosId == PosId && bu.UserId == UserId
                //                 select new
                //                 {
                //                     bu.branchsUsersId,
                //                     bu.branchId,
                //                     bu.UserId,
                //                 }).ToList();
                //
                //if (usersList == null || usersList.Count == 0)
                //{
                //    can = 0;
                //}
                //else
                //{
                //    can = 1;
                //}

                return can;
            }
        }
        [HttpPost]
        [Route("checkLoginAvalability")]
        public string checkLoginAvalability(string token)
        {
            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                string deviceCode = "";
                int PosId = 0;
                string userName = "";
                string Password = "";
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "deviceCode")
                    {
                        deviceCode = c.Value;
                    }
                    else if (c.Type == "PosId")
                    {
                        PosId = int.Parse(c.Value);
                    }
                    else if (c.Type == "userName")
                    {
                        userName = c.Value;
                    }
                    else if (c.Type == "Password")
                    {
                        Password = c.Value;
                    }
                }
                string res = checkLoginAvalability(PosId, deviceCode, userName, Password);
                return TokenManager.GenerateToken(res);

            }
        }
        public string checkLoginAvalability(int PosId, string deviceCode, string userName, string Password)
        {
            // valid :  can login-
            //  failed : error 
            // -1 : package is expired 
            // -2 : device Code is not correct 
            // -3 : serial is not active 
            // -4 : customer server Code is wrong
            // wrongDate : login date is before last login date

            try
            {
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    //check support user
                    if (userName == "Support@Increase")
                    {
                        var suppUser = entity.User.Where(u => u.IsActive == true && u.UserName == userName && u.Password == Password && u.IsAdmin == true).FirstOrDefault();
                        if (suppUser != null)
                            return "valid";
                    }
                    //compair login date with last login date for this user
                    var user = entity.User.Where(x => x.UserName == userName && x.Password == Password && x.IsActive == true).FirstOrDefault();
                    if (user != null)
                    {
                        var logs = entity.UserLog.Where(x => x.UserId == user.UserId).OrderByDescending(x => x.SInDate).FirstOrDefault();
                        if (logs != null && logs.SInDate > cc.AddOffsetTodate(DateTime.Now))
                            return "wrongDate";
                    }
                    //ActivateController ac = new ActivateController();
                    //int active = ac.CheckPeriod();
                    //if (active == 0)
                    //    return -1;
                    //else
                    //{
                    //    var tmpObject = entity.posSetting.Where(x => x.PosId == PosId).FirstOrDefault();
                    //    if (tmpObject != null)
                    //    {
                    //        // check customer Code
                    //        if (tmpObject.posDeviceCode != deviceCode)
                    //        {
                    //            return -2;
                    //        }
                    //        //check customer server Code
                    //        ProgramDetailsController pc = new ProgramDetailsController();
                    //        var programD = pc.getCustomerServerCode();
                    //        if (programD == null || programD.customerServerCode != ac.ServerID())
                    //        {
                    //            return -4;
                    //        }
                    //    }
                    //    // check serial && package avalilability
                    //    var serial = entity.posSetting.Where(x => x.PosId == PosId && x.posSerials.IsActive == true).FirstOrDefault();
                    //    var programDetails = entity.ProgramDetails.Where(x => x.IsActive == true).FirstOrDefault();
                    //    if (serial == null || programDetails == null)
                    //        return -3;
                    //}

                    return "valid";
                }
            }
            catch
            {

                return "failed";

            }

        }

        [HttpPost]
        [Route("updateOnline")]
        public string updateOnline(string token)
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
                long UserId = 0;
                User userObj = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "UserId")
                    {
                        UserId = long.Parse(c.Value);
                    }
                }
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var userEntity = entity.Set<User>();

                        if (UserId > 0)
                        {
                            userObj = entity.User.Where(p => p.UserId == UserId).FirstOrDefault();
                            userObj.IsOnline = false;
                            entity.SaveChanges().ToString();
                            message = userObj.UserId.ToString();
                            return TokenManager.GenerateToken(message);
                        }
                        else
                        {
                            return TokenManager.GenerateToken("0");
                        }
                    }
                }
                catch
                {
                    message = "0";
                    return TokenManager.GenerateToken(message);
                    // return TokenManager.GenerateToken(ex.ToString());
                }
            }
        }
    }
}

