using System;
using System.Collections.Generic;
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
    public class WageController : Controller
    {
        public IList<wage> WageList { get; set; }
        WageEntities2 we = new WageEntities2();
        // GET: Wage
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
            WageList = we.Set<wage>().ToList();
            ViewBag.WageList = WageList;
            return View();
        }


        public ActionResult NewWageDetail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewWageDetail(wage newWage)
        {
            if (!ModelState.IsValid)
            {
                return Content("error");
            }
            newWage.id = Guid.NewGuid().ToInt32();
            //NewNote.subtime = DateTime.Now;
            //NewNote.edittime = DateTime.Now;
            //we.notice.Add(NewNote);
            we.wage.Add(newWage);
            //ne.publishes.Add(NewNote);
            we.SaveChanges();
            WageList = we.Set<wage>().ToList();
            ViewBag.WageList = WageList;
            return RedirectToAction("Index");
        }

        public ActionResult WageModify(int id)
        {
            wage wageInfo = we.Set<wage>().Where(u => u.id == id).FirstOrDefault();
            return View(wageInfo);
        }

        [HttpPost]
        public ActionResult WageModify(wage EditWage)
        {
            if (!ModelState.IsValid)
            {
                return Content("error");
            }
            //publishInfo.EditTime = DateTime.Now;
            //EditWage.edittime = DateTime.Now;
            UpdateModel<wage>(EditWage);
            we.Entry(EditWage).State = EntityState.Modified;
            we.SaveChanges();
            WageList = we.Set<wage>().ToList();
            ViewBag.NoteList = WageList;
            //return View("NoticeIndex");
            return RedirectToAction("Index");
        }

        public ActionResult WageDelete(int id)
        {
            wage WageInfo = we.wage.SingleOrDefault(a => a.id == id);
            we.Set<wage>().Remove(WageInfo);
            we.SaveChanges();

            WageList = we.Set<wage>().ToList();
            ViewBag.WageList = WageList;
            return RedirectToAction("Index");
        }
    }
}