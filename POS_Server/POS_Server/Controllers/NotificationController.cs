using Newtonsoft.Json;
using POS_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace POS_Server.Controllers
{
    [RoutePrefix("api/Notification")]
    public class NotificationController : ApiController
    {
        CountriesController coctrlr = new CountriesController();
        public void addNotifications(string objectName, string notificationObj, int branchId, string itemName)
        {

            Notification Object = JsonConvert.DeserializeObject<Notification>(notificationObj, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });
            //try
            //{
            DateTime datenow = coctrlr.AddOffsetTodate(DateTime.Now);
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                var users = (from u in entity.User.Where(x => x.IsActive == true && x.IsAdmin == false)
                             join b in entity.BranchUser on u.UserId equals b.UserId
                             where b.BranchId == branchId
                             select new UserModel()
                             { UserId = u.UserId }
                          ).ToList();

                Object.Ncontent = itemName + ":" + Object.Ncontent;
                Object.IsActive = true;
                Object.CreateDate = datenow;
                Object.UpdateDate = datenow;

                var notEntity = entity.Set<Notification>();
                Notification not = notEntity.Add(Object);

                entity.SaveChanges();
                NotificationUser notUser;
                var notUserEntity = entity.Set<NotificationUser>();
                foreach (UserModel user in users)
                {
                    //var groupObjects = (from GO in entity.groupObject
                    //                    join U in entity.User on GO.groupId equals U.GroupId
                    //                    join G in entity.groups on GO.groupId equals G.groupId
                    //                    join O in entity.objects on GO.objectId equals O.objectId
                    //                    where U.userId == user.UserId
                    //                    select new
                    //                    {
                    //                        //group object
                    //                        GO.id,
                    //                        GO.groupId,
                    //                        GO.objectId,
                    //                        GO.addOb,
                    //                        GO.updateOb,
                    //                        GO.deleteOb,
                    //                        GO.showOb,
                    //                        GO.reportOb,
                    //                        GO.levelOb,
                    //                        //group 
                    //                        GroupName = G.name,
                    //                        //object
                    //                        ObjectName = O.name,
 
                    //                        O.objectType,
                    //                        parentObjectName = O.parentObjectName,
                    //                    }).ToList();

                    //var element = groupObjects.Where(X => X.ObjectName == objectName).FirstOrDefault();
                    //if (element != null && element.showOb == 1)
                        {
                            // add notification to users
                            notUser = new NotificationUser()
                            {
                                NotId = not.NotId,
                                UserId = user.UserId,
                                IsRead = false,
                                CreateDate = datenow,
                                UpdateDate = datenow,
                                CreateUserId = Object.CreateUserId,
                                UpdateUserId = Object.CreateUserId,
                            };
                            notUserEntity.Add(notUser);
                        }
                }
                var admins = (from u in entity.User.Where(x => x.IsActive == true && x.IsAdmin == true)
                              select new UserModel()
                              { UserId = u.UserId }
                              ).ToList();
                foreach (UserModel user in admins)
                {
                    notUser = new NotificationUser()
                    {
                        NotId = not.NotId,
                        UserId = user.UserId,
                        IsRead = false,
                        CreateDate = coctrlr.AddOffsetTodate(DateTime.Now),
                        UpdateDate = coctrlr.AddOffsetTodate(DateTime.Now),
                        CreateUserId = Object.CreateUserId,
                        UpdateUserId = Object.CreateUserId,
                    };
                    notUserEntity.Add(notUser);
                }
                entity.SaveChanges();
            }
        }

        public string save(Notification Object, string objectName, string prefix, int branchId, long userId = 0, int posId = 0)
        {
            string message = "1";
            using (EasyGoDBEntities entity4 = new EasyGoDBEntities())
            {

                Object.Ncontent = prefix + ":" + Object.Ncontent;
                Object.IsActive = true;
                Object.CreateDate = coctrlr.AddOffsetTodate(DateTime.Now);
                Object.UpdateDate = coctrlr.AddOffsetTodate(DateTime.Now);

                var notEntity = entity4.Set<Notification>();
                Notification not = notEntity.Add(Object);
                entity4.SaveChanges();
                DateTime datenow = coctrlr.AddOffsetTodate(DateTime.Now);
                #region pos notifications
                if (posId != 0 && posId != null)
                {
                    var notUserEntity = entity4.Set<NotificationUser>();
                    NotificationUser notUser = new NotificationUser()
                    {
                        NotId = not.NotId,
                        PosId = posId,
                        IsRead = false,
                        CreateDate = datenow,
                        UpdateDate = datenow,
                        CreateUserId = Object.CreateUserId,
                        UpdateUserId = Object.CreateUserId,
                    };
                    notUserEntity.Add(notUser);
                }
                #endregion
                else if (userId == 0)
                {
                    var users = (from u in entity4.User.Where(x => x.IsActive == true)
                                 join b in entity4.BranchUser on u.UserId equals b.UserId
                                 where b.BranchId == branchId
                                 select new UserModel()
                                 { UserId = u.UserId }
                             ).ToList();

                    foreach (UserModel user in users)
                    {
                        //var groupObjects = (from GO in entity4.groupObject
                        //                    join U in entity4.users on GO.groupId equals U.groupId
                        //                    join G in entity4.groups on GO.groupId equals G.groupId
                        //                    join O in entity4.objects on GO.objectId equals O.objectId
                        //                    //   join POO in entity4.objects on O.parentObjectId equals POO.objectId into JP

                        //                    //   from PO in JP.DefaultIfEmpty()
                        //                    where U.userId == user.userId
                        //                    select new
                        //                    {
                        //                        //group object
                        //                        GO.id,
                        //                        GO.groupId,
                        //                        GO.objectId,
                        //                        GO.addOb,
                        //                        GO.updateOb,
                        //                        GO.deleteOb,
                        //                        GO.showOb,
                        //                        GO.reportOb,
                        //                        GO.levelOb,
                        //                        //group 
                        //                        GroupName = G.name,
                        //                        //object
                        //                        ObjectName = O.name,
                        //                        O.objectType,
                        //                        parentObjectName = O.parentObjectName,

                        //                    }).ToList();

                        //var element = groupObjects.Where(X => X.ObjectName == objectName).FirstOrDefault();
                        //if (element != null && element.showOb == 1)
                            {
                                // add notification to users
                                var notUserEntity = entity4.Set<NotificationUser>();
                            NotificationUser notUser = new NotificationUser()
                                {
                                    NotId = not.NotId,
                                    UserId = user.UserId,
                                    IsRead = false,
                                    CreateDate = datenow,
                                    UpdateDate = datenow,
                                    CreateUserId = Object.CreateUserId,
                                    UpdateUserId = Object.CreateUserId,
                                };
                                notUserEntity.Add(notUser);
                        }
                    }
                }
                else // add notification to one user whose id = userId
                {
                    var notUserEntity = entity4.Set<NotificationUser>();
                    NotificationUser notUser = new NotificationUser()
                    {
                        NotId = not.NotId,
                        UserId = userId,
                        IsRead = false,
                        CreateDate = datenow,
                        UpdateDate = datenow,
                        CreateUserId = Object.CreateUserId,
                        UpdateUserId = Object.CreateUserId,
                    };
                    notUserEntity.Add(notUser);
                }
                entity4.SaveChanges();
            }
            return message;
        }
    }
}