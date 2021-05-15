using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bishe1.Models;
using MySql.Data.MySqlClient;

namespace bishe1.Controllers
{
    public class BackStageController : Controller
    {
        // GET: BackStage
        public ActionResult Index()
        {
            //获取cookie中的数据
            HttpCookie ReadCookie = Request.Cookies.Get("RemeAdminUser");

            //判断cookie是否空值
            if (ReadCookie != null)
            {
                Admin_Info reUser = new Admin_Info();
                //把保存的用户名和密码赋值给对应的文本框
                //用户名
                string name3 = ReadCookie.Values["UserName"].ToString();

                ViewBag.UserName = name3;
                //ViewBag.UserName = "123456";
                //密码
                string pwd3 = ReadCookie.Values["UserPwd"].ToString();
                ViewBag.Password = pwd3;
                reUser.Username = name3;

                reUser.Password = pwd3;
                //ViewData["remeuser"] = reUser;

            }
            else
            {

            }
            return View();
            return View();
        }

        public ActionResult NoticeView()
        {
            return View();
        }

        /// <summary>
        /// 后台登录。
        /// </summary>
        private string au1, ap1, ap11, au2, ap2;
        public static string connectstr = DBManager.LocalhostMySQLdbstr;

        MySqlConnection loginConnection = new MySqlConnection(connectstr);
        [HttpPost]
        public ActionResult AdminLoginExcessive(Admin_Info info)
        {
            au1 = info.Username;
            ap1 = info.Password;
            try
            {
                loginConnection.Open();
                MySqlCommand CheckLoginCmd = loginConnection.CreateCommand();
                CheckLoginCmd.CommandText = "select * from admin_userinfo where adminname = '" + au1 + "'";
                MySqlDataReader CheckLoginReader = CheckLoginCmd.ExecuteReader();
                string pwd11 = MD5Str.MD5(ap1);
                while (CheckLoginReader.Read())
                {
                    ap2 = CheckLoginReader["password"].ToString();
                }

                if (ap2 == pwd11)
                {
                    //Session["uinfo"] = user;
                    //ViewData["test1"] = user;
                    HttpCookie RememberCookie = new HttpCookie("RemeAdminUser");
                    RememberCookie["UserName"] = au1;
                    RememberCookie["UserPwd"] = ap1;
                    RememberCookie.Expires = DateTime.Now.AddDays(3);

                    //RememberCookie.Value = "username";
                    //RememberCookie.Expires = DateTime.Now;
                    Response.Cookies.Add(RememberCookie);
                    //Response.Write("<script>alert('"+ RemCheck.ToString() + "');</script>");

                    loginConnection.Close();
                    return RedirectToAction("../BackManager/Index");
                    /*else
                    {
                        HttpCookie RememberCookie = new HttpCookie("RemeUser");
                        
                        RememberCookie.Expires = DateTime.Now.AddDays(-1);

                        
                        Response.Cookies.Add(RememberCookie);
                        Response.Write("<script>alert('Login Success.');</script>");
                        loginConnection.Close();
                    }*/
                }
                else
                {
                    //return RedirectToAction("Index");
                    //Response.Write("<script>alert('Login Failed,your username or password is error.');</script>");
                    loginConnection.Close();
                    return RedirectToAction("ErrorView");

                }
                return View();
            }
            catch (Exception exception)
            {
                string ex = exception.ToString();
                return View(ex);
                //return RedirectToAction("ErrorView");
            }
            //return View();
        }


        public ActionResult ErrorView()
        {
            return View();
        }
    }
}