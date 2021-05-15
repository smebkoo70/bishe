using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace bishe1.Models
{
    public class Admin_Info
    {
        public string Username { get; set; }
        //public DateTime ReleaseDate { get; set; }
        public string Password { get; set; }

    }
    public class Admin_InfoDBContext : DbContext
    {
        public DbSet<Admin_Info> Users { get; set; }

        public System.Data.Entity.DbSet<bishe1.Models.Admin_Info> Admin_Info { get; set; }
    }

}