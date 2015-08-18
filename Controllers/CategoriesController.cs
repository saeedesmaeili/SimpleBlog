using Blog.Helpers;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
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

        [OutputCache(Duration  = 43200)]
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
                    var item = new SyndicationItem(
                                    p.Title,
                                    p.Content,
                                    new Uri(new Uri(baseUrl), "BlogPosts/Details/" + p.Id),
                                    p.Id.ToString(),
                                    p.LastModifiedDate);
                    item.Categories.Add(new SyndicationCategory(p.Category.Name));
                    item.Authors.Add(new SyndicationPerson(p.Author.Email, p.Author.FirstName + " " + p.Author.LastName, ""));
                    item.Copyright = new TextSyndicationContent("Copyright " + DateTime.Now.Year + " By <a href=\"Http://saeedEsmaeili.ir\">Saeed Esmaeili</a>", TextSyndicationContentKind.Html);
                    item.Summary = new TextSyndicationContent(p.Preview, TextSyndicationContentKind.Html);
                    item.Content = new TextSyndicationContent(p.Content, TextSyndicationContentKind.Html);
                    item.PublishDate = p.PublishDate;
                    
                    feedItems.Add(item);
                }
                SyndicationFeed feed = new SyndicationFeed(feedItems);
                //foreach (Category category in db.Categories.ToList()) {
                //    feed.Categories.Add(new SyndicationCategory(category.Name));
                //}
                return new RssActionResult(feed);
            }
        }
    }
}