using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Tag
    {
        
        public int Id { get; set; }

        [Required , Display(Name ="عنوان")]
        public string Title { get; set; }

        public virtual ICollection<BlogPost> BlogPosts { get; set; }
    }
}