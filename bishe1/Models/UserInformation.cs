using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace bishe1.Models
{
    //用户类，所需要的子项动态添加。
    public class UserInformation
    {
        public int ID { get; set; }
        public string Username { get; set; }
        //public DateTime ReleaseDate { get; set; }
        public string Password { get; set; }
        public string LoginID { get; set; }
        public int Flag { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public string QQ { get; set; }
        public string Tel { get; set; }
        public string Type { get; set; }
        public string RegisterTime { get; set; }
        /*public static implicit operator Var(User v)
        {
            throw new NotImplementedException();
        }*/
        //public decimal Price { get; set; }
    }
    public class UserInformationDBContext : DbContext
    {
        public DbSet<UserInformation> Users { get; set; }
    }
}