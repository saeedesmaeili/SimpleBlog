using Blog.Helpers;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using Blog.Helpers;
using System.Xml.Linq;
using System.Web.Configuration;

namespace Blog.Controllers
{
    public class CategoriesController : Controller
    {


        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Categories
        public ActionResult Menu(Guid? SelectedCategoryId, Guid? SelectedCategoryParentId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ViewBag.SelectedCategoryParentId = SelectedCategoryParentId;
                ViewBag.SelectedCategoryId = SelectedCategoryId;
                return PartialView("_Categories", db.Categories
                                                    .OrderBy(x => x.Order)
                                                    .ToList());
            }
        }
        //[OutputCache(Duration = 43200)]
        public ActionResult Rss(Guid? id)
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var posts = from p in db.BlogPosts select p;
                string feedTitle = "وبلاگ سعید اسماعیلی";
                List<SyndicationItem> feedItems = new List<SyndicationItem>();
                if (id != null)
                {
                    posts = posts.Where(post => post.CategoryId == id);
                    Category category = db.Categories.Find(id);
                    feedTitle += "-" + category.Name;
                }

                foreach (BlogPost p in posts.OrderBy(p => p.PublishDate).Take(25).ToList())
                {
                    if (true)
                    {

                    }
                    string picture = p.Picture + "?w=900&h=300&mode=crop";
                    var Summary = "<div dir=\"rtl\" style=\"font-family:tahoma\"><img src=\"" + baseUrl + "Uploads/images_post/" + picture + "\" /><br/>" + p.Preview + "</div>";
                    var item = new SyndicationItem();
                    item.AddPermalink(new Uri(new Uri(baseUrl), "BlogPosts/Details/" + p.CategoryId + "?postId=" + p.Id));
                    item.Id = p.Id.ToString();
                    item.Title = new TextSyndicationContent(p.Title, TextSyndicationContentKind.Html);
                    item.Summary = new CDataSyndicationContent(new TextSyndicationContent(Summary, TextSyndicationContentKind.Html));
                    item.Categories.Add(new SyndicationCategory(p.Category.Name));
                    item.Authors.Add(new SyndicationPerson(p.Author.Email, p.Author.FirstName + " " + p.Author.LastName, ""));
                    item.Copyright = new TextSyndicationContent("Copyright " + DateTime.Now.Year + " By <a href=\"Http://saeedEsmaeili.ir\">Saeed Esmaeili</a>", TextSyndicationContentKind.Html);
                    item.PublishDate = p.PublishDate;
                    item.LastUpdatedTime = p.LastModifiedDate;
                    var content = "<div dir=\"rtl\" style=\"font-family:tahoma\"><img src=\"" + baseUrl + "Uploads/images_post/" + picture +"\" /><br/>" + p.Content + "</div>";
                    item.Content = new CDataSyndicationContent(new TextSyndicationContent(content, TextSyndicationContentKind.Html));
                    feedItems.Add(item);
                }
                SyndicationFeed feed = new SyndicationFeed(feedItems) {
                                                                         Id = "0",
                                                                         Title = new TextSyndicationContent(feedTitle, TextSyndicationContentKind.Plaintext),
                                                                         ImageUrl = new Uri(new Uri(baseUrl ), "favicon-32x32.png")

                };

                //foreach (Category category in db.Categories.ToList()) {
                //    feed.Categories.Add(new SyndicationCategory(category.Name));
                //}
                return new RssActionResult(feed);
            }
        }
        
                                 


        // GET: Categories1
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Categories
                            .OrderBy(x => x.Order)
                            .ToList()
                        );
        }


        // GET: Categories1/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.Categories, "Id", "Name").InsertEmptyFirst("شاخه اصلی", Guid.Empty.ToString());
            Category category = new Category();
            category.IconClass = "uk-icon-caret-right";
            return View(category);
        }

        // POST: Categories1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IconClass,Order,ParentId")] Category category)
        {
            if (category.ParentId == null)
            {
                category.ParentId = Guid.Empty;
            }

            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.Categories, "Id", "Name", category.ParentId).InsertEmptyFirst("شاخه اصلی", Guid.Empty.ToString());

            return View(category);
        }

        // GET: Categories1/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentId = new SelectList(db.Categories.Where(x => x.Id != category.Id), "Id", "Name", category.ParentId).InsertEmptyFirst("شاخه اصلی", Guid.Empty.ToString());

            return View(category);
        }

        // POST: Categories1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IconClass,Order,ParentId")] Category category)
        {

            if (category.ParentId == null)
            {
                category.ParentId = Guid.Empty;
            }
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.Categories.Where(x => x.Id != category.Id), "Id", "Name", category.ParentId).InsertEmptyFirst("شاخه اصلی", Guid.Empty.ToString());
            return View(category);
        }

        // GET: Categories1/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid? id, string message)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(message ?? ""))
            {
                ViewBag.message = message.ToString();
                return View(category);
            }

            return View(category);
        }

        // POST: Categories1/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Category category = db.Categories.Find(id);
            if (db.Categories.Where(x => x.ParentId == id).Count() > 0 || db.BlogPosts.Where(x => x.CategoryId == id).Count() > 0)
            {
                return RedirectToAction("Delete", new { id = id, message = "ابتدا زیر شاخه ها و پست های این شاخه را حذف کنید" });
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}