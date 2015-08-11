using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        public ActionResult Menu()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return PartialView("_Categories", db.Categories.ToList());
            }
        }
    }
}