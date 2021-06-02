using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bishe1.Models;
using bishe1.Entity;
using System.Data.Entity;

namespace bishe1.Controllers
{
    public class BackEditController : Controller
    {
        // GET: BackEdit
        public IList<user_information> publishList { get; set; }
        bisheEntities2 be = new bisheEntities2();
        public ActionResult Index()
        {
            //publishList = be.Set<user_information>().Where(x => x.id == 2).ToList();
            publishList = be.Set<user_information>().ToList();
            //var testpublishlist = be.user_information.Where(u => u.id == 5);
            //publishList = testpublishlist.ToList();
            ViewBag.publishList = publishList;
            return View();
        }
        /// <summary>
        /// publish == user_information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            user_information publishInfo = be.Set<user_information>().Where(u => u.id == id).FirstOrDefault();
            return View(publishInfo);
        }

        [HttpPost]
        public ActionResult Edit(user_information publishInfo)
        {
            if (!ModelState.IsValid)
            {
                return Content("error");
            }
            //publishInfo.EditTime = DateTime.Now;
            UpdateModel<user_information>(publishInfo);
            be.Entry(publishInfo).State = EntityState.Modified;
            be.SaveChanges();
            publishList = be.Set<user_information>().ToList();
            ViewBag.publishList = publishList;
            return View("Index");
        }


    }

}