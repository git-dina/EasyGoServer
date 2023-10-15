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
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
//using System.Windows.Threading;
using System.Timers;
namespace POS_Server.Controllers
{

    [RoutePrefix("api/UsersLogs")]
    public class UsersLogsController : ApiController
    {
        private static System.Timers.Timer logoutTimer;
        private static System.Timers.Timer oneminuteTimer;
        public static double oneMtime = 60000;
        // public DispatcherTimer logoutTimer;
        public static double Repeattime = 600000;//milliSecond//600000=10 minute
        public int maxIdleperiod = 15;//minute 

        CountriesController cc = new CountriesController();
        // GET api/<controller>
        [HttpPost]
        [Route("Get")]
        public string Get(string token)
        {
            //public string GetPurinv(string token)




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
                        var list = (from S in entity.UserLog
                                    select new UsersLogsModel()
                                    {
                                        LogId = S.LogId,

                                        SInDate = S.SInDate,
                                        SOutDate = S.SOutDate,
                                        PosId = S.PosId,
                                        UserId = S.UserId,

                                    }).ToList();
                        return TokenManager.GenerateToken(list);


                    }



                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }


            }

            //          
            //         
            //            string token = "";
            //            bool canDelete = false;

            //            if (headers.Contains("APIKey"))
            //            {
            //                token = headers.GetValues("APIKey").First();
            //            }
            //            Validation validation = new Validation();
            //            bool valid = validation.CheckApiKey(token);

            //            if (valid) // APIKey is valid
            //            {
            //                using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //                {
            //                    var List = (from S in  entity.UserLog                                         
            //                                         select new UsersLogsModel()
            //                                         {
            //                                            LogId=S.LogId,

            //                                             SInDate=S.SInDate,
            //                                             SOutDate=S.SOutDate,
            //                                             PosId=S.PosId,
            //                                             UserId=S.UserId,

            //                                         }).ToList();
            //                    /*
            //                     * 


            //    public int LogId { get; set; }
            //            public Nullable<System.DateTime> SInDate { get; set; }
            //            public Nullable<System.DateTime> SOutDate { get; set; }
            //            public Nullable<int> PosId { get; set; }
            //            public Nullable<int> UserId { get; set; }
            //            public bool canDelete { get; set; }

            //LogId
            //SInDate
            //SOutDate
            //PosId
            //UserId
            //canDelete


            //                    */



            //                    if (List == null)
            //                        return NotFound();
            //                    else
            //                        return Ok(List);
            //                }
            //            }
            //            //else
            //            return NotFound();
        }


        // get by UserId
        [HttpPost]
        [Route("GetByUserId")]
        public string GetByUserId(string token)
        {
            //public string GetPurinv(string token)int UserId




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
                    if (c.Type == "UserId")
                    {
                        UserId = int.Parse(c.Value);
                    }


                }


                try
                {

                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var item = (from S in entity.UserLog
                                    where S.UserId == UserId
                                    select new UsersLogsModel()
                                    {
                                        LogId = S.LogId,

                                        SInDate = S.SInDate,
                                        SOutDate = S.SOutDate,
                                        PosId = S.PosId,
                                        UserId = S.UserId,

                                    }).ToList();





                        return TokenManager.GenerateToken(item);

                    }

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }

            }
            //          
            //         
            //            string token = "";


            //            if (headers.Contains("APIKey"))
            //            {
            //                token = headers.GetValues("APIKey").First();
            //            }
            //            Validation validation = new Validation();
            //            bool valid = validation.CheckApiKey(token);

            //            if (valid) // APIKey is valid
            //            {
            //                using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //                {
            //                    var List = (from S in entity.UserLog
            //                                where S.UserId== UserId
            //                                select new UsersLogsModel()
            //                                {
            //                                    LogId = S.LogId,

            //                                    SInDate = S.SInDate,
            //                                    SOutDate = S.SOutDate,
            //                                    PosId = S.PosId,
            //                                    UserId = S.UserId,

            //                                }).ToList();
            //                    /*
            //                     * 


            //    public int LogId { get; set; }
            //            public Nullable<System.DateTime> SInDate { get; set; }
            //            public Nullable<System.DateTime> SOutDate { get; set; }
            //            public Nullable<int> PosId { get; set; }
            //            public Nullable<int> UserId { get; set; }
            //            public bool canDelete { get; set; }

            //LogId
            //SInDate
            //SOutDate
            //PosId
            //UserId
            //canDelete


            //                    */



            //                    if (List == null)
            //                        return NotFound();
            //                    else
            //                        return Ok(List);
            //                }
            //            }
            //            //else
            //            return NotFound();
        }

        // GET api/<controller> 
        [HttpPost]
        [Route("GetByID")]
        public string GetByID(string token)
        {

            //public string GetPurinv(string token)int LogId




            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int LogId = 0;


                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "LogId")
                    {
                        LogId = int.Parse(c.Value);
                    }
                }
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var item = entity.UserLog
                       .Where(u => u.LogId == LogId)
                       .Select(S => new
                       {
                           S.LogId,
                           S.SInDate,
                           S.SOutDate,
                           S.PosId,
                           S.UserId,
                       })
                       .FirstOrDefault();
                        return TokenManager.GenerateToken(item);
                        // return TokenManager.GenerateToken(item);
                    }

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                    //    return TokenManager.GenerateToken("0");
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

            //if (valid)
            //{
            //    using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //    {
            //        var row = entity.UserLog
            //       .Where(u => u.LogId == LogId)
            //       .Select(S => new
            //       {

            //              S.LogId,
            //              S.SInDate,
            //              S.SOutDate,
            //              S.PosId,
            //              S.UserId,


            //       })
            //       .FirstOrDefault();

            //        if (row == null)
            //            return NotFound();
            //        else
            //            return Ok(row);
            //    }
            //}
            //else
            //    return NotFound();
        }


        //checkOtherUser
        [HttpPost]
        [Route("checkOtherUser")]
        public string checkOtherUser(string token)
        {
            string message = "";

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
                    if (c.Type == "UserId")
                    {
                        UserId = int.Parse(c.Value);
                    }
                }
                try
                {
                    checkOtherUser(UserId);
                    //using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    //{
                    //    List<UserLog> List = entity.UserLog.Where(S => S.UserId == UserId && S.SOutDate == null).ToList();
                    //    if (List != null)
                    //    {
                    //        foreach (UserLog row in List)
                    //        {
                    //            row.SOutDate = cc.AddOffsetTodate(DateTime.Now);
                    //            message = entity.SaveChanges().ToString();
                    //        }

                    //        //  message = List.LastOrDefault().SOutDate.ToString();


                    //        //  return Ok(msg);
                    //    }
                    //    else
                    //    {

                    //        message = "-1";
                    //        // return Ok("none");
                    //    }

                    //     return TokenManager.GenerateToken(message);
                    //}
                    return TokenManager.GenerateToken("1");


                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }

            }

        }

        public void checkOtherUser(long UserId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                List<UserLog> List = entity.UserLog.Where(S => S.UserId == UserId && S.SOutDate == null).ToList();
                if (List != null)
                {
                    foreach (UserLog row in List)
                    {
                        row.SOutDate = cc.AddOffsetTodate(DateTime.Now);
                        entity.SaveChanges();
                    }

                }

            }
        }

        // add or update location
        [HttpPost]
        [Route("Save")]
        public string Save(string token)
        {
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
                UserLog newObject = null;
                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "Object")
                    {
                        Object = c.Value.Replace("\\", string.Empty);
                        Object = Object.Trim('"');
                        newObject = JsonConvert.DeserializeObject<UserLog>(Object, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
                if (newObject != null)
                {


                    UserLog tmpObject = null;


                    try
                    {
                        if (newObject.PosId == 0 || newObject.PosId == null)
                        {
                            Nullable<int> id = null;
                            newObject.PosId = id;
                        }
                        if (newObject.UserId == 0 || newObject.UserId == null)
                        {
                            Nullable<int> id = null;
                            newObject.UserId = id;
                        }
                        using (EasyGoDBEntities entity = new EasyGoDBEntities())
                        {
                            var locationEntity = entity.Set<UserLog>();
                            if (newObject.LogId == 0 || newObject.LogId == null)
                            {
                                // signIn

                                newObject.SInDate = cc.AddOffsetTodate(DateTime.Now);


                                locationEntity.Add(newObject);
                                entity.SaveChanges();
                                message = newObject.LogId.ToString();
                                //sign out old user

                                using (EasyGoDBEntities entity2 = new EasyGoDBEntities())
                                {
                                    List<UserLog> ul = new List<UserLog>();
                                    List<UserLog> locationE = entity2.UserLog.ToList();
                                    ul = locationE.Where(s => s.SOutDate == null &&
                                   ((DateTime.Now - (DateTime)s.SInDate).TotalHours >= 8)).ToList();
                                    if (ul != null)
                                    {
                                        foreach (UserLog row in ul)
                                        {
                                            row.SOutDate = cc.AddOffsetTodate(DateTime.Now);
                                            entity2.SaveChanges();
                                        }
                                    }
                                }

                            }



                            else
                            {//signOut
                                tmpObject = entity.UserLog.Where(p => p.LogId == newObject.LogId).FirstOrDefault();



                                tmpObject.LogId = newObject.LogId;
                                //  tmpObject.SInDate=newObject.SInDate;
                                tmpObject.SOutDate = cc.AddOffsetTodate(DateTime.Now);
                                //    tmpObject.PosId=newObject.PosId;
                                //  tmpObject.UserId = newObject.UserId;


                                entity.SaveChanges();

                                message = tmpObject.LogId.ToString();
                            }
                            //  entity.SaveChanges();
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
            //var headers = re.Headers;
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
            //    Object = Object.Replace("\\", string.Empty);
            //    Object = Object.Trim('"');
            //    UserLog newObject = JsonConvert.DeserializeObject<UserLog>(Object, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
            //    if (newObject.PosId == 0 || newObject.PosId == null)
            //    {
            //        Nullable<int> id = null;
            //        newObject.PosId = id;
            //    }
            //    if (newObject.UserId == 0 || newObject.UserId == null)
            //    {
            //        Nullable<int> id = null;
            //        newObject.UserId = id;
            //    }



            //    try
            //    {
            //        using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //        {
            //            var locationEntity = entity.Set<UserLog>();
            //            if (newObject.LogId == 0 || newObject.LogId == null)
            //            {
            //                // signIn

            //                newObject.SInDate = cc.AddOffsetTodate(DateTime.Now);


            //                locationEntity.Add(newObject);
            //                entity.SaveChanges();
            //                message = newObject.LogId.ToString();
            //                //sign out old user

            //                using (EasyGoDBEntities entity2 = new EasyGoDBEntities())
            //                {
            //                    List<UserLog> ul = new List<UserLog>();
            //                    List<UserLog>  locationE = entity2.UserLog.ToList();
            //                    ul = locationE.Where(s => s.SOutDate == null &&
            //                   ( (DateTime.Now-(DateTime)s.SInDate).TotalHours>=24)).ToList();
            //                    if (ul != null)
            //                    {
            //                        foreach(UserLog row in ul)
            //                        {
            //                            row.SOutDate = cc.AddOffsetTodate(DateTime.Now);
            //                            entity2.SaveChanges();
            //                        }
            //                    }
            //                }

            //                }



            //            else
            //            {//signOut
            //                var tmpObject = entity.UserLog.Where(p => p.LogId == newObject.LogId).FirstOrDefault();



            //                tmpObject.LogId = newObject.LogId;
            //               //  tmpObject.SInDate=newObject.SInDate;
            //                 tmpObject.SOutDate= cc.AddOffsetTodate(DateTime.Now);
            //             //    tmpObject.PosId=newObject.PosId;
            //              //  tmpObject.UserId = newObject.UserId;


            //                entity.SaveChanges();

            //                message = tmpObject.LogId.ToString();
            //            }
            //          //  entity.SaveChanges();
            //        }
            //    }
            //    catch
            //    {
            //        message = "-1";
            //    }
            //}
            //return message;
        }
        public bool checkLogByID(int LogId)
        {
            try
            {
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var item = entity.UserLog.Where(u => u.LogId == LogId).FirstOrDefault();
                    // log out if client turnd off
                    UpdateLastRequest(item);
                    //UserRequest urModel = new UserRequest();

                    //urModel = entity.UserRequest.Where(r => r.UserId == item.UserId).FirstOrDefault();
                    //if (urModel == null || urModel.UserId == 0)
                    //{
                    //    //new user in list
                    //    //urModel.LogId = item.LogId;
                    //    urModel.UserId = item.UserId;
                    //    urModel.SInDate = item.SInDate;
                    //    urModel.LastRequestDate = cc.AddOffsetTodate(DateTime.Now);
                    //    Saverequest(urModel);
                    //    //entity.UserRequest.Add(urModel);
                    //    //entity.SaveChanges();
                    //}
                    //else
                    //{
                    //    urModel.LastRequestDate = cc.AddOffsetTodate(DateTime.Now);
                    //    Saverequest(urModel);
                    //    //user exist -> update request date
                    //    //entity.UserRequest.Where(r => r.UserId == item.UserId).ToList().ForEach(r =>
                    //    //{
                    //    //    // r.LogId = item.LogId;
                    //    //    r.LastRequestDate = cc.AddOffsetTodate(DateTime.Now);
                    //    //});
                    //}
                    //
                    //check if user change server date
                    if (item.SInDate > cc.AddOffsetTodate(DateTime.Now))
                        return true;
                    if (item.SOutDate != null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        [HttpPost]
        [Route("Delete")]
        public string Delete(string token)
        {

            //public string Delete(string token)int LogId, int UserId,bool final
            //int Id, int UserId
            string message = "";



            token = TokenManager.readToken(HttpContext.Current.Request);
            var strP = TokenManager.GetPrincipal(token);
            if (strP != "0") //invalid authorization
            {
                return TokenManager.GenerateToken(strP);
            }
            else
            {
                int LogId = 0;
                int UserId = 0;
                bool final = false;

                IEnumerable<Claim> claims = TokenManager.getTokenClaims(token);
                foreach (Claim c in claims)
                {
                    if (c.Type == "LogId")
                    {
                        LogId = int.Parse(c.Value);
                    }
                    else if (c.Type == "UserId")
                    {
                        UserId = int.Parse(c.Value);
                    }
                    else if (c.Type == "final")
                    {
                        final = bool.Parse(c.Value);
                    }

                }

                try
                {
                    if (final)
                    {

                        using (EasyGoDBEntities entity = new EasyGoDBEntities())
                        {
                            UserLog objectDelete = entity.UserLog.Find(LogId);

                            entity.UserLog.Remove(objectDelete);
                            message = entity.SaveChanges().ToString();

                            //   return TokenManager.GenerateToken(message);

                        }

                        return TokenManager.GenerateToken(message);

                    }
                    else
                    {
                        return TokenManager.GenerateToken("-2");

                    }

                }
                catch
                {
                    return TokenManager.GenerateToken("0");
                }
            }
            //var re = Request;
            //var headers = re.Headers;
            //string token = "";
            //int message = 0;
            //if (headers.Contains("APIKey"))
            //{
            //    token = headers.GetValues("APIKey").First();
            //}

            //Validation validation = new Validation();
            //bool valid = validation.CheckApiKey(token);
            //if (valid)
            //{
            //    if (final)
            //    {
            //        try
            //        {
            //            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            //            {
            //                UserLog objectDelete = entity.UserLog.Find(LogId);

            //                entity.UserLog.Remove(objectDelete);
            //            message=    entity.SaveChanges();

            //                return message.ToString();
            //            }
            //        }
            //        catch
            //        { 
            //            return "-1";
            //        }
            //    }
            //    return "-2";

            //}
            //else
            //    return "-3";
        }

        //////////////////////
        ///

        public string Save(UserLog newObject)
        {
            //public string Save(string token)
            //string Object string newObject
            string message = "";


            if (newObject != null)
            {


                UserLog tmpObject = null;


                try
                {
                    if (newObject.PosId == 0 || newObject.PosId == null)
                    {
                        Nullable<int> id = null;
                        newObject.PosId = id;
                    }
                    if (newObject.UserId == 0 || newObject.UserId == null)
                    {
                        Nullable<int> id = null;
                        newObject.UserId = id;
                    }
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var locationEntity = entity.Set<UserLog>();
                        if (newObject.LogId == 0 || newObject.LogId == null)
                        {
                            // signIn
                            // sign out old
                            using (EasyGoDBEntities entity2 = new EasyGoDBEntities())
                            {
                                List<UserLog> ul = new List<UserLog>();
                                List<UserLog> locationE = entity2.UserLog.ToList();
                                ul = locationE.Where(s => s.SOutDate == null &&
                               ((DateTime.Now - (DateTime)s.SInDate).TotalHours >= 8) || (s.UserId == newObject.UserId && s.SOutDate == null)).ToList();
                                if (ul != null)
                                {
                                    foreach (UserLog row in ul)
                                    {
                                        row.SOutDate = cc.AddOffsetTodate(DateTime.Now);
                                        entity2.SaveChanges();
                                    }
                                }
                            }
                            newObject.SInDate = cc.AddOffsetTodate(DateTime.Now);


                            locationEntity.Add(newObject);
                            entity.SaveChanges();
                            message = newObject.LogId.ToString();
                            //sign out old user



                        }



                        else
                        {//signOut
                            tmpObject = entity.UserLog.Where(p => p.LogId == newObject.LogId).FirstOrDefault();



                            tmpObject.LogId = newObject.LogId;
                            //  tmpObject.SInDate=newObject.SInDate;
                            tmpObject.SOutDate = cc.AddOffsetTodate(DateTime.Now);
                            //    tmpObject.PosId=newObject.PosId;
                            //  tmpObject.UserId = newObject.UserId;


                            entity.SaveChanges();

                            message = tmpObject.LogId.ToString();
                        }
                        //  entity.SaveChanges();
                    }

                    return message;

                }
                catch
                {
                    message = "0";
                    return message;
                }


            }
            else
            {
                return "0";
            }
        }
        public string SignOutOld(long UserId)
        {

            string message = "";

            //UserLog tmpObject = null;

            try
            {

                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var locationEntity = entity.Set<UserLog>();
                    // signIn
                    // sign out old
                    using (EasyGoDBEntities entity2 = new EasyGoDBEntities())
                    {
                        DateTime datenow = cc.AddOffsetTodate(DateTime.Now);
                        List<UserLog> ul = new List<UserLog>();
                        List<UserLog> locationE = entity2.UserLog.ToList();
                        locationE.Where(s => s.SOutDate == null &&
                         ((datenow - (DateTime)s.SInDate).TotalHours >= 8)).ToList()
                         .ForEach(s => s.SOutDate = datenow);
                        message = entity2.SaveChanges().ToString();
                        //if (ul != null)
                        //        {
                        //            foreach (UserLog row in ul)
                        //            {
                        //                row.SOutDate = cc.AddOffsetTodate(DateTime.Now);

                        //            }
                        //        }
                    }

                    //  entity.SaveChanges();
                }

                return message;
            }
            catch
            {
                message = "0";
                return message;
            }
        }
        public UserLog GetByID(int LogId)
        {
            UserLog item = new UserLog();

            if (LogId > 0)
            {
                try
                {
                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var itemlist = entity.UserLog.Where(u => u.LogId == LogId).ToList();

                        item = itemlist.Where(u => u.LogId == LogId)
                               .Select(S => new UserLog
                               {
                                   LogId = S.LogId,
                                   SInDate = S.SInDate,
                                   SOutDate = S.SOutDate,
                                   PosId = S.PosId,
                                   UserId = S.UserId,
                               })
                               .FirstOrDefault();
                        return item;

                    }

                }
                catch (Exception ex)
                {
                    return item;

                }
            }
            else
            {
                return item;
            }



        }


        public void startapp()
        {
            int res = 0;
            try
            {
                logoutTimer = new System.Timers.Timer();
                logoutTimer.Interval = Repeattime;
                // logoutTimer.Interval = Repeattime;
                logoutTimer.Elapsed += timerFunction;

                logoutTimer.AutoReset = true;//to repeat timer
                logoutTimer.Enabled = true;// to start timer
                                           //                
                oneminuteTimer = new System.Timers.Timer();
                oneminuteTimer.Interval = oneMtime;
                // logoutTimer.Interval = Repeattime;
                oneminuteTimer.Elapsed += oneminuteTimerFunction;
                oneminuteTimer.AutoReset = true;//to repeat timer
                oneminuteTimer.Enabled = true;// to start timer
            }
            catch
            {

                // return 0;

            }

        }

        void timerFunction(object sender, EventArgs e)
        {

            try
            {

                SignOutErrorExit();
                TokenManager.deleteDirectoryFiles();
                //BackupController backcntrlr = new BackupController();
                //backcntrlr.autoBackup();
                //notificationTimer(); //for expired items
            }
            catch (Exception ex)
            {

            }

        }
        public async void oneminuteTimerFunction(object sender, EventArgs e)
        {

            try
            {

                //StatisticsController sts = new StatisticsController();
                //await sts.MakeStatement();

            }
            catch  
            {

            }

        }

        private void SignOutErrorExit()
        {
            int res = 0;
            try
            {
                DateTime datenow = cc.AddOffsetTodate(DateTime.Now);
                LogoutOld(datenow);
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    List<UserRequest> soutlistt = entity.UserRequest.ToList();
                    List<UserRequest> soutlist = soutlistt.Where(r => (datenow - (DateTime)r.LastRequestDate).Minutes >= maxIdleperiod).ToList();
                    if (soutlist != null)
                    {
                        if (soutlist.Count > 0)
                        {
                            foreach (var urout in soutlist)
                            {
                                var userloglist = entity.UserLog.Where(u => u.UserId == urout.UserId && u.SOutDate == null).ToList();
                                if (userloglist != null)
                                {
                                    if (userloglist.Count > 0)
                                    {
                                        UserLog userlogobj = userloglist.LastOrDefault();
                                        if (userlogobj.LogId > 0)
                                        {
                                            userlogobj.SOutDate = datenow;
                                            urout.SOutDate = userlogobj.SOutDate;
                                            entity.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {

            }
        }
        public string LogoutOld(DateTime datenow)
        {
            string res = "0";
            try
            {
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    var allUsersl = entity.User.ToList();
                    var allUsers = allUsersl
                   .Select(u => new UserModel()
                   {
                       UserId = u.UserId,

                   }).ToList();
                    foreach (UserModel urow in allUsers)
                    {
                        UserLog userloglast = new UserLog();
                        userloglast = entity.UserLog.Where(u => u.UserId == urow.UserId).ToList().LastOrDefault();

                        if (userloglast != null && userloglast.LogId > 0)
                        {
                            if (userloglast.SOutDate != null)
                            {
                                List<UserLog> userlogList = entity.UserLog.Where(u => u.UserId == urow.UserId && userloglast.SInDate > u.SInDate && u.SOutDate == null).ToList();
                                if (userlogList.Count > 0)
                                {
                                    userlogList.ForEach(x => x.SOutDate = datenow);
                                    entity.SaveChanges();

                                }

                            }
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                // return ex.ToString();
                return "0";
            }




        }

        public string Saverequest(UserRequest newObject)
        {
            string res = "0";
            if (newObject != null)
            {
                try
                {
                    UserRequest tmpObject = null;
                    if (newObject.UserId == 0 || newObject.UserId == null)
                    {
                        Nullable<int> id = null;
                        newObject.UserId = id;
                    }

                    using (EasyGoDBEntities entity = new EasyGoDBEntities())
                    {
                        var locationEntity = entity.Set<UserRequest>();
                        if (newObject.UserRequestId == 0 || newObject.UserRequestId == null)
                        {

                            locationEntity.Add(newObject);
                            res = entity.SaveChanges().ToString();


                        }

                        else
                        {//signOut
                            tmpObject = entity.UserRequest.Where(p => p.UserId == newObject.UserId).FirstOrDefault();
                            tmpObject.SInDate = newObject.SInDate;
                            tmpObject.SOutDate = newObject.SOutDate;

                            tmpObject.LastRequestDate = newObject.LastRequestDate;


                            res = entity.SaveChanges().ToString();


                        }

                    }

                    return res;

                }
                catch (Exception ex)
                {
                    // return ex.ToString();
                    return "0";
                }




            }
            else
            {
                return "0";
            }


        }

        public string UpdateLastRequest(UserLog item)
        {
            UserRequest urModel = new UserRequest();
            string res = "0";
            List<UserRequest> reqlist = new List<UserRequest>();
            try
            {
                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    reqlist = entity.UserRequest.Where(r => r.UserId == item.UserId).ToList();

                }

                if (reqlist.Count() == 0)
                {
                    //new user in list

                    urModel.UserId = item.UserId;
                    urModel.SInDate = item.SInDate;
                    urModel.SOutDate = item.SOutDate;
                    urModel.LastRequestDate = cc.AddOffsetTodate(DateTime.Now);
                    res = Saverequest(urModel);
                    //entity.UserRequest.Add(urModel);
                    //entity.SaveChanges();
                }
                else
                {
                    //update
                    urModel = reqlist.FirstOrDefault();
                    urModel.SInDate = item.SInDate;
                    urModel.SOutDate = item.SOutDate;
                    urModel.LastRequestDate = cc.AddOffsetTodate(DateTime.Now);
                    res = Saverequest(urModel);

                }
                return res;
            }
            catch (Exception ex)
            {
                return "0";
            }

        }


        //public void notificationTimer()
        //{
        //    setValues setValuesModel = new setValues();
        //    NotificationController notctrlr = new NotificationController();
        //    setValuesController svalctrlr = new setValuesController();
        //    DateTime datetoday = cc.AddOffsetTodate(DateTime.Now);
        //    setValuesModel = svalctrlr.GetRowBySettingName("isAlertDone");
        //    try
        //    {


        //        DateTime Lastdate = Convert.ToDateTime(setValuesModel.value);
        //        //    DateTime Lastdate= DateTime.Parse();
        //        if (datetoday.Date > Lastdate.Date)
        //        {
        //            notctrlr.addExpiredAlert();
        //            setValuesModel.value = datetoday.Date.ToString("yyyy-MM-dd");
        //            svalctrlr.Save(setValuesModel);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        setValuesModel.value = datetoday.Date.ToString("yyyy-MM-dd");
        //        svalctrlr.Save(setValuesModel);
        //    }

        //}

    }
}