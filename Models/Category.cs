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

        [Required(ErrorMessage = "وارد کردن نام شاخه الزامی است")]
        [Display(Name = "نام شاخه")]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفا برای این شاخه یک Icon انتخاب کنید")]
        [Display(Name = "Icon")]
        public string IconClass { get; set; }

        [Required(ErrorMessage = "وارد کردن اولویت چیدمان الزامی است" )]
        [Display(Name = "اولویت چیدمان")]
        public int Order { get; set; }

        [Display(Name = "شاخه پدر")]
        public int ParentId { get; set; }

        public virtual ICollection<BlogPost> BlogPosts { get; set; }

    }
}