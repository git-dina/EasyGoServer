using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Classes
{
    public class ProgramInfo
    {
        public int getBranchCount()
        {
            int branchCount = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                branchCount = entity.ProgramDetails.Select(x => x.BranchCount).SingleOrDefault();
            }
            return branchCount;
        }
        public int getStroeCount()
        {
            int storeCount = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                storeCount = entity.ProgramDetails.Select(x => x.StoreCount).SingleOrDefault();
            }
            return storeCount;
        }
        public int getPosCount()
        {
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
               var posCount = entity.ProgramDetails.Select(x => x.PosCount).SingleOrDefault();
                return (int)posCount;
            }
        }
        public int getUserCount()
        {
            int userCount = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                userCount = entity.ProgramDetails.Select(x => x.UserCount).SingleOrDefault();
            }
            return userCount;
        }
        public int getVendorCount()
        {
            int vendorCount = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                vendorCount = entity.ProgramDetails.Select(x => x.VendorCount).SingleOrDefault();
            }
            return vendorCount;
        }
        public int getCustomerCount()
        {
            int customerCount = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                customerCount = entity.ProgramDetails.Select(x => x.CustomerCount).SingleOrDefault();
            }
            return customerCount;
        }
        public int getItemCount()
        {
            int itemCount = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                itemCount = entity.ProgramDetails.Select(x => x.ItemCount).SingleOrDefault();
            }
            return itemCount;
        }
        public int getSaleinvCount()
        {
            int invCount = 0;
            using (EasyGoDBEntities entity = new EasyGoDBEntities())
            {
                invCount = entity.ProgramDetails.Select(x => x.SaleinvCount).SingleOrDefault();
            }
            return invCount;
        }
    }
}