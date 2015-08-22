using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Configuration;

namespace Blog.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Search
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string query, Guid? id, int? page)
        {
            int BlogPostsPageSize = int.Parse(ConfigurationManager.AppSettings["BlogPostsPageSize"]);
            ViewBag.SearchTerm = query;
            return View(db.BlogPosts
                        .OrderByDescending(x=>x.PublishDate)
                        .Where(x => x.Title.Contains(query) || x.Content.Contains(query) || x.Preview.Contains(query))
                        .ToPagedList((page ?? 1), BlogPostsPageSize)
                );

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}