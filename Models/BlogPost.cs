using Blog.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    public class BlogPost
    {

        public int Id { get; set; }

        [Display(Name = "عنوان مطلب")]
        [Required(ErrorMessage ="وارد کردن عنوان مطلب الزامی است")]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        public string Picture { get; set; }

        [Display(Name = "نمایش تصویر")]
        public bool ShowPicture { get; set; }

        [Display(Name = "Meta Description")]
        [Required(ErrorMessage = "وارد کردن Meta Description الزامی است")]
        [DataType(DataType.MultilineText)]
        [MaxLength(150 , ErrorMessage = "حداکثر طول مجاز 150 کاراکتر میباشد")]
        public string MetaDescription { get; set; }

        [Display(Name = "متن کامل")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [AllowHtml]
        [Display(Name = "پیش نمایش")]
        [DataType(DataType.MultilineText)]
        public string Preview { get; set; }

        [Display(Name = "کلمات کلیدی")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage ="واردکردن کلمات کلیدی الزامی است")]
        public string Keywords { get; set; }

        [Display(Name = "نویسنده")]
        public string AuthorId { get; set; }

        [Display(Name = "نویسنده")]
        public virtual ApplicationUser Author { get; set; }

        [Display(Name = "منتشر شده")]
        public bool IsPublished { get; set; }


        private DateTime _publishDate;
        [Display(Name = "تاریخ انتشار")]
        [UIHint("PersianDate")]
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
        [Required(ErrorMessage ="لطفا برای این مطلب یک شاخه انتخاب کنید")]
        public int CategoryId { get; set; }

        [Display(Name = "شاخه")]
        public virtual Category Category { get; set; }

        [Display(Name = "برچسب ها")]
        public virtual ICollection<Tag> Tags { get; set; }



        public string GenerateSlug()
        {
            string phrase = string.Format("{0}-{1}", Id, Title);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9آ-ی\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

    }
}