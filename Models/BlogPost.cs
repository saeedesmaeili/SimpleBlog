using Blog.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [Display(Name = "عنوان مطلب")]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        public string Picture { get; set; }

        [Display(Name = "نمایش تصویر")]
        public bool ShowPicture { get; set; }


        [Display(Name = "متن کامل")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "پیش نمایش")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Preview { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [DataType(DataType.MultilineText)]
        public string Keywords { get; set; }

        [Display(Name = "نویسنده")]
        public string AuthorId { get; set; }

        [Display(Name = "نویسنده")]
        public virtual ApplicationUser Author { get; set; }

        [Display(Name = "منتشر شده")]
        public bool IsPublished { get; set; }


        private DateTime _publishDate;
        [Display(Name = "تاریخ انتشار")]
        [UIHint("PersianDateTime")]
        public DateTime PublishDate
        {
            get
            {
                return this._publishDate;
            }
            set
            {
                this._publishDate = value;
                this.PublishShamsiMounth = value.GetPersianMonth();
                this.PublishShamsiYear = value.GetPersianYear();
            }
        }

        public int PublishShamsiMounth { get; set; }

        public int PublishShamsiYear { get; set; }

        [Display(Name = "تاریخ تولید")]
        [UIHint("PersianDate")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        [UIHint("PersianDate")]
        public DateTime LastModifiedDate { get; set; }

        [Display(Name = "شاخه")]
        public Guid CategoryId { get; set; }

        [Display(Name = "شاخه")]
        public virtual Category Category { get; set; }

        [Display(Name = "برچسب ها")]
        public virtual ICollection<Tag> Tags { get; set; }

    }
}