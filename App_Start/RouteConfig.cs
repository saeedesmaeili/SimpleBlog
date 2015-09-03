using Blog.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Admin",
                url: "Admin",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Search",
                url: "Search/{query}",
                defaults: new { controller = "Search", action = "Index", query = UrlParameter.Optional }
            );


            //routes.MapRoute(
            //    name: "PostDetails",
            //    url: "BlogPosts/Details/{id}/{postId}",
            //    defaults: new { controller = "BlogPosts", action = "Details", id = UrlParameter.Optional , postId = UrlParameter.Optional }
            //);

            routes.Add("PostDetails", new BlogPostSeoFriendlyRoute("BlogPosts/Details/{id}/{postId}",
           new RouteValueDictionary(new { controller = "BlogPosts", action = "Details", id = UrlParameter.Optional, postId = UrlParameter.Optional }),
           new MvcRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "BlogPosts", action = "ShowByCategory", id = UrlParameter.Optional }
            );



            //Add the following line of code
            routes.MapMvcAttributeRoutes();

        }
    }
}
