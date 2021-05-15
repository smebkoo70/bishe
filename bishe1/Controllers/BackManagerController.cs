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
                return Redirect("../BackStage/Index");
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


    }
}