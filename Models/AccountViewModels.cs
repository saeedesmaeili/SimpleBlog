using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        public string Provider { get; set; }

        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [Display(Name = "کد")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "این مرورگر را به خاطر بسپارم ؟")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده معتبر نیست")]
        public string Email { get; set; }

        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "به خاطر بسپارم ؟")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [StringLength(100, ErrorMessage = "{0} حداقل باید {2} کاراکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمزعبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمزعبور")]
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن یکی نیستند.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [StringLength(100, ErrorMessage = "{0} حداقل باید {2} کاراکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمزعبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکراررمزعبور")]
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار آن یکی نیستند.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage ="وارد کردن {0} الزامیست")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
}
