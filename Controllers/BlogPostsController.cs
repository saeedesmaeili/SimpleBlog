using Blog.Helpers;
using Blog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class BlogPostsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts

        [SiteMapTitle("Title")]
        public ActionResult Details(Guid? id)
        {
            BlogPost blogPost = db.BlogPosts.Find(id);

            // SiteMap and Breadcrumb
            var node = SiteMaps.Current.FindSiteMapNodeFromKey("BlogPosts_Details");
            if (node.ParentNode != null)
            {
                node.ParentNode.Title = blogPost.Category.Name;
                node.ParentNode.RouteValues["id"] = blogPost.CategoryId;
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        public ActionResult ShowByCategory(Guid? id)
        {
            var category = db.Categories.Find(id);
            var posts = from p in db.BlogPosts select p;
            if (id != null)
            {
                ViewBag.CategoryId = id;
                posts = posts.Where(x => x.CategoryId == id);
                ViewBag.CategoryParentId = category.ParentId;

                // SiteMap and Breadcrumb
                var node = SiteMaps.Current.FindSiteMapNodeFromKey("BlogPosts_Category");
                if (node != null)
                {
                    node.Title = category.Name;
                }

            }

            if (!(User.IsInRole("Admin") || User.IsInRole("Author")))
            {
                posts = posts.Where(x => x.IsPublished);
            }
            

                return View("Index", posts
                                        .Include("Author")
                                        .OrderByDescending(x => x.PublishDate)
                                        .ToList()
                            );
        }

        public ActionResult ShowArchive(int year, int mounth)
        {

            var posts = from p in db.BlogPosts select p;

            // Set Beardcrumb
            var node = SiteMaps.Current.FindSiteMapNodeFromKey("BlogPosts_ShowArchive");
            if (node != null)
            {
                node.Title = "آرشیو " + mounth.GetPersianMounthName() + " ماه " + year;
            }

            // Show UnPublished Post if user is in Role admin or Author 
            if (User.IsInRole("Admin") || User.IsInRole("Author"))
            {
                posts = posts.Where(x => x.PublishShamsiYear == year && x.PublishShamsiMounth == mounth);
            }
            else
            {
                posts = posts.Where(x => x.PublishShamsiYear == year && x.PublishShamsiMounth == mounth && x.IsPublished);
            }

            return View("Index", posts
                                .OrderByDescending(x => x.PublishDate)
                                .Include("Author")
                                .ToList()
                        );
        }

        public ActionResult Archive()
        {
            return PartialView("_Archive", db.BlogPosts
                                            .GroupBy(x => new
                                            {
                                                Year = x.PublishShamsiYear,
                                                Mounth = x.PublishShamsiMounth
                                            })
                                            .OrderByDescending(o => o.Key.Year)
                                            .ThenBy(n => n.Key.Mounth).Select(x => new ArchiveItemViewModel() { Year = x.Key.Year, Mounth = x.Key.Mounth }));
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create(Guid? id)
        {
            var node = SiteMaps.Current.FindSiteMapNodeFromKey("BlogPosts_Details");

            // if Category Selected and We Have it
            if (id != null)
            {
                ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != Guid.Empty), "Id", "Name", id);
                // SiteMap and Breadcrumb
                Category category = db.Categories.Find(id);
                if (node.ParentNode != null)
                {
                    node.ParentNode.Title = category.Name;
                    node.ParentNode.RouteValues["id"] = category.Id;
                }
            }
            else
            {
                ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != Guid.Empty), "Id", "Name");
                if (node.ParentNode != null)
                {
                    node.ParentNode.Title = "شاخه نامشخص";
                    node.ParentNode.RouteValues["id"] = "";
                }
            }

            BlogPost post = new BlogPost();
            post.PublishDate = DateTime.Now;
            return View(post);
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,Preview,Keywords,IsPublished,PublishDate,CategoryId")] BlogPost blogPost)
        {


            if (Request.Files[0].ContentLength != 0)
            {
                string pathToSave = Server.MapPath(WebConfigurationManager.AppSettings["ImagePostPath"]);
                string filename = Path.GetFileName(Request.Files[0].FileName);
                Request.Files[0].SaveAs(Path.Combine(pathToSave, filename));
                blogPost.Picture = filename;
            }
            else
            {
                blogPost.Picture = "empty.svg";
            }



            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            blogPost.AuthorId = currentUser.Id.ToString();
            blogPost.CreateDate = DateTime.Now;
            blogPost.LastModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                blogPost.Id = Guid.NewGuid();
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("ShowByCategory", new { id = blogPost.CategoryId });
            }

            ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != Guid.Empty), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }


        [Authorize(Roles = "Admin")]
        // GET: BlogPosts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != Guid.Empty), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,Picture,Preview,Keywords,Author,IsPublished,PublishDate,CreateDate,CategoryId")] BlogPost blogPost)
        {

            if (Request.Files[0].ContentLength != 0)
            {
                string pathToSave = Server.MapPath(WebConfigurationManager.AppSettings["ImagePostPath"]);
                if (blogPost.Picture != "empty.svg")
                {
                    System.IO.File.Delete(pathToSave + blogPost.Picture);
                }
                string filename = Path.GetFileName(Request.Files[0].FileName);
                Request.Files[0].SaveAs(Path.Combine(pathToSave, filename));
                blogPost.Picture = filename;
            }


            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            blogPost.AuthorId = currentUser.Id.ToString();
            blogPost.LastModifiedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowByCategory", new { id = blogPost.CategoryId });
            }
            ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != Guid.Empty), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }


        // GET: BlogPosts1/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts1/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
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