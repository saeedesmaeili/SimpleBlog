using Blog.Models;
using MvcSiteMapProvider;
using reCaptcha;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutMe()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ContactMe()
        {
            
            ViewBag.Recaptcha = ReCaptcha.GetHtml(ConfigurationManager.AppSettings["ReCaptcha:SiteKey"]);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactMe(EmailFormViewModel model)
        {
            if (ModelState.IsValid && ReCaptcha.Validate(ConfigurationManager.AppSettings["ReCaptcha:SecretKey"]))
            {

                var body = "<div dir=\"rtl\"><p >ایمیل از: {0} ({1})</p><p>پیام:</p><p>{2}</p></div>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("smaeily@gmail.com")); //replace with valid value
                message.Subject = "وبلاگ سعید اسماعیلی";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return View("MailSent");
                }
            }

            ViewBag.RecaptchaLastErros = ReCaptcha.GetLastErrors(this.HttpContext);
            ViewBag.Recaptcha = ReCaptcha.GetHtml(ConfigurationManager.AppSettings["ReCaptcha:SiteKey"]);
            return View(model);
        }
    }
}