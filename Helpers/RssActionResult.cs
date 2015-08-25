using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Blog.Helpers
{
    public class RssActionResult : FileResult
    {
        private readonly SyndicationFeed _feed;

        /// <summary>
        /// Creates a new instance of RssResult
        /// based on this sample 
        /// http://www.developerzen.com/2009/01/11/aspnet-mvc-rss-feed-action-result/
        /// </summary>
        /// <param name="feed">The feed to return the user.</param>
        public RssActionResult(SyndicationFeed feed)
            : base("application/xml")
        {
            _feed = feed;
        }

        /// <summary>
        /// Creates a new instance of RssResult
        /// </summary>
        /// <param name="title">The title for the feed.</param>
        /// <param name="feedItems">The items of the feed.</param>
        public RssActionResult(string title, List<SyndicationItem> feedItems)
            : base("application/xml")
        {
            _feed = new SyndicationFeed(title, title, HttpContext.Current.Request.Url) { Items = feedItems };
        }
        //var formatter = new Rss20FeedFormatter(feed);
        //    Atom10FeedFormatter formatter = new Atom10FeedFormatter(feed);
        //using (var writer = XmlWriter.Create(response.Output, new XmlWriterSettings { Indent = true }))
        //{
        //    formatter.WriteTo(writer);
        //}
        protected override void WriteFile(HttpResponseBase response)
        {
            Atom10FeedFormatter formatter = new Atom10FeedFormatter(_feed);
            using (XmlWriter writer = XmlWriter.Create(response.OutputStream, new XmlWriterSettings { Indent = true }))
            {
                formatter.WriteTo(writer);
            }
        }
    }

}