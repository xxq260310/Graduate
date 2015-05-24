using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [Required]
        [Display(Name = "性别")]
        public int Sex { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "联系电话")]
        [DataType(DataType.PhoneNumber)]
        public string Contact { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "头像")]
        public byte[] ImageData { get; set; }

        public string ImageType { get; set; }
    }
}