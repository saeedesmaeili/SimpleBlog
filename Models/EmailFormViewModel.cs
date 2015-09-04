using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class EmailFormViewModel
    {
        [Required(ErrorMessage ="لطفا نام خود را وارد کنید"), Display(Name = "نام شما")]
        public string FromName { get; set; }

        [Required(ErrorMessage = "وارد کردن پست الکترونیکی الزامی است"), Display(Name = "پست الکترونیکی"), EmailAddress(ErrorMessage ="پست الکترونیکی وارد شده معتبر نیست")]
        
        public string FromEmail { get; set; }

        [Required(ErrorMessage ="پیام نمی تواند خالی باشد"),Display(Name = "پیام شما")]
        public string Message { get; set; }
    }
}