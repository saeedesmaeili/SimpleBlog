using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;

namespace Blog.Helpers
{
    public class CDataSyndicationContent : TextSyndicationContent
    {
        public CDataSyndicationContent(TextSyndicationContent content)
            : base(content)
        { }

        
        protected override void WriteContentsTo(System.Xml.XmlWriter writer)
        {
            writer.WriteCData(Text);
        }
    }
}