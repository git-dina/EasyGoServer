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

        public List<Branch> BrListByBranchandUser(int mainBranchId, long userId)
        {
            List<BranchModel> Listb = new List<BranchModel>();
            List<BranchModel> Listu = new List<BranchModel>();
            List<BranchModel> Lists = new List<BranchModel>();
            List<int> brIds = new List<int>();
            List<int> usrIds = new List<int>();
            List<int> intersectIds = new List<int>();

            List<Branch> List = new List<Branch>();
            User thisuser = new User();
            try
            {

                using (EasyGoDBEntities entity = new EasyGoDBEntities())
                {
                    thisuser = entity.User.Where(x => x.UserId == userId).Select(u => new User
                    {
                        UserId = u.UserId,

                        IsAdmin = u.IsAdmin,
                    }).FirstOrDefault();
                   
                }

                if (thisuser.IsAdmin == true)
                {
                    //admin user return all branches
                    List = Allbranches();
                    return List;
                }
                else
                {
                    //Listb = BranchesByBranch(mainBranchId);
                    Lists = BranchSonsbyId(mainBranchId);

                    Listu = BranchesByUser(userId);

                    Listb = Listb.Union(Lists).ToList();
                    brIds = Listb.Select(b => b.BranchId).ToList();
                    usrIds = Listu.Select(b => b.BranchId).ToList();

                    int id = 0;
                    foreach (int rowid in usrIds)
                    {
                        id = 0;
                        id = brIds.Where(x => x == rowid).FirstOrDefault();

                        intersectIds.Add(id);
                    }

                    List = Listu.Where(x => intersectIds.Contains(x.BranchId)).GroupBy(X => X.BranchId).Select(X => new Branch
                    {
                        BranchId = X.FirstOrDefault().BranchId,
                        Code = X.FirstOrDefault().Code,
                        Name = X.FirstOrDefault().Name,
                        Address = X.FirstOrDefault().Address,
                        Email = X.FirstOrDefault().Email,
                        Phone = X.FirstOrDefault().Phone,
                        Mobile = X.FirstOrDefault().Mobile,
                        CreateDate = X.FirstOrDefault().CreateDate,
                        UpdateDate = X.FirstOrDefault().UpdateDate,
                        CreateUserId = X.FirstOrDefault().CreateUserId,
                        UpdateUserId = X.FirstOrDefault().UpdateUserId,
                        Notes = X.FirstOrDefault().Notes,
                        ParentId = X.FirstOrDefault().ParentId,
                        IsActive = X.FirstOrDefault().IsActive,
                        Type = X.FirstOrDefault().Type,

                    }).ToList();

                    return List;
                }


            }
            catch
            {

                List = new List<Branch>();
                return List;
            }



        }

        public List<Branch> Allbranches()
        {
            List<Branch> List = new List<Branch>();
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                List = entity.Branch.Where(b => b.BranchId != 1).Select(B => new Branch
                {
                    BranchId = B.BranchId,//
                    Code = B.Code,//
                    Name = B.Name,//
                    Address = B.Address,//
                    Email = B.Email,//
                    Phone = B.Phone,//
                    Mobile = B.Mobile,//
                    CreateDate = B.CreateDate,//
                    UpdateDate = B.UpdateDate,//
                    CreateUserId = B.CreateUserId,//
                    UpdateUserId = B.UpdateUserId,//
                    Notes = B.Notes,//
                    ParentId = B.ParentId,//
                    IsActive = B.IsActive,//
                    Type = B.Type,//
                }
                ).ToList();

                return List;
            }
        }

        public List<BranchModel> BranchSonsbyId(int parentId)
        {

            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {  // get all sub categories of categoryId
                List<Branch> branchList = entity.Branch
                 .ToList()
                  .Select(p => new Branch
                  {
                      BranchId = p.BranchId,
                      Name = p.Name,
                      ParentId = p.ParentId,
                  })
                 .ToList();

                List<int> branchesIdlist = new List<int>();
                List<int> catIdlist = new List<int>();
                branchesIdlist.Add(parentId);


                var result = Recursive(branchList, parentId).ToList();


                foreach (var r in result)
                {
                    catIdlist.Add(r.BranchId);

                }

                List<Branch> branchListR = entity.Branch.Where(U => catIdlist.Contains(U.BranchId)).ToList();
                List<BranchModel> branchListreturn = branchListR.Select(b => new BranchModel
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
                }
                ).ToList();


                return branchListreturn;

            }

        }

        List<int> branchIdlist = new List<int>();
        public IEnumerable<Branch> Recursive(List<Branch> branchList, int toplevelid)
        {
            List<Branch> inner = new List<Branch>();

            foreach (var t in branchList.Where(item => item.ParentId == toplevelid))
            {
                branchIdlist.Add(t.BranchId);
                inner.Add(t);
                inner = inner.Union(Recursive(branchList, t.BranchId)).ToList();
            }

            return inner;
        }

        public List<BranchModel> BranchesByUser(long userId)
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                List<BranchModel> List = (from S in entity.BranchUser
                                          join B in entity.Branch on S.BranchId equals B.BranchId into JB
                                          join U in entity.User on S.UserId equals U.UserId into JU
                                          from JBB in JB.DefaultIfEmpty()
                                          from JUU in JU.DefaultIfEmpty()
                                          where S.UserId == userId
                                          select new BranchModel()
                                          {

                                              // branch
                                              BranchId = JBB.BranchId,
                                              Code = JBB.Code,
                                              Name = JBB.Name,
                                              Address = JBB.Address,
                                              Email = JBB.Email,
                                              Phone = JBB.Phone,
                                              Mobile = JBB.Mobile,
                                              CreateDate = JBB.CreateDate,
                                              UpdateDate = JBB.UpdateDate,
                                              CreateUserId = JBB.CreateUserId,
                                              UpdateUserId = JBB.UpdateUserId,
                                              Notes = JBB.Notes,
                                              ParentId = JBB.ParentId,
                                              IsActive = JBB.IsActive,
                                              Type = JBB.Type,

                                          }).ToList();
                return List;
            }

        }
    }
}