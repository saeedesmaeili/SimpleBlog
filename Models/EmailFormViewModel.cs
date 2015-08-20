using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class EmailFormViewModel
    {
        [Required, Display(Name = "نام شما")]
        public string FromName { get; set; }

        [Required, Display(Name = "پست الکترونیکی"), EmailAddress]
        public string FromEmail { get; set; }

        [Required,Display(Name = "پیام شما")]
        public string Message { get; set; }
    }
}