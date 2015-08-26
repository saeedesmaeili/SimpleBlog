using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Display(Name = "نام شاخه")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Icon")]
        public string IconClass { get; set; }

        [Required]
        [Display(Name = "اولویت چیدمان")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "شاخه پدر")]
        public Guid ParentId { get; set; }

        public virtual ICollection<BlogPost> BlogPosts { get; set; }

    }
}