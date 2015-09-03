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
            return RedirectToAction("ShowByCategory", "BlogPosts");
            //return View();
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

                var body = "<table width=\"100%\" dir=\"rtl\" cellpadding=\"10\"><tr><td><h1>{0}</h1><br/><h2>{1}</h2></td></tr><tr><td><p>{2}</p></td></tr></table>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(ConfigurationManager.AppSettings["ContactMeMailReciver"])); //replace with valid value
                message.Subject = String.Format(ConfigurationManager.AppSettings["ContactMeMailSubject"] , model.FromName );
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