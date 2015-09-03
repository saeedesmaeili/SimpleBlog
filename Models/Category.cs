using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Category
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "نام شاخه")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Icon")]
        public string IconClass { get; set; }

        [Required]
        [Display(Name = "اولویت چیدمان")]
        public int Order { get; set; }

        [Display(Name = "شاخه پدر")]
        public int ParentId { get; set; }

        public virtual ICollection<BlogPost> BlogPosts { get; set; }

    }
}