using Blog.Models;
using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Helpers
{
    public class BlogPostsDynamicNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            using (var db = new ApplicationDbContext())
            {
                foreach (var post in db.BlogPosts.Include("Category").ToList())
                {
                    DynamicNode dynamicNode = new DynamicNode();
                    dynamicNode.Title = post.Title;
                    //dynamicNode.ParentKey = "Category_" + post.Category.Name;
                    dynamicNode.Action = "Details";
                    dynamicNode.Controller = "BlogPosts";
                    dynamicNode.RouteValues.Add("id", post.CategoryId);
                    dynamicNode.RouteValues.Add("postId", post.GenerateSlug());
                    //dynamicNode.RouteValues.Add("CategoryName", post.Category.Name.Replace(" ", "-"));
                    //dynamicNode.RouteValues.Add("Title", post.Title.Replace(" ","-"));
                    dynamicNode.LastModifiedDate = post.LastModifiedDate;
                    dynamicNode.Description = post.Title;
                    dynamicNode.ChangeFrequency = ChangeFrequency.Weekly;
                    dynamicNode.UpdatePriority = UpdatePriority.Normal;
                    yield return dynamicNode;
                }
            }
        }
    }
}