using bishe1.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCWin.SkinClass;

namespace bishe1.Controllers
{
    public class BackAssessController : Controller
    {
        // GET: BackAssess
        public static string connectstr = DBManager.LocalhostMySQLdbstr;

        MySqlConnection InfoLoadConn = new MySqlConnection(connectstr);


        public IList<assess1> AssessList { get; set; }
        public IList<assess1> AssessList2 { get; set; }
        AssessEntities2 ae = new AssessEntities2();
        public ActionResult Index()
        {
            AssessList = ae.Set<assess1>().Where(u => u.auditstatus == 0).ToList();
            ViewBag.AssessList = AssessList;

            AssessList2 = ae.Set<assess1>().Where(u => u.auditstatus == 1).ToList();
            ViewBag.AssessList2 = AssessList2;
            return View();
        }

        /// <summary>
        /// 最后的评估
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private float basis, performance, craft, manqin;

        public ActionResult AssessModify(int id)
        {
            assess1 assessInfo = ae.Set<assess1>().Where(u => u.id == id).FirstOrDefault();
            return View(assessInfo);
            
        }

        [HttpPost]
        public ActionResult AssessModify(assess1 NewAssess1)
        {

            //NewAssess1.id = Guid.NewGuid().ToInt32();
            //评估单的ID，随机生成
            //NewAssess1.auditID = RandomStr.RandomAuditID();
            //评估提交日期，系统时间
            //NewAssess1.date = DateTime.Now;
            //loginID,直接读
            //NewAssess1.loginID = loginID;
            //审核状态，0为未审核，期初都填0
            NewAssess1.auditstatus = 1;
            //后台给分，刚开始默认-1;
            //NewAssess1.backres = -1;
            //工资，刚开始也是0
            NewAssess1.wage = 0;
            //总结果，也是0
            //int self1 = int.Parse(NewAssess1.self);
            string chazhii = (NewAssess1.self - NewAssess1.backres).ToString();
            int chazhi = Math.Abs(int.Parse(chazhii));
            if (chazhi < 3)
            {
                string res1 = (NewAssess1.self + NewAssess1.backres).ToString();
                int res2 = int.Parse(res1);
                float res = res2/2;
                NewAssess1.result = res;


            }
            else if (chazhi >= 3)
            {
                string self1 = NewAssess1.self.ToString();
                string backres1 = NewAssess1.backres.ToString();
                int self2 = int.Parse(self1);
                int backres2 = int.Parse(backres1);
                
                //int res2 = int.Parse(res1);
                float res = (self2 / 5  ) +  (backres2/5 )*4;
                NewAssess1.result = res;

            }

            try
            {
                InfoLoadConn.Open();
                MySqlCommand WageInfoReadCommand = InfoLoadConn.CreateCommand();
                WageInfoReadCommand.CommandText = "select * from wage where type = '"
                                                 + NewAssess1.type + "'";
                MySqlDataReader WageInfoReader = WageInfoReadCommand.ExecuteReader();
                while (WageInfoReader.Read())
                {
                    basis = float.Parse(WageInfoReader["basis"].ToString());
                    performance = float.Parse(WageInfoReader["performance"].ToString());
                    manqin = float.Parse(WageInfoReader["manqin"].ToString());
                    craft = float.Parse(WageInfoReader["craft"].ToString());
                }
                InfoLoadConn.Close();
            }
            
            catch (Exception e)
            {
                string ee = e.ToString();
                return View(ee);
            }

            float wageres = basis + basis * manqin / 100 + craft +
                            basis * performance * float.Parse((NewAssess1.result).ToString()) / 1000;
            NewAssess1.wage = wageres;
            if (!ModelState.IsValid)
            {
                return Content("error");
            }
            //publishInfo.EditTime = DateTime.Now;
            //NewAssess1.edittime = DateTime.Now;
            UpdateModel<assess1>(NewAssess1);
            ae.Entry(NewAssess1).State = EntityState.Modified;
            ae.SaveChanges();
            AssessList = ae.Set<assess1>().ToList();
            ViewBag.NewAssess1 = NewAssess1;
            return RedirectToAction("Index");
        }


        public ActionResult AssessDetails(int id)
        {
            assess1 assessInfo = ae.Set<assess1>().Where(u => u.id == id).FirstOrDefault();
            return View(assessInfo);
        }
    }
}