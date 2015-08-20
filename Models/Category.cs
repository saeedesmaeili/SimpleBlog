using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Category
    {
        public Category()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IconClass { get; set; }

        public int Order { get; set; }

        public Guid ParentId { get; set; }

        public virtual ICollection<BlogPost> BlogPosts { get; set; }

    }
}