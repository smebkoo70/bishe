using bishe1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Org.BouncyCastle.Asn1.Crmf;
using MySql.Data.MySqlClient;

namespace bishe1.Controllers
{
    public class MemberLogonController : Controller
    {

        public ActionResult Jumpto()
        {
            Response.Write("<script>alert('Login Success.');</script>");
            return View();
        }


        // GET: MemberLogon
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
                //Response.Write("<script>alert(' name3 = " + name3 + "');</script>");
                
                if (name3 == null)
                {
                    return Redirect("../Login/Index");
                }
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
                return Redirect("../Login/Index");

            }
            //Response.Write("<script>alert('Login Success.');</script>");
            return View();
        }


        public ActionResult TesvView()
        {
            Response.Write("<script>alert('why bug?');</script>");
            return View();
        }



        /// <summary>
        /// 信息修改页面，需要载入数据
        /// </summary>

        private string name1, name2, pwd1, pwd2, type;
        //name1,pwd1是表单的用户名和密码,name2是数据库的
        public static string connectstr = DBManager.LocalhostMySQLdbstr;

        MySqlConnection InfoLoadConn = new MySqlConnection(connectstr);

        public ActionResult InfoModifyPage()
        {
            //获取cookie中的数据
            HttpCookie ReadCookie = Request.Cookies.Get("Remeuser");
            string name3 = "",pwd3 = "";
            string loginID, type, qq, email, tel, flag, registertime;
            UserInformation InfoUser = new UserInformation();
            //判断cookie是否空值
            if (ReadCookie != null)
            {

                
                //把保存的用户名和密码赋值给对应的文本框
                //用户名
                name3 = ReadCookie.Values["UserName"].ToString();
                //Response.Write("<script>alert(' name3 = " + name3 + "');</script>");

                if (name3 == null)
                {
                    return Redirect("../Login/Index");
                }
                ViewBag.UserName = name3;
                //ViewBag.UserName = "123456";
                //密码
                pwd3 = ReadCookie.Values["UserPwd"].ToString();
                ViewBag.Password = pwd3;
                InfoUser.Username = name3;

                InfoUser.Password = pwd3;
                ViewData["remeuser"] = InfoUser;

            }
            else
            {
                return Redirect("../Login/Index");

            }

            try
            {
                InfoLoadConn.Open();
                MySqlCommand InfoLoadCmd = InfoLoadConn.CreateCommand();
                InfoLoadCmd.CommandText = "select * from user_information where username = '" + name3 + "'";
                MySqlDataReader InfoReader = InfoLoadCmd.ExecuteReader();
                while (InfoReader.Read())
                {
                    ViewBag.LoginID = InfoReader["loginID"].ToString();
                    ViewBag.Flag = InfoReader["flag"].ToString();
                    ViewBag.Email = InfoReader["email"].ToString();
                    ViewBag.QQ = InfoReader["qq"].ToString();

                    ViewBag.Tel = InfoReader["tel"].ToString();

                    ViewBag.Registertime = InfoReader["registertime"].ToString();
                    ViewBag.Type = InfoReader["type1"].ToString();

                    InfoUser.LoginID = InfoReader["loginID"].ToString();
                    //InfoUser.Flag = InfoReader["flag"].ToString();
                    InfoUser.Email = InfoReader["email"].ToString();
                    InfoUser.QQ = InfoReader["qq"].ToString();

                    InfoUser.Tel = InfoReader["tel"].ToString();
                    InfoUser.RegisterTime = InfoReader["Registertime"].ToString();
                    //InfoUser.Registertime = InfoReader["registertime"].ToString();
                    InfoUser.Type = InfoReader["type1"].ToString();

                }
                InfoLoadConn.Close();
                //Response.Write("<script>alert(' name3 = " + ViewBag.LoginID.ToString()  + "');</script>");
                return View(InfoUser);
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                InfoLoadConn.Close();
                return View(ee);
                throw;

            }
            
        }

        /// <summary>
        /// 更新操作，也是更新到两个表。
        /// </summary>
        MySqlConnection UpdateConn = new MySqlConnection(connectstr);

        private string u_username, upwd, uloginID, uemail, uqq, utel;
        [HttpPost]
        public ActionResult UpdateView(UserInformation updateUser)
        {
            HttpCookie ReadCookie = Request.Cookies.Get("Remeuser");
            u_username = ReadCookie.Values["UserName"].ToString();
            upwd = updateUser.Password;
            uloginID = updateUser.LoginID;
            uemail = updateUser.Email;
            uqq = updateUser.QQ;
            utel = updateUser.Tel;
            
            
            string text = "update user_information set password = '"
                          + upwd + "',loginID = '" + uloginID + "',email = '" + uemail
                          + "',qq = '" + uqq + "',tel = '" + utel + "' where username = '" + u_username + "'";
            //Response.Write("<script>alert(' 到底bug在哪呢？ ');</script>");
            Response.Write("<script>alert(' name,loginID = " + u_username + "');</script>");
            ViewBag.MysqlText = text.ToString();
            
            try
            {
                //Response.Write("<script>alert(' name,loginID = " + u_username + "," + uloginID + "');</script>");
                UpdateConn.Open();
                string upwd1 = MD5Str.MD5(upwd);
                MySqlCommand updateInfoCmd = UpdateConn.CreateCommand();
                updateInfoCmd.CommandText = "update user_information set password = '" 
                                            + upwd1 + "',loginID = '" + uloginID + "',email = '" + uemail
                                            + "',qq = '" + uqq + "',tel = '" + utel + "' where username = '" + u_username + "'";
                int res = 0;
                res = updateInfoCmd.ExecuteNonQuery();
                if (res > 0)
                {
                    res = 0;
                    updateInfoCmd.CommandText = "update user set password = '"
                                                + upwd + "',name = '" + uloginID + "' where username = '" + u_username + "'";
                    res = updateInfoCmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        //Response.Write("<script>alert('Update User's Info Success.');</script>");
                        UpdateConn.Close();
                        return RedirectToAction("UpdateSuccess");
                    }
                }
                else
                {
                    UpdateConn.Close();
                    return RedirectToAction("UpdateFailed");
                }
                return View();
            }
            catch (Exception e)
            {
                string ss = e.ToString();
                Console.WriteLine(ss);
                return View(ss);
                //return RedirectToAction("UpdateFailed");
                throw;
            }
            return View();
        }

        public ActionResult UpdateSuccess()
        {
            Response.Write("<script>alert('Update User's Info Success.');</script>");
            return View();
        }

        public ActionResult UpdateFailed()
        {
            Response.Write("<script>alert('Update User's Info Failed.');</script>");
            return View();
        }

        public ActionResult GxView()
        {
            
            Response.Write("<script>alert('why GX bug?');</script>");
            return View();
        }
    }
}
/*
 @Html.EditorFor(model => model.Username, new { htmlAttributes = new { @class = "form-control" } })
   @Html.ValidationMessageFor(model => model.Username, "", new { @class = "text-danger" })

 */
