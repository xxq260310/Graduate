using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.ViewModels
{
    public class UserProfileViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public byte[] ImageData { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string RoleName { get; set; }
        public string ImageType { get; set; }
        public string Sex { get; set; }
    }
}