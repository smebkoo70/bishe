using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bishe1.Entity;
using bishe1.Models;
using CCWin.SkinClass;
using CCWin.SkinControl;
using MySql.Data.MySqlClient;
using Controller = System.Web.Mvc.Controller;

namespace bishe1.Controllers
{
    public class BackManagerController : Controller
    {
        // GET: BackManager
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
                if (name3 == "" || name3 == null)
                {
                    return Redirect("../BackStage/Index");
                }
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
                return RedirectToAction("../BackStage/Index");
            }
            return View();
        }

        public ActionResult NoticePublishView()
        {
            return View();
        }


        public ActionResult LogOut()
        {
            HttpCookie cookie = Request.Cookies["RemeAdminUser"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Today.AddDays(-1);
                Response.Cookies.Add(cookie);
                Response.Redirect(Request.Url.ToString());//刷新该页
            }
            //Response.Write("<script>alert('Registration failed. Maybe the user name has already been used!');</script>");
            return View();
        }

        /// <summary>
        /// 浏览员工信息界面。
        /// </summary>
        /// <returns></returns>
        private string uid, name, uname, pwd, type;
        public static string connectstr = DBManager.LocalhostMySQLdbstr;
        private UserDBContext db = new UserDBContext();

        MySqlConnection InfoLoadConnector = new MySqlConnection(connectstr);
        public ActionResult UserListView()
        {
            try
            {
                InfoLoadConnector.Open();
                MySqlCommand InfoLoadCommand = InfoLoadConnector.CreateCommand();
                InfoLoadCommand.CommandText = "select * from user_information";
                //MySqlCommand testCommand = new MySqlCommand("select * from user");
                //MySqlDataReader loginReader = loginCommand.ExecuteReader();
                DataTable dataSet = new DataTable();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
                dataAdapter.SelectCommand = InfoLoadCommand;
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
                ViewData["UserInfo"] = dataSet;
                InfoLoadConnector.Close();
                return View();

            }
            catch (Exception exception)
            {
                string ex = exception.ToString();
                InfoLoadConnector.Close();
                return View(ex);
                //MessageBox.Show(exception.ToString(), "异常提示");
                //mySqlConnector.Close();
                //MessageBox.Show("数据库连接异常！","异常提示");
            }
            return View();
        }


        public IList<notice> NoticeList { get; set; }
        NoticeEntities1 ne = new NoticeEntities1();

        public ActionResult NoticeIndex()
        {
            NoticeList = ne.Set<notice>().ToList();
            ViewBag.NoteList = NoticeList;
            return View();
            
        }
        /// <summary>
        /// 发布一篇新的通知
        /// </summary>
        /// <returns></returns>
        public ActionResult NewNotice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewNotice(notice NewNote)
        {
            if (!ModelState.IsValid)
            {
                return Content("error");
            }
            NewNote.id = Guid.NewGuid().ToInt32();
            NewNote.subtime = DateTime.Now;
            NewNote.edittime = DateTime.Now;
            ne.notice.Add(NewNote);
            //ne.publishes.Add(NewNote);
            ne.SaveChanges();
            NoticeList = ne.Set<notice>().ToList();
            ViewBag.NoteList = NoticeList;
            return Redirect("NoticeIndex");
        }


        public ActionResult NoticeVisit(int id)
        {
            notice noteInfo = ne.Set<notice>().Where(u => u.id == id).FirstOrDefault();
            return View(noteInfo);
        }

        public ActionResult NoticeModify(int id)
        {
            notice noteInfo = ne.Set<notice>().Where(u => u.id == id).FirstOrDefault();
            return View(noteInfo);
        }

        [HttpPost]
        public ActionResult NoticeModify(notice EditNote)
        {
            if (!ModelState.IsValid)
            {
                return Content("error");
            }
            //publishInfo.EditTime = DateTime.Now;
            EditNote.edittime = DateTime.Now;
            UpdateModel<notice>(EditNote);
            ne.Entry(EditNote).State = EntityState.Modified;
            ne.SaveChanges();
            NoticeList = ne.Set<notice>().ToList();
            ViewBag.NoteList = NoticeList;
            //return View("NoticeIndex");
            return RedirectToAction("NoticeIndex");
        }

        public ActionResult NoticeDelete(int id)
        {
            notice NoteInfo = ne.notice.SingleOrDefault(a => a.id == id);
            ne.Set<notice>().Remove(NoteInfo);
            ne.SaveChanges();

            NoticeList = ne.Set<notice>().ToList();
            ViewBag.NoteList = NoticeList;
            return RedirectToAction("NoticeIndex");
        }

        /*[HttpPost]
        public ActionResult Edit(notice publishInfo)
        {
            if (!ModelState.IsValid)
            {
                return Content("error");
            }
            //publishInfo.EditTime = DateTime.Now;
            UpdateModel<user_information>(publishInfo);
            ne.Entry(publishInfo).State = EntityState.Modified;
            ne.SaveChanges();
            publishList = be.Set<user_information>().ToList();
            ViewBag.publishList = publishList;
            return View("Index");
        }*/
        public ActionResult EmailView()
        {
            return View();
        }
        /// <summary>
        /// 发送邮件,根据收件人的用户名获取邮箱
        /// </summary>

        //public static string connectstr = DBManager.LocalhostMySQLdbstr;

        MySqlConnection SendEmailConn = new MySqlConnection(connectstr);

        private string receiver, e_title, e_contents, emailaddress, title, email, content;
        [HttpPost]
        public ActionResult SendEmail(user_elist elist)
        {
            receiver = elist.loginID;
            e_title = elist.title;
            e_contents = elist.content;
            if (receiver == "")
            {
                RedirectToAction("EmailView");
            }
            try
            {
                SendEmailConn.Open();
                MySqlCommand EmailCmd = SendEmailConn.CreateCommand();
                //根据用户名，找到电子邮件地址
                EmailCmd.CommandText = "select email from user_information where loginID = '" 
                                       + receiver + "'";
                MySqlDataReader EmailReader = EmailCmd.ExecuteReader();
                while (EmailReader.Read())
                {
                    emailaddress = EmailReader["email"].ToString();
                }

                title = e_title;
                email = emailaddress;
                content = e_contents;
                string senderServerIp = "smtp.qq.com";   //使用163代理邮箱服务器（也可是使用qq的代理邮箱服务器，但需要与具体邮箱对相应）
                string toMailAddress = email;              //要发送对象的邮箱
                string fromMailAddress = "645979671@qq.com";//你的邮箱
                string subjectInfo = title;                  //主题
                string bodyInfo = "<p>" + content + "</p>";//以Html格式发送的邮件
                string mailUsername = "645979671@qq.com";              //登录邮箱的用户名
                string mailPassword = "gmrgirkqabjtbbfg"; //对应的登录邮箱的第三方密码（你的邮箱不论是163还是qq邮箱，都需要自行开通stmp服务）
                string mailPort = "25";                      //发送邮箱的端口号
                //string attachPath = "E:\\123123.txt; E:\\haha.pdf";

                //创建发送邮箱的对象
                Email myEmail = new Email(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);

                //添加附件
                //email.AddAttachments(attachPath);

                if (myEmail.Send())
                {
                    Response.Write("<script>alert('邮件已成功发送!');</script>");
                    string log = "邮件已成功发送!";
                    ViewBag.log = log;
                    return View();
                    //return Content("<script>alert('邮件已成功发送!')</script>");
                    //RedirectToAction();
                }
                else
                {
                    Response.Write("<script>alert('邮件发送失败了!');</script>");
                    string log = "邮件发送失败了!";
                    ViewBag.log = log;
                    return View();
                    //return Content("<script>alert('邮件发送失败!')</script>");
                }
            }
            catch (Exception e)
            {
                string ee = e.ToString();
                return View(ee);
                //RedirectToAction();
            }
            return View();
        }

    }
}