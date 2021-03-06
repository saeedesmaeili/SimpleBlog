﻿using Blog.Helpers;
using Blog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc.Filters;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public ActionResult Details(int? id, int? postId)
        {
            BlogPost blogPost = db.BlogPosts.Find(postId);

            // SiteMap and Breadcrumb
            var node = SiteMaps.Current.FindSiteMapNodeFromKey("BlogPosts_Category");
            if (node != null)
            {
                node.Title = blogPost.Category.Name;
            }
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        public ActionResult ShowByCategory(int? id, int? page)
        {
            int BlogPostsPageSize = int.Parse(ConfigurationManager.AppSettings["BlogPostsPageSize"]);
            var category = db.Categories.Find(id);
            var posts = from p in db.BlogPosts select p;
            if (id != null)
            {
                ViewBag.CategoryId = id;
                ViewBag.CategoryName = category.Name;
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
            posts = posts
                        .Include("Author")
                        .OrderByDescending(x => x.PublishDate);
            return View("Index", posts.ToPagedList((page ?? 1), BlogPostsPageSize));
        }

        public ActionResult ArchiveList()
        {
            var blogPosts = from a in db.BlogPosts select a;
            if (!User.IsInRole("Admin") && !User.IsInRole("Author"))
            {
                blogPosts = blogPosts.Where(x => x.IsPublished);
            }
            return View(blogPosts
                            .GroupBy(x => new
                            {
                                Year = x.PublishShamsiYear,
                                Mounth = x.PublishShamsiMounth
                            })
                            .OrderByDescending(o => o.Key.Year)
                            .ThenByDescending(n => n.Key.Mounth)
                            .Select(x => new ArchiveItemViewModel()
                            {
                                Year = x.Key.Year,
                                Mounth = x.Key.Mounth,
                                Count = x.Count()
                            }));
        }

        public ActionResult ShowArchive(int year, int mounth, int? page)
        {

            int BlogPostsPageSize = int.Parse(ConfigurationManager.AppSettings["BlogPostsPageSize"]);
            var posts = from p in db.BlogPosts select p;

            // Set Beardcrumb
            var node = SiteMaps.Current.FindSiteMapNodeFromKey("BlogPosts_ShowArchive");
            if (node != null)
            {
                node.Title = mounth.GetPersianMounthName() + " " + year;
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
            ViewBag.Mounth = mounth;
            ViewBag.Year = year;
            return View("ShowArchive", posts
                                .OrderByDescending(x => x.PublishDate)
                                .Include("Author")
                                .ToPagedList((page ?? 1), BlogPostsPageSize)
                        );
        }

        //[OutputCache(Duration = 3600)]
        public ActionResult Archive(int? year , int? mounth)
        {
            var blogPosts = from a in db.BlogPosts select a;
            if (!User.IsInRole("Admin") && !User.IsInRole("Author"))
            {
                blogPosts = blogPosts.Where(x => x.IsPublished);
            }
            ViewBag.Mounth = mounth;
            ViewBag.Year = year;
            return PartialView("_Archive", blogPosts
                                            .GroupBy(x => new
                                            {
                                                Year = x.PublishShamsiYear,
                                                Mounth = x.PublishShamsiMounth
                                            })
                                            .OrderByDescending(o => o.Key.Year)
                                            .ThenByDescending(n => n.Key.Mounth)
                                            .Select(x => new ArchiveItemViewModel()
                                            {
                                                Year = x.Key.Year,
                                                Mounth = x.Key.Mounth,
                                                Count = x.Count()
                                            })
                                );
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? id)
        {
            var node = SiteMaps.Current.FindSiteMapNodeFromKey("BlogPosts_Create");

            // if Category Selected and We Have it
            if (id != null)
            {
                ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != 0), "Id", "Name", id);
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
                ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != 0), "Id", "Name");
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
        public ActionResult Create([Bind(Include = "Id,Title,Content,Preview,ShowPicture,Keywords,MetaDescription,IsPublished,PublishDate,CategoryId")] BlogPost blogPost)
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
                blogPost.Picture = WebConfigurationManager.AppSettings["NoPostPictureName"];

            }



            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            blogPost.AuthorId = currentUser.Id.ToString();
            blogPost.CreateDate = DateTime.Now;
            blogPost.LastModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {

                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = blogPost.CategoryId, postId = blogPost.Id });
            }

            ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != 0), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }


        [Authorize(Roles = "Admin")]
        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != 0), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,Picture,Preview,Keywords,MetaDescription,ShowPicture,Author,IsPublished,PublishDate,CreateDate,CategoryId")] BlogPost blogPost)
        {

            if (Request.Files[0].ContentLength != 0)
            {
                string pathToSave = Server.MapPath(WebConfigurationManager.AppSettings["ImagePostPath"]);
                if (blogPost.Picture != WebConfigurationManager.AppSettings["NoPostPictureName"])
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
                return RedirectToAction("Details", new { id = blogPost.CategoryId , postId = blogPost.Id });
            }
            ViewBag.CategoryId = new SelectList(db.Categories.Where(x => x.ParentId != 0), "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }


        // GET: BlogPosts1/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
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
        public void DeleteConfirmed(int id, string urlReferrer)
        {
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            if (urlReferrer.Contains("/delete"))
            {
                Response.Redirect(Url.Action("ShowByCategory"));
            }
            Response.Redirect(urlReferrer);
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