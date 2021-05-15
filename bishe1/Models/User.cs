using System;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;

namespace bishe1.Models
{
    //用户类，所需要的子项动态添加。
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        //public DateTime ReleaseDate { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string[] RemCheckBox { get; set; }
        /*public static implicit operator Var(User v)
        {
            throw new NotImplementedException();
        }*/
        //public decimal Price { get; set; }
    }
    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public System.Data.Entity.DbSet<bishe1.Models.UserInformation> UserInformations { get; set; }
    }
}