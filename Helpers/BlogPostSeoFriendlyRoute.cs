using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace Blog.Helpers
{
    public class BlogPostSeoFriendlyRoute : Route
    {
        public BlogPostSeoFriendlyRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("postId"))
                    routeData.Values["postId"] = GetIdValue(routeData.Values["postId"]);
            }

            return routeData;
        }

        private object GetIdValue(object postId)
        {
            if (postId != null)
            {
                string idValue = postId.ToString();

                var regex = new Regex(@"^(?<postId>\d+).*$");
                var match = regex.Match(idValue);

                if (match.Success)
                {
                    return match.Groups["postId"].Value;
                }
            }

            return postId;
        }
    }
}