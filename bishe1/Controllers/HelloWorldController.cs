using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using bishe1.Models;
using CCWin.SkinClass;
using CCWin.SkinControl;
using Microsoft.Ajax.Utilities;
using MySql.Data.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Controller = System.Web.Mvc.Controller;

namespace bishe1.Controllers
{
    
    public class HelloWorldController : Controller
    {
        private string uid, name, uname, pwd, type;
        public static string connectstr = DBManager.LocalhostMySQLdbstr;
        private UserDBContext db = new UserDBContext();
        
        MySqlConnection mySqlConnector = new MySqlConnection(connectstr);
        
        public ActionResult Index()
        {
            //mySqlConnector.Open();
            User testUser; 
            
            try
            {
                mySqlConnector.Open();
                MySqlCommand loginCommand = mySqlConnector.CreateCommand();
                loginCommand.CommandText = "select * from user";
                //MySqlCommand testCommand = new MySqlCommand("select * from user");
                //MySqlDataReader loginReader = loginCommand.ExecuteReader();
                DataTable dataSet = new DataTable();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
                dataAdapter.SelectCommand = loginCommand;
                dataAdapter.Fill(dataSet);
                /*while (loginReader.Read())
                {
                    uid = loginReader["id"].ToString();
                    uname = loginReader["username"].ToString();
                    pwd = loginReader["password"].ToString();
                    uname = loginReader["name"].ToString();
                    type = loginReader["type1"].ToString();
                }
                testUser = new User()
                {
                    ID = int.Parse(uid),
                    Username = uname,
                    Password = pwd,
                    Name = name,
                    Type = type
                };*/
                ViewData["User"] = dataSet;
                mySqlConnector.Close();
                return View();

            }
            catch (Exception exception)
            {
                string ex = exception.ToString();
                return View(ex);
                //MessageBox.Show(exception.ToString(), "异常提示");
                mySqlConnector.Close();
                //MessageBox.Show("数据库连接异常！","异常提示");
            }
            /*User testUser = new User()
            {
                ID = 12,
                Username = "testconview",
                Password = "控制器到视图测试，非数据库",
                Name = "控制器到视图测试，非数据库",
                Type = "454"
            };*/
            

            
            /*MySqlConnection mySqlConnection = new MySqlConnection(connectstr);
            try
            {
                MySqlCommand testCommand = mySqlConnection.CreateCommand();
                testCommand.CommandText = "select * from user";
                ViewBag.Message = "测试未发现异常";
            }
            catch (Exception e)
            {
                ViewBag.Message = e.ToString();
                throw;
            }*/
            //testUser = db.Users;
            
        }

        public ActionResult Welcome(string name, int numTimes = 1)
        {
            
            
            
            ViewBag.NumTimes = numTimes;

            return View();
        }
    }
}