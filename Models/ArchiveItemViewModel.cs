using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class ArchiveItemViewModel
    {
        public int Year { get; set; }
        public int Mounth { get; set; }
        public int Count { get; set; }
        public List<BlogPost> Posts { get; set; }
    }
}