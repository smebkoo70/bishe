using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bishe1.Models;
using System.Collections;

namespace bishe1.Controllers
{
    public class LoginController : Controller
    {
        
        // GET: Login
        public ActionResult Index()
        {

            //获取cookie中的数据
            HttpCookie ReadCookie = Request.Cookies.Get("Remeuser");
            
            //判断cookie是否空值
            if (ReadCookie != null)
            {
                User reUser = new User();
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
                ViewData["remeuser"] = reUser;

            }
            else
            {
                
            }
            return View();
           
        }
        
        
        public ActionResult TestView()
        {
            //获取cookie中的数据
            HttpCookie ReadCookie = Request.Cookies.Get("Remeuser");

            //判断cookie是否空值
            if (ReadCookie != null)
            {
                User reUser = new User();
                //把保存的用户名和密码赋值给对应的文本框
                //用户名
                string name3 = ReadCookie.Values["UserName"].ToString();

                //ViewBag.UserName = name3;
                ViewBag.UserName = "123456";
                //密码
                string pwd3 = ReadCookie.Values["UserPwd"].ToString();
                ViewBag.Password = pwd3;
                reUser.Username = name3;

                reUser.Password = pwd3;
                ViewData["remeuser"] = reUser;
                Response.Write("<script>alert('" + name3 + " + " + pwd3 + "');</script>");

            }
            else
            {
                Response.Write("<script>alert('Cookie什么都没有');</script>");

            }
            return View();
        }


        /// <summary>
        /// 数据库登录验证
        /// </summary>
        private string name1="", name2="", pwd1="", pwd2, type;
        //name1,pwd1是表单的用户名和密码,name2是数据库的
        public static string connectstr = DBManager.LocalhostMySQLdbstr;
        
        MySqlConnection loginConnection = new MySqlConnection(connectstr);
        /// <summary>
        /// 登录模块
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoginPage(User user)
        {
            name1 = user.Username;
            pwd1 = user.Password;
            
            if (name1 == "" || pwd1 == "")
            {
                
                //Response.Write("<script>alert('Login Failed,your username or password can not be empty!');</script>");
                return RedirectToAction("ErrorView");
            }
            
            try
            {
                loginConnection.Open();
                MySqlCommand CheckLoginCmd = loginConnection.CreateCommand();
                CheckLoginCmd.CommandText = "select * from user where username = '" + name1 + "'";
                MySqlDataReader CheckLoginReader = CheckLoginCmd.ExecuteReader();
                string pwd11 = MD5Str.MD5(pwd1);
                while (CheckLoginReader.Read())
                {
                    pwd2 = CheckLoginReader["password"].ToString();
                }

                if (pwd2 == pwd11)
                {
                    //Session["uinfo"] = user;
                    ViewData["test1"] = user;
                    HttpCookie RememberCookie = new HttpCookie("RemeUser");
                    RememberCookie["UserName"] = name1;
                    RememberCookie["UserPwd"] = pwd1;
                    RememberCookie.Expires = DateTime.Now.AddDays(3);

                    //RememberCookie.Value = "username";
                    //RememberCookie.Expires = DateTime.Now;
                    Response.Cookies.Add(RememberCookie);
                    //Response.Write("<script>alert('"+ RemCheck.ToString() + "');</script>");
                    
                    loginConnection.Close();
                    return RedirectToAction("../MemberLogon/Jumpto");
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
                //return View(ex);
                return RedirectToAction("ErrorView");
            }

        }


        public ActionResult ErrorView()
        {
            Response.Write("<script>alert('Login Failed,your username or password is error.');</script>");
            return View();
            //return RedirectToAction("../Home/Index");
            //return RedirectToAction("Index");
        }

        public ActionResult RegisterPage()
        {
            
            return View();
            
        }

        /// <summary>
        /// 注册模块,要写入两个表。
        /// </summary>
        private string rusername,rpwd,rloginID,remail,rqq,rtel,rtime;

        //public static string connectstr = DBManager.LocalhostMySQLdbstr;
        
        MySqlConnection RegisterConn = new MySqlConnection(connectstr);
        [HttpPost]
        public ActionResult RegisterView(UserInformation info)
        {
            rusername = info.Username;
            rpwd = info.Password;
            rloginID = info.LoginID;
            remail = info.Email;
            rqq = info.QQ;
            rtel = info.Tel;
            rtime = DateTime.Now.ToString();
            ViewData["regtest"] = info;
            /*string text1 = "insert into user_information (username,loginID,password,flag,email,qq,tel,registertime,type1) values ('"
                + rusername + "','" + rloginID + "','" + rpwd + "',1,'" + remail + "','" + rqq + "','" + rtel +
                "','" + rtime + "','员工')";
            Response.Write("<script>alert('" + text1 + "');</script>");*/
            try
            {
                RegisterConn.Open();
                string rpwd1 = MD5Str.MD5(rpwd);
                MySqlCommand RegisterCmd = RegisterConn.CreateCommand();
                RegisterCmd.CommandText =
                    "insert into user_information (username,loginID,password,flag,email,qq,tel,registertime,type1) values ('"
                    + rusername + "','" + rloginID + "','" + rpwd1 + "',1,'" + remail + "','" + rqq + "','" + rtel +
                    "','" + rtime + "','员工')";
                int res = 0;
                res = RegisterCmd.ExecuteNonQuery();
                if (res > 0)
                {
                    res = 0;
                    RegisterCmd.CommandText =
                        "insert into user (username,password,name,type1) values ('"
                        + rusername + "','"  + rpwd1 + "','" + rloginID +  "','员工')";
                    res = RegisterCmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Response.Write("<script>alert('Register Success.');</script>");
                        RegisterConn.Close();
                        return RedirectToAction("RegSuccess");
                    }
                }
                return View();
            }
            catch (Exception exception)
            {
                string ex = exception.ToString();
                //return View(ex);
                return RedirectToAction("RegFailed");
                //MessageBox.Show(exception.ToString(), "异常提示");
                //mySqlConnector.Close();
                //MessageBox.Show("数据库连接异常！","异常提示");
            }
            
            
        }


        public ActionResult RegSuccess()
        {
            Response.Write("<script>alert('Register Success.');</script>");
            return View();
        }


        public ActionResult RegFailed()
        {
            Response.Write("<script>alert('Registration failed. Maybe the user name has already been used!');</script>");
            return View();
        }

        /// <summary>
        /// 注销登录状态，删除cookie
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            HttpCookie cookie = Request.Cookies["Remeuser"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Today.AddDays(-1);
                Response.Cookies.Add(cookie);
                Response.Redirect(Request.Url.ToString());//刷新该页
            }
            //Response.Write("<script>alert('Registration failed. Maybe the user name has already been used!');</script>");
            return View();
        }
    }
}
//@(!Html.isSignedIn) ? Html.ActionLink("登陆", "Index", "Login") : Html.ActionLink("登出", "aaaa", "bbbb");
