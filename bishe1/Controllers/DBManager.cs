using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.EntityFrameworkCore;
namespace bishe1.Controllers
{
    public class DBManager
    {
        /// <summary>
        /// 本地数据库连接语句
        /// </summary>
        public static string LocalhostMySQLdbstr = "server=127.0.0.1;port=3306;user=root;password=; database=bishe;charset=gbk";
        //public static string LocalhostMySQLdbstr = "server=49.233.85.114;port=3306;user=root;password=qwe123789; database=vs628;";

        /// <summary>
        /// 远程数据库连接语句(腾讯云服务器)
        /// </summary>
        //public static string TencentCloudMySQLdbstr = "server=49.233.85.114;port=3306;user=root;password=qwe123789; database=vs628;";

    }
}