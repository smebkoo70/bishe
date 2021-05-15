using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bishe1.Controllers
{
    public class EmailTestController : Controller
    {
        // GET: EmailTest
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">邮件主题</param>
        /// <param name="email">要发送对象的邮箱</param>
        /// <param name="content">邮件内容</param>
        /// <returns></returns>
        public ActionResult Index(string title, string email, string content)
        {
            title = "ASP.NET MVC 测试邮件";
            email = "419683141@qq.com";
            content = DateTime.Now.ToString();
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
                return Content("<script>alert('邮件已成功发送!')</script>");
            }
            else
            {
                return Content("<script>alert('邮件发送失败!')</script>");
            }

            return View();

        }
    }
}